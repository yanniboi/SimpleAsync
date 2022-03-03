using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceLayer.Responses;
using ServiceLayer.Settings;

namespace ServiceLayer
{
    public class GithubInfoService : IGithubUserInfo // might add more interfaces on here later or not
    {
        private readonly HttpClient _client;
        private readonly GitHubSettings _settings;

        public GithubInfoService(HttpClient client, GitHubSettings settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task<ServiceResponse<GitHubResponse>> GetGitHubInfo(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return new ServiceResponse<GitHubResponse> {Status = Status.BadRequest, Message = $"Name is required"}; // might even want to load messages for config so can be swapped out per region
                }
            
                var request = new HttpRequestMessage() {
                    RequestUri = new Uri($"{_settings.BaseUrl.TrimEnd('/')}/users/{name}"),
                    Method = HttpMethod.Get,
                };
                request.Headers.Add("User-Agent","MyApp"); // required
                var result = await _client.SendAsync(request);
                if (!result.IsSuccessStatusCode)
                {
                    // might want to add a logger in here and serialize entire reponse and save it for checking later
                    return new ServiceResponse<GitHubResponse> {Status = Status.Error, Message = $"Call to Github info failed"};
                }

                var json = await result.Content.ReadAsStringAsync();

                var ob =  Newtonsoft.Json.JsonConvert.DeserializeObject<GitHubResponse>(json);

                return new ServiceResponse<GitHubResponse> {Data = ob, Status = Status.Ok};
            }
            catch (Exception e)
            {
                // might want to log exception
               // and throw or return bad status
               
               return new ServiceResponse<GitHubResponse> {Status = Status.Error, Message = "Catch happend"};     // might not want the exception message might contain senstive info if being returned to user calling
            }
    
        }
    }
}