using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerCenter
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api","myApi")
            };
        }

        public static IEnumerable<Client> GetClient()
        {
            return new List<Client>()
            {
                new Client(){
                    ClientId="client",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets={
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={ "api"}
                },
                 new Client(){
                    ClientId="pwdclient",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    ClientSecrets={
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={ "api"}
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
                     SubjectId="1"
                }
            };
        }
    }
}
