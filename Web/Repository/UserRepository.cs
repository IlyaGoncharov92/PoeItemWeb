using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Web.Models;

namespace Web.Repository
{
    public class UserRepository
    {
        public List<UserDetails> GetAll()
        {
            try
            {
                using (var context = new PoeContext())
                {
                    var test = context.Items.ToList();

                    var test2 = context.Users.Include(x => x.Items).ToList();

                    var test4 = context.Users
                        .Include(x => x.Items)
                        .Select(x => new
                        {
                            Id = x.Id,
                            Items = x.Items
                        })
                        .ToList();

                    var users = context.Users
                        .Select(x => new UserDetails
                        {
                            Id = x.Id,
                            Items = x.Items.ToList()
                        })
                    .ToList();

                    var details = context.ItemDetails.ToList();

                    foreach (var user in users)
                    {
                        foreach (var item in user.Items)
                        {
                            item.ItemDetails = details.Where(x => x.ItemId == item.Id).ToList();
                        }
                    }

                    return users;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}