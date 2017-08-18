using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class UserDetails
    {
        public UserDetails()
        {
            Items = new List<Item>();
        }

        public string Id { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}