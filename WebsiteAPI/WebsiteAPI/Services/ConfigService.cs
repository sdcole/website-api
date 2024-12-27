namespace WebsiteAPI.Services
{
    public class ConfigService
    {
        private IConfiguration _configuration;

        public ConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionSetting()
        {
            // Retrieve a setting by its key
            return _configuration["CONN_STRING"];
        }
    }
}
