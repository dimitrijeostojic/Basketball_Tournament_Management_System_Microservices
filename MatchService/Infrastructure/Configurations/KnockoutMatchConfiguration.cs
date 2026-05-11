using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class KnockoutMatchConfiguration : IEntityTypeConfiguration<KnockoutMatch>
{
    public void Configure(EntityTypeBuilder<KnockoutMatch> builder)
    {
        builder.ToTable("KnockoutMatches");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PublicId).IsRequired();

        // BracketId je int FK → KnockoutBracket.Id (PK)
        builder.Property(x => x.BracketId).IsRequired();

        // Ovi propertyji su nullable — popunjavaju se tek kada tim prođe u rundu
        builder.Property(x => x.HomeTeamPublicId);
        builder.Property(x => x.AwayTeamPublicId);
        builder.Property(x => x.HomeTeamName).HasMaxLength(200);
        builder.Property(x => x.AwayTeamName).HasMaxLength(200);
        builder.Property(x => x.HomePoints);
        builder.Property(x => x.AwayPoints);
        builder.Property(x => x.WinnerPublicId);

        builder.Property(x => x.StadiumPublicId);
        builder.Property(x => x.ScheduledAt);

        builder.Property(x => x.Round).IsRequired();
        builder.Property(x => x.MatchOrder).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        // Standardni FK: BracketId → KnockoutBracket.Id (int PK), nema potrebe za HasPrincipalKey
        builder.HasOne(x => x.Bracket)
            .WithMany(x => x.Matches)
            .HasForeignKey(x => x.BracketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
