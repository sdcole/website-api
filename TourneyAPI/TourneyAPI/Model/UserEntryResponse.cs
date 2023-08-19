using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace TourneyAPI.Model
{
    public class UserEntryResponse
    {
        [JsonProperty("validSteamUser")]
        public bool validSteamUser { get; set; }

        [JsonProperty("steamAvatar")]
        public string steamAvatar { get; set; }

        [JsonProperty("steamName")]
        public string steamName { get; set; }

        [JsonProperty("SteamID")]
        public string steamID { get; set; }

        [JsonProperty("validDiscordUser")]
        public bool validDiscordUser { get; set; }

        [JsonProperty("discordID")]
        public string discordID { get; set; }

        [JsonProperty("discordUserName")]
        public string discordUserName { get; set; }

        [JsonProperty("discordGlobalName")]
        public string discordGlobalName { get; set; }

        [JsonProperty("discordAvatar")]
        public string discordAvatar { get; set; }




        public UserEntryResponse()
        {
            this.validSteamUser = false;
            this.steamID = string.Empty;
            this.steamAvatar = string.Empty;
            this.steamName = string.Empty;

            this.validDiscordUser = false;
            this.discordID = string.Empty;
            this.discordUserName = string.Empty;
            this.discordGlobalName = string.Empty;
            this.discordAvatar = string.Empty;

            


        }



        public string getJsonResponse()
        {

            return JsonConvert.SerializeObject(this);
        }


    }
}
