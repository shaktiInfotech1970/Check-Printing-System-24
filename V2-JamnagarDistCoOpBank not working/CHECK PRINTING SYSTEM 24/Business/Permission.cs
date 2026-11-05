using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CPS.Business
{
    [Table("Permission")]
    public class PermissionDTO : BaseEntity
    {
        public int UserId { get; set; }
        public CPS.Common.Page Page { get; set; }
        public int Permission { get; set; }
    }

    public class UserPermission
    {
        public List<PermissionDTO> Permissions { get; set; }

        public bool HasAny(CPS.Common.Page page)
        {
            return Permissions.Where(o => o.Page == page).Select(s => s.Permission).FirstOrDefault() > 0;
        }

        public bool Has(CPS.Common.Page page, CPS.Common.Permission permission)
        {
            return Permissions.Where(o => o.Page == page && (o.Permission & (int)permission) == (int)permission).FirstOrDefault() != null;
        }
    }
}
