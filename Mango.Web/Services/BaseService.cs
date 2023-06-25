using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using System.Text;
using static Mango.Web.Utlities.SD;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;  
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
               
                //token 

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data),encoding: Encoding.UTF8, "application/json");
                }
                HttpResponseMessage? apiResponse = null;
                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        {
                            message.Method = HttpMethod.Post;
                            break;
                        }
                    case ApiType.PUT:
                        {
                            message.Method = HttpMethod.Put;
                            break;
                        }
                    case ApiType.DELETE:
                        {
                            message.Method = HttpMethod.Delete;
                            break;
                        }
                    default:
                        {
                            message.Method = HttpMethod.Get;
                            break;
                        }
                }

                apiResponse = await httpClient.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        {
                            return new() { IsSuccess = false, Message = "Not Found" };
                        }
                    case System.Net.HttpStatusCode.Forbidden:
                        {
                            return new() { IsSuccess = false, Message = "Access denied" };
                        }
                    case System.Net.HttpStatusCode.Unauthorized:
                        {
                            return new() { IsSuccess = false, Message = "Unauthorizied" };
                        }
                    case System.Net.HttpStatusCode.InternalServerError:
                        {
                            return new() { IsSuccess = false, Message = "Internal Server Error" };
                        }
                    default:
                        {
                            var apiContent = await apiResponse.Content.ReadAsStringAsync();
                            var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                            return apiResponseDto;
                        }
                }
           

            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false,
                };
                return dto; 
            }
        }
    }
}
