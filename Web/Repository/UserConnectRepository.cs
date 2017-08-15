using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Repository
{
    public class UserConnectRepository
    {
        public void AddConnect(string userId, string connectionId)
        {
            using (var context = new PoeContext())
            {
                var connect = new UserConnection
                {
                    ConnectionId = connectionId,
                    UserId = userId
                };

                context.UserConnections.Add(connect);
                context.SaveChanges();
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            using (var context = new PoeContext())
            {
                var result = context.UserConnections
                    .Where(x => x.UserId == userId)
                    .Select(x => x.ConnectionId)
                    .ToList();

                return result;
            }
        }

        public void DeleteConnect(string connectionId)
        {
            using (var context = new PoeContext())
            {
                var entity = context.UserConnections.FirstOrDefault(x => x.ConnectionId == connectionId);

                if (entity != null)
                {
                    context.UserConnections.Remove(entity);
                    context.SaveChanges();
                }
            }
        }
    }
}