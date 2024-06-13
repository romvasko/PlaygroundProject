using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("web-api"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)

            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>()
            {
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
                new ApiResource("web-api-resource", "web-api-resource")
                {
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    Scopes=
                    {
                        "web-api"
                    }
                }
            };
        public static IEnumerable<Client> Clients =>
             new List<Client> 
             {
                new Client
                {
                    ClientId = "web-api",
                    ClientName = "web-api",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
                    .Append(OidcConstants.GrantTypes.RefreshToken).ToList(),
                    RequireClientSecret = false,
                    ClientSecrets = { new Secret("my_secret".Sha256()) },
                    RedirectUris =
                    {
                        "http://localhost:5003/callback.html"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:5003"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:5003/index.html"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                        "web-api"
                    },


                }
             };
    }
}
