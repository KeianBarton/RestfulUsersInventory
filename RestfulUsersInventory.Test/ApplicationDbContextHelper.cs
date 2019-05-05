using Microsoft.EntityFrameworkCore;
using RestfulUsersInventory.DataAccess;
using System;

namespace RestfulUsersInventory.Test
{
    public static class ApplicationDbContextHelper
    {
        public static ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Ensures new databases are created for every test
                .Options;
            return new ApplicationDbContext(options);
        }
    }
}
