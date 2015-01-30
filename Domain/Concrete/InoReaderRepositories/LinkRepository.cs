using System.Collections.Generic;
using System.Linq;
using System;

using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class LinkRepository : GenericRepository<Link>, ILinkRepository
    {
        public LinkRepository(IMainContext context)
            : base(context) { }

        public Link AddLink(Link newLink, ref List<Tag> newTags)
        {
            if (_dbSet.Any(x => x.Url == newLink.Url && x.UserId == newLink.UserId))
                throw new Exception("Вы уже добавили данный ресурс");

            TagRepository tags = new TagRepository(this._context);

            foreach (var tag in newTags)
            {
                var dbTag = tags.Get(x => x.Title == tag.Title && x.UserId == tag.UserId);

                if (dbTag.Count() == 0) newLink.Tags.Add(tag);
                else newLink.Tags.Add(dbTag.First());
            }
            _dbSet.Add(newLink);
            _context.SaveChanges();

            return newLink;
        }

        public Link GetLink(int linkId, int userId)
        {
            return _dbSet.FirstOrDefault(x => x.LinkId == linkId && x.UserId == userId);
        }

        public bool ModifyLink(Link link)
        {
            var dbLink = _dbSet.FirstOrDefault(x => x.UserId == link.UserId && x.LinkId == link.LinkId);
            dbLink.Tags.Clear();

            TagRepository tags = new TagRepository(this._context);

            foreach (var tag in link.Tags)
            {
                var dbTag = tags.Get(x => x.Title == tag.Title && x.UserId == tag.UserId);

                if (dbTag.Count() > 0)
                {
                    dbLink.Tags.Add(dbTag.First());
                }
                else dbLink.Tags.Add(tag);
            }

            _context.Entry(dbLink).CurrentValues.SetValues(link);


            return true;
        }

        public Category GetLinksByCategory(int userId, int categoryId)
        {
            if (categoryId >= 0)
                return new Category
                {
                    CategoryId = categoryId,
                    Links = _dbSet.Where(x => x.CategoryId == categoryId && x.UserId == userId).OrderBy(x => x.Title).ToList()
                };

            return new Category
            {
                CategoryId = -1,

                Links = _dbSet.Where(x => x.CategoryId == null && x.UserId == userId).OrderBy(x => x.Title).ToList()
            };
        }

        public IEnumerable<Link> GetLinks(int userId, int? categoryId, int? tagId, bool orderByDate, int page, int perPage, out int totalPages)
        {
            IEnumerable<Link> links;

            //if order == false return links ordered by name, else by date
            if (orderByDate)
            {
                if (categoryId != null)
                {
                    if (categoryId < 0)
                    {
                        links = _dbSet.Where(x => x.UserId == userId && x.CategoryId == null);

                        totalPages = (int)Math.Ceiling((decimal)links.Count() / perPage);
                        page = (page > totalPages) ? totalPages : page;

                        return links
                            .OrderByDescending(x => x.DateWhenAdded)
                            .Skip(page * perPage).Take(perPage);
                    }

                    links = _dbSet.Where(x => x.UserId == userId && x.CategoryId == categoryId);

                    totalPages = (int)Math.Ceiling((decimal)links.Count() / perPage);
                    page = (page > totalPages) ? totalPages : page;

                    return links
                        .OrderByDescending(x => x.DateWhenAdded)
                        .Skip(page * perPage).Take(perPage);
                }

                if (tagId != null)
                {
                    links = _dbSet.Where(x => x.Tags.Any(t => t.UserId == userId && t.TagId == tagId));

                    totalPages = (int)Math.Ceiling((decimal)links.Count() / perPage);
                    page = (page > totalPages) ? totalPages : page;

                    return links
                        .OrderByDescending(x => x.DateWhenAdded)
                        .Skip(page * perPage).Take(perPage);
                }
            }

            //Order By Name
            if (categoryId != null)
            {
                if (categoryId < 0)
                {
                    links = _dbSet.Where(x => x.UserId == userId && x.CategoryId == null);

                    totalPages = (int)Math.Ceiling((decimal)links.Count() / perPage);
                    page = (page > totalPages) ? totalPages : page;

                    return links
                        .OrderBy(x => x.Title)
                        .Skip(page * perPage).Take(perPage);
                }

                links = _dbSet.Where(x => x.UserId == userId && x.CategoryId == categoryId);

                totalPages = (int)Math.Ceiling((decimal)links.Count() / perPage);
                page = (page > totalPages) ? totalPages : page;

                return links
                    .OrderBy(x => x.Title)
                    .Skip(page * perPage).Take(perPage);
            }

            if (tagId != null)
            {
                links = _dbSet.Where(x => x.Tags.Any(t => t.TagId == tagId && t.UserId == userId));

                totalPages = (int)Math.Ceiling((decimal)links.Count() / perPage);
                page = (page > totalPages) ? totalPages : page;

                return links
                    .OrderBy(x => x.Title)
                    .Skip(page * perPage).Take(perPage);
            }

            throw new Exception("links selection by no tags and no category not implemented yet");
        }

        public IEnumerable<Link> GetAllUserLinks(int userId, bool orderByDate, int page, int perPage, out int totalPages)
        {
            var links = _dbSet.Where(x => x.UserId == userId);

            totalPages = (int)Math.Ceiling((decimal)links.Count() / perPage);
            page = (page > totalPages) ? totalPages : page;

            if (orderByDate)
                return links.OrderByDescending(x => x.DateWhenAdded)
                            .Skip(page * perPage).Take(perPage);

            return links.OrderBy(x => x.Title)
                        .Skip(page * perPage).Take(perPage);

        }

        public IEnumerable<Link> SearchLinks(int userId, string searcString, bool orderByDate, int page, int perPage, out int totalPages)
        {
            var links = _dbSet.Where(x => (
                               x.Tags.Any(t => t.Title.ToLower() == searcString && t.UserId == userId) ||
                               x.Category.Title.ToLower().Contains(searcString) && x.UserId == userId ||
                               x.Title.Contains(searcString) && x.UserId == userId));


            totalPages = (int)Math.Ceiling((decimal)links.Count() / perPage);
            page = (page > totalPages) ? totalPages : page;

            if (orderByDate)
                return links
                      .OrderByDescending(x => x.DateWhenAdded)
                      .Skip(page * perPage)
                      .Take(perPage);

            return links
                   .OrderBy(x => x.Title)
                   .Skip(page * perPage).Take(perPage);
        }

        public bool DeleteLink(int linkId, int userId)
        {
            Link link = _dbSet.FirstOrDefault(x => x.UserId == userId && x.LinkId == linkId);

            if (link != null)
            {
                if (link.CategoryId != null)
                    link.CategoryId = null;

                _dbSet.Remove(link);
                _context.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
