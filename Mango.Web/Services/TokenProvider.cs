using Mango.Web.Services.IServices;
using Mango.Web.Utlities;

namespace Mango.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor; 
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(SD.Token); 
        }

        public string? GetToken()
        {
            var hasToken = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.Token, out string token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(SD.Token, token); 
        }
    }
}
