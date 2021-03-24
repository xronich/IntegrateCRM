using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IntegrateCRM.Controllers
{
    public abstract class AppControllerBase : ControllerBase
    {
        protected BadRequestObjectResult BadRequestResponse(string message, string code)
        {
            return base.BadRequest(new Dictionary<string, string>() { { "Message", message }, { "Code", code } });
        }

        protected string GetRemoteIpAddress()
        {
            return Request.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
