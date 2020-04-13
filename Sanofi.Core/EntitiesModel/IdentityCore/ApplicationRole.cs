using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Sanofi.Core.EntitiesModel.IdentityCore
{
    public class ApplicationRole : IdentityRole
    {
        public string CustomRoleName { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsAdministrator { get; set; }
    }
}
