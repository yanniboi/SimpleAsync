using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ServiceLayer.Responses;

namespace ServiceLayer
{
    public class GithubInfoFromFile : IGithubUserInfo // might add more interfaces on here later or not
    {
        public async Task<ServiceResponse<GitHubResponse>> GetGitHubInfo(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return new ServiceResponse<GitHubResponse>
                    {
                        Status = Status.BadRequest, Message = $"Name is required"
                    }; // might even want to load messages for config so can be swapped out per region
                }

                var loc = Assembly.GetEntryAssembly().Location;
                var fileInfo = new FileInfo(loc).Directory;
                var json = await System.IO.File.ReadAllTextAsync(fileInfo + "/text.json");

                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<GitHubResponse>(json);
                return new ServiceResponse<GitHubResponse>
                {
                    Data = response,
                };
            }
            catch (Exception e)
            {
                // might want to log exception
                // and throw or return bad status

                return new ServiceResponse<GitHubResponse>
                {
                    Status = Status.Error, Message = "Catch happend"
                }; // might not want the exception message might contain senstive info if being returned to user calling
            }
        }
    }
}