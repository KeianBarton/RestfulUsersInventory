namespace RestfulUsersInventory.DataQueries.DTOs
{
    public class UserDto
    {
        public const int MaximumNumberOfAnyItemTypeAllowed = 50;
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
