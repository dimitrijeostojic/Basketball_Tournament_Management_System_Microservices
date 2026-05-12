using Domain.Entities;

namespace Domain.RepositoryInterfaces;

public interface IMatchRepository
{
    Task<Match?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default);
    Task<List<Match>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsTeamConflictAsync(Guid homeTeamPublicId, Guid awayTeamPublicId, DateTime startTime, CancellationToken cancellationToken = default);
    Task<bool> ExistsStadiumConflictAsync(Guid stadiumPublicId, DateTime startTime, CancellationToken cancellationToken = default);
    Task<bool> ExistsSameMatchAsync(Guid homeTeamPublicId, Guid awayTeamPublicId, CancellationToken cancellationToken = default);
    Task AddAsync(Match match, CancellationToken cancellationToken = default);
    void Delete(Match match, CancellationToken cancellationToken = default);
}
