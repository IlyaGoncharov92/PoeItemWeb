using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Web.Helpers
{
    public class ItemDetailNodeHelper
    {
        public static string GetMessage(HtmlNode htmlNode)
        {
            var attributes = htmlNode.Attributes;

            var dataNameValue = attributes.FirstOrDefault(x => x.Name == "data-name")?.Value;
            var dataName = ReplaceApost(dataNameValue);
            var dataBuyout = attributes.FirstOrDefault(x => x.Name == "data-buyout")?.Value;
            var dataIgn = attributes.FirstOrDefault(x => x.Name == "data-ign")?.Value;
            var dataLeague = attributes.FirstOrDefault(x => x.Name == "data-league")?.Value;
            var dataTab = attributes.FirstOrDefault(x => x.Name == "data-tab")?.Value;
            var dataX_Value = attributes.FirstOrDefault(x => x.Name == "data-x")?.Value;
            var dataY_Value = attributes.FirstOrDefault(x => x.Name == "data-y")?.Value;

            var dataX = int.Parse(dataX_Value) + 1;
            var dataY = int.Parse(dataY_Value) + 1;

            var message = $"@{dataIgn} Hi, I would like to buy your {dataName} listed for {dataBuyout} in {dataLeague} (stash tab {dataTab}; position: left {dataX}, top {dataY})";

            return message;
        }

        private static string ReplaceApost(string str)
        {
            const string ch = "&#39;";

            return str.Replace(ch, @"'");
        }

        public static IEnumerable<HtmlNode> GetLiNodes(HtmlNode liNode)
        {
            var trNodes = liNode.ChildNodes.Where(x => x.Name == "tr");
            var trNode = trNodes.First(x => x.Attributes["class"].Value == "bottom-row");
            var ulNode = trNode.ChildNodes.FindFirst("ul");
            var liNodes = ulNode.ChildNodes.Where(x => x.Name == "li");
            return liNodes;
        }

        public static string GetUserName(List<HtmlNode> liNodes)
        {
            const string flag = "IGN: ";

            var text = liNodes[1].InnerText;

            var indexStart = text.IndexOf(flag, StringComparison.Ordinal);

            if (indexStart == -1)
                return string.Empty;

            var start = indexStart + flag.Length;

            var userName = text.Substring(start);

            return userName;
        }

        public static string GetPrice(HtmlNode htmlNode)
        {
            var price = htmlNode.Attributes
                            .FirstOrDefault(x => x.Name == "data-buyout")?.Value;
            return price ?? string.Empty;
        }

        public static string GetItemId(HtmlNode htmlNode)
        {
            var result = htmlNode.Attributes["class"].Value;
            return result ?? string.Empty;
        }

        public static string GetHtml(HtmlNode htmlNode)
        {
            var itemHtml = htmlNode.Descendants("ul")
                .FirstOrDefault(d => d.GetAttributeValue("class", "").Contains("item-mods"))?.InnerHtml;
            return itemHtml ?? string.Empty;
        }

        public static string GetTimeAge(HtmlNode htmlNode)
        {
            var result = htmlNode.Descendants("span")
                .FirstOrDefault(d => d.GetAttributeValue("class", "").Contains("found-time-ago"))?.InnerText;

            return result ?? string.Empty;
        }

        public static string GetWikiLink(HtmlNode htmlNode)
        {
            var result = htmlNode.Descendants("a")
                .FirstOrDefault(d => d.GetAttributeValue("class", "").Contains("wiki-link"))
                ?.Attributes["href"].Value;

            return result ?? string.Empty;
        }

        public static string GetItemName(HtmlNode htmlNode)
        {
            var test2 = htmlNode.ChildNodes.Where(x => x.Name == "tr");
            var test3 = test2?.FirstOrDefault(x => x.Attributes["class"].Value == "first-line");
            var test4 = test3?.ChildNodes.Where(x => x.Name == "td");
            var test5 = test4?.FirstOrDefault(x => x.Attributes["class"].Value == "item-cell");
            var result = test5?.Descendants("a").FirstOrDefault()?.InnerText;

            return result;
        }

        public static string GetImagePath(IEnumerable<HtmlNode> htmlNodes)
        {
            var test1 = htmlNodes.FirstOrDefault();
            var test2 = test1?.ChildNodes.Where(x => x.Name == "tr");
            var test3 = test2?.FirstOrDefault(x => x.Attributes["class"].Value == "first-line");
            var test4 = test3?.ChildNodes.Where(x => x.Name == "td");
            var test5 = test4?.FirstOrDefault(x => x.Attributes["class"].Value == "icon-td");
            var result = test5?.Descendants("img").FirstOrDefault()?.Attributes["src"].Value;

            return result;
        }
    }
}