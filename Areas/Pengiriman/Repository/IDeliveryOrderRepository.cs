using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Pengiriman.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Pengiriman.Repository
{
    public class IDeliveryOrderRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IDeliveryOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public DeliveryOrder Tambah(DeliveryOrder DeliveryOrder)
        {
            _context.DeliveryOrders.Add(DeliveryOrder);
            _context.SaveChanges();
            return DeliveryOrder;
        }

        public async Task<DeliveryOrder> GetDeliveryOrderById(Guid Id)
        {
            var DeliveryOrder = _context.DeliveryOrders
                .Where(i => i.DeliveryOrderId == Id)
                .Include(d => d.DeliveryOrderDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .FirstOrDefault(p => p.DeliveryOrderId == Id);

            if (DeliveryOrder != null)
            {
                var DeliveryOrderDetail = new DeliveryOrder()
                {
                    DeliveryOrderId = DeliveryOrder.DeliveryOrderId,
                    DeliveryOrderNumber = DeliveryOrder.DeliveryOrderNumber,
                    PurchaseOrderId = DeliveryOrder.PurchaseOrderId,
                    PurchaseOrderNumber = DeliveryOrder.PurchaseOrderNumber,
                    UserId = DeliveryOrder.UserId,
                    ApplicationUser = DeliveryOrder.ApplicationUser,
                    PenggunaId = DeliveryOrder.PenggunaId,
                    Pengguna = DeliveryOrder.Pengguna,
                    Termin = DeliveryOrder.Termin,
                    BengkelId = DeliveryOrder.BengkelId,
                    Bengkel = DeliveryOrder.Bengkel,
                    Status = DeliveryOrder.Status,
                    QtyTotal = DeliveryOrder.QtyTotal,
                    GrandTotal = DeliveryOrder.GrandTotal,
                    Keterangan = DeliveryOrder.Keterangan,
                    DeliveryOrderDetails = DeliveryOrder.DeliveryOrderDetails
                };
                return DeliveryOrderDetail;
            }
            return DeliveryOrder;
        }

        public async Task<DeliveryOrder> GetDeliveryOrderByIdNoTracking(Guid Id)
        {
            return await _context.DeliveryOrders.AsNoTracking()
                .Where(i => i.DeliveryOrderId == Id)
                .Include(r => r.PurchaseOrder)
                .Include(d => d.DeliveryOrderDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .FirstOrDefaultAsync(a => a.DeliveryOrderId == Id);
        }

        public async Task<List<DeliveryOrder>> GetDeliveryOrders()
        {
            return await _context.DeliveryOrders.Where(s => s.Status != "Selesai").OrderBy(p => p.CreateDateTime).Select(DeliveryOrder => new DeliveryOrder()
            {
                DeliveryOrderId = DeliveryOrder.DeliveryOrderId,
                DeliveryOrderNumber = DeliveryOrder.DeliveryOrderNumber,
                PurchaseOrderId = DeliveryOrder.PurchaseOrderId,
                PurchaseOrderNumber = DeliveryOrder.PurchaseOrderNumber,
                UserId = DeliveryOrder.UserId,
                PenggunaId = DeliveryOrder.PenggunaId,
                Termin = DeliveryOrder.Termin,
                BengkelId = DeliveryOrder.BengkelId,
                Status = DeliveryOrder.Status,
                QtyTotal = DeliveryOrder.QtyTotal,
                GrandTotal = DeliveryOrder.GrandTotal,
                Keterangan = DeliveryOrder.Keterangan
            }).ToListAsync();
        }

        public async Task<List<DeliveryOrder>> GetDeliveryOrderDetails()
        {
            return await _context.DeliveryOrders.OrderBy(p => p.CreateDateTime).Select(DeliveryOrder => new DeliveryOrder()
            {
                DeliveryOrderId = DeliveryOrder.DeliveryOrderId,
                DeliveryOrderNumber = DeliveryOrder.DeliveryOrderNumber,
                PurchaseOrderId = DeliveryOrder.PurchaseOrderId,
                PurchaseOrderNumber = DeliveryOrder.PurchaseOrderNumber,
                UserId = DeliveryOrder.UserId,
                PenggunaId = DeliveryOrder.PenggunaId,
                Termin = DeliveryOrder.Termin,
                BengkelId = DeliveryOrder.BengkelId,
                Status = DeliveryOrder.Status,
                QtyTotal = DeliveryOrder.QtyTotal,
                GrandTotal = DeliveryOrder.GrandTotal,
                Keterangan = DeliveryOrder.Keterangan
            }).ToListAsync();
        }

        public IEnumerable<DeliveryOrder> GetAllDeliveryOrder()
        {
            return _context.DeliveryOrders
                .Include(r => r.PurchaseOrder)
                .Include(d => d.DeliveryOrderDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .ToList();
        }
    }
}
