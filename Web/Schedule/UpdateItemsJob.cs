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
        private readonly ItemRepository _itemRepository;
        private readonly ItemDetailRepository _itemDetailRepository;
        private readonly UserRepository _userRepository;

        public UpdateItemsJob()
        {
            _poeTradeHost = ConfigurationManager.AppSettings["PoeTradeHost"];
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
            var users = _userRepository.GetAll();

            foreach (var user in users)
            {
                var items = user.Items;
                var isCounNew = false; // Проверка - появились ли новые товары

                foreach (var item in items)
                {
                    try
                    {
                        var html = RestHelper.HttpGet(_poeTradeHost, item.UrlParams);

                        if (string.IsNullOrEmpty(html))
                            continue;

                        var oldDetails = item.ItemDetails.ToList();
                        var newDetails = GetDetails(html, item.Id);

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
                    PushHub.Send(user.Id);
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

        private List<ItemDetail> GetDetails(string html, int itemId)
        {
            var list = new List<ItemDetail>();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var elem = doc.GetElementbyId("search-results-first");
                if (elem != null)
                {
                    var htmlNodes = elem.ChildNodes.Where(x => x.Name == "tbody");

                    foreach (var htmlNode in htmlNodes)
                    {
                        var message = ItemDetailNodeHelper.GetMessage(htmlNode);

                        var liNodes = ItemDetailNodeHelper.GetLiNodes(htmlNode).ToList();

                        var itemDetail = new ItemDetail
                        {
                            TradeId = ItemDetailNodeHelper.GetItemId(htmlNode),
                            Price = ItemDetailNodeHelper.GetPrice(liNodes),
                            UserName = ItemDetailNodeHelper.GetUserName(liNodes),
                            IsVerified = false,
                            Message = message,
                            ItemId = itemId
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