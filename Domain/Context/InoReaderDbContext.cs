using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Domain.Entities;

using Domain.Abstract;

namespace Domain.Context
{
    public class InoReaderDbContext : DbContext, IMainContext
    {
        private readonly Dictionary<Type, Func<object>> dbSets;

        public InoReaderDbContext() : base("InoReaderDbContext")
        {
             dbSets = new Dictionary<Type, Func<object>> {
                    {typeof (Category), () => base.Set<Category>()},
                    {typeof (Link), () => base.Set<Link>()},
                    {typeof (RssCanal), () => base.Set<RssCanal>()},
                    {typeof (RssNew), () => base.Set<RssNew>()},
                    {typeof (Tag), () => base.Set<Tag>()},
                    {typeof (User), () => base.Set<User>()}
            };
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<RssCanal> RssCanals { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RssNew> RssNews { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            if (!this.dbSets.ContainsKey(typeof(TEntity)))
                throw new Exception("База не содержит такой таблицы");

            return this.dbSets[typeof(TEntity)]() as DbSet<TEntity>;
        }

        //http://stackoverflow.com/questions/14489676/entity-framework-how-to-solve-foreign-key-constraint-may-cause-cycles-or-multi/25513748#25513748
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
