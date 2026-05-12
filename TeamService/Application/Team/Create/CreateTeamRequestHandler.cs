using Application.Common;
using Core;
using Domain.Abstractions;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Team.Create;

internal sealed class CreateTeamRequestHandler(
    ITeamRepository teamRepository,
    IGroupRepository groupRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTeamRequest, Result<CreateTeamResponse>>
{
    private readonly int _maxTeams = 4;
    private readonly ITeamRepository _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
    private readonly IGroupRepository _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<CreateTeamResponse>> Handle(
        CreateTeamRequest request,
        CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByPublicIdAsync(request.GroupPublicId, cancellationToken);
        if (group is null)
        {
            return Result<CreateTeamResponse>.Failure(ApplicationErrors.NotFound);
        }
        if (group.Teams.Count >= _maxTeams)
        {
            return Result<CreateTeamResponse>.Failure(ApplicationErrors.GroupTeamLimitReached);
        }
        var existingTeam = await _teamRepository.GetByNameAsync(request.TeamName, cancellationToken);
        if (existingTeam is not null)
        {
            return Result<CreateTeamResponse>.Failure(ApplicationErrors.MultipleTeamsError);
        }
        var team = Domain.Entities.Team.Create(request.TeamName, request.FlagIcon, group);
        await _teamRepository.AddAsync(team, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateTeamResponse>.Success(new CreateTeamResponse
        {
            PublicId = team.PublicId,
            TeamName = team.TeamName,
            FlagIcon = team.FlagIcon
        });
    }
}