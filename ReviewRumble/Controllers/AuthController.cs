using Microsoft.AspNetCore.Mvc;
using ReviewRumble.Business;

namespace ReviewRumble.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager userManager;

        public AuthController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string code)
        {
            try
            {
                var response = await userManager.GetAccessToken(code);
                return Ok(response);
            }
            catch(Exception) 
            {
                return Unauthorized("Not Authorized");
            }
        }
    }
}