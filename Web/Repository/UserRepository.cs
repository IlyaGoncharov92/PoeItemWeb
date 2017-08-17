using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Web.Models;

namespace Web.Repository
{
    public class UserRepository
    {
        public List<UserDetails> GetAll()
        {
            using (var context = new PoeContext())
            {
                var users = context.Users
                    .Select(x => new UserDetails
                    {
                        Id = x.Id,
                        Items = x.Items
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
    }
}