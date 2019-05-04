using System.Collections.Generic;

namespace RestfulUsersInventory.DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<UserItem> Items { get; set; }
    }
}
