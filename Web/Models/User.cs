using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class User
    {
        public User()
        {
            UserConnections = new List<UserConnection>();
            Items = new List<Item>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public IEnumerable<UserConnection> UserConnections { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}