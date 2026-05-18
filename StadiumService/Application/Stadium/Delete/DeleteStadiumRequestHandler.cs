using Application.Common;
using Core;
using Domain.Abstractions;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Stadium.Delete;

internal sealed class DeleteStadiumRequestHandler(
    IStadiumRepository stadiumRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteStadiumRequest, Result<DeleteStadiumResponse>>
{
    private readonly IStadiumRepository _stadiumRepository = stadiumRepository ?? throw new ArgumentNullException(nameof(stadiumRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<DeleteStadiumResponse>> Handle(DeleteStadiumRequest request, CancellationToken cancellationToken)
    {
        var stadium = await _stadiumRepository.GetByPublicIdAsync(request.PublicId, cancellationToken);
        if (stadium is null)
            return Result<DeleteStadiumResponse>.Failure(ApplicationErrors.NotFound);

        _stadiumRepository.Delete(stadium);
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result<DeleteStadiumResponse>.Success(new DeleteStadiumResponse()
        {
            Message = $"Stadium with PublicId {request.PublicId} has been deleted successfully."
        });
    }
}
