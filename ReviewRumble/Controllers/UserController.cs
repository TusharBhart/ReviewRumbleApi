using Microsoft.AspNetCore.Mvc;
using Refit;
using ReviewRumble.Business;

namespace ReviewRumble.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserManager userManager;

		public UserController(IUserManager userManager)
		{ 
            this.userManager = userManager;
		}

		[HttpGet]

		public async Task<object> GetUserDetails()
		{
			var response = await userManager.GetUser();
			return response;
		}
	}
}