using Microsoft.AspNetCore.Identity;

namespace NoiSys.Areas.MasterData.Repository
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
