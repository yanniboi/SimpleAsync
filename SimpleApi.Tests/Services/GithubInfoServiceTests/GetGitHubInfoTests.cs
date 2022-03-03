using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using RichardSzalay.MockHttp;
using ServiceLayer.Responses;
using Xunit;

namespace SimpleApi.Tests.Services.GithubInfoServiceTests
{
    public class GetGitHubInfoTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Ensure_if_name_is_empty_we_exit(string name)
        {
            var setup = GithubInfoServiceTestFactory.Build();
            var result = await setup.Sut.GetGitHubInfo(name);

            Assert.Equal(Status.BadRequest, result.Status);
            Assert.Equal("Name is required", result.Message);
        }

        [Fact]
        public async Task Ensure_if_we_get_a_bad_status_code_we_report()
        {
            const string name = "mkb";
            var setup = GithubInfoServiceTestFactory.Build();
            setup.Settings.BaseUrl = "https://test.com";
            setup.Handler.When(HttpMethod.Get, $"{setup.Settings.BaseUrl}/users/{name}")
                .Respond(HttpStatusCode.Forbidden);
            var result = await setup.Sut.GetGitHubInfo(name);

            Assert.Equal(Status.Error, result.Status);
            Assert.Equal("Call to Github info failed", result.Message);
        }

        [Fact]
        public async Task Ensure_if_error_happens_we_report() // we deserialse a empty response we re
        {
            const string name = "mkb";
            var setup = GithubInfoServiceTestFactory.Build();
            setup.Settings.BaseUrl = "https://test.com";
            setup.Handler.When(HttpMethod.Get, $"{setup.Settings.BaseUrl}/users/{name}").Respond(HttpStatusCode.OK);
            var result = await setup.Sut.GetGitHubInfo(name);

            Assert.Equal(Status.Error, result.Status);
            Assert.Equal("Catch happend", result.Message);
        }

        [Fact]
        public async Task Ensure_success() // we deserialse a empty response we re
        {
            const string name = "mkb";
            var setup = GithubInfoServiceTestFactory.Build();
            setup.Settings.BaseUrl = "https://test.com";
            var expectedResponse = new GitHubResponse
            {
                Login = "log",
                Location = "location"
            };
            setup.Handler.When(HttpMethod.Get, $"{setup.Settings.BaseUrl}/users/{name}").Respond("application/json",
                Newtonsoft.Json.JsonConvert.SerializeObject(expectedResponse));
            var result = await setup.Sut.GetGitHubInfo(name);

            Assert.Equal(Status.Ok, result.Status);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedResponse.Login, result.Data.Login);
            Assert.Equal(expectedResponse.Location, result.Data.Location); // might want to check more
        }
    }
}