using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Helpers;
using Web.Models;
using System.Data.Entity;
using EntityFramework.Utilities;

namespace Web.Repository
{
    public class ItemRepository
    {
        public void Save(Item item, string userId)
        {
            using (var context = new PoeContext())
            {
                var urlParams = UrlParseHelper.UrlParse(item.Url);
                if (string.IsNullOrEmpty(urlParams))
                    return;

                item.UrlParams = urlParams;
                item.UserId = userId;

                context.Items.Add(item);
                context.SaveChanges();
            }
        }

        public IEnumerable<Item> GetAll()
        {
            using (var context = new PoeContext())
            {
                return context.Items.ToList();
            }
        }

        public Item GetById(int id)
        {
            using (var context = new PoeContext())
            {
                var result = context.Items
                    .Include(x => x.ItemDetails)
                    .FirstOrDefault(x => x.Id == id);
                
                var details = context.ItemDetails
                    .Where(x => x.ItemId == id)
                    .OrderBy(x => x.IsVerified)
                    .Take(20);

                if(result != null)
                    result.ItemDetails = details.ToList();
                
                return result;
            }
        }

        public IEnumerable<Item> GetAllByUser(string userId)
        {
            using (var context = new PoeContext())
            {
                var result = context.Items
                    .Where(x => x.UserId == userId)
                    .ToList();

                return result;
            }
        }

        public void UpdateCountDetails(int id)
        {
            using (var context = new PoeContext())
            {
                var count = context.Items.Where(x => x.Id == id)
                    .Include(x => x.ItemDetails)
                    .SelectMany(x => x.ItemDetails)
                    .Select(x => !x.IsVerified)
                    .Count();

                var item = context.Items.FirstOrDefault(x => x.Id == id);

                item.CountNewDetails = count;

                context.Items.Attach(item);
                context.Entry(item).Property(x => x.CountNewDetails).IsModified = true;
                context.SaveChanges();

                //EFBatchOperation.For(context, context.Items)
                //    .Where(x => x.Id == id)
                //    .Update(x => x.CountNewDetails, x => count);
            }
        }

        public void UpdateWikiImage(int id, string imageHtml)
        {
            using (var context = new PoeContext())
            {
                var item = context.Items.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    item.ImageWikiHtml = imageHtml;

                    context.Items.Attach(item);
                    context.Entry(item).Property(x => x.ImageWikiHtml).IsModified = true;
                    context.SaveChanges();
                }
            }
        }

        public void Detele(int id)
        {
            using (var context = new PoeContext())
            {
                var item = context.Items.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    context.Items.Remove(item);
                    context.SaveChanges();
                }
            }
        }

        public void Verified(int itemId)
        {
            using (var context = new PoeContext())
            {
                var details = context.ItemDetails
                    .Where(x => x.ItemId == itemId)
                    .Where(x => !x.IsVerified)
                    .ToList();

                foreach (var detail in details)
                {
                    detail.IsVerified = true;
                    context.ItemDetails.Attach(detail);
                    context.Entry(detail).Property(x => x.IsVerified).IsModified = true;
                }

                var item = context.Items.FirstOrDefault(x => x.Id == itemId);
                if (item != null)
                {
                    item.CountNewDetails = 0;
                    context.Items.Attach(item);
                    context.Entry(item).Property(x => x.CountNewDetails).IsModified = true;
                }

                context.SaveChanges();
            }
        }
    }
}