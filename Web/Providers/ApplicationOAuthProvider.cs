//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Web;
//using Microsoft.Owin.Security.OAuth;

//namespace Web.Providers
//{
//    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
//    {
//        protected IMemberService MemberService { get; set; }

        

//        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
//        {
//            context.Validated();
//            return base.ValidateClientAuthentication(context);
//        }

//        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
//        {
//            InitializeServices(context);

//            var member = MemberService.GetByEmail(context.UserName);

//            if (member == null)
//            {
//                context.SetError("invalid_credentials", "User name or password is incorrect");
//                return base.GrantResourceOwnerCredentials(context);
//            }

//            if (SaltedHash.VerifyHashString(context.Password, member.Password, member.Salt))
//            {
//                //
//            }
//            else
//            {
//                context.SetError("invalid_grant", "User name or password is incorrect.");
//                return base.GrantResourceOwnerCredentials(context);
//            }

//            context.OwinContext.Set("as:userId", member.Id);

//            var claims = GetClaims(member);
//            var identity = new ClaimsIdentity(claims, context.Options.AuthenticationType);

//            context.Validated(identity);

//            return base.GrantResourceOwnerCredentials(context);
//        }

//        private static List<Claim> GetClaims(MemberDetails member)
//        {
//            return new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
//                new Claim(ClaimTypes.Email, member.Email)
//            };
//        }
//    }
//}