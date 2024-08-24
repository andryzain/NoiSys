using NoiSys.Areas.MasterData.Repository;

namespace NoiSys.Repository
{
    public interface IUnitOfWork
    {
        IAccountRepository User { get; }
        IRoleRepository Role { get; }
    }
}
