using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Infrastructure.Data;

public class MongoDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public DbSet<Stadium> Stadiums { get; init; } = null!;

    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Stadium>().ToCollection("stadiums");
    }
}
