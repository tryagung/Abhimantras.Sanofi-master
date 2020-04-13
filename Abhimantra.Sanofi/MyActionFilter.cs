using Abhimantra.Sanofi.Base;
using Microsoft.AspNetCore.Mvc.Filters;
using Sanofi.Core;
using Sanofi.Core.EntitiesModel.Administrator;
using Sanofi.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Abhimantra.Sanofi
{
    public class MyActionFilter : BaseRepository, IActionFilter
    {
        public MyActionFilter(ApplicationDbContext context, IPrincipal principal, GlobalVariableParamModel globalParameter) : base(context, principal, globalParameter) { }

        public IQueryable<Feature> AllFeature
        {
            get
            {
                return context.Feature.Where(feature => feature.DelDate == null);
            }
        }

        public string GetFeature(string area, string controller, string action)
        {
            return AllFeature.Where(a => a.AreaName == area && a.ControllerName == controller && (a.ActionName == "index" || a.ActionName == action)).FirstOrDefault().FeatureName;

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var x = "after action";
            //context.RouteData.Values.Add("tes2", x);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var x = "before action";
                string action = context.RouteData.Values["action"].ToString();
                if (action.ToLower().Contains("index") || action.ToLower().Contains("create") || action.ToLower().Contains("edit") || action.ToLower().Contains("detail"))
                {
                    string area = context.RouteData.Values["area"].ToString();
                    string controller = context.RouteData.Values["controller"].ToString();
                    x = GetFeature(area, controller, action);
                    context.RouteData.Values.Add("Title", x);
                }
                //var tes = context.RouteData.Values["Title"];
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }
    }
}
