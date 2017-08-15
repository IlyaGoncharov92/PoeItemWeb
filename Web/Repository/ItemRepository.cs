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
    }
}