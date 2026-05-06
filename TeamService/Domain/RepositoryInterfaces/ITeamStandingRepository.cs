using Domain.Entities;

namespace Domain.RepositoryInterfaces;

public interface ITeamStandingRepository
{
    Task<TeamStanding?> GetByTeamPublicIdAsync(Guid teamPublicId, CancellationToken cancellationToken = default);
    Task<List<TeamStanding>> GetByGroupPublicIdAsync(Guid groupPublicId, CancellationToken cancellationToken = default);
    Task AddAsync(TeamStanding standing, CancellationToken cancellationToken = default);
    Task DeleteByTeamPublicIdAsync(Guid teamPublicId, CancellationToken cancellationToken = default);
}
