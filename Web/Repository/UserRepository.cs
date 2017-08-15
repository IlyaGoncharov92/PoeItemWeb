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
                var result = context.Users
                    .Include(x => x.Items.Select(y => y.ItemDetails))
                    .Select(x => new UserDetails
                    {
                        Id = x.Id,
                        Items = x.Items
                    })
                .ToList();
                
                return result;
            }
        }
    }
}