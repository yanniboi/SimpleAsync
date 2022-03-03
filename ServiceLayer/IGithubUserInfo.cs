using System.Threading.Tasks;
using ServiceLayer.Responses;

namespace ServiceLayer
{
    public interface IGithubUserInfo
    {
        Task<ServiceResponse<GitHubResponse>> GetGitHubInfo(string name);
    }
}