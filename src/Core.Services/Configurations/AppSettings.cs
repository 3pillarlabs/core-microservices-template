
namespace Core.Services.Configurations
{
    public interface IAppSettings
    {
        string ApplicationName { get; set; }
        string ConnectionString { get; set; }
        bool IsProd { get; set; }
        string AuthKeyName { get; set; }
        string ApiKey { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public string ApplicationName { get; set; }
        public string ConnectionString { get; set; }
        public bool IsProd { get; set; }
        public string AuthKeyName { get; set; }
        public string ApiKey { get; set; }
       
        public AppSettings()
            {

            }  
    }
}
