using Domain.Abstract;

namespace Domain.Abstract
{
   
    public interface IUnitOfWork
    {
        ICategoryRepository CategoriesRepository { get; }
        ILinkRepository LinksRepository { get; }
        IRssCanalRepository RssCanalsRepository { get;}
        IRssNewRepository RssNewsRepository { get; }
        ITagRepository TagsRepository { get;}
        IUserRepository UsersRepository { get;}

        void Commit();
    }
}
