using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdServer.Web
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>()
            {
                new("testApi", "Test Api") { Scopes = new List<string> { "testApi" } },
                new("appsApi", "Apps Api") { Scopes = new List<string> { "appsApi" } }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new()
                {
                    ClientId = "client",
                    AllowedGrantTypes = new[] { GrantType.ClientCredentials },
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedScopes = { "testApi" }
                },
                new()
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = new[] { GrantType.ResourceOwnerPassword },
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedScopes = { "testApi" }
                },
                new()
                {
                    ClientId = "api.user",
                    AllowedGrantTypes = new[] { GrantType.ResourceOwnerPassword },
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedScopes = { "appsApi" }
                },
                new()
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris = { "https://localhost:7169/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:7169/signout-callback-oidc"},

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new()
                {
                    ClientId = "swaggerapiui",
                    ClientName = "Swagger API UI",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris = { "http://localhost:59337/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "http://localhost:59337/swagger"},

                    AllowedScopes = { "bankOfDotNetApi" },
                    AllowAccessTokensViaBrowser = true
                },
                new()
                {
                    ClientId = "api",
                    ClientName = "API Client",
                    ClientSecrets = new [] { new Secret("secret_api".Sha256()) },
                    AllowedGrantTypes = new[] { GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },

                    //RedirectUris = { "https://localhost:7260/signin-oidc" },
                    //PostLogoutRedirectUris = { "https://localhost:7260/signout-callback-oidc"},

                    AllowedScopes = new List<string> { "appsApi" }
                },
                new()
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("supersecret".ToSha256()) },
                    ClientName = "MVC Client 2",
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:7169/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7169/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7169/signout-callback-oidc"},

                    AllowOfflineAccess = true,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "appsApi"
                    },
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = true,
                },
            };
        }

        public static IEnumerable<ApiScope> GetScopes()
        {
            return new ApiScope[]
            {
                new("testApi", "Test Api"),
                new("appsApi", "Apps Api")
            };
        }

        internal static List<TestUser> GetTestUsers()
        {
            return new()
            {
                new()
                {
                    SubjectId = "1",
                    Username = "penCsharpener",
                    IsActive = true,
                    Password = "pwd",
                    ProviderName = "",
                    ProviderSubjectId = "1",
                    Claims = new List<Claim>()
                }
            };
        }
    }
}
