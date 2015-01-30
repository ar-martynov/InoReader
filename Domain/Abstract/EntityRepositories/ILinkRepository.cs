using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface ILinkRepository : IGenericRepository<Link>
    {
        Link AddLink(Link newLink, ref List<Tag> newTags);
        Link GetLink(int linkId, int userId);
        bool ModifyLink(Link link);
        Category GetLinksByCategory(int userId, int categoryId);
        IEnumerable<Link> GetLinks(int userId, int? categoryId, int? tagId, bool orderByDate, int page, int perPage, out int totalPages);
        IEnumerable<Link> GetAllUserLinks(int userId, bool orderByDate, int page, int perPage, out int totalPages);
        IEnumerable<Link> SearchLinks(int userId, string searcString, bool orderByDate, int page, int perPage, out int totalPages);
        bool DeleteLink(int linkId, int userId);
    }
}
