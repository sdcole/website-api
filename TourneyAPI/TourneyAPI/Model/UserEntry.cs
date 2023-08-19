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



        public UserEntry(string steamID,string discordID)
        {
            this.steamID = steamID; 
            this.discordID = discordID;

        }
 

    }
}
