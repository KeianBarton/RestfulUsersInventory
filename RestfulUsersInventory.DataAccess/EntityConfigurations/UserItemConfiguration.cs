using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestfulUsersInventory.DataAccess.Entities;

namespace RestfulUsersInventory.DataAccess.EntityConfigurations
{
    public class UserItemConfiguration : IEntityTypeConfiguration<UserItem>
    {
        public void Configure(EntityTypeBuilder<UserItem> builder)
        {
            // Property configurations
            builder.HasKey(ui => ui.Id);

            // Relationship configurations
            builder.HasOne(ui => ui.Item)
                   .WithMany(i => i.Users)
                   .HasForeignKey(ui => ui.ItemId);

            builder.HasOne(ui => ui.User)
                   .WithMany(i => i.Items)
                   .HasForeignKey(ui => ui.UserId);
        }
    }
}
