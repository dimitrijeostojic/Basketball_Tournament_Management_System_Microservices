using Application.Common.Collection;
using Application.Stadium.GetAllStadiums;

namespace Application.Stadium.GetAll;

public sealed class GetAllStadiumsResponse(ICollection<GetAllStadiumsDto> items)
    : EntityCollectionResult<GetAllStadiumsDto>(items)
{
}
