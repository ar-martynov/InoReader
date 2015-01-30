using Domain.Entities;

namespace Domain.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetUser(User user);
        User Registration(ref User user);
        bool IsUserValid(User user);

        Category AddCategory(Category newCategory);
        RssCanal AddRssCanal(RssCanal canal, int userId);
        bool UnSubscribeRssCanal(int userId, RssCanal canalId);
    }
}
