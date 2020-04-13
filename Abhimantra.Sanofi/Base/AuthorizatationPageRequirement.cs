using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Sanofi.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abhimantra.Sanofi.Base
{
    public class AuthorizationPageRequirement : IAuthorizationRequirement
    {
    }

    public class AuthorizationPageHandler : AuthorizationHandler<AuthorizationPageRequirement>
    {
        private readonly ApplicationDbContext dbContext;
        public AuthorizationPageHandler(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       AuthorizationPageRequirement requirement)
        {
            var mvcContext = context.Resource as AuthorizationFilterContext;
            ControllerActionDescriptor descriptor = mvcContext?.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {

                var actionName = descriptor.ActionName;
                var controllerName = descriptor.ControllerName.ToLower();

                var currentUserName = context.User.Identity.Name;

                if (currentUserName != null)
                {
                    if (descriptor.MethodInfo.ReturnType == typeof(IActionResult) ||
                   descriptor.MethodInfo.ReturnType == typeof(Task<IActionResult>) ||
                   descriptor.MethodInfo.ReturnType == typeof(ActionResult) ||
                   descriptor.MethodInfo.ReturnType == typeof(Task<ActionResult>))
                    {
                        var hasAccess = false;

                        if (controllerName == "home" || controllerName == "file")// && actionName == "Index")
                        {
                            hasAccess = true;
                        }
                        else
                        {
                            if (controllerName == "investigation")
                                controllerName = "incident";

                            var currentUser = dbContext.Users.FirstOrDefault(user => user.UserName == currentUserName);
                            if (currentUser != null)
                            {
                                var currentUserRoles = dbContext.UserRoles.Where(rol => rol.UserId == currentUser.Id).ToList();

                                hasAccess = (from userRole in currentUserRoles
                                             join roleAccess in dbContext.RoleFeature
                                             on userRole.RoleId equals roleAccess.RoleID
                                             join feature in dbContext.Feature
                                             on roleAccess.FeatureID equals feature.ID
                                             where userRole.UserId == currentUser.Id
                                             && feature.ControllerName?.ToLower() == controllerName
                                             && roleAccess.IsAddView//Tambahan, yg bisa lihat menu cuman group yg IsAddViewnya di centang -rezkar 27/01/2020
                                             //&& feature.ActionName == actionName
                                             select userRole).Any();

                                if (!hasAccess)
                                {
                                    hasAccess = (from userRole in currentUserRoles
                                                 join role in dbContext.Roles
                                                 on userRole.RoleId equals role.Id
                                                 select role).Any();
                                }
                            }
                        }
                        if (hasAccess)
                        {
                            context.Succeed(requirement);
                        }
                    }
                    else
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            //TODO: Use the following if targeting a version of
            //.NET Framework older than 4.6:
            //      return Task.FromResult(0);
            return Task.CompletedTask;
        }
    }
}
