using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementations;

public sealed class TeamStandingRepository(ApplicationDbContext context) : ITeamStandingRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<TeamStanding?> GetByTeamPublicIdAsync(
        Guid teamPublicId,
        CancellationToken cancellationToken = default)
        => await _context.TeamStandings
            .FirstOrDefaultAsync(s => s.TeamPublicId == teamPublicId, cancellationToken);

    public async Task<List<TeamStanding>> GetByGroupPublicIdAsync(
        Guid groupPublicId,
        CancellationToken cancellationToken = default)
        => await _context.TeamStandings
            .Where(s => _context.Teams
                .Any(t => t.PublicId == s.TeamPublicId &&
                          t.Group!.PublicId == groupPublicId))
            .ToListAsync(cancellationToken);

    public async Task AddAsync(
        TeamStanding standing,
        CancellationToken cancellationToken = default)
        => await _context.TeamStandings.AddAsync(standing, cancellationToken);

    public async Task DeleteByTeamPublicIdAsync(
        Guid teamPublicId,
        CancellationToken cancellationToken = default)
    {
        var standing = await _context.TeamStandings
            .FirstOrDefaultAsync(s => s.TeamPublicId == teamPublicId, cancellationToken);

        if (standing is not null)
            _context.TeamStandings.Remove(standing);
    }
}
