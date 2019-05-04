using RestfulUsersInventory.DataQueries.Queries;

namespace RestfulUsersInventory.DataQueries
{
    public class QueryHelper : IQueryHelper
    {
        public IItemQueries ItemQueries { get; }
        public IUserQueries UserQueries { get; }
        public IUserInventoryQueries UserInventoryQueries { get; }
    }
}
