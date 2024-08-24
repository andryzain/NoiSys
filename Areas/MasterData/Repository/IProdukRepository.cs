using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IProdukRepository
    {
        private readonly ApplicationDbContext _context;

        public IProdukRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Produk Tambah(Produk produk)
        {
            _context.Produks.Add(produk);
            _context.SaveChanges();
            return produk;
        }

        public async Task<Produk> GetProdukById(Guid Id)
        {
            var produk = await _context.Produks
                .Include(prin => prin.Principal)
                .Include(kate => kate.Kategori)
                .Include(satu => satu.Satuan)
                .Include(disk => disk.Diskon)
                .SingleOrDefaultAsync(p => p.ProdukId == Id);

            if (produk != null)
            {
                var produkDetail = new Produk()
                {
                    ProdukId = produk.ProdukId,
                    KodeProduk = produk.KodeProduk,
                    NamaProduk = produk.NamaProduk,
                    PrincipalId = produk.PrincipalId,
                    KategoriId = produk.KategoriId,
                    JumlahStok = produk.JumlahStok,
                    SatuanId = produk.SatuanId,
                    HargaBeli = produk.HargaBeli,
                    HargaJual = produk.HargaJual,
                    Cogs = produk.Cogs,
                    DiskonId = produk.DiskonId,
                    Catatan = produk.Catatan
                };
                return produkDetail;
            }
            return null;
        }

        public async Task<Produk> GetProdukByIdNoTracking(Guid Id)
        {
            return await _context.Produks.AsNoTracking().FirstOrDefaultAsync(a => a.ProdukId == Id);
        }

        public async Task<List<Produk>> GetProduks()
        {
            return await _context.Produks.OrderBy(p => p.CreateDateTime).Select(produk => new Produk()
            {
                ProdukId = produk.ProdukId,
                KodeProduk = produk.KodeProduk,
                NamaProduk = produk.NamaProduk,
                PrincipalId = produk.PrincipalId,
                KategoriId = produk.KategoriId,
                JumlahStok = produk.JumlahStok,
                SatuanId = produk.SatuanId,
                HargaBeli = produk.HargaBeli,
                HargaJual = produk.HargaJual,
                Cogs = produk.Cogs,
                DiskonId = produk.DiskonId,
                Catatan = produk.Catatan
            }).ToListAsync();
        }

        public IEnumerable<Produk> GetAllProduk()
        {
            return _context.Produks.OrderByDescending(d => d.CreateDateTime)
                .Include(prin => prin.Principal)
                .Include(kate => kate.Kategori)
                .Include(satu => satu.Satuan)
                .Include(disk => disk.Diskon)
                .AsNoTracking();
        }

        public Produk Update(Produk update)
        {
            var produk = _context.Produks.Attach(update);
            produk.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Produk Delete(Guid Id)
        {
            var produk = _context.Produks.Find(Id);
            if (produk != null)
            {
                _context.Produks.Remove(produk);
                _context.SaveChanges();
            }
            return produk;
        }
    }
}
