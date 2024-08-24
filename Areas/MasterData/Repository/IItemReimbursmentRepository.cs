using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IItemReimbursmentRepository
    {
        private readonly ApplicationDbContext _context;

        public IItemReimbursmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ItemReimbursment Tambah(ItemReimbursment ItemReimbursment)
        {
            _context.ItemReimbursments.Add(ItemReimbursment);
            _context.SaveChanges();
            return ItemReimbursment;
        }

        public async Task<ItemReimbursment> GetItemReimbursmentById(Guid Id)
        {
            var ItemReimbursment = await _context.ItemReimbursments.FindAsync(Id);

            if (ItemReimbursment != null)
            {
                var ItemReimbursmentDetail = new ItemReimbursment()
                {
                    ItemReimbursmentId = ItemReimbursment.ItemReimbursmentId,
                    KodeItemReimbursment = ItemReimbursment.KodeItemReimbursment,
                    NamaItemReimbursment = ItemReimbursment.NamaItemReimbursment
                };
                return ItemReimbursmentDetail;
            }
            return null;
        }

        public async Task<ItemReimbursment> GetItemReimbursmentByIdNoTracking(Guid Id)
        {
            return await _context.ItemReimbursments.AsNoTracking().FirstOrDefaultAsync(a => a.ItemReimbursmentId == Id);
        }

        public async Task<List<ItemReimbursment>> GetItemReimbursments()
        {
            return await _context.ItemReimbursments.OrderBy(p => p.CreateDateTime).Select(ItemReimbursment => new ItemReimbursment()
            {
                ItemReimbursmentId = ItemReimbursment.ItemReimbursmentId,
                KodeItemReimbursment = ItemReimbursment.KodeItemReimbursment,
                NamaItemReimbursment = ItemReimbursment.NamaItemReimbursment,
            }).ToListAsync();
        }

        public IEnumerable<ItemReimbursment> GetAllItemReimbursment()
        {
            return _context.ItemReimbursments.AsNoTracking();
        }

        public ItemReimbursment Update(ItemReimbursment update)
        {
            var ItemReimbursment = _context.ItemReimbursments.Attach(update);
            ItemReimbursment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public ItemReimbursment Delete(Guid Id)
        {
            var ItemReimbursment = _context.ItemReimbursments.Find(Id);
            if (ItemReimbursment != null)
            {
                _context.ItemReimbursments.Remove(ItemReimbursment);
                _context.SaveChanges();
            }
            return ItemReimbursment;
        }
    }
}
