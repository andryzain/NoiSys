using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IBankCabangRepository
    {
        private readonly ApplicationDbContext _context;

        public IBankCabangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public BankCabang Add(BankCabang bank)
        {
            _context.BankCabangs.Add(bank);
            _context.SaveChanges();
            return bank;
        }

        public BankCabang Update(BankCabang bankChanges)
        {
            var bank = _context.BankCabangs.Attach(bankChanges);
            bank.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return bankChanges;
        }

        public BankCabang Delete(Guid Id)
        {
            var bankCabang = _context.BankCabangs.Find(Id);
            if (bankCabang != null)
            {
                _context.BankCabangs.Remove(bankCabang);
                _context.SaveChanges();
            }
            return bankCabang;
        }

        public IEnumerable<BankCabang> GetAllBankCabang()
        {
            return _context.BankCabangs
                .Include(b => b.Bank)
                .AsNoTracking();
        }

        public async Task<BankCabang> GetBankCabangById(Guid Id)
        {
            var bankCabang = await _context.BankCabangs.FindAsync(Id);

            if (bankCabang != null)
            {
                var bankDetail = new BankCabang()
                {
                    BankCabangId = bankCabang.BankCabangId,
                    BankId = bankCabang.BankId,
                    KodeBankCabang = bankCabang.KodeBankCabang,
                    NamaCabang = bankCabang.NamaCabang,
                    NomorRekening = bankCabang.NomorRekening,
                    AtasNama = bankCabang.AtasNama,
                };
                return bankDetail;
            }
            return null;
        }

        public async Task<BankCabang> GetBankCabangByIdNoTracking(Guid Id)
        {
            return await _context.BankCabangs.AsNoTracking().FirstOrDefaultAsync(a => a.BankCabangId == Id);
        }

        public async Task<List<BankCabang>> GetBankCabangs()
        {
            return await _context.BankCabangs.OrderBy(p => p.NamaCabang).Select(x => new BankCabang()
            {
                BankCabangId = x.BankCabangId,
                BankId = x.BankId,
                KodeBankCabang = x.KodeBankCabang,
                NamaCabang = x.NamaCabang,
                NomorRekening = x.NomorRekening,
                AtasNama = x.AtasNama,
            }).ToListAsync();
        }
    }
}
