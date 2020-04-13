using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sanofi.Core
{
    public class AppSettingJson
    {
        public static IConfigurationRoot GetConfigurationSetting()
        {
            string applicationExeDirectory = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
            .SetBasePath(applicationExeDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public static string GetConnectionString()
        {
            var connectionString = GetConfigurationSetting()["ConnectionStrings:DefaultConnection"];

            return connectionString;
        }

        public static string GetDocumentManagementSetting()
        {
            var setting = GetConfigurationSetting()["DocumentManagementSettings:DefaultSetting"];

            return setting;
        }

        public static string GetDocumentManagementHost()
        {
            var host = GetConfigurationSetting()["DocumentManagementSettings:Host"];

            return host;
        }

        public static string GetDocumentManagementSystemPath()
        {
            var systemPath = GetConfigurationSetting()["DocumentManagementSettings:SystemPath"];

            return systemPath;
        }

        public static string[] GetDocumentManagementServerContextLogin()
        {
            var stringServerContextLogin = GetConfigurationSetting()["DocumentManagementSettings:ServerContextLogin"];

            var serverContextLogin = stringServerContextLogin.Split(";");

            if (serverContextLogin.Length < 2)
                throw new Exception("Destination Server must be set first");

            return serverContextLogin;
        }
    }
}
