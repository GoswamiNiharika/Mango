using Mango.Web.Models;
using Mango.Web.Resources;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static System.Net.WebRequestMethods;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = productDto,
                ApiUrl = StaticDetails.ProductAPIBase + ApiUrlResource.Product
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                ApiUrl = StaticDetails.ProductAPIBase + ApiUrlResource.Product + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.ProductAPIBase + ApiUrlResource.Product
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.ProductAPIBase + ApiUrlResource.Product + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = productDto,
                ApiUrl = StaticDetails.ProductAPIBase + ApiUrlResource.Product
            });
        }
    }
}
