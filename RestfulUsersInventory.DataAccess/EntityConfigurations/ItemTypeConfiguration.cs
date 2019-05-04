using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestfulUsersInventory.DataAccess.Entities;

namespace RestfulUsersInventory.DataAccess.EntityConfigurations
{
    public class ItemTypeConfiguration : IEntityTypeConfiguration<ItemType>
    {
        public void Configure(EntityTypeBuilder<ItemType> builder)
        {
            // Property configurations
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Relationship configurations
            builder.HasMany(it => it.Items)
                .WithOne(it => it.Type)
                .IsRequired();
        }
    }
}
