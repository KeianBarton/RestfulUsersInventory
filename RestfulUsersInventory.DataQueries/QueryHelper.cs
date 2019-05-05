using AutoMapper;
using RestfulUsersInventory.DataAccess;
using RestfulUsersInventory.DataQueries.Queries;

namespace RestfulUsersInventory.DataQueries
{
    public class QueryHelper : IQueryHelper
    {
        public IItemQueries ItemQueries { get; }
        public IUserQueries UserQueries { get; }
        public IUserItemQueries UserItemQueries { get; }

        public QueryHelper(ApplicationDbContext context, IMapper mapper)
        {
            ItemQueries = new ItemQueries(context, mapper);
            UserQueries = new UserQueries(context, mapper);
            UserItemQueries = new UserItemQueries(context, mapper);
        }
    }
}
