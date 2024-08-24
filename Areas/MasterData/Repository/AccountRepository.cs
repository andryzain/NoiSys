﻿using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Identity.Data;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<ApplicationUser> GetUsers()
        {
            return _context.Users
                .Include(lp => lp.LevelPengguna)
                .ToList();
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();

            return user;
        }
    }

    public interface IAccountRepository
    {
        ICollection<ApplicationUser> GetUsers();

        ApplicationUser GetUser(string id);
        ApplicationUser UpdateUser(ApplicationUser user);
    }
}
