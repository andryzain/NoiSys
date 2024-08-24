using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.Transaksi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Reimbursment.Models
{
    [Table("RbsPersetujuan", Schema = "dbo")]
    public class Persetujuan : AktivitasPengguna
    {
        [Key]
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
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public List<PengajuanDetail> PengajuanDetails { get; set; }

        //Relationship
        [ForeignKey("PengajuanId")]
        public Pengajuan? Pengajuan { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("BankId")]
        public Bank? Bank { get; set; }
    }
}
