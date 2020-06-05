using IdentityModel;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer.Testing
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConfigurationApiResources(this IApplicationBuilder app, IConfiguration configuration)
        {
            var apiResources = new List<ApiResource>();
            configuration.GetSection("ApiResources").Bind(apiResources);
            InMemoryResources.ApiResources.AddRange(apiResources);

            var defaultApiResourcesSection = configuration.GetSection("DefaultApiResources");

            if (defaultApiResourcesSection.Exists())
            {
                InMemoryResources.ApiResources.AddRange(defaultApiResourcesSection.GetChildren()
                    .Select(resourceName =>
                    {
                        var apiResource = new ApiResource(resourceName.Value);
                        apiResource.UserClaims = new List<string> { "role" };

                        return apiResource;
                    }));
            }

            if (!InMemoryResources.ApiResources.Any())
            {
                InMemoryResources.ApiResources.Add(new ApiResource("api", "Web Api")
                {
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                });
            }

            return app;
        }
        
        public static IApplicationBuilder UseConfigurationIdentityResources(this IApplicationBuilder app, IConfiguration configuration)
        {
            var identityResources = new List<IdentityResource>();
            configuration.GetSection("IdentityResources").Bind(identityResources);
            InMemoryResources.IdentityResources.AddRange(identityResources);

            if (!InMemoryResources.IdentityResources.Any())
            {
                InMemoryResources.IdentityResources.Add(new IdentityResources.OpenId());
                InMemoryResources.IdentityResources.Add(new IdentityResources.Profile());
                InMemoryResources.IdentityResources.Add(new IdentityResources.Email());
                InMemoryResources.IdentityResources.Add(new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    UserClaims = { "role" }
                });

            }

            return app;
        }
        
        public static IApplicationBuilder UseConfigurationClients(this IApplicationBuilder app, IConfiguration configuration)
        {
            var clients = new List<Client>();
            configuration.GetSection("Clients").Bind(clients);
            InMemoryResources.Clients.AddRange(clients);
            
            var defaultClientsSection = configuration.GetSection("DefaultClients");

            if (defaultClientsSection.Exists())
            {
                InMemoryResources.Clients.AddRange(defaultClientsSection.GetChildren()
                    .Select(section =>
                    {
                        var client = new Client();
                        client.ClientId = section.Key;
                        client.AllowedGrantTypes = GrantTypes.Code;
                        client.RequirePkce = true;
                        client.AllowAccessTokensViaBrowser = true;
                        client.RequireClientSecret = false;
                        client.ClientSecrets = new List<Secret> { new Secret(section.GetValue("Secret", "changeit")) };
                        client.RedirectUris = section.GetSection("RedirectUris").GetChildren().Select(host => host.Value).ToList();
                        client.PostLogoutRedirectUris = section.GetSection("PostLogoutRedirectUris").GetChildren().Select(host => host.Value).ToList();
                        client.AllowedCorsOrigins = section.GetSection("AllowedCorsOrigins").GetChildren().Select(host => host.Value).ToList();
                        client.AllowedScopes = new List<string> { "openid", "profile", "roles", "guid" };

                        if (section.GetSection("Scopes").Exists())
                        {
                            foreach (var scope in section.GetSection("Scopes").GetChildren())
                            {
                                client.AllowedScopes.Add(scope.Value);
                            }
                        }

                        return client;
                    }));
            }

            if (!InMemoryResources.Clients.Any())
            {
                InMemoryResources.Clients.Add(new Client
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
                });
            }

            return app;
        }
        
        public static IApplicationBuilder UseConfigurationTestUsers(this IApplicationBuilder app, IConfiguration configuration)
        {
            var usersSection = configuration.GetSection("Users");

            if (usersSection.Exists())
            {
                IdentityServer4.Quickstart.UI.TestUsers.Users.AddRange(usersSection.GetChildren()
                    .Select(section =>
                    {
                        var givenName = section.GetValue("GivenName", "TestName");
                        var familyName = section.GetValue("Surname", "TestSurname");
                        var name = section.GetValue("Name", $"{givenName} {familyName}");

                        var testUser = new IdentityServer4.Test.TestUser()
                        {
                            Username = section.Key,
                            Password = section.GetValue("Password", "changeit"),
                            IsActive = true,
                            SubjectId = section.GetValue("SubjectId", Guid.NewGuid().ToString()),
                            Claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Name, name),
                                new Claim(JwtClaimTypes.GivenName, givenName),
                                new Claim(JwtClaimTypes.FamilyName, familyName),
                                new Claim(JwtClaimTypes.Email, section.GetValue("email", "testuser@example.com"))
                            }
                        };

                        var rolesSection = section.GetSection("Roles");

                        if (rolesSection.Exists())
                        {
                            foreach (var role in rolesSection.GetChildren())
                            {
                                testUser.Claims.Add(new Claim(JwtClaimTypes.Role, role.Value, "role"));
                            }
                        }

                        return testUser;
                    }));
            }

            return app;
        }
    }
}
