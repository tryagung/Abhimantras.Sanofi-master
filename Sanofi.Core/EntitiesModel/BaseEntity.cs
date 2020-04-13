using Sanofi.Core.EntitiesModel.IdentityCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Sanofi.Core.EntitiesModel
{
    public class BaseEntity
    {
        [Key]
        public string ID { get; set; }

        [ForeignKey("AppUserCreateBy")]
        public string CreatedBy { get; set; }
        public virtual ApplicationUser AppUserCreateBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }

        [ForeignKey("AppUserUpdateBy")]
        public string UpdatedBy { get; set; }
        public virtual ApplicationUser AppUserUpdateBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<DateTime> DelDate { get; set; }
        //public Boolean IsActived { get; set; }

        public object this[string propertyName]
        {
            get
            {
                Type myType = GetType();// typeof(BaseEntity);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(BaseEntity);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }
    }
}
