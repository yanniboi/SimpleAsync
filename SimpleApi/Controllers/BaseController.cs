using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Responses;

namespace SimpleApi.Controllers
{
    public class BaseController : ControllerBase
    {
     
        protected static IActionResult HandleResponse(ServiceResponse response,
            HttpStatusCode successDefaultStatus = HttpStatusCode.OK)
        {
            var obResult = new ObjectResult(response);
            switch (response.Status)
            {
                case Status.BadRequest:
                    obResult.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                case Status.Error:
                    obResult.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
                case Status.Ok:
                    obResult.StatusCode = (int) successDefaultStatus;
                    break;
                default:
                    throw new Exception();
            }

            return obResult;
        }
    }
}