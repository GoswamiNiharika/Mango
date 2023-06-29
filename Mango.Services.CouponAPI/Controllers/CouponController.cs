using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Mango.Services.CouponAPI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ResponseDto response;
        private readonly IMapper _mapper;
        public CouponController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            response = new ResponseDto();
        }

        [HttpGet]
        [Authorize]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> couponList = _context.Coupons.ToList();
                response.Result = _mapper.Map<IEnumerable<CouponDto>>(couponList);
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = _context.Coupons.First( u => u.CouponId == id);
                response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto Get(string code)
        {
            try
            {
                Coupon coupon = _context.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());
                response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Add(coupon);
                _context.SaveChanges();

                response.Result = _mapper.Map<CouponDto>(coupon);
                response.Message = NotificationMessageResource.CouponCreated;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(coupon);
                _context.SaveChanges();

                response.Result = _mapper.Map<CouponDto>(coupon);
                response.Message = NotificationMessageResource.CouponUpdated;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon coupon = _context.Coupons.First(u => u.CouponId == id);
                _context.Coupons.Remove(coupon);
                _context.SaveChanges();
                response.Message = NotificationMessageResource.CouponDeleted;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
