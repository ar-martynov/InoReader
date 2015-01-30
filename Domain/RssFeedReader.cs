using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;

using Domain.Models;

namespace Domain
{
    public static class RssFeedReader
    {
        private static bool IsFeedExist(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        public static RssCanalModel GetRss(RssCanalModel canal)
        {
            if (IsFeedExist(canal.Url))
            {
                return GetCanalWithNews(canal.Url);
            }

            throw new Exception("Такой канал не существует или в данный момент не работает");
        }

        public static RssCanalModel GetCanalWithNews(string url)
        {
            RssCanalModel rss = new RssCanalModel();
            SyndicationFeed feed;

            using (XmlReader reader = XmlReader.Create(url))
            {
                feed = SyndicationFeed.Load(reader);
                reader.Close();
            }

            rss.RssNews = GetNewsFromCanal(feed);
            rss.Title = feed.Title.Text;
            rss.Url = url;

            return rss;
        }

        public static List<RssNewsItemModel> GetNewsFromCanal(SyndicationFeed feed)
        {
            List<RssNewsItemModel> news = new List<RssNewsItemModel>();

            foreach (SyndicationItem item in feed.Items)
            {
                //item.Copyright = (feed.Copyright == null) ? new TextSyndicationContent(feed.Generator) : new TextSyndicationContent(feed.Copyright.Text);
                item.Id = (item.Id == null) ? item.Links[0].Uri.ToString() : item.Id;

                news.Add( new RssNewsItemModel() {
                                    PublishDate = item.PublishDate,
                                    Url = item.Id,
                                    Title = item.Title.Text,
                                    Text = item.Summary.Text
                                });
            }
            return news;
        }
    }
}
