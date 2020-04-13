using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Sanofi.Core.EntitiesModel.IdentityCore
{
    public class ApplicationUser : IdentityUser
    {
        public string UserCode { get; set; } // Isi Nya NIK
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public Nullable<DateTime> Birthdate { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public string Status { get; set; }
        public Nullable<DateTime> StatusEndDate { get; set; }
        public string JobTitle { get; set; }
        public string EmploymentType { get; set; }

        public string ProfilePic { get; set; }
        public Nullable<bool> IsActive { get; set; }

        [ForeignKey("AppUserCreateBy")]
        public string CreatedBy { get; set; }
        public virtual ApplicationUser AppUserCreateBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }

        [ForeignKey("AppUserUpdateBy")]
        public string UpdatedBy { get; set; }
        public virtual ApplicationUser AppUserUpdateBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }

        public Nullable<DateTime> DelDate { get; set; }

    }
}
