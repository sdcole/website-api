using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourneyAPI.Model;

namespace TourneyAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/main")]
    public class MainController : Controller
    {


        [HttpPost]
        [Route("post")]
        [EnableCors("DefaultPolicy")]
        public JsonResult addUser([FromBody] UserEntry userModel)
        {
            var user = userModel;

            return new JsonResult(user.AddUser()) ;
        }


    }
}
