using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReviewRumble.Business;
using ReviewRumble.Models;

namespace ReviewRumble.Controllers;

[ApiController]
[Route("api/pullRequest")]
public class PullRequestController : ControllerBase
{
    private readonly IPullRequestBal pullRequestBal;
    public PullRequestController(IPullRequestBal pullRequestBal)
    {
        this.pullRequestBal = pullRequestBal;
    }

    [HttpPost("reviewers")]
    [ProducesResponseType(typeof(PrReviewers), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public IActionResult GetReviewers([FromBody] GetPullRequest pullRequest)
    {
        try
        {
            var reviewers = pullRequestBal.GetReviewers(pullRequest);

            return Ok(reviewers);
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

