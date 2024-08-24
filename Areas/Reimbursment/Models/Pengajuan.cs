using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.Transaksi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Reimbursment.Models
{
    [Table("RbsPengajuan", Schema = "dbo")]
    public class Pengajuan : AktivitasPengguna
    {
        [Key]
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
        
        //Relationship
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("BankId")]
        public Bank? Bank { get; set; }
    }

    [Table("RbsPengajuanDetail", Schema = "dbo")]
    public class PengajuanDetail : AktivitasPengguna
    {
        [Key]
        public Guid PengajuanDetailId { get; set; }
        public Guid PengajuanId { get; set; }
        public string NamaItem { get; set; }
        public string KodeItem { get; set; }
        public string Qty { get; set; }
        public string Nominal { get; set; }
        public string SubTotal { get; set; }
        public string? Catatan { get; set; }

        //Relationship
        [ForeignKey("PengajuanId")]
        public Pengajuan? Pengajuan { get; set; }
    }
}
