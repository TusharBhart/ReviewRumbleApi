using System.Net;
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
    public async Task<ActionResult<PullRequestViewModel>> AddPullRequest([FromBody] NewPullRequest newPullRequest)
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
    public async Task<ActionResult<List<PullRequestViewModel>>> GetPullRequests()
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
}

