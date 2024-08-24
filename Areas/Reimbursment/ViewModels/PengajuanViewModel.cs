using NoiSys.Areas.Reimbursment.Models;

namespace NoiSys.Areas.Reimbursment.ViewModels
{
    public class PengajuanViewModel
    {
        public Guid PengajuanId { get; set; }
        public string PengajuanNumber { get; set; }
        public string UserId { get; set; }
        public Guid? BankId { get; set; }
        public string NomorRekening { get; set; }
        public string AtasNama { get; set; }
        public string Status { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public List<PengajuanDetail> PengajuanDetails { get; set; } = new List<PengajuanDetail>();
    }
}
