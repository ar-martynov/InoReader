using System.Collections.Generic;
using System.Linq;

using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(IMainContext context)
            : base(context) { }

        public IEnumerable<Tag> GetTags(int userId)
        {
            return _dbSet.Where(x => x.UserId == userId).OrderBy(x => x.Title);
        }
    }
}
