using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Web.Hubs;
using Web.Models;
using Web.Schedule;

[assembly: OwinStartup(typeof(Web.Startup))]

namespace Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {/*
            app.UseWebApi(new HttpConfiguration());
            app.UseCors(CorsOptions.AllowAll);

            var options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(15),
                Provider = new ApplicationOAuthProvider(),
                RefreshTokenProvider = new AuthTokenProvider()
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            */

            GlobalHost.DependencyResolver.Register(typeof(MyHub1), () => new MyHub1());
            app.MapSignalR();
            
            Task.Run(() => JobScheduler.Start());
        }
    }
}