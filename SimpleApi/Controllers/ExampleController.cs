using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using SimpleApi.Requests;

namespace SimpleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : BaseController
    {
        private readonly IGithubUserInfo _githubUserInfoService;

        private readonly ILogger<ExampleController> _logger;

        public ExampleController(ILogger<ExampleController> logger, IGithubUserInfo githubUserInfoService)
        {
            _githubUserInfoService = githubUserInfoService;
            _logger = logger;
        }

        [HttpGet("GithubInfo")] // can do explict routes
        public async Task<IActionResult> Get([FromQuery] string name)
        {
            var result = await _githubUserInfoService.GetGitHubInfo(name);
            return HandleResponse(result);
        }

        [HttpGet("GithubInfo2/{name}")] 
        public async Task<IActionResult> GetByRoute(string name)
        {
            var result = await _githubUserInfoService.GetGitHubInfo(name);
            return HandleResponse(result);
        }

        // [HttpPatch] 
        // [HttpPut]   other options
        // [HttpDelete] 
        [HttpPost] // with no route will just be contoller 
        public async Task<IActionResult> PostExample([FromBody] ExamplePostRequest model)
        {
            var result = await _githubUserInfoService.GetGitHubInfo(model.Name);
            return HandleResponse(result);
        }
    }
}