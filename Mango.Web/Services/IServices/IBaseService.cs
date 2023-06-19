using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Services.IServices
{
    public interface IBaseService
    {
       Task<ResponseDto?> SendAsync(RequestDto requestDto); 
    }
}
