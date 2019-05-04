using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestfulUsersInventory.DataAccess.Entities;

namespace RestfulUsersInventory.DataAccess.EntityConfigurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            // Property configurations
            builder.HasKey(x => x.Id);

            builder.HasAlternateKey(x => x.Name); // Ensure name uniqueness

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.Weight)
                .IsRequired();

            // Relationship configurations
            builder.HasOne(i => i.Type)
                .WithMany(it => it.Items)
                .HasForeignKey(i => i.ItemTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
