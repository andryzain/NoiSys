using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Transaksi.Repository
{
    public class IApprovalPurchaseRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public IApprovalPurchaseRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApprovalPurchaseRequest Tambah(ApprovalPurchaseRequest approvalPurchaseRequest)
        {
            _context.ApprovalPurchaseRequests.Add(approvalPurchaseRequest);
            _context.SaveChanges();
            return approvalPurchaseRequest;
        }

        public async Task<ApprovalPurchaseRequest> GetApprovalPurchaseRequestById(Guid Id)
        {
            var approvalPurchaseRequest = await _context.ApprovalPurchaseRequests
                .Include(d => d.PurchaseRequest)
                .Include(p => p.Pengguna)
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bengkel)
                .SingleOrDefaultAsync(p => p.ApprovalId == Id);

            if (approvalPurchaseRequest != null)
            {
                var approvalPurchaseRequestDetail = new ApprovalPurchaseRequest()
                {
                    ApprovalId = approvalPurchaseRequest.ApprovalId,
                    PurchaseRequestId = approvalPurchaseRequest.PurchaseRequestId,
                    PurchaseRequestNumber = approvalPurchaseRequest.PurchaseRequestNumber,
                    UserId = approvalPurchaseRequest.UserId,
                    PenggunaId = approvalPurchaseRequest.PenggunaId,
                    BengkelId = approvalPurchaseRequest.BengkelId,
                    ApproveDate = approvalPurchaseRequest.ApproveDate,
                    ApproveBy = approvalPurchaseRequest.ApproveBy,
                    Status = approvalPurchaseRequest.Status,
                    Keterangan = approvalPurchaseRequest.Keterangan,
                };
                return approvalPurchaseRequestDetail;
            }
            return null;
        }

        public async Task<ApprovalPurchaseRequest> GetApprovalPurchaseRequestByIdNoTracking(Guid Id)
        {
            return await _context.ApprovalPurchaseRequests.AsNoTracking().FirstOrDefaultAsync(a => a.ApprovalId == Id);
        }

        public async Task<List<ApprovalPurchaseRequest>> GetApprovalPurchaseRequests()
        {
            return await _context.ApprovalPurchaseRequests.OrderBy(p => p.CreateDateTime).Select(approvalPurchaseRequest => new ApprovalPurchaseRequest()
            {
                PurchaseRequestId = approvalPurchaseRequest.PurchaseRequestId,
                PurchaseRequestNumber = approvalPurchaseRequest.PurchaseRequestNumber,
                UserId = approvalPurchaseRequest.UserId,
                PenggunaId = approvalPurchaseRequest.PenggunaId,
                BengkelId = approvalPurchaseRequest.BengkelId,
                ApproveDate = approvalPurchaseRequest.ApproveDate,
                ApproveBy = approvalPurchaseRequest.ApproveBy,
                Status = approvalPurchaseRequest.Status,
                Keterangan = approvalPurchaseRequest.Keterangan,
            }).ToListAsync();
        }

        public IEnumerable<ApprovalPurchaseRequest> GetAllApprovalPurchaseRequest()
        {
            return _context.ApprovalPurchaseRequests
                .Include(d => d.PurchaseRequest)
                .Include(p => p.Pengguna)
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bengkel)
                .AsNoTracking();
        }

        public ApprovalPurchaseRequest Update(ApprovalPurchaseRequest update)
        {
            var approve = _context.ApprovalPurchaseRequests.Attach(update);
            approve.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public ApprovalPurchaseRequest Delete(Guid Id)
        {
            var ApprovalPurchaseRequest = _context.ApprovalPurchaseRequests.Find(Id);
            if (ApprovalPurchaseRequest != null)
            {
                _context.ApprovalPurchaseRequests.Remove(ApprovalPurchaseRequest);
                _context.SaveChanges();
            }
            return ApprovalPurchaseRequest;
        }
    }
}
