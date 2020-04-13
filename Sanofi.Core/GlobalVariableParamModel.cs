using System;
using System.Collections.Generic;
using System.Text;

namespace Sanofi.Core
{
    public class GlobalVariableParamModel
    {
        public string ConnectionString { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationDomain { get; set; }
        public string ApplicationWebAppDomain { get; set; }
        public string ContentRootPath { get; set; }
        public string ReportBaseUrl { get; set; }

        public string EmailNotificationUsername { get; set; }
        public string EmailNotificationPassword { get; set; }
        public string EmailNotificationHost { get; set; }
        public int EmailNotificationPort { get; set; }
        public string SensenetBaseUrl { get; set; }
        public string EnvironmentVariable { get; set; }
    }
}
