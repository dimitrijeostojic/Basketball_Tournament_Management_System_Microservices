using Domain.Entities;

namespace Domain.RepositoryInterfaces;

public interface IKnockoutBracketRepository
{
    Task<KnockoutBracket?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default);
    Task AddAsync(KnockoutBracket bracket, CancellationToken cancellationToken = default);
}
