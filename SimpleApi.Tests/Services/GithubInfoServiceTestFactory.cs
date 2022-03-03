using System.Net.Http;
using RichardSzalay.MockHttp;
using ServiceLayer;
using ServiceLayer.Settings;

namespace SimpleApi.Tests.Services
{
    // You might want to use something like this https://github.com/mkbmain/ClassContext/blob/main/MoqClassContext.cs  << which can do most of this dynmically for you but overkill for this example
    public class GithubInfoServiceTestFactory
    {
        public static GithubInfoServiceTestBase Build()
        {
            var handler = new MockHttpMessageHandler();
            var client = new HttpClient(handler);
            var settings = new GitHubSettings();
            return new GithubInfoServiceTestBase
            {
                Handler = handler,
                Client = client,
                Sut = new GithubInfoService(client, settings),
                Settings = settings
            };
        }
    }

    public class GithubInfoServiceTestBase
    {
        public HttpClient Client { get; set; }
        public MockHttpMessageHandler Handler { get; set; }
        public GithubInfoService Sut { get; set; }
        public GitHubSettings Settings { get; set; }
    }
}