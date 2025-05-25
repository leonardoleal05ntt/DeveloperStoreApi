using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.Date)
                   .IsRequired();

            builder.Property(s => s.CustomerId)
                   .IsRequired()
                   .HasColumnType("uuid");

            builder.Property(s => s.BranchId)
                   .IsRequired()
                   .HasColumnType("uuid");

            builder.Property(s => s.IsCancelled)
                   .IsRequired();

            builder.Ignore(s => s.Total);

            builder.HasOne(s => s.Customer)
                    .WithMany()
                    .HasForeignKey(s => s.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Branch)
                    .WithMany()
                    .HasForeignKey(s => s.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Items)
                   .WithOne()
                   .HasForeignKey(si => si.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
