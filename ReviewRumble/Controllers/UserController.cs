using Microsoft.AspNetCore.Mvc;
using ReviewRumble.Business;
using ReviewRumble.Models;
using System.Net;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace ReviewRumble.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserBal userManager;

		public UserController(IUserBal userManager)
		{ 
            this.userManager = userManager;
		}

		[HttpGet]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<UserInfo>> GetUserDetails()
		{
            try
            {
                var author = User.Identity?.Name ?? "gauravpreet-wg";
                var user =  await userManager.GetUser(author);
                return Ok(user);
            }
            catch (Exception)
            {
                return Problem(
                    "Error occurred while processing your request.",
                    statusCode: (int?)HttpStatusCode.InternalServerError,
                    type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                    instance: HttpContext.Request.Path);
            }
		}
	}
}