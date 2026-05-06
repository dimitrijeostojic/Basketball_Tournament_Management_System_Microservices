using Application.Common;
using Core;
using Domain.Abstractions;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Team.Delete;

internal sealed class DeleteTeamRequestHandler(
    ITeamRepository teamRepository,
    ITeamStandingRepository standingRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTeamRequest, Result<DeleteTeamResponse>>
{
    private readonly ITeamRepository _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
    private readonly ITeamStandingRepository _standingRepository = standingRepository ?? throw new ArgumentNullException(nameof(standingRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<DeleteTeamResponse>> Handle(
        DeleteTeamRequest request,
        CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByPublicIdAsync(request.PublicId, cancellationToken);
        if (team is null)
            return Result<DeleteTeamResponse>.Failure(ApplicationErrors.NotFound);

        await _standingRepository.DeleteByTeamPublicIdAsync(team.PublicId, cancellationToken);
        _teamRepository.Delete(team);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<DeleteTeamResponse>.Success(new DeleteTeamResponse
        {
            PublicId = team.PublicId,
            TeamName = team.TeamName
        });
    }
}