using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementations;

public sealed class MatchRepository(ApplicationDbContext context) : IMatchRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<bool> ExistsSameMatchAsync(Guid homeTeamPublicId, Guid awayTeamPublicId, CancellationToken cancellationToken = default)
        => await _context.Matches.AnyAsync(m =>
            (m.HomeTeamPublicId == homeTeamPublicId && m.AwayTeamPublicId == awayTeamPublicId) ||
            (m.HomeTeamPublicId == awayTeamPublicId && m.AwayTeamPublicId == homeTeamPublicId),
            cancellationToken);

    public async Task<bool> ExistsStadiumConflictAsync(Guid stadiumPublicId, DateTime startTime, CancellationToken cancellationToken = default)
      => await _context.Matches.AnyAsync(m =>
          m.StadiumPublicId == stadiumPublicId &&
          m.StartTime.Date == startTime.Date, cancellationToken);

    public async Task<bool> ExistsTeamConflictAsync(Guid homeTeamPublicId, Guid awayTeamPublicId, DateTime startTime, CancellationToken cancellationToken = default)
       => await _context.Matches.AnyAsync(m =>
           (m.HomeTeamPublicId == homeTeamPublicId || m.AwayTeamPublicId == awayTeamPublicId) &&
           m.StartTime.Date == startTime.Date, cancellationToken);

    public async Task<Match?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default)
      => await _context.Matches
          .FirstOrDefaultAsync(m => m.PublicId == publicId, cancellationToken);

    public async Task<List<Match>> GetAllAsync(CancellationToken cancellationToken = default)
       => await _context.Matches
           .OrderBy(m => m.StartTime)
           .ToListAsync(cancellationToken);

    public async Task AddAsync(Match match, CancellationToken cancellationToken = default)
       => await _context.Matches.AddAsync(match, cancellationToken);

    public void Delete(Match match, CancellationToken cancellationToken = default)
    => _context.Matches.Remove(match);
}

