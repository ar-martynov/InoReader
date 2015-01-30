using System;
using System.Collections.Generic;
using System.Linq;

using Domain.Context;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private IMainContext context;

        private ICategoryRepository categoriesRepository;
        private ILinkRepository linksRepository;
        private IRssCanalRepository rssCanalsRepository;
        private IRssNewRepository rssNewsRepository;
        private ITagRepository tagsRepository;
        private IUserRepository usersRepository;

        
        public UnitOfWork(IMainContext context)
        {
            this.context = context;
        }



        public ICategoryRepository CategoriesRepository
        {
            get
            {
                if (this.categoriesRepository == null)
                {
                    this.categoriesRepository = new CategoryRepository(context);
                }
                return categoriesRepository;
            }
        }

        public ILinkRepository LinksRepository
        {
            get
            {
                if (this.linksRepository == null)
                {
                    this.linksRepository = new LinkRepository(context);
                }
                return linksRepository;
            }

        }

        public IRssCanalRepository RssCanalsRepository
        {
            get
            {
                if (this.rssCanalsRepository == null)
                {
                    this.rssCanalsRepository = new RssCanalRepository(context);
                }
                return rssCanalsRepository;
            }
        }

        public IRssNewRepository RssNewsRepository
        {
            get
            {
                if (this.rssNewsRepository == null)
                {
                    this.rssNewsRepository = new RssNewRepository(context);
                }
                return rssNewsRepository;
            }
        }

        public ITagRepository TagsRepository
        {
            get
            {
                if (this.tagsRepository == null)
                {
                    this.tagsRepository = new TagRepository(context);
                }
                return tagsRepository;
            }
        }

        public IUserRepository UsersRepository
        {
            get
            {
                if (this.usersRepository == null)
                {
                    this.usersRepository = new UserRepository(context);
                }
                return usersRepository;
            }
        }
    
        public void Commit()
        {
 	        context.SaveChanges();
        }

        
    }
}
