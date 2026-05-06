using Application.Common;
using Application.Contracts;
using Core;
using Domain.Abstraction;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Knockout.CreateKnockoutBracket;

public sealed class CreateKnockoutBracketRequestHandler(
    IKnockoutBracketRepository bracketRepository,
    ITeamServiceClient teamServiceClient,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateKnockoutBracketRequest, Result<CreateKnockoutBracketResponse>>
{
    public async Task<Result<CreateKnockoutBracketResponse>> Handle(
        CreateKnockoutBracketRequest request,
        CancellationToken cancellationToken)
    {
        if (request.SeededTeamPublicIds.Count != 8)
            return Result<CreateKnockoutBracketResponse>.Failure(ApplicationErrors.InvalidSeededTeamsCount);

        if (request.SeededTeamPublicIds.Distinct().Count() != 8)
            return Result<CreateKnockoutBracketResponse>.Failure(ApplicationErrors.DuplicateSeededTeams);

        // Dohvati nazive svih timova iz TeamService
        var seededTeams = new List<(Guid PublicId, string Name)>();

        foreach (var teamPublicId in request.SeededTeamPublicIds)
        {
            var team = await teamServiceClient.GetTeamByPublicIdAsync(teamPublicId, cancellationToken);

            if (team is null || !team.Exists)
                return Result<CreateKnockoutBracketResponse>.Failure(ApplicationErrors.NotFound);

            seededTeams.Add((teamPublicId, team.TeamName));
        }

        var bracket = KnockoutBracket.Create(seededTeams);

        await bracketRepository.AddAsync(bracket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateKnockoutBracketResponse>.Success(new CreateKnockoutBracketResponse
        {
            PublicId = bracket.PublicId
        });
    }
}
