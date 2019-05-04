namespace RestfulUsersInventory.DataAccess.Entities
{
    public class UserItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }

        public virtual User User { get; set; }
        public virtual Item Item { get; set; }
    }
}
