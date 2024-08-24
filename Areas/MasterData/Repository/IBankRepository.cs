using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IBankRepository
    {
        private readonly ApplicationDbContext _context;

        public IBankRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Bank Add(Bank bank)
        {
            _context.Banks.Add(bank);
            _context.SaveChanges();
            return bank;
        }

        public Bank Update(Bank bankChanges)
        {
            var bank = _context.Banks.Attach(bankChanges);
            bank.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return bankChanges;
        }

        public Bank Delete(Guid Id)
        {
            var bank = _context.Banks.Find(Id);
            if (bank != null)
            {
                _context.Banks.Remove(bank);
                _context.SaveChanges();
            }
            return bank;
        }

        public IEnumerable<Bank> GetAllBank()
        {
            return _context.Banks
                .Include(cbg => cbg.BankCabang)
                .AsNoTracking();
        }

        public async Task<Bank> GetBankById(Guid Id)
        {
            var Bank = await _context.Banks.FindAsync(Id);

            if (Bank != null)
            {
                var BankDetail = new Bank()
                {
                    BankId = Bank.BankId,
                    KodeBank = Bank.KodeBank,
                    NamaBank = Bank.NamaBank
                };
                return BankDetail;
            }
            return null;
        }

        public async Task<Bank> GetBankByIdNoTracking(Guid Id)
        {
            return await _context.Banks.AsNoTracking().FirstOrDefaultAsync(a => a.BankId == Id);
        }

        public async Task<List<Bank>> GetBanks()
        {
            return await _context.Banks.OrderBy(p => p.NamaBank).Select(x => new Bank()
            {
                BankId = x.BankId,
                KodeBank = x.KodeBank,
                NamaBank = x.NamaBank
            }).ToListAsync();
        }
    }
}
