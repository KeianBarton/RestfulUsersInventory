using Microsoft.EntityFrameworkCore;
using RestfulUsersInventory.DataAccess.Entities;
using RestfulUsersInventory.DataAccess.EntityConfigurations;

namespace RestfulUsersInventory.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserItem> UserItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ItemConfiguration());
            builder.ApplyConfiguration(new ItemTypeConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserItemConfiguration());

            base.OnModelCreating(builder);
        }

    }
}
