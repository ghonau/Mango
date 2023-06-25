using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utlities;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService ; 
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.POST,
                Data = couponDto,
                Url = SD.CouponAPIBase + $"/api/coupon",

            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + $"/api/coupon/{couponId}",

            });
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.GET,
                Url = SD.CouponAPIBase+ "/api/coupon",

            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/coupon/GetByCode/{couponCode}",

            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/coupon/{couponId}",

            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.PUT,
                Data = couponDto,
                Url = SD.CouponAPIBase + $"/api/coupon",

            });
        }
    }
}
