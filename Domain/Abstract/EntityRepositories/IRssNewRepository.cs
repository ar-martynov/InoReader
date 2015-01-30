using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Abstract
{
    public interface IRssNewRepository : IGenericRepository<RssNew>
    {
        IEnumerable<RssNew> GetTodayNews(int userId, int page, int perPage, out int totalPages);
    }
}
