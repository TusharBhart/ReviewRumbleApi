using Microsoft.AspNetCore.Mvc;
using ReviewRumble.Business;
using ReviewRumble.Models;
using System.Net;
using System.Security.Claims;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace ReviewRumble.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly IUserBal userBal;

	public UserController(IUserBal userBal)
	{ 
        this.userBal = userBal;
	}

	[HttpGet]
    [ProducesResponseType(typeof(UserInfo), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult<UserInfo>> GetDetailsAsync()
	{
        try
        {
            var author = User.Identity?.Name ?? "tusharbhart-wg";
            var user =  await userBal.GetUserAsync(author);
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

    [HttpGet("assigned-pull-requests")]
    [ProducesResponseType(typeof(List<PullRequestViewModel>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult<List<PullRequestViewModel>>> GetAssignedPullRequestsAsync()
    {
        try
        {
            var id = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ?? "1";

            var user = await userBal.GetAssignedPullRequestsAsync(Convert.ToInt32(id));
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

    [HttpGet("my-pull-requests")]
    [ProducesResponseType(typeof(List<PullRequestViewModel>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult<List<PullRequestViewModel>>> GetMyPullRequestsAsync()
    {
        try
        {
            var id = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ?? "7";

            var user = await userBal.GetMyPullRequestsAsync(Convert.ToInt32(id));
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

    [HttpGet("leaderboard")]
    [ProducesResponseType(typeof(List<UserInfo>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult<List<UserInfo>>> GetLeaderboardAsync()
    {
        try
        {
            var users = await userBal.GetLeaderboard();
            return Ok(users);
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

    [HttpPut("status")]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult> UpdateStatusAsync([FromBody] UpdateReviewerStatus status)
    {
        try
        {
            var id = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ?? "7";

            await userBal.UpdateStatusAsync(Convert.ToInt32(id), status.Status);
            return Ok(new { Message = "User status updated successfully." });
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
