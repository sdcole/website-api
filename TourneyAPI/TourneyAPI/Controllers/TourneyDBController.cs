using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourneyAPI.Model;

namespace TourneyAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/main")]
    public class TourneyDBController : Controller
    {

        private readonly IConfiguration _configuration;

        public TourneyDBController(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        [HttpPost]
        [Route("post")]
        [EnableCors("DefaultPolicy")]
        public JsonResult addUser([FromBody] UserEntry userModel)
        {
            //Assign model
            var user = userModel;

            bool validSteamUser = false;
            bool validDiscordUser = false;



            return new JsonResult(user.AddUser(_configuration)) ;
        }


    }
}
