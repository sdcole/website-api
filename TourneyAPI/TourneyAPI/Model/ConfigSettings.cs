namespace TourneyAPI
{
    public class ConfigSettings
    {
        public string DBConnection { get; set; }

        public string Email { get; set; }
        
        public int SMTPPort { get; set; }

        public string SteamKey { get; set; }

        public string DiscordKey { get; set; }
    }
}
