using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EntityFramework.Utilities;
using Web.Models;

namespace Web.Repository
{
    public class ItemDetailRepository
    {
        private const int _deleteMaxCount = 30;

        public void AddRange(List<ItemDetail> items)
        {
            using (var context = new PoeContext())
            {
                try
                {
                    EFBatchOperation.For(context, context.ItemDetails).InsertAll(items);
                }
                catch (Exception e)
                {
                    Logger.Log.Error(e);
                    AddRangeEF(items);
                }
            }
        }

        public void UpdateAllDelete(List<ItemDetail> items)
        {
            using (var context = new PoeContext())
            {
                try
                {
                    EFBatchOperation.For(context, context.ItemDetails)
                        .UpdateAll(items, x => x.ColumnsToUpdate(c => c.DeleteNumber));
                }
                catch (Exception e)
                {
                    Logger.Log.Error(e);
                    UpdateAllDeleteEF(items);
                }
            }
        }

        public void DeleteItemDetails()
        {
            using (var context = new PoeContext())
            {
                try
                {
                    EFBatchOperation.For(context, context.ItemDetails)
                        .Where(x => x.DeleteNumber > _deleteMaxCount)
                        .Delete();
                }
                catch (Exception e)
                {
                    Logger.Log.Error(e);
                    DeleteItemDetailsEF();
                }
            }
        }

        //--------------- EF ----------------

        public void UpdateAllDeleteEF(List<ItemDetail> items)
        {
            using (var context = new PoeContext())
            {
                foreach (var item in items)
                {
                    context.ItemDetails.Attach(item);
                    context.Entry(item).Property(x => x.DeleteNumber).IsModified = true;
                }
                context.SaveChanges();
            }
        }

        public void AddRangeEF(List<ItemDetail> items)
        {
            using (var context = new PoeContext())
            {
                context.ItemDetails.AddRange(items);
                context.SaveChanges();
            }
        }

        public void DeleteItemDetailsEF()
        {
            using (var context = new PoeContext())
            {
                var items = context.ItemDetails.Select(x => new
                {
                    Id = x.Id,
                    DeleteNumber = x.DeleteNumber
                })
                .ToList();
                
                foreach (var item in items)
                {
                    if (item.DeleteNumber >= _deleteMaxCount)
                    {
                        var itemDetail = new ItemDetail { Id = item.Id };
                        context.Entry(itemDetail).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}