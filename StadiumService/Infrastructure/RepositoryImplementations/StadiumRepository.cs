using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementations;

public sealed class StadiumRepository(MongoDbContext context) : IStadiumRepository
{
    private readonly MongoDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public Task CreateAsync(Stadium stadium, CancellationToken cancellationToken)
    {
        _context.Stadiums.Add(stadium);
        return Task.CompletedTask;
    }

    public async Task<List<Stadium>> GetAllStadiumsAsync(CancellationToken cancellationToken)
    {
        return await _context.Stadiums.ToListAsync(cancellationToken);
    }

    public async Task<Stadium?> GetByPublicIdAsync(Guid stadiumPublicId, CancellationToken cancellationToken)
    {
        return await _context.Stadiums
            .FirstOrDefaultAsync(s => s.PublicId == stadiumPublicId, cancellationToken);
    }

    public void Delete(Stadium stadium)
    {
        _context.Stadiums.Remove(stadium);
    }
}
