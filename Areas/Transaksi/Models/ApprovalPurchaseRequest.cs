using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Transaksi.Models
{
    [Table("TrsApprovalPurchaseRequest", Schema = "dbo")]
    public class ApprovalPurchaseRequest : AktivitasPengguna
    {
        [Key]
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

        //Relationship
        [ForeignKey("PurchaseRequestId")]
        public PurchaseRequest? PurchaseRequest { get; set; }
        [ForeignKey("PenggunaId")]
        public Pengguna? Pengguna { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("BengkelId")]
        public Bengkel? Bengkel { get; set; }
    }
}
