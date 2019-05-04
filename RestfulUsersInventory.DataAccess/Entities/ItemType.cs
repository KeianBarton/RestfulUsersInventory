using System.Collections.Generic;

namespace RestfulUsersInventory.DataAccess.Entities
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<Item> Items { get; set; }
    }
}
