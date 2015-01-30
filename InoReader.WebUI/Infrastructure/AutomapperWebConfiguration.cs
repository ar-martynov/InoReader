using AutoMapper;
using Domain.Models;
using Domain.Entities;
using System.ServiceModel.Syndication;

namespace InoReader.WebUI
{
    public static class AutomapperWebConfiguration 
    {
        public static void InitializeMappingRules()
        {

#region Account
            Mapper.CreateMap<LoginModel, User>();
            Mapper.CreateMap<RegistrationModel, User>();
#endregion

#region InoReader
            Mapper.CreateMap<Category, CategoryModel>();
            Mapper.CreateMap<CategoryModel, Category>();
            Mapper.CreateMap<Category, SimpleCategoryModel>();

            Mapper.CreateMap<NewLinkModel, Link>();
            Mapper.CreateMap<Link,NewLinkModel>();
            Mapper.CreateMap<Link, LinkModel>();

            Mapper.CreateMap<TagModel, Tag>();
            Mapper.CreateMap<Tag, TagModel>();

            Mapper.CreateMap<RssCanalModel, RssCanal>();
            Mapper.CreateMap<RssCanal, RssCanalModel>();

            Mapper.CreateMap<RssNew, RssNewsItemModel>();
            Mapper.CreateMap<RssNewsItemModel, RssNew>();
#endregion
        }

    }
}
