using Application.Common.Collection;

namespace Application.Match.GetMatches;

public sealed class GetMatchesResponse(ICollection<GetMatchesDto> matches)
    : EntityCollectionResult<GetMatchesDto>(matches)
{
}
