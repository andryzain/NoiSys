using NoiSys.Areas.Transaksi.Models;

namespace NoiSys.Areas.Transaksi.ViewModels
{
    public class ApprovalPurchaseRequestViewModel
    {
        public Guid ApprovalId { get; set; }
        public Guid? PurchaseRequestId { get; set; }
        public string PurchaseRequestNumber { get; set; }
        public string UserId { get; set; }
        public Guid? PenggunaId { get; set; }
        public Guid? BengkelId { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public string Status { get; set; }
        public string? Keterangan { get; set; }
        public List<PurchaseRequestDetail> PurchaseRequestDetails { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
