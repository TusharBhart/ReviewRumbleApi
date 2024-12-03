using Microsoft.AspNetCore.Mvc;
using Refit;
using ReviewRumble.Business;

namespace ReviewRumble.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : Controller
	{
		private readonly IUserManager _userManager;
		private readonly IConfiguration _configuration;

		public UserController(IUserManager userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([Query] string code)
		{
			try
			{
				var response = await _userManager.GetAccessToken(_configuration["GithubApiClientSettings:ClientId"],
					_configuration["GithubApiClientSettings:ClientSecret"],
					code);
				return Ok(response);
			}
			catch(Exception ex) 
			{
				return Unauthorized("Not Authorized");
			}
		}

		[HttpGet("UserDetails")]

		public async Task<object> GetUserDetails()
		{
			var response = await _userManager.GetUser();
			return response;
		}
	}
}