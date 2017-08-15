using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Web.Repository;

namespace Web.Hubs
{
    public static class PushHub
    {
        private static readonly IHubContext HubContext;
        private static readonly UserConnectRepository ConnectRepository;

        static PushHub()
        {
            HubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
            ConnectRepository = new UserConnectRepository();
        }

        public static void Send(string userId)
        {
            var connectIds = ConnectRepository.GetUserConnections(userId);

            foreach (var connectId in connectIds)
            {
                HubContext.Clients.Client(connectId).Update();
            }
        }
    }
}