using Application.Common.Collection;

namespace Application.Knockout.GetKnockoutBracket;

public sealed class GetKnockoutBracketResponse(ICollection<KnockoutMatchDto> matches)
    : EntityCollectionResult<KnockoutMatchDto>(matches)
{
}
