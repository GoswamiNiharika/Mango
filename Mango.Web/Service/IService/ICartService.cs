using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserId(string userId);
        Task<ResponseDto?> UpsertCart(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCart(int cartDetailsId);
        Task<ResponseDto?> ApplyCoupon(CartDto cartDto);
    }
}
