using Mango.Web.Models;
using Mango.Web.Resources;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCoupon(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                ApiUrl = StaticDetails.CartAPIBase + ApiUrlResource.ApplyCoupon
            });
        }

        public async Task<ResponseDto?> GetCartByUserId(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.CartAPIBase + ApiUrlResource.GetCart + userId
            });
        }

        public async Task<ResponseDto?> RemoveFromCart(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDetailsId,
                ApiUrl = StaticDetails.CartAPIBase + ApiUrlResource.RemoveCart
            });
        }

        public async Task<ResponseDto?> UpsertCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                ApiUrl = StaticDetails.CartAPIBase + ApiUrlResource.UpsertCart
            });
        }
    }
}
