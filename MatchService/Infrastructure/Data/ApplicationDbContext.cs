using Domain.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<KnockoutBracket> KnockoutBrackets => Set<KnockoutBracket>();
    public DbSet<KnockoutMatch> KnockoutMatches => Set<KnockoutMatch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PublicId).IsRequired();
            entity.Property(e => e.HomeTeamPublicId).IsRequired();
            entity.Property(e => e.AwayTeamPublicId).IsRequired();
            entity.Property(e => e.StadiumPublicId).IsRequired();
            entity.Property(e => e.HomeTeamName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.AwayTeamName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.StadiumName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.Status).IsRequired();
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        using var transaction = await base.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await action();
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

        }
    }
}
