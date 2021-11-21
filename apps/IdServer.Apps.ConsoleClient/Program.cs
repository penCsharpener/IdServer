// discover endpoints from metadata
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using static IdentityModel.OidcConstants;

namespace IdServer.Apps.ConsoleClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        var program = new Program();
        //await program.RequestFromAppsApi_Client();
        await program.RequestFromAppsApi_User();
    }

    private async Task RequestFromAppsApi_Client()
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5881");
        if (disco.IsError)
        {
            Console.WriteLine(disco.Error);
            return;
        }

        // request token
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,

            ClientId = "api",
            ClientSecret = "secret_api",
            Scope = "appsApi"
        });

        if (tokenResponse.IsError)
        {
            Console.WriteLine(tokenResponse.Error);
            return;
        }

        Console.WriteLine(tokenResponse.Json);

        // call api
        var apiClient = new HttpClient();
        apiClient.SetBearerToken(tokenResponse.AccessToken);

        var response = await apiClient.GetAsync("https://localhost:7260/identity");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.StatusCode);
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));
        }
    }

    private async Task RequestFromAppsApi_User()
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5881");
        if (disco.IsError)
        {
            Console.WriteLine(disco.Error);
            return;
        }

        // request token
        //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        //{
        //    Address = disco.TokenEndpoint,

        //    ClientId = "api.user",
        //    ClientSecret = "secret_api",
        //    Scope = "appsApi",
        //});
        var tokenResponse = await client.RequestPasswordTokenAsync(new()
        {
            Address = disco.TokenEndpoint,
            ClientId = "api.user",
            Scope = "appsApi",
            UserName = "penCsharpener",
            Password = "pwd",
            GrantType = GrantTypes.Password
        });

        if (tokenResponse.IsError)
        {
            Console.WriteLine(tokenResponse.Error);
            return;
        }

        Console.WriteLine(tokenResponse.Json);

        // call api
        var apiClient = new HttpClient();
        apiClient.SetBearerToken(tokenResponse.AccessToken);

        var response = await apiClient.GetAsync("https://localhost:7260/identity");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.StatusCode);
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));
        }
    }
}
