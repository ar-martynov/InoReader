using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        IEnumerable<Tag> GetTags(int userId);
    }
}
