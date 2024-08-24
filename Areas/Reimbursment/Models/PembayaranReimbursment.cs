using NoiSys.Areas.Administrasi.Models;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Reimbursment.Models
{
    [Table("RbsPembayaranReimbursment", Schema = "dbo")]
    public class PembayaranReimbursment : AktivitasPengguna
    {
        [Key]
        public Guid PaymentId { get; set; }
        public string PaymentNumber { get; set; }
        public Guid? PengajuanId { get; set; }
        public string PengajuanNumber { get; set; }        
        public string UserId { get; set; }
        public Guid? BankId { get; set; }
        public string NomorRekening { get; set; }
        public string AtasNama { get; set; }
        public string Status { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Keterangan { get; set; }

        //Relationship
        [ForeignKey("PengajuanId")]
        public Pengajuan? Pengajuan { get; set; }
        [ForeignKey("BankId")]
        public Bank? Bank { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
