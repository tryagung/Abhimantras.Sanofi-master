using Sanofi.Core.EntitiesModel.IdentityCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sanofi.Core.EntitiesModel.Administrator
{
    [Table("RoleFeature", Schema = "Administrator")]
    public class RoleFeature : BaseEntity
    {
        public string FeatureID { get; set; }
        public Feature Feature { get; set; }

        public string RoleID { get; set; }
        public ApplicationRole Role { get; set; }

        // Ali
        public bool IsAddView { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
    }
}
