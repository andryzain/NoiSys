using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Areas.Transaksi.Models;

namespace NoiSys.Areas.Reimbursment.ViewModels
{
    public class PersetujuanViewModel
    {
        public Guid PersetujuanId { get; set; }
        public Guid? PengajuanId { get; set; }
        public string PengajuanNumber { get; set; }
        public string UserId { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public Guid? BankId { get; set; }
        public string NomorRekening { get; set; }
        public string AtasNama { get; set; }
        public string Status { get; set; }
        public string? Keterangan { get; set; }
        public List<PengajuanDetail> PengajuanDetails { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
