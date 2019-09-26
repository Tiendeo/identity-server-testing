using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace IdentityServer.Testing
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "Web Api")
                {
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };

        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    UserClaims = { "role" }
                },
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "api",
                        "openid",
                        "profile",
                        "roles",
                        "email"
                    }
                },
                 new Client
                {
                    ClientId = "mvcvue",
                    ClientName = "MVC VueJS Client",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "http://localhost:8657/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:8657/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        "api",
                        "openid",
                        "profile",
                        "roles",
                        "email"
                    },
                    //AllowOfflineAccess = true
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                }
            };
        }
    }
}
