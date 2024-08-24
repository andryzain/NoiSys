using Microsoft.AspNetCore.Mvc.Rendering;
using NoiSys.Areas.Identity.Data;

namespace NoiSys.Areas.MasterData.ViewModels
{
    public class RoleViewModel
    {
        public string RoleName { get; set; }
        //public string Module { get; set; }
        //public string Menu { get; set; }
        //public string Keterangan { get; set; }
        public ApplicationUser? User { get; set; }
        public IList<SelectListItem>? Roles { get; set; }
    }
}
