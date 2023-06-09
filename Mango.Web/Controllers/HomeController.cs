﻿using Mango.Web.Models;
using Mango.Web.Resources;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
		public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
			_productService = productService;
		}

        public async Task<IActionResult> Index()
        {
			List<ProductDto>? productList = new();
			ResponseDto? response = await _productService.GetAllProductsAsync();
			if (response != null && response.IsSuccess)
			{
				productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
			}
			else
			{
				TempData[ConstantResource.Tempdata_Error] = response?.Message;
			}
			return View(productList);
		}

        [Authorize]
        public async Task<IActionResult> Details(int productId)
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}