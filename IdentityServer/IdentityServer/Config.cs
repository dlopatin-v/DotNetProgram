using Duende.IdentityServer.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
                {
                    new ApiScope("catalogapi")
                };
        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource("catalogapi")
            {
                Scopes = new List<string> { "catalogapi.read", "catalogapi.write"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                UserClaims = new List<string> {"role"}
            }
        };
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "catalog_api_swagger",
                    ClientName = "Swagger UI for Catalog service",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    RequirePkce = false,
                    RedirectUris =
                    {
                        "https://localhost:7007/swagger/oauth2-redirect.html"
                    },
                    AllowedScopes = {
                        "catalogapi"
                    }
                },
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,                    

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "catalogapi" }
                }
            };

    }
}