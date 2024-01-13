using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Net;
using TourneyAPI.Model;
using static System.Net.WebRequestMethods;

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
            //Assign transfer model
            var user = userModel;

            //Assign response model
            UserEntryResponse response = new UserEntryResponse();

            bool success = false;





            string steamKey;
            string discordKey;

            using var client = new WebClient();
            JObject jObj;
            #region SteamAPI
            //This is the part of code that handles API call to steam 
            steamKey = _configuration.GetSection("ConfigSettings").GetSection("SteamKey").Value;

            string result = client.DownloadString("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + steamKey + "&steamids=" + userModel.steamID);
            response.steamID = userModel.steamID;

            jObj = JObject.Parse(result);

            if (jObj.SelectToken("response.players[0].steamid") != null)
            {
                try
                {
                    //Since we know that we have a valid steam account assign the variables
                    response.validSteamUser = true;
                    response.steamName = (string)jObj.SelectToken("response.players[0].personaname");
                    response.steamAvatar = (string)jObj.SelectToken("response.players[0].avatar");
                }
                catch (Exception ex)
                {
                    //TODO LOGGING ERRORS
                }
            }
            #endregion

            #region DiscordAPI
            discordKey = _configuration.GetSection("ConfigSettings").GetSection("DiscordKey").Value;
            client.Headers.Add(HttpRequestHeader.Authorization, "Bot " + discordKey);
            result = client.DownloadString("https://discord.com/api/v9/users/" + userModel.discordID);

            jObj = JObject.Parse(result);

            if (jObj.SelectToken("username") != null)
            {
                try
                {
                    response.validDiscordUser = true;
                    response.discordID = (string)jObj.SelectToken("id");
                    response.discordUserName = (string)jObj.SelectToken("username");
                    response.discordGlobalName = (string)jObj.SelectToken("global_name");


                    response.discordAvatar = "https://cdn.discordapp.com/avatars/" + response.discordID + "/" + (string)jObj.SelectToken("response.avatar");




                }
                catch (Exception ex)
                {
                    //TODO ADD LOGGING
                }
            }

            #endregion

            #region AddToDB
            //This region validates both steam and discord are valid if so save to DB

            if (response.validSteamUser && response.validDiscordUser)
            {
                try
                {
                    var conn = new NpgsqlConnection(_configuration.GetSection("ConfigSettings").GetSection("DbConnection").Value);
                    conn.Open();
                    var cmd = new NpgsqlCommand("INSERT INTO player(steam_ID, discord_ID, steam_name, steam_avatar, discord_user_name, discord_avatar) VALUES (:steamID, :discordID, :steamName, :steamAvatar, :discordUserName, :discordAvatar);", conn);

                    var sID = cmd.Parameters.Add(":steamID", NpgsqlTypes.NpgsqlDbType.Varchar);
                    var dID = cmd.Parameters.Add(":discordID", NpgsqlTypes.NpgsqlDbType.Varchar);
                    var sNm = cmd.Parameters.Add(":steamName", NpgsqlTypes.NpgsqlDbType.Varchar);
                    var sAv = cmd.Parameters.Add(":steamAvatar", NpgsqlTypes.NpgsqlDbType.Varchar);
                    var dUn = cmd.Parameters.Add(":discordUserName", NpgsqlTypes.NpgsqlDbType.Varchar);
                    var dAv = cmd.Parameters.Add(":discordAvatar", NpgsqlTypes.NpgsqlDbType.Varchar);
                    cmd.Prepare();
                    sID.Value = response.steamID;
                    dID.Value = response.discordID;
                    sNm.Value = response.steamName;
                    sAv.Value = response.steamAvatar;
                    dUn.Value = response.discordUserName;
                    dAv.Value = response.discordAvatar;


                    cmd.ExecuteNonQuery();
                    conn.Close();
                    response.success = true;
                }
                catch 
                {
                    //TODO LOGGING
                }
            }
            #endregion

            JsonResult returnResponse;
            if (response.success)
            {
                returnResponse = new JsonResult(response);
                returnResponse.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                returnResponse = new JsonResult(response);
                returnResponse.StatusCode = (int)(HttpStatusCode.BadRequest);
            }
            return returnResponse;
        }


    }
}
