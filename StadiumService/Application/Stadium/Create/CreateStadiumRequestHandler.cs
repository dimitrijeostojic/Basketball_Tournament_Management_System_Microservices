using Core;
using Domain.Abstractions;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Stadium.Create;

internal sealed class CreateStadiumRequestHandler(
    IStadiumRepository stadiumRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateStadiumRequest, Result<CreateStadiumResponse>>
{
    private readonly IStadiumRepository _stadiumRepository = stadiumRepository ?? throw new ArgumentNullException(nameof(stadiumRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<CreateStadiumResponse>> Handle(CreateStadiumRequest request, CancellationToken cancellationToken)
    {
        var stadium = Domain.Entities.Stadium.Create(request.StadiumName, request.City, request.Capacity);
        await _stadiumRepository.CreateAsync(stadium, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result<CreateStadiumResponse>.Success(new CreateStadiumResponse
        {
            PublicId = stadium.PublicId,
            StadiumName = stadium.StadiumName,
            City = stadium.City,
            Capacity = stadium.Capacity
        });
    }
}
