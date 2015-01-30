using System.Collections.Generic;
using System.Linq;
using System;

using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IMainContext context)
            : base(context) { }

        public User GetUser(User user)
        {
            user = _dbSet.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            if (user != null) return user;
            return null;
        }

        public User Registration(ref User user)
        {
            Insert(user);
            return user;
        }

        public bool IsUserValid(User user)
        {
            if (_dbSet.Any(x => x.UserName == user.UserName)) throw new Exception("Пользователь с таким мименем уже существует");
            else if (_dbSet.Any(x => x.Email == user.Email)) throw new Exception("E-mail уже используется");

            return true;
        }


        public RssCanal AddRssCanal(RssCanal canal, int userId)
        {
            GetByID(userId).RssCanals.Add(canal);
            _context.SaveChanges();
            return canal;
        }

        public Category AddCategory(Category newCategory)
        {
            if (_dbSet.Any(x => x.Categories.Any(c => c.Title == newCategory.Title) && x.UserId==newCategory.UserId))
                throw new Exception("Вы уже добавили данную категорию");

            var userToUpdate = GetByID(newCategory.UserId);
            userToUpdate.Categories.Add(newCategory);

            _context.SaveChanges();
            return newCategory;
        }

        public bool UnSubscribeRssCanal(int userId, RssCanal canal)
        {
            try
            {
                User currentUser = GetByID(userId);
                currentUser.RssCanals.Remove(canal);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
