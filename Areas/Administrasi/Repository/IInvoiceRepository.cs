using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Administrasi.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Administrasi.Repository
{
    public class IInvoiceRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IInvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public Invoice Tambah(Invoice Invoice)
        {
            _context.Invoices.Add(Invoice);
            _context.SaveChanges();
            return Invoice;
        }

        public async Task<Invoice> GetInvoiceById(Guid Id)
        {
            var Invoice = _context.Invoices
                .Where(i => i.InvoiceId == Id)
                .Include(d => d.InvoiceDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .FirstOrDefault(p => p.InvoiceId == Id);

            if (Invoice != null)
            {
                var InvoiceDetail = new Invoice()
                {
                    InvoiceId = Invoice.InvoiceId,
                    InvoiceNumber = Invoice.InvoiceNumber,
                    PurchaseOrderId = Invoice.PurchaseOrderId,
                    PurchaseOrderNumber = Invoice.PurchaseOrderNumber,
                    UserId = Invoice.UserId,
                    ApplicationUser = Invoice.ApplicationUser,
                    PenggunaId = Invoice.PenggunaId,
                    Pengguna = Invoice.Pengguna,
                    Termin = Invoice.Termin,
                    BengkelId = Invoice.BengkelId,
                    Bengkel = Invoice.Bengkel,
                    Status = Invoice.Status,
                    QtyTotal = Invoice.QtyTotal,
                    GrandTotal = Invoice.GrandTotal,
                    Keterangan = Invoice.Keterangan,
                    InvoiceDetails = Invoice.InvoiceDetails
                };
                return InvoiceDetail;
            }
            return Invoice;
        }

        public async Task<Invoice> GetInvoiceByIdNoTracking(Guid Id)
        {
            return await _context.Invoices.AsNoTracking()
                .Where(i => i.InvoiceId == Id)
                .Include(r => r.PurchaseOrder)
                .Include(d => d.InvoiceDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .FirstOrDefaultAsync(a => a.InvoiceId == Id);
        }

        public async Task<List<Invoice>> GetInvoices()
        {
            return await _context.Invoices.OrderBy(p => p.CreateDateTime).Select(Invoice => new Invoice()
            {
                InvoiceId = Invoice.InvoiceId,
                InvoiceNumber = Invoice.InvoiceNumber,
                PurchaseOrderId = Invoice.PurchaseOrderId,
                PurchaseOrderNumber = Invoice.PurchaseOrderNumber,
                UserId = Invoice.UserId,
                PenggunaId = Invoice.PenggunaId,
                Termin = Invoice.Termin,
                BengkelId = Invoice.BengkelId,
                Status = Invoice.Status,
                QtyTotal = Invoice.QtyTotal,
                GrandTotal = Invoice.GrandTotal,
                Keterangan = Invoice.Keterangan
            }).ToListAsync();
        }

        public async Task<List<Invoice>> GetInvoiceDetails()
        {
            return await _context.Invoices.OrderBy(p => p.CreateDateTime).Select(Invoice => new Invoice()
            {
                InvoiceId = Invoice.InvoiceId,
                InvoiceNumber = Invoice.InvoiceNumber,
                PurchaseOrderId = Invoice.PurchaseOrderId,
                PurchaseOrderNumber = Invoice.PurchaseOrderNumber,
                UserId = Invoice.UserId,
                PenggunaId = Invoice.PenggunaId,
                Termin = Invoice.Termin,
                BengkelId = Invoice.BengkelId,
                Status = Invoice.Status,
                QtyTotal = Invoice.QtyTotal,
                GrandTotal = Invoice.GrandTotal,
                Keterangan = Invoice.Keterangan
            }).ToListAsync();
        }

        public IEnumerable<Invoice> GetAllInvoice()
        {
            return _context.Invoices
                .Include(r => r.PurchaseOrder)
                .Include(d => d.InvoiceDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .ToList();
        }
    }
}
