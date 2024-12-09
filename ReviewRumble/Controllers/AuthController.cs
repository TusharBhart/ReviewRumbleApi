using Microsoft.AspNetCore.Mvc;
using ReviewRumble.Business;
using ReviewRumble.Models;

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
        public async Task<IActionResult> Login([FromBody] AuthCode code)
        {
            try
            {
                var response = await _authBusiness.GetAccessTokenAsync(code.code);
                return Ok(new TokenResponse()
                {
                    Token = response
                });
            }
            catch(Exception) 
            {
                return Unauthorized("NotAuthorized");
            }
        }
    }
}