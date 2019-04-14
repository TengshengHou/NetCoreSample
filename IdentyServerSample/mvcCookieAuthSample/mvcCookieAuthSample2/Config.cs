using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace mvcCookieAuthSample
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api1","API Application")
            };
        }

        public static IEnumerable<Client> GetClient()
        {
            return new List<Client>()
            {
              new Client(){
                    ClientId="mvc",
                    ClientName="mvc client",
                     ClientUri="http://localhost:5001",
                     LogoUri="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTY_6tPDEd_cbogLHLoQXdEg1P3jKR_Ad56lmWv3TItiHtNAjLvLA",
                     AllowedGrantTypes=GrantTypes.HybridAndClientCredentials,
                     ClientSecrets={
                        new Secret("secret".Sha256())
                    },
                     AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris={ "http://localhost:5001/signin-oidc"},
                    PostLogoutRedirectUris={ "http://localhost:5001/signout-callback-oidc"},
                    //AlwaysIncludeUserClaimsInIdToken=true,//设置此项IdToken 时带入用户信息 否则需要用AccessToken 调用指定接口换取用户信息（）
                    
                    
                    AllowedScopes={
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    },

                    AllowRememberConsent=true,
                    RequireConsent=true,
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser(){
                     Username="admin",
                     Password="123456",
                     SubjectId="1000",
                     Claims=new List<Claim>{
                         new Claim("name","hts"),
                         new Claim("webSite","hts92.com")
                     }
                }
            };
        }
        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
               new IdentityResources.OpenId(),
               new IdentityResources.Profile(),
               new IdentityResources.Email(),
            };
        }
    }
}
