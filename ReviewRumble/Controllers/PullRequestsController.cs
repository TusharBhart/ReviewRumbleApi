using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewRumble.Business;
using ReviewRumble.Models;

namespace ReviewRumble.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PullRequestsController : ControllerBase
{
    private readonly IPullRequestBal pullRequestBal;

    public PullRequestsController(IPullRequestBal pullRequestBal)
    {
        this.pullRequestBal = pullRequestBal;
    }

	[HttpPost]
    [ProducesResponseType(typeof(PullRequestViewModel), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult<PullRequestViewModel>> AddAsync([FromBody] NewPullRequest newPullRequest)
    {
        try
        {
            var author = User.Identity?.Name ?? "tusharbhart-wg";
            var pullRequest = await pullRequestBal.AddAsync(newPullRequest, author);

            return Ok(pullRequest);
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

    [HttpGet]
    [ProducesResponseType(typeof(List<PullRequestViewModel>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult<List<PullRequestViewModel>>> GetAsync()
    {
        try
        {
            var pullRequests = await pullRequestBal.GetAllAsync();

            return Ok(pullRequests);
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

    [HttpPut("stauts")]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult<List<PullRequestViewModel>>> UpdateStatusAsync([FromBody] UpdatePullRequestStatus updatePullRequestStatus)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ?? "1";

            await pullRequestBal.UpdateStatusAsync(updatePullRequestStatus.Id, Convert.ToInt32(userId), updatePullRequestStatus.Status);

            return Ok(new { Message = "Pull request status updated successfully."});
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

