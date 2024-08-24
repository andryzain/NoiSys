using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Transaksi.Repository
{
    public class IPurchaseOrderRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPurchaseOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public PurchaseOrder Tambah(PurchaseOrder PurchaseOrder)
        {
            _context.PurchaseOrders.Add(PurchaseOrder);
            _context.SaveChanges();
            return PurchaseOrder;
        }

        public async Task<PurchaseOrder> GetPurchaseOrderById(Guid Id)
        {
            var purchaseOrder = _context.PurchaseOrders
                .Where(i => i.PurchaseOrderId == Id)
                .Include(d => d.PurchaseOrderDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .FirstOrDefault(p => p.PurchaseOrderId == Id);

            if (purchaseOrder != null)
            {
                var purchaseOrderDetail = new PurchaseOrder()
                {
                    PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                    PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                    PurchaseRequestId = purchaseOrder.PurchaseRequestId,
                    PurchaseRequestNumber = purchaseOrder.PurchaseRequestNumber,
                    UserId = purchaseOrder.UserId,
                    ApplicationUser = purchaseOrder.ApplicationUser,
                    PenggunaId = purchaseOrder.PenggunaId,
                    Pengguna = purchaseOrder.Pengguna,
                    Termin = purchaseOrder.Termin,
                    BengkelId = purchaseOrder.BengkelId,
                    Bengkel = purchaseOrder.Bengkel,
                    Status = purchaseOrder.Status,
                    QtyTotal = purchaseOrder.QtyTotal,
                    GrandTotal = purchaseOrder.GrandTotal,
                    Keterangan = purchaseOrder.Keterangan,
                    PurchaseOrderDetails = purchaseOrder.PurchaseOrderDetails
                };
                return purchaseOrderDetail;
            }
            return purchaseOrder;
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByIdNoTracking(Guid Id)
        {
            return await _context.PurchaseOrders.AsNoTracking().Where(i => i.PurchaseOrderId == Id).FirstOrDefaultAsync(a => a.PurchaseOrderId == Id);
        }

        public async Task<List<PurchaseOrder>> GetPurchaseOrders()
        {
            return await _context.PurchaseOrders.OrderBy(p => p.CreateDateTime).Select(purchaseOrder => new PurchaseOrder()
            {
                PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                PurchaseRequestId = purchaseOrder.PurchaseRequestId,
                PurchaseRequestNumber = purchaseOrder.PurchaseRequestNumber,
                UserId = purchaseOrder.UserId,
                PenggunaId = purchaseOrder.PenggunaId,
                Termin = purchaseOrder.Termin,
                BengkelId = purchaseOrder.BengkelId,
                Status = purchaseOrder.Status,
                QtyTotal = purchaseOrder.QtyTotal,
                GrandTotal = purchaseOrder.GrandTotal,
                Keterangan = purchaseOrder.Keterangan
            }).ToListAsync();
        }

        public async Task<List<PurchaseOrder>> GetPurchaseOrderDetails()
        {
            return await _context.PurchaseOrders.OrderBy(p => p.CreateDateTime).Select(purchaseOrder => new PurchaseOrder()
            {
                PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                PurchaseRequestId = purchaseOrder.PurchaseRequestId,
                PurchaseRequestNumber = purchaseOrder.PurchaseRequestNumber,
                UserId = purchaseOrder.UserId,
                PenggunaId = purchaseOrder.PenggunaId,
                Termin = purchaseOrder.Termin,
                BengkelId = purchaseOrder.BengkelId,
                Status = purchaseOrder.Status,
                QtyTotal = purchaseOrder.QtyTotal,
                GrandTotal = purchaseOrder.GrandTotal,
                Keterangan = purchaseOrder.Keterangan
            }).ToListAsync();
        }

        public IEnumerable<PurchaseOrder> GetAllPurchaseOrder()
        {
            return _context.PurchaseOrders
                .Include(r => r.PurchaseRequest)
                .Include(d => d.PurchaseOrderDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .ToList();
        }       
    }
}
