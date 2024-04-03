namespace serviceIssues.Source.Data.GitData
{
    public static class AppConfig
    {
        public static IConfiguration? configuration { get; private set; }

        public static string? gitlabUsername { get; private set; }
        public static string? gitlabPassword { get; private set; }
        public static string? gitlabPrivateToken { get; private set; }
        public static string? exportImageFolder { get; private set; }
        public static string? exportDataPath { get; private set; }
        public static string? gitlabWebAPI { get; private set; }
        public static string? gitlabRestAPI { get; private set; }
        public static string? logFileName { get; private set; }

        public static void Initialize(IConfiguration configuration = null)
        {
            if (configuration is not null)
            {
                AppConfig.configuration = configuration;
            }

            /* 
             * Get fields from appsettings.json
                string urls = configuration["Urls"];
                string allowedHosts = configuration["AllowedHosts"];
                string defaultLogLevel = configuration["Logging:LogLevel:Default"];
                string aspNetCoreLogLevel = configuration["Logging:LogLevel:Microsoft.AspNetCore"];
            */

            gitlabUsername = "hamidreza";
            gitlabPassword = "n@9fmm@DYi$A5$5^drRK";
            gitlabPrivateToken = "UuD8Ct1oDMcM_siqKEfC";
            exportImageFolder = @"C:\img";
            exportDataPath = @"C:\export";
            gitlabWebAPI = @"https://gitlab.medix.at:7443";
            gitlabRestAPI = @"https://gitlab.medix.at:7443/api/v4";
            logFileName = @"serviceIssues.log";
        }
    }
}
