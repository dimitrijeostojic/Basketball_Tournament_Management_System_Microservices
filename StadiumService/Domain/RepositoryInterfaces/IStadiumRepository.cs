using Domain.Entities;

namespace Domain.RepositoryInterfaces;

public interface IStadiumRepository
{
    Task<List<Stadium>> GetAllStadiumsAsync(CancellationToken cancellationToken);
    Task<Stadium?> GetByPublicIdAsync(Guid stadiumPublicId, CancellationToken cancellationToken);
    Task CreateAsync(Stadium stadium, CancellationToken cancellationToken);
    void Delete(Stadium stadium);
}
