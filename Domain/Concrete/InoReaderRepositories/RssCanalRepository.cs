using System;
using System.Collections.Generic;
using System.Linq;

using Domain.Abstract;
using Domain.Concrete;
using Domain.Context;
using Domain.Entities;

namespace Domain.Concrete
{
    public class RssCanalRepository : GenericRepository<RssCanal>, IRssCanalRepository
    {
        public RssCanalRepository(IMainContext context)
            : base(context) { }

        public RssCanal AddRssCanal(RssCanal canal, int userId)
        {
            if (_dbSet.Any(x => x.Users.Select(u => u.UserId).Contains(userId) && x.Url == canal.Url))
                throw new Exception("Вы уже подписаны на канал");

            var dbCanal = _dbSet.FirstOrDefault(x => x.Url == canal.Url);
            if (dbCanal != null)
                return dbCanal;
            return canal;
        }

        public IEnumerable<Domain.Models.RssCanalModel> GetRssCanals(int userId)
        {
            return _dbSet.Where(x => x.Users.Select(u => u.UserId).Contains(userId))
                                        .Select(canal => new Domain.Models.RssCanalModel
                                            {
                                                RssCanalId = canal.RssCanalId,
                                                Title = canal.Title,
                                                Url = canal.Url
                                            });
        }

        public RssCanal GetRssCanalNews(int userId, int canalId, int page, int perPage, out int totalPages)
        {
            UserRepository users = new UserRepository(this._context);
            var flag = users.GetByID(userId).RssCanals.Any(x => x.Users.Select(u => u.UserId).Contains(userId) && x.RssCanalId == canalId);
            if (!flag)
                throw new Exception("Вы не подписаны на канал");
            
            var canal = _dbSet.FirstOrDefault(x => x.Users.Select(u => u.UserId).Contains(userId) && x.RssCanalId == canalId);
            

            totalPages = (int)Math.Ceiling((decimal)canal.RssNews.Count() / perPage);
            page = (page > totalPages) ? totalPages : page;

            canal.RssNews = canal.RssNews
                .OrderByDescending(x => x.PublishDate)
                .Skip(page * perPage)
                .Take(perPage).ToList();

            return canal;
        }
    }
}
