using Sanofi.Core;
using Sanofi.Core.EntitiesModel.Administrator;
using Sanofi.Core.EntitiesModel.IdentityCore;
using Sanofi.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Abhimantra.Sanofi.Base
{
    public abstract class BaseRepository : IDisposable
    {
        protected readonly ApplicationDbContext context;
        protected readonly IPrincipal principal;
        //protected readonly string ReportBaseUrl = "http://52.221.206.186:8085/";
        private readonly GlobalVariableParamModel _globalVariable;
        private readonly string _documentManagementSetting = AppSettingJson.GetDocumentManagementSetting();
        protected readonly string safepediaScheme = HttpHelper.HttpContext?.Request.Scheme;
        protected readonly string safepediaHost = HttpHelper.HttpContext?.Request.Host.Value;

        protected BaseRepository(ApplicationDbContext context, IPrincipal principal, GlobalVariableParamModel globalVariable)
        {
            this.context = context;
            this.principal = principal;
            this._globalVariable = globalVariable;
        }


        public string ReportBaseUrl
        {
            get
            {
                return this._globalVariable.ReportBaseUrl;
            }
        }
        public string BaseAwsDirectoryDomain
        {
            get
            {
                var documentManagementHost = AppSettingJson.GetDocumentManagementHost();
                if (!string.IsNullOrEmpty(documentManagementHost))
                {
                    var documentManagemenSetting = AppSettingJson.GetDocumentManagementSetting();
                    if (documentManagemenSetting?.ToLower() == "sensenet")
                    {
                        var documentManagemenSystemPath = AppSettingJson.GetDocumentManagementSystemPath();

                        return documentManagementHost + documentManagemenSystemPath;
                    }
                    else
                    {
                        return documentManagementHost;
                    }
                }

                return "https://s3-ap-southeast-1.amazonaws.com/";
            }
        }

        public string BaseUrl
        {
            get
            {
                var url = $"{safepediaScheme}://{safepediaHost}";

                return url;
            }
        }

        public string CurrentUserId
        {
            get
            {
                try
                {
                    var currentUser = context.Users.FirstOrDefault(user => user.UserName == CurrentUserName);
                    //return currentUser == null ? null : currentUser.Id;
                    if (currentUser != null)
                    {
                        return currentUser.Id;
                    }
                    else
                    {
                        var currentUserById = context.Users.FirstOrDefault(user => user.Id == CurrentUserName);
                        if (currentUserById != null)
                        {
                            return currentUserById.Id;
                        }
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string CurrentUserName
        {
            get
            {
                if (principal != null)
                {
                    return principal.Identity.Name;
                }
                return null;
            }
        }
        public IQueryable<ApplicationUser> AllUsers
        {
            get
            {
                return context.Users.Where(user => user.DelDate == null);
            }
        }

        [DebuggerStepThrough]
        protected void SetAuditFields(dynamic entity)
        {
            if (entity.CreatedDate == null)
            {
                if (IsPropertyExist(entity, "CreatedDate")) { entity.CreatedDate = DateTime.Now; };
                if (IsPropertyExist(entity, "CreatedBy")) { entity.CreatedBy = CurrentUserId; };
                if (IsPropertyExist(entity, "IsActived")) { entity.IsActived = true; };
            }
            else
            {
                if (IsPropertyExist(entity, "UpdatedDate")) { entity.UpdatedDate = DateTime.Now; };
                if (IsPropertyExist(entity, "UpdatedBy")) { entity.UpdatedBy = CurrentUserId; };
            }
        }

        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }

        public void Dispose()
        {
            if (context == null)
                return;

            context.Dispose();
        }
        private static Random random = new Random();
        public static string GetRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-/";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }




        private IQueryable<Feature> AllFeature
        {
            get
            {
                return context.Feature.Where(doc => doc.DelDate == null);
            }
        }

 



        public string InspectionFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.FeatureID == null && fet.AreaName == "Inspection" && fet.ControllerName == "Inspection" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Inspection";
            }
        }
        public string TSVFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "TaskSelfVerification" && fet.ControllerName == "TaskSelfVerification" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Task Self Verification";
            }
        }
        public string ShocardFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "Hazob" && fet.ControllerName == "Hazob" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Hazob";
            }
        }
        public string AuditFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "Audit" && fet.ControllerName == "Audit" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Internal Audit";
            }
        }
        public string NonconformityFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "Nonconformity" && fet.ControllerName == "Nonconformity" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Corrective Action Report";
            }
        }
        public string ObservationFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "Observation" && fet.ControllerName == "Observation" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "BBS";
            }
        }
        public string PunchListFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "CorrectiveAction" && fet.ControllerName == "CorrectiveAction" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Action List";

            }
        }
        public string SHEDailyReportFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "SHEReport" && fet.ControllerName == "SHEDailyReport" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "SHE Daily Report";
            }
        }
        public string IncidentFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "Incident" && fet.ControllerName == "Incident" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Incident";
            }
        }
        public string UserFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "Identity" && fet.ControllerName == "User" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "User Management";
            }
        }
        public string HirarcFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "RiskManagement" && fet.ControllerName == "Hirardc" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "HIRADC";// "Hirardc";
            }
        }

        public string PTWFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "Meeting" && fet.ControllerName == "PermitToWork" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Permit To Work";
            }
        }

        public string SafetyMeetingFeatureName
        {
            get
            {
                var feature = AllFeature.FirstOrDefault(fet => fet.AreaName == "Meeting" && fet.ControllerName == "SafetyMeeting" && fet.ActionName == "Index");
                if (feature != null)
                {
                    return feature.FeatureName;
                }
                return "Minutes of Meeting";
            }
        }
    }
}
