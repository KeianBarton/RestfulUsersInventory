using RestfulUsersInventory.DataQueries.Queries;

namespace RestfulUsersInventory.DataQueries
{
    public interface IQueryHelper
    {
        IItemQueries ItemQueries { get; }
        IUserQueries UserQueries { get; }
        IUserItemQueries UserItemQueries { get; }
    }
}
