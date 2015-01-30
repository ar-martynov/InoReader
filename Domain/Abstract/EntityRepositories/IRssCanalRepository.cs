using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IRssCanalRepository : IGenericRepository<RssCanal>
    {
        RssCanal AddRssCanal(RssCanal canal, int userId);
        IEnumerable<Domain.Models.RssCanalModel> GetRssCanals(int userId);
        RssCanal GetRssCanalNews(int userId, int canalId, int page, int perPage, out int totalPages);
    }
}
