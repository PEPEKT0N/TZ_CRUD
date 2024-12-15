using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TZ_CRUD_app.Api
{
    [Route("api")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        // GET /api
        [HttpGet]
        public StringMessage Root()
        {
            return new StringMessage(Message: "Server is running");
        }
    }
}
