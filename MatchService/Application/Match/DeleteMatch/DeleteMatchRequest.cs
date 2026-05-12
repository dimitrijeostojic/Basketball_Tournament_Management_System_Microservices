using Core;
using MediatR;

namespace Application.Match.DeleteMatch;

public sealed class DeleteMatchRequest
    : IRequest<Result<DeleteMatchResponse>>
{
    public Guid MatchPublicId { get; set; }
}
