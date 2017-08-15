using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Web.Models;
using Web.Repository;

namespace Web.Hubs
{
    public class MyHub1 : Hub
    {
        private readonly UserConnectRepository _connectRepository;

        public MyHub1()
        {
            _connectRepository = new UserConnectRepository();
        }

        public override Task OnConnected()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var connectionId = Context.ConnectionId;

            _connectRepository.AddConnect(userId, connectionId);

            Clients.All.Test("message test");

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;

            _connectRepository.DeleteConnect(connectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}