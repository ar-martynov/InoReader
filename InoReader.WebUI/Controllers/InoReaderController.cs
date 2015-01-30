using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;

using AutoMapper;

using Domain;
using Domain.Abstract;
using Domain.Models;
using Domain.Entities;

namespace InoReader.WebUI.Controllers
{
    [Authorize]
    public class InoReaderController : Controller
    {
        private IUnitOfWork unitOfWork;
        private static int dataPerPage = 4;
        public InoReaderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index(bool? rssOnly)
        {
            var repositoryLinks = unitOfWork.LinksRepository;
            var repositoryCategories = unitOfWork.CategoriesRepository;

            int currentUser = CurrentUserId;
            int newsTotalPages;
            int linksTotalPages;
            IndexModel model;
            if (rssOnly != null)
            {

                ViewBag.Title = "Свежие RSS новости";

                model = new IndexModel()
                {
                    IsLinksEnabled = false,
                    IsRssNewsEnabled = true,
                    News = Mapper.Map<IEnumerable<RssNew>, List<RssNewsItemModel>>(unitOfWork.RssNewsRepository.GetTodayNews(currentUser, 0, dataPerPage, out newsTotalPages)),
                };
                
            }
            else
            {
                model = new IndexModel()
                {
                    IsLinksEnabled = true,
                    IsRssNewsEnabled = true,
                    News = Mapper.Map<IEnumerable<RssNew>, List<RssNewsItemModel>>(unitOfWork.RssNewsRepository.GetTodayNews(currentUser, 0, dataPerPage, out newsTotalPages)),

                    NewsCurrentPage = 0,
                    NewsTotalPages = newsTotalPages,

                    PagedLinks = new PagedLinksModel()
                    {
                        ReaderLinks = Mapper.Map<IEnumerable<Link>, List<LinkModel>>(repositoryLinks.GetAllUserLinks(currentUser, true, 0, dataPerPage, out linksTotalPages)),
                        ActionName = "IndexLinksContent",
                        CurrentPage = 0,
                        Order = true,
                        TotalPages = linksTotalPages,
                    }
                };

                foreach (var link in model.PagedLinks.ReaderLinks)
                {
                    link.CategoryName = repositoryCategories.GetCategoryName(link.CategoryId, currentUser);
                }
                ViewBag.Title = "Все ссылки и RSS подписки";
                if (Request.IsAjaxRequest()) return PartialView("_Index", model);
            }

            ViewData["NewsCurrentPage"] = 0;
            ViewData["NewsTotalPages"] = newsTotalPages;
            return View(model);
        }

        [HttpGet]
        public ActionResult IndexLinksContent(string itemName, bool order, int page)
        {
            var repositoryLinks = unitOfWork.LinksRepository;
            var repositoryCategories = unitOfWork.CategoriesRepository;

            int currentUser = CurrentUserId;
            int linksTotalPages;

            PagedLinksModel PagedLinks = new PagedLinksModel()
                {
                    ReaderLinks = Mapper.Map<IEnumerable<Link>, List<LinkModel>>(repositoryLinks.GetAllUserLinks(currentUser, order, page, dataPerPage, out linksTotalPages)),
                    ActionName = "IndexLinksContent",
                    CurrentPage = page,
                    Order = order,
                    TotalPages = linksTotalPages,
                };

            foreach (var link in PagedLinks.ReaderLinks)
            {
                link.CategoryName = repositoryCategories.GetCategoryName(link.CategoryId, currentUser);
            }
            ViewBag.Title = "Все ссылки и RSS подписки";

            if (Request.IsAjaxRequest())
                return PartialView("_Links", PagedLinks);

            return View("Links", PagedLinks);
        }

        [HttpGet]
        public ActionResult IndexRssNewsContent(int page)
        {
            ViewBag.Title = "Свежие RSS новости";
            int newsTotalPages;

            List<RssNewsItemModel> news =
                Mapper.Map<IEnumerable<RssNew>, List<RssNewsItemModel>>(
                unitOfWork.RssNewsRepository.GetTodayNews(CurrentUserId, page, dataPerPage, out newsTotalPages));
            ViewData["NewsCurrentPage"] = page;
            ViewData["NewsTotalPages"] = newsTotalPages;

            return PartialView("_RssTodayNews", news);
        }


        [HttpGet]
        public ActionResult AllCategories()
        {
            ViewBag.Title = "Все категории";

            var repositoryLinks = unitOfWork.LinksRepository;
            var repositoryCategories = unitOfWork.CategoriesRepository;

            AllLinksAndCategories model = new AllLinksAndCategories();
            model.CategoriesWithLinks = Mapper.Map<IEnumerable<Category>, List<CategoryModel>>(repositoryCategories.GetCategories(CurrentUserId));
            model.LinksWithoutCategory = Mapper.Map<Category, CategoryModel>(repositoryLinks.GetLinksByCategory(CurrentUserId, -1));
            foreach (var category in model.CategoriesWithLinks)
            {
                category.Links = category.Links.OrderBy(x => x.Title).ToList();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                Category newCategory = Mapper.Map<CategoryModel, Category>(category);

                try
                {
                    newCategory.UserId = CurrentUserId;
                    unitOfWork.UsersRepository.AddCategory(newCategory);
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CategoryError", ex.Message);
                    return PartialView(category);
                }

                return Json(new { success = true, categoryId = newCategory.CategoryId, categoryTitle = newCategory.Title });
            }
            return PartialView(category);
        }

        [HttpGet]
        public ActionResult DeleteCategory(int categoryId)
        {
            bool flag = unitOfWork.CategoriesRepository.DeleteCategory(categoryId, CurrentUserId);
            unitOfWork.Commit();

            return RedirectToAction("AllCategories", "InoReader");
        }

        [HttpGet]
        public ActionResult AddTag()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult AllTags()
        {
            ViewBag.Title = "Все тэги";
            return View(Mapper.Map<IEnumerable<Tag>, List<TagModel>>(unitOfWork.TagsRepository.GetTags(CurrentUserId)));
        }

        [HttpGet]
        public ActionResult SubscribeRss()
        {
            ViewBag.Title = "Добавление новой RSS подписки";
            return PartialView();
        }

        [HttpPost]
        public ActionResult SubscribeRss(RssCanalModel rssCanal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rssCanal = RssFeedReader.GetRss(rssCanal);
                    var newCanal = Mapper.Map<RssCanalModel, RssCanal>(rssCanal);

                    if (false)
                    {
                        throw new Exception("Ошибка разбора ресурса");
                    }

                    newCanal = unitOfWork.RssCanalsRepository.AddRssCanal(newCanal, CurrentUserId);

                    newCanal = unitOfWork.UsersRepository.AddRssCanal(newCanal, CurrentUserId);
                    unitOfWork.Commit();

                    string redirect = Url.Action("GetRss", new { itemId = newCanal.RssCanalId, page = 0 });

                    return Json(new { success = true, redirectLink = redirect });
                }
                catch 
                {
                    ModelState.AddModelError("RssError", "Канал не доступен или не является RSS");
                    return PartialView("_SubscribeRss", rssCanal);
                }
            }

