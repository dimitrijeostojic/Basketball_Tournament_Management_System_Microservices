using Application.Knockout.CreateKnockoutBracket;
using Application.Knockout.GetKnockoutBracket;
using Application.Knockout.RecordKnockoutResult;
using Application.Match.CreateMatch;
using Application.Match.ForfeitMatch;
using Application.Match.GetMatches;
using Application.Match.RecordMatchResult;
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

    [HttpPost]
    public async Task<IActionResult> CreateMatch(CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{publicId}/result")]
    public async Task<IActionResult> RecordResult(Guid publicId, RecordMatchResultRequest request, CancellationToken cancellationToken)
    {
        request.MatchPublicId = publicId;
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{publicId}/forfeit")]
    public async Task<IActionResult> Forfeit(Guid publicId, ForfeitMatchRequest request, CancellationToken cancellationToken)
    {
        request.MatchPublicId = publicId;
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetMatchesRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("knockout/brackets/{id:guid}")]
    public async Task<IActionResult> GetKnockoutBracket([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetKnockoutBracketRequest { BracketPublicId = id };
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("knockout/results")]
    public async Task<IActionResult> RecordKnockoutResult([FromBody] RecordKnockoutResultRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost("knockout/brackets")]
    public async Task<IActionResult> CreateKnockoutBracket([FromBody] CreateKnockoutBracketRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }
}
