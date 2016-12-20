using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace InfotraficParser
{
    public class PageCrawler
    {
        private readonly HtmlWeb _web;
        private readonly string _pageUrl;
        private int _pageNumber;

        public PageCrawler(string pageUrl)
        {
            _pageUrl = pageUrl;

            _web = new HtmlWeb();
        }

        public IEnumerable<ContentItem> ReadContentItems(out int itemsCount)
        {
            var links = GetLinksInPage(_pageNumber).ToArray();
            itemsCount = links.Length;

            if (itemsCount == 0)
            {
                _pageNumber = 0; //reset
                return Enumerable.Empty<ContentItem>();
            }

            _pageNumber++;

            // get items with deferred execution for GetLinkContent()
            return links.Select((link, index) => new ContentItem(link) { Content = GetLinkContent(link) });
        }

        private IEnumerable<string> GetLinksInPage(int pageNo)
        {
            var pageDoc = _web.Load($"http://{_pageUrl}&page={pageNo}");
            if (pageDoc?.DocumentNode == null)
            {
                // problem
                return Enumerable.Empty<string>();
            }

            const string xPath = "//*[@class=\"contentList\"]/*[@class=\"boxInfotrafic\"]/*[@class=\"boxInfoRight\"]/h3/a";
            var nodes = pageDoc.DocumentNode.SelectNodes(xPath);
            if (nodes == null || !nodes.Any())
            {
                // problem
                return Enumerable.Empty<string>();
            }

            return nodes.Select(node => node.GetAttributeValue("href", "(no address)"));
        }

        private IEnumerable<string> GetLinkContent(string link)
        {
            var dayPage = _web.Load(link);
            var tags = dayPage.DocumentNode.SelectSingleNode("//*[@class=\"descDetaliiObiecte\"]").ChildNodes.Where(item => !item.Name.StartsWith("#"));
            if (!tags.Any())
            {
                return Enumerable.Empty<string>();
            }

            return tags.Select(item => GetCleanValue(item.InnerText));
        }

        private string GetCleanValue(string input)
        {
            return HttpUtility.HtmlDecode(input.Replace(Environment.NewLine, string.Empty).Trim());
        }
    }
}