using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Penerimaan.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Penerimaan.Repository
{
    public class IReceiveOrderRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IReceiveOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public ReceiveOrder Tambah(ReceiveOrder ReceiveOrder)
        {
            _context.ReceiveOrders.Add(ReceiveOrder);
            _context.SaveChanges();
            return ReceiveOrder;
        }

        public async Task<ReceiveOrder> GetReceiveOrderById(Guid Id)
        {
            var receiveOrder = _context.ReceiveOrders
                .Where(i => i.ReceiveOrderId == Id)
                .Include(d => d.PembelianDetails)
                .Include(r => r.ReceiveOrderDetails)
                .Include(u => u.ApplicationUser)
                .FirstOrDefault(p => p.ReceiveOrderId == Id);

            if (receiveOrder != null)
            {
                var receiveOrderDetail = new ReceiveOrder()
                {
                    ReceiveOrderId = receiveOrder.ReceiveOrderId,
                    ReceiveOrderNumber = receiveOrder.ReceiveOrderNumber,
                    PembelianId = receiveOrder.PembelianId,                    
                    ReceiveById = receiveOrder.ReceiveById,                    
                    Status = receiveOrder.Status,
                    Catatan = receiveOrder.Catatan,
                    ReceiveOrderDetails = receiveOrder.ReceiveOrderDetails
                };
                return receiveOrderDetail;
            }
            return receiveOrder;
        }

        public async Task<ReceiveOrder> GetReceiveOrderByIdNoTracking(Guid Id)
        {
            return await _context.ReceiveOrders.AsNoTracking()
                .Where(i => i.ReceiveOrderId == Id)
                .Include(d => d.PembelianDetails)
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(a => a.ReceiveOrderId == Id);
        }

        public async Task<List<ReceiveOrder>> GetReceiveOrders()
        {
            return await _context.ReceiveOrders.OrderBy(p => p.CreateDateTime).Select(ReceiveOrder => new ReceiveOrder()
            {
                ReceiveOrderId = ReceiveOrder.ReceiveOrderId,
                ReceiveOrderNumber = ReceiveOrder.ReceiveOrderNumber,
                PembelianId = ReceiveOrder.PembelianId,
                ReceiveById = ReceiveOrder.ReceiveById,
                Status = ReceiveOrder.Status,
                Catatan = ReceiveOrder.Catatan
            }).ToListAsync();
        }

        public IEnumerable<ReceiveOrder> GetAllReceiveOrder()
        {
            return _context.ReceiveOrders
                .Include(p => p.Pembelian)
                .Include(d => d.PembelianDetails)
                .Include(u => u.ApplicationUser)
                .ToList();
        }
    }
}
