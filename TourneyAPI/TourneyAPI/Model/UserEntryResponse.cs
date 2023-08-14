using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace TourneyAPI.Model
{
    public class UserEntryResponse
    {
        [JsonProperty("valid_user")]
        public bool validUser { get; set; }

        [JsonProperty("steam_id")]
        public string steamID { get; set; }

        [JsonProperty("discord_id")]
        public string discordID { get; set; }

        [JsonProperty("steam_avatar")]
        public string steamAvatar { get; set; }

        [JsonProperty("steam_name")]
        public string steamName { get; set; }

        [JsonProperty("success")]
        public bool success { get; set; }   


        public UserEntryResponse()
        {
            this.validUser = false;
            this.steamID = string.Empty;
            this.discordID = string.Empty;
            this.steamAvatar = string.Empty;
            this.steamName = string.Empty;
            this.success = false;

        }



        public string getJsonResponse()
        {

            return JsonConvert.SerializeObject(this);
        }


    }
}
