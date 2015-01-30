using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

using Domain.Entities;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Context;

using Moq;
using Ninject;
using Domain;

namespace InoReader.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //kernel.Bind<IReaderRepository>().To<InoReaderRepository>();

            kernel.Bind<IMainContext>().To<InoReaderDbContext>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();

            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<ILinkRepository>().To<LinkRepository>();
            kernel.Bind<IRssCanalRepository>().To<RssCanalRepository>();
            kernel.Bind<IRssNewRepository>().To<RssNewRepository>();
            kernel.Bind<ITagRepository>().To<TagRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
        }
    }
}