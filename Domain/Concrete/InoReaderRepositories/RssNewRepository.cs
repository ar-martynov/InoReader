using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;

using Domain.Abstract;
using Domain.Entities;


namespace Domain.Concrete
{
    public class RssNewRepository : GenericRepository<RssNew>, IRssNewRepository
    {
        public RssNewRepository(IMainContext context)
            : base(context) { }


        public IEnumerable<RssNew> GetTodayNews(int userId, int page, int perPage, out int totalPages)
        {
            //DateTimeOffset today = DateTimeOffset.UtcNow;
            //DateTimeOffset afternoon = DateTimeOffset.UtcNow.AddDays(-1);

            //var news = _dbSet.Where(x => x.RssCanal.Users.Select(u => u.UserId).Contains(userId) && x.PublishDate > afternoon && x.PublishDate < today);
            var news = _dbSet.Where(x => x.RssCanal.Users.Select(u => u.UserId).Contains(userId));

            totalPages = (int)Math.Ceiling((decimal)news.Count() / perPage);
            page = (page > totalPages) ? totalPages : page;

            return news.OrderByDescending(x => x.PublishDate).Skip(page * perPage).Take(perPage);
        }
    }
}
