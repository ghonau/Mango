using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private IMapper _mapper;
        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> coupons = _db.Coupons.ToList(); 
                _responseDto.Result = _mapper.Map<List<CouponDto>>(coupons);

                
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto; 
            

        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.Single(coupon => coupon.CouponId == id);
                _responseDto.Result = _mapper.Map<CouponDto>(coupon); 
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;


        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto Get(string code)
        {
            try
            {
                Coupon coupon = _db.Coupons.FirstOrDefault(coupon => coupon.CouponCode.ToLower() == code.ToLower());
                if(coupon == null)
                {
                    _responseDto.IsSuccess = false;
                }
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;


        }
        [HttpPost]        
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                _db.Coupons.Add(_mapper.Map<Coupon>(couponDto));
                _db.SaveChanges(); 
                _responseDto.Result = _mapper.Map<CouponDto>(couponDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;


        }
        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                _db.Coupons.Update(_mapper.Map<Coupon>(couponDto));
                _db.SaveChanges();
                _responseDto.Result = _mapper.Map<CouponDto>(couponDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;


        }
        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.Find(id);
                if(obj == null)
                {
                    _responseDto.IsSuccess = false; 
                    return _responseDto;

                }
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
               
                
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;


        }
    }
}
