using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Microsoft.Extensions.Configuration;
using WebsiteAPI.Services;

namespace WebsiteAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api")]
    public class VisitorTrackerController : Controller
    {

        private readonly ConfigService _configService;

        public VisitorTrackerController(ConfigService myService)
        {
            _configService = myService;
        }


        [HttpPost]
        [Route("v1/visitor-tracker")]
        [EnableCors("DefaultPolicy")]
        public IActionResult VisitorTracker()
        {
            try
            {
                int count = 0;

                using (NpgsqlConnection conn = new NpgsqlConnection(_configService.GetConnectionSetting()))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT FROM visitors", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        // Retrieve data by column index or column name
                        count = reader.GetInt32(0); // Column index
                    }
                    count++;

                    cmd = new NpgsqlCommand("UPDATE visitors set count = " + count, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return StatusCode(200, count);
                }
                    
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internall Error Occurred");
            }
            
        }


    }
}