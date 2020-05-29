# identity-server-testing

You can configure the startup ApiResources, Clients and Users via environment variables:

```yaml
version: "3.5"

services:
  identityserver:
    image: tiendeo/identity-server-testing
    environment:
      DefaultApiResources__0: "apiresource"
      DefaultClients__testclient__Secret: "changeit"
      DefaultClients__testclient__RedirectUris__0: "http://localhost:5001/api-docs/oauth2-redirect.html"
      DefaultClients__testclient__Scopes__0: "apiresource"
      Users__admin__Password: "changeit"
      Users__admin__Roles__0: "apiresource-rw"
```

You can also add more complex api resources, identity resources or clients and they will be mapped to the objects `IdentityServer4.Models.ApiResource`, `IdentityServer4.Models.IdentityResource` or `IdentityServer4.Models.Client` respectively.

```yaml
version: "3.5"

services:
  identityserver:
    image: tiendeo/identity-server-testing
    environment:
      ApiResources__0__Name: "apiresource"
      ApiResources__0__DisplayName: "Api Resource"

      Clients__0__ClientId: "changeit"
      Clients__0__ClientSecrets__0__Value: "changeit"
      Clients__0__AllowedGrantTypes__0: "implicit"
      Clients__0__RedirectUris__0: "http://localhost:5001/api-docs/oauth2-redirect.html"
      Clients__0__AllowedScopes__0: "apiresource"
      Clients__0__AllowedScopes__1: "openid"
      Clients__0__AllowedScopes__2: "roles"
      Clients__0__AllowedScopes__3: "guid"
      Clients__0__AllowedScopes__4: "profile"
```

Or with an `appsettings.json` file:

```json
{
  "ApiResources": [
    {
      "Name": "api",
      "DisplayName": "Api",
      "Scopes": [
        {
          "Name": "api"
        }
      ],
      "UserClaims": [ "role" ]
    }
  ],
  "Clients": [
    {
      "ClientId": "api",
      "AllowedGrantTypes": [ "implicit" ],
      "AllowAccessTokensViaBrowser": true,
      "ClientSecrets": [
        {
          "Value": "changeit"
        }
      ],
      "RedirectUris": [
        "https://localhost:5001/api-docs/oauth2-redirect.html"
      ],
      "AllowedScopes": [ "openid", "profile", "roles", "guid", "api" ]
    }
  ],
  "Users": {
    "admin": {
      "Password": "changeit",
      "Roles": [ "api-rw" ]
    }
  }
}
```