using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.Testing
{
    public class InMemoryResources
    {
        public static readonly List<ApiResource> ApiResources = new List<ApiResource>();
        public static readonly List<IdentityResource> IdentityResources = new List<IdentityResource>();
        public static readonly List<Client> Clients = new List<Client>();
        public static List<TestUser> Users => IdentityServer4.Quickstart.UI.TestUsers.Users;
    }
}
