using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sanofi.Core.EntitiesModel.Administrator
{
    [Table("Feature", Schema = "Administrator")]
    public class Feature : BaseEntity
    {
        [DisplayName("Feature Name")]
        public string FeatureName { get; set; }
        [DisplayName("Area Name")]
        public string AreaName { get; set; }
        [DisplayName("Controller Name")]
        public string ControllerName { get; set; }
        [DisplayName("Action Name")]
        public string ActionName { get; set; }
        [DisplayName("Menu Icon")]
        public string MenuIcon { get; set; }
        [DisplayName("IsMenu")]
        public Nullable<bool> IsMenu { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public string FeatureID { get; set; }
        public virtual Feature ParentMenu { get; set; }

        public int Sequence { get; set; }

        public int SequenceChild { get; set; }
    }
}
