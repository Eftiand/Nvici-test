
using API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations;

public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
{
      public void Configure(EntityTypeBuilder<Tournament> builder)
      {
            builder.HasKey(t => t.Id);

            // Name property
            builder.Property(t => t.Name)
                  .IsRequired();

            builder.HasMany(t => t.SubTournaments)
                  .WithOne(t => t.Parent)
                  .HasForeignKey(t => t.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Configure navigation property for Players
            builder.Navigation(t => t.Players)
                  .UsePropertyAccessMode(PropertyAccessMode.Field)
                  .AutoInclude();
      }
}