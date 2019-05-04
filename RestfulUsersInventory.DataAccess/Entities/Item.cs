using System.Collections.Generic;

namespace RestfulUsersInventory.DataAccess.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemTypeId { get; set; }
        public decimal Value { get; set; }
        public double Weight { get; set; }

        public virtual ItemType Type { get; set; }
        public virtual IEnumerable<UserItem> Users { get; set; }
    }
}
