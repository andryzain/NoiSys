using NoiSys.Areas.MasterData.Repository;

namespace NoiSys.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountRepository User { get; }
        public IRoleRepository Role { get; }

        public UnitOfWork(IAccountRepository user, IRoleRepository role)
        {
            User = user;
            Role = role;
        }
    }    
}
