using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementations;

public sealed class KnockoutBracketRepository(ApplicationDbContext context) : IKnockoutBracketRepository
{
    public async Task<KnockoutBracket?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default)
        => await context.KnockoutBrackets
            .Include(b => b.Matches)
            .FirstOrDefaultAsync(b => b.PublicId == publicId, cancellationToken);

    public async Task AddAsync(KnockoutBracket bracket, CancellationToken cancellationToken = default)
        => await context.KnockoutBrackets.AddAsync(bracket, cancellationToken);
}
