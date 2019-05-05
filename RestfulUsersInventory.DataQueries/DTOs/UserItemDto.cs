using System;
using System.Collections.Generic;
using System.Text;

namespace RestfulUsersInventory.DataQueries.DTOs
{
    public class UserItemDto
    {
        public const int MaximumNumberOfAnyItemAllowed = 50;

        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemCount { get; set; }
    }
}
