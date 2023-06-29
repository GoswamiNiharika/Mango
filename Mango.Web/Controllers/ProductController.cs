using Mango.Web.Models;
using Mango.Web.Resources;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? productList = new();
            ResponseDto? response = await _productService.GetAllProductsAsync();
            if(response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response?.Message;
            }
            return View(productList);
        }

        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(productDto);
                if (response != null && response.IsSuccess)
                {
                    TempData[ConstantResource.Tempdata_Success] = response?.Message;
                    return RedirectToAction(nameof(ProductIndex));                    
                }
                else
                {
                    TempData[ConstantResource.Tempdata_Error] = response?.Message;                    
                }                
            }
            return View(productDto);
        }

        public async Task<IActionResult> DeleteProduct(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(productDto.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData[ConstantResource.Tempdata_Success] = response?.Message;
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response?.Message;
            }
            return View(productDto);
        }

        public async Task<IActionResult> EditProduct(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductDto productDto)
        {
            ResponseDto? response = await _productService.UpdateProductAsync(productDto);
            if (response != null && response.IsSuccess)
            {
                TempData[ConstantResource.Tempdata_Success] = response?.Message;
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response?.Message;
            }
            return View(productDto);
        }
    }
}
