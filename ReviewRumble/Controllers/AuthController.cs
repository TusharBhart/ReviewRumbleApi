using Microsoft.AspNetCore.Mvc;
using ReviewRumble.Business;

namespace ReviewRumble.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBal _authBusiness;

        public AuthController(IAuthBal authBusiness)
        {
            this._authBusiness = authBusiness;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] string code)
        {
            try
            {
                var response = await _authBusiness.GetAccessTokenAsync(code);
                return Ok(response);
            }
            catch(Exception) 
            {
                return Unauthorized("NotAuthorized");
            }
        }
    }
}