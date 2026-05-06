using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class KnockoutBracketConfiguration : IEntityTypeConfiguration<KnockoutBracket>
{
    public void Configure(EntityTypeBuilder<KnockoutBracket> builder)
    {
        builder.ToTable("KnockoutBrackets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PublicId).IsRequired();
        builder.HasIndex(x => x.PublicId).IsUnique();
    }
}