            return PartialView("_SubscribeRss", rssCanal);
        }

        [HttpGet]
        public ActionResult GetRss(int itemId, int page)
        {
            int totalPages;
            RssCanalModel canal = Mapper.Map<RssCanal, RssCanalModel>(unitOfWork.RssCanalsRepository.GetRssCanalNews(CurrentUserId, itemId, page, dataPerPage, out totalPages));

            canal.TotalPages = totalPages;
            canal.CurrentPage = page;
            canal.ActionName = "GetRss";

            ViewBag.oldCanal = true;

            ViewBag.Title = canal.Title;

            if (Request.IsAjaxRequest()) return PartialView("_RssCanal", canal);
            return View("RssCanal", canal);


        }

        [HttpPost]
        public ActionResult UnSubscribeRss(int rssCanalId)
        {
            unitOfWork.UsersRepository.UnSubscribeRssCanal(CurrentUserId, unitOfWork.RssCanalsRepository.GetByID(rssCanalId));
            unitOfWork.Commit();

            return Json(new { success = true, redirectLink = Url.Action("Index", "InoReader", new { rssOnly = true }) });
        }

        [HttpGet]
        public ActionResult AddLink()
        {
            ViewBag.Title = "Добавление новой ссылки";

            NewLinkModel model = new NewLinkModel();
            model.Categories = Mapper.Map<List<Category>, List<CategoryModel>>(unitOfWork.CategoriesRepository.GetCategories(CurrentUserId).ToList());
            unitOfWork.Commit();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AddLink(NewLinkModel link)
        {
            var repositoryCategories = unitOfWork.CategoriesRepository;

            if (ModelState.IsValid)
            {
                List<Tag> tags = new List<Tag>();

                var repositoryLink = unitOfWork.LinksRepository;

                if (link.TagsInString != "" && link.TagsInString != null)
                {
                    foreach (var value in link.TagsInString.Split(','))
                        tags.Add(new Tag { Title = value.ToLower(), UserId = CurrentUserId });
                }

                Link newLink;

                try
                {
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.Default;
                    string source = client.DownloadString(link.Url);
                    link.Title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;

                    newLink = Mapper.Map<NewLinkModel, Link>(link);
                    newLink.UserId = CurrentUserId;
                    newLink.DateWhenAdded = DateTime.Now;

                    repositoryLink.AddLink(newLink, ref tags);

                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    link.Categories = Mapper.Map<List<Category>, List<CategoryModel>>(repositoryCategories.GetCategories(CurrentUserId).ToList());
                    ModelState.AddModelError("LinkError", ex.Message);
                    return PartialView("_AddLink", link);
                }

                string redirect;
                if (newLink.CategoryId == null)
                {
                    redirect = Url.Action("GetLinksByCategory", new { itemId = -1, itemName = "Без категории", order = true, page = 0 });
                    return Json(new { success = true, redirectLink = redirect });
                }

                string categoryName = repositoryCategories.GetCategoryName(newLink.CategoryId, CurrentUserId);
                redirect = Url.Action("GetLinksByCategory", new { itemId = (int)newLink.CategoryId, itemName = categoryName, order = true, page = 0 });

                return Json(new { success = true, redirectLink = redirect });
            }
            else
            {
                link.Categories = Mapper.Map<List<Category>, List<CategoryModel>>(repositoryCategories.GetCategories(CurrentUserId).ToList());
                return PartialView("_AddLink", link);
            }

        }
        [HttpGet]
        public ActionResult ModifyLink(int linkId)
        {
            var repositoryLinks = unitOfWork.LinksRepository;
            var repositoryCategories = unitOfWork.CategoriesRepository;

            NewLinkModel model = Mapper.Map<Link, NewLinkModel>(repositoryLinks.GetLink(linkId, CurrentUserId));
            model.Categories = Mapper.Map<List<Category>, List<CategoryModel>>(repositoryCategories.GetCategories(CurrentUserId).ToList());
            foreach (var tag in model.Tags) model.TagsInString += tag.Title + ",";
            ViewBag.Title = model.Title;
            return View(model);
        }

        [HttpPost]
        public ActionResult ModifyLink(NewLinkModel link)
        {
            var repositoryLinks = unitOfWork.LinksRepository;

            Link updatedLink = Mapper.Map<NewLinkModel, Link>(link);

            if (link.TagsInString != "" && link.TagsInString != null)
            {
                foreach (var value in link.TagsInString.Split(','))
                    updatedLink.Tags.Add(new Tag { Title = value.ToLower(), UserId = CurrentUserId });
            }
            updatedLink.UserId = CurrentUserId;

            repositoryLinks.ModifyLink(updatedLink);
            unitOfWork.Commit();

            return RedirectToAction("AllCategories","InoReader");

        }

        [HttpGet]
        public ActionResult DeleteLink(int linkId)
        {

            unitOfWork.LinksRepository.DeleteLink(linkId, CurrentUserId);
            unitOfWork.Commit();

            return RedirectToAction("AllCategories", "InoReader");
        }

        [HttpGet]
        public ActionResult GetLinksByCategory(int itemId, string itemName, bool order, int page)
        {
            int currentUser = CurrentUserId;
            int totalPages;


            PagedLinksModel model = new PagedLinksModel
            {
                ReaderLinks = Mapper.Map<IEnumerable<Link>, List<LinkModel>>(unitOfWork.LinksRepository.GetLinks(currentUser, itemId, null, order, page, dataPerPage, out totalPages)),
                TotalPages = totalPages,
                CurrentPage = page,
                ItemId = itemId,
                ActionName = "GetLinksByCategory",
                Order = order
            };

            foreach (var link in model.ReaderLinks)
            {
                link.CategoryName = itemName;
            }

            ViewBag.Title = itemName;

            if (Request.IsAjaxRequest())
                return PartialView("_Links", model);

            return View("Links", model);
        }

        [HttpGet]
        public ActionResult GetLinksByTag(int itemId, string itemName, bool order, int page)
        {
            int currentUser = CurrentUserId;
            int totalPages;

            PagedLinksModel model = new PagedLinksModel
            {
                ReaderLinks = Mapper.Map<IEnumerable<Link>, List<LinkModel>>(unitOfWork.LinksRepository.GetLinks(currentUser, null, itemId, order, page, dataPerPage, out totalPages)),
                TotalPages = totalPages,
                CurrentPage = page,
                ItemId = itemId,
                ActionName = "GetLinksByTag",
                Order = order
            };

            foreach (var link in model.ReaderLinks)
            {
                link.CategoryName = unitOfWork.CategoriesRepository.GetCategoryName(link.CategoryId, currentUser);
            }

            ViewBag.Title = itemName;

            if (Request.IsAjaxRequest())
                return PartialView("_Links", model);

            return View("Links", model);
        }

        [HttpGet]
        public ActionResult SearchLinks(string itemId, bool? order, int? page)
        {
            if (itemId == null || itemId == "")
                return View("Links", new PagedLinksModel() { ReaderLinks = new List<LinkModel>() });

            if (page == null && order == null)
            {
                page = 0;
                order = false;
            }

            int totalPages;
            int currentUser = CurrentUserId;

            PagedLinksModel model = new PagedLinksModel
            {
                ReaderLinks = Mapper.Map<IEnumerable<Link>, List<LinkModel>>(unitOfWork.LinksRepository.SearchLinks(currentUser, itemId, (bool)order, (int)page, dataPerPage, out totalPages)),
                TotalPages = totalPages,
                CurrentPage = (int)page,
                ItemId = itemId,
                ActionName = "SearchLinks",
                Order = (bool)order
            };

            foreach (var link in model.ReaderLinks)
            {
                link.CategoryName = unitOfWork.CategoriesRepository.GetCategoryName(link.CategoryId, currentUser);
            }

            ViewBag.Title = itemId;

            if (Request.IsAjaxRequest())
                return PartialView("_Links", model);

            return View("Links", model);
        }

        [HttpGet]
        public ActionResult ManagementMenu()
        {
            return View();
        }

        public ActionResult SideBar()
        {
            SideBarModel sideBar = new SideBarModel();
            sideBar.CategoriesList = Mapper.Map<IEnumerable<Category>, List<SimpleCategoryModel>>(unitOfWork.CategoriesRepository.GetCategories(CurrentUserId));
            sideBar.TagsList = Mapper.Map<IEnumerable<Tag>, List<TagModel>>(unitOfWork.TagsRepository.GetTags(CurrentUserId));
            sideBar.RssCanals = unitOfWork.RssCanalsRepository.GetRssCanals(CurrentUserId).ToList();
            return View(sideBar);
        }

        private int CurrentUserId
        {
            get
            {
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);

                int userId;
                if (int.TryParse(ticket.UserData, out userId))
                    return userId;
                else
                    return -1;
            }
        }
    }
}
