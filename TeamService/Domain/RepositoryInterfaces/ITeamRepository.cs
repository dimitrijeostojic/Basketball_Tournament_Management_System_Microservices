using Domain.Entities;

namespace Domain.RepositoryInterfaces;

public interface ITeamRepository
{
    Task<List<Team>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Team?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default);
    Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Team>> GetByGroupPublicIdAsync(Guid groupPublicId, CancellationToken cancellationToken = default);
    Task AddAsync(Team team, CancellationToken cancellationToken = default);
    void Delete(Team team);
}
