//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using Microsoft.Owin.Security.Infrastructure;

//namespace Web.Providers
//{
//    public class AuthTokenProvider : IAuthenticationTokenProvider
//    {
//        public void Create(AuthenticationTokenCreateContext context)
//        {
//            throw new NotImplementedException();
//        }

//        public void Receive(AuthenticationTokenReceiveContext context)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task CreateAsync(AuthenticationTokenCreateContext context)
//        {
//            var tokenService = context.OwinContext.GetAutofacLifetimeScope().Resolve<ITokenService>();

//            var userId = context.OwinContext.Get<long>("as:userId");

//            var isNew = false;
//            var token = tokenService.GetByMemberId(userId);

//            if (token == null)
//            {
//                isNew = true;
//                token = new TokenDetails
//                {
//                    Id = Protect(Guid.NewGuid().ToString("n")),
//                    IssuedUtc = DateTime.UtcNow,
//                    ExpiresUtc = DateTime.UtcNow.AddDays(2),
//                    MemberId = userId
//                };
//            }

//            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
//            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

//            if (isNew)
//            {
//                token.ProtectedTicket = context.SerializeTicket();
//                tokenService.Add(token);
//            }
//            context.SetToken(Unprotect(token.Id));
//        }

//        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
//        {
//            var tokenService = context.OwinContext.GetAutofacLifetimeScope().Resolve<ITokenService>();

//            var hashedTokenId = Protect(context.Token);

//            var token = tokenService.GetById(hashedTokenId);

//            if (token != null)
//            {
//                context.DeserializeTicket(token.ProtectedTicket);
//            }
//        }

//        public static string Protect(string text)
//        {
//            return Encryptor.Encrypt(text);
//        }

//        public static string Unprotect(string text)
//        {
//            return Encryptor.Decrypt(text);
//        }
//    }
//}