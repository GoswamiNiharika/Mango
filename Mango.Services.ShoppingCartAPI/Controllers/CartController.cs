using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Resources;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using System.Xml;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public CartController(ApplicationDbContext context, 
                              IMapper mapper,
                              IProductService productService,
                              ICouponService couponService)
        {
            _context = context;
            _response = new ResponseDto();
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_context.CartHeaders.First(u => u.UserId == userId)),
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>
                                   (_context.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> products = await _productService.GetProducts();
                foreach(var item in cart.CartDetails)
                {
                    item.Product = products.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                //apply coupon if any
                if(!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }
                _response.Result = cart;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cart = await _context.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cart.CouponCode = cartDto.CartHeader.CouponCode;
                _context.CartHeaders.Update(cart);
                await _context.SaveChangesAsync();
                _response.Result = true;
                _response.Message = NotificationMessageResource.CouponAdded;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cart = await _context.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cart.CouponCode = "";
                _context.CartHeaders.Update(cart);
                await _context.SaveChangesAsync();
                _response.Result = true;
                _response.Message = NotificationMessageResource.CouponRemoved;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }
            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cart)
        {
            try
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
                if(cartHeader == null) 
                {
                    CartHeader header = _mapper.Map<CartHeader>(cart.CartHeader);
                    _context.CartHeaders.Add(header);
                    await _context.SaveChangesAsync();
                    cart.CartDetails.First().CartHeaderId = header.CartHeaderId;
                    _context.CartDetails.Add(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var cartDetails = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == cart.CartDetails.First().ProductId &&
                                                                          u.CartHeaderId == cartHeader.CartHeaderId);
                    if(cartDetails == null)
                    {
                        cart.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                        _context.CartDetails.Add(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        cart.CartDetails.First().Count += cartDetails.Count;
                        cart.CartDetails.First().CartHeaderId = cartDetails.CartHeaderId;
                        cart.CartDetails.First().CartDetailsId = cartDetails.CartDetailsId;
                        _context.CartDetails.Update(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                }
                _response.Result = cart;
                _response.Message = NotificationMessageResource.ProductAdded;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("CartRemove")]
        public async Task<ResponseDto> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await _context.CartDetails.FirstAsync(u => u.CartDetailsId == cartDetailsId);
                int totalCountOfCart = _context.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _context.CartDetails.Remove(cartDetails);
                if(totalCountOfCart == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _context.SaveChangesAsync(); 
                _response.Result = true;
                _response.Message = NotificationMessageResource.ProductRemoved;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
