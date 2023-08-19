using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NLog.LayoutRenderers;
using Npgsql;
using System.Net;

namespace TourneyAPI.Model
{
    public class UserEntry
    {
        public string steamID { get;set;}
        public string discordID { get; set; }

        private string steamKey;

        private IConfiguration configuration;

        public UserEntry(string steamID,string discordID)
        {
            this.steamID = steamID; 
            this.discordID = discordID;
            this.steamKey = null;
        }
        public string AddUser(IConfiguration configuration)
        {
            UserEntryResponse response = new UserEntryResponse();
            //sets config
            this.configuration = configuration;

            steamKey = configuration.GetSection("ConfigSettings").GetSection("SteamKey").Value;
            //Calls steam
            using var client = new WebClient();
            string result = client.DownloadString("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + steamKey + "&steamids=" + steamID);

            JObject jObj = JObject.Parse(result);

            if (jObj.SelectToken("response.players[0].steamid") != null) {
                try
                {
                    Console.WriteLine("VALID USER");
                    response.validUser = true;
                    response.steamID = this.steamID;
                    this.discordID = this.discordID;
                    response.steamName = (string)jObj.SelectToken("response.players[0].personaname");
                    response.steamAvatar = (string)jObj.SelectToken("response.players[0].avatar");


                    //Handles adding to DB
                    var connectionString = configuration.GetSection("ConfigSettings").GetSection("DbConnection").Value;
                    //var connectionString = "Host=localhost:4200;Username=postgres;Password=Ir1dedirtbikes!;Database=websiteDB;Pooling=false;Timeout=300;CommandTimeout=300";
                    var conn = new NpgsqlConnection(connectionString);

                    conn.Open();
                    var cmd = new NpgsqlCommand("INSERT INTO player(steam_ID, discord_ID, steam_name, steam_avatar) VALUES (:steamID, :discordID, :steamName, :steamAvatar);", conn);

                    var sID = cmd.Parameters.Add(":steamID", NpgsqlTypes.NpgsqlDbType.Varchar);
                    var dID = cmd.Parameters.Add(":discordID", NpgsqlTypes.NpgsqlDbType.Varchar);
                    var sNm = cmd.Parameters.Add(":steamName", NpgsqlTypes.NpgsqlDbType.Varchar);
                    var sAv = cmd.Parameters.Add(":steamAvatar", NpgsqlTypes.NpgsqlDbType.Varchar);

                    cmd.Prepare();
                    sID.Value = steamID;
                    dID.Value = discordID;
                    sNm.Value = response.steamName;
                    sAv.Value = response.steamAvatar;


                    cmd.ExecuteNonQuery();
                    response.success = true;



                } catch (Exception e)
                {
                    Console.WriteLine("ERROR");
                    response.success = false;
                }
                
            }
            else
            {
                response.validUser = false;
                
            }

            return JsonConvert.SerializeObject(response).Replace(@"\", " ");





        }
    }
}
