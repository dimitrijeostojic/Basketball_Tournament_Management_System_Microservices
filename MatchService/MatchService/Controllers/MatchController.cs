using Application.Knockout.CreateKnockoutBracket;
using Application.Knockout.GetKnockoutBracket;
using Application.Knockout.RecordKnockoutResult;
using Application.Knockout.ScheduleKnockoutMatch;
using Application.Match.CreateMatch;
using Application.Match.DeleteMatch;
using Application.Match.ForfeitMatch;
using Application.Match.GetMatchByPublicId;
using Application.Match.GetMatches;
using Application.Match.RecordMatchResult;
using Application.Match.UpdateMatch;
using Core;
using MatchService.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MatchController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpPost]
    public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpPut("{publicId}/result")]
    public async Task<IActionResult> RecordResult([FromRoute] Guid publicId, [FromBody] RecordMatchResultRequest request, CancellationToken cancellationToken)
    {
        request.MatchPublicId = publicId;
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [AllowAnonymous]
    [HttpGet("{publicId:guid}")]
    public async Task<IActionResult> GetMatchByPublicId([FromRoute] Guid publicId, CancellationToken cancellationToken)
    {
        var request = new GetMatchByPublicIdRequest { MatchPublicId = publicId };
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpPut("{publicId}/forfeit")]
    public async Task<IActionResult> Forfeit([FromRoute] Guid publicId, ForfeitMatchRequest request, CancellationToken cancellationToken)
    {
        request.MatchPublicId = publicId;
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetMatchesRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteMatch([FromBody] DeleteMatchRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpPut]
    public async Task<IActionResult> UpdateMatch([FromBody] UpdateMatchRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [AllowAnonymous]
    [HttpGet("knockout/brackets/{id:guid}")]
    public async Task<IActionResult> GetKnockoutBracket([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetKnockoutBracketRequest { BracketPublicId = id };
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpPost("knockout/results")]
    public async Task<IActionResult> RecordKnockoutResult([FromBody] RecordKnockoutResultRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost("knockout/brackets")]
    public async Task<IActionResult> CreateKnockoutBracket([FromBody] CreateKnockoutBracketRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost("knockout/{matchPublicId}/schedule")]
    public async Task<IActionResult> ScheduleKnockoutMatch([FromRoute] Guid matchPublicId, [FromBody] ScheduleKnockoutMatchRequest request, CancellationToken cancellationToken)
    {
        request.MatchPublicId = matchPublicId;
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }
}
