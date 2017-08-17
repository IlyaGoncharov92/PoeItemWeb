using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Quartz;
using Web.Helpers;
using Web.Hubs;
using Web.Models;
using Web.Repository;

namespace Web.Schedule
{
    public class UpdateItemsJob : IJob
    {
        private readonly string _poeTradeHost;
        private readonly string _poeWikiHost;
        private readonly ItemRepository _itemRepository;
        private readonly ItemDetailRepository _itemDetailRepository;
        private readonly UserRepository _userRepository;

        public UpdateItemsJob()
        {
            _poeTradeHost = ConfigurationManager.AppSettings["PoeTradeHost"];
            _poeWikiHost = ConfigurationManager.AppSettings["PoeWikiHost"];
            _itemRepository = new ItemRepository();
            _itemDetailRepository = new ItemDetailRepository();
            _userRepository = new UserRepository();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                Logger.Log.Error(e);
            }
        }

        public void Start()
        {
            Logger.Log.Debug("TESET");
            var users = _userRepository.GetAll();

            foreach (var user in users)
            {
                var items = user.Items;
                var isCounNew = false; // Проверка - появились ли новые товары

                foreach (var item in items)
                {
                    try
                    {
                        var oldDetails = new List<ItemDetail>();
                        try
                        {
                            oldDetails = item.ItemDetails.ToList();
                        }
                        catch (Exception e)
                        {
                        }

                        var newDetails = GetDetails(item);

                        var wikiLink = newDetails.FirstOrDefault()?.WikiLink;
                        UpdateWikiImage(item, wikiLink);
                        
                        oldDetails = UpdateOlDetails(oldDetails, newDetails);
                        newDetails = UpdateNewDetails(oldDetails, newDetails);

                        _itemDetailRepository.AddRange(newDetails);

                        _itemDetailRepository.UpdateAllDelete(oldDetails);

                        _itemRepository.UpdateCountDetails(item.Id);

                        if (newDetails.Count > 0)
                        {
                            isCounNew = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(e);
                    }
                }

                if (isCounNew)
                {
                    PushHub.Send(user.Id, isCounNew);
                }
            }

            _itemDetailRepository.DeleteItemDetails();
        }

        private List<ItemDetail> UpdateNewDetails(List<ItemDetail> oldDetails, List<ItemDetail> newDetails)
        {
            var list = new List<ItemDetail>();

            foreach (var detail in newDetails)
            {
                var res = oldDetails.FirstOrDefault(x => x.TradeId == detail.TradeId);
                if (res == null)
                {
                    list.Add(detail);
                }
            }
            return list;
        }

        private List<ItemDetail> UpdateOlDetails(List<ItemDetail> oldDetails, List<ItemDetail> newDetails)
        {
            foreach (var detail in oldDetails)
            {
                var res = newDetails.FirstOrDefault(x => x.TradeId == detail.TradeId);
                if (res == null)
                {
                    detail.DeleteNumber += 1;
                }
            }

            return oldDetails;
        }

        private void UpdateWikiImage(Item item, string wikiLink)
        {
            try
            {
                if (string.IsNullOrEmpty(item.ImageWikiHtml) && !string.IsNullOrEmpty(wikiLink))
                {
                    var urlParams = UrlParseHelper.UrlWikiParse(wikiLink);

                    var html = RestHelper.HttpGet(_poeWikiHost, urlParams);

                    if (string.IsNullOrEmpty(html))
                        return;

                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var cotainer = doc.GetElementbyId("mw-content-text");

                    var elem = cotainer.Descendants("span")
                        .FirstOrDefault(d => d.GetAttributeValue("class", "").Contains("infobox-page-container"));

                    var elem2 = elem?.FirstChild.InnerHtml;

                    _itemRepository.UpdateWikiImage(item.Id, elem2);
                }

            }
            catch (Exception e)
            {
                Logger.Log.Error(e);
            }
        }

        private List<ItemDetail> GetDetails(Item item)
        {
            var list = new List<ItemDetail>();

            try
            {
                var html = RestHelper.HttpGet(_poeTradeHost, item.UrlParams);

                if (string.IsNullOrEmpty(html))
                    return list;

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var elem = doc.GetElementbyId("search-results-first");
                if (elem != null)
                {
                    var htmlNodes = elem.ChildNodes.Where(x => x.Name == "tbody");
                    
                    foreach (var htmlNode in htmlNodes)
                    {
                        var liNodes = ItemDetailNodeHelper.GetLiNodes(htmlNode).ToList(); 

                        var itemDetail = new ItemDetail
                        {
                            TradeId = ItemDetailNodeHelper.GetItemId(htmlNode),
                            Price = ItemDetailNodeHelper.GetPrice(htmlNode),
                            UserName = ItemDetailNodeHelper.GetUserName(liNodes),
                            ItemHtml = ItemDetailNodeHelper.GetHtml(htmlNode),
                            Message = ItemDetailNodeHelper.GetMessage(htmlNode),
                            TimeAgo = ItemDetailNodeHelper.GetTimeAge(htmlNode),
                            WikiLink = ItemDetailNodeHelper.GetWikiLink(htmlNode),
                            ItemName = ItemDetailNodeHelper.GetItemName(htmlNode),
                            IsVerified = false,
                            ItemId = item.Id
                        };

                        list.Add(itemDetail);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(e);
            }

            return list;
        }
    }
}