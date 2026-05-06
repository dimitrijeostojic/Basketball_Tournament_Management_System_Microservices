using Core;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Stadium.CheckAvailability;

public sealed class CheckStadiumAvailabilityRequestHandler(IStadiumRepository stadiumRepository)
    : IRequestHandler<CheckStadiumAvailabilityRequest, Result<CheckStadiumAvailabilityResponse>>
{
    public async Task<Result<CheckStadiumAvailabilityResponse>> Handle(
        CheckStadiumAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var stadium = await stadiumRepository.GetByPublicIdAsync(request.PublicId, cancellationToken);

        return Result<CheckStadiumAvailabilityResponse>.Success(
            new CheckStadiumAvailabilityResponse
            {
                Exists = stadium is not null,
                PublicId = request.PublicId,
                StadiumName = stadium?.StadiumName ?? string.Empty
            });
    }
}