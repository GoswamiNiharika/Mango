using Mango.Web.Models;
using Mango.Web.Resources;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? couponList = new();
            ResponseDto? response = await _couponService.GetAllCouponsAsync();
            if(response != null && response.IsSuccess)
            {
                couponList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response?.Message;
            }
            return View(couponList);
        }

        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDto couponDto)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(couponDto);
                if (response != null && response.IsSuccess)
                {
                    TempData[ConstantResource.Tempdata_Success] = response?.Message;
                    return RedirectToAction(nameof(CouponIndex));                    
                }
                else
                {
                    TempData[ConstantResource.Tempdata_Error] = response?.Message;                    
                }                
            }
            return View(couponDto);
        }

        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);
            if (response != null && response.IsSuccess)
            {
                CouponDto? coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(coupon);
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCoupon(CouponDto couponDto)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);
            if (response != null && response.IsSuccess)
            {
                TempData[ConstantResource.Tempdata_Success] = response?.Message;
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response?.Message;
            }
            return View(couponDto);
        }
    }
}
