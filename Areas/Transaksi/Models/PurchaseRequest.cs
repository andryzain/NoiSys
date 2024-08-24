using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Transaksi.Models
{
    [Table("TrsPurchaseRequest", Schema = "dbo")]
    public class PurchaseRequest : AktivitasPengguna
    {
        [Key]
        public Guid PurchaseRequestId { get; set; }
        public string PurchaseRequestNumber { get; set; }
        public string UserId { get; set; }
        public Guid? PenggunaId { get; set; }
        public string Termin { get; set; }
        public Guid? BengkelId { get; set; }
        public string Status { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Keterangan { get; set; }
        public List<PurchaseRequestDetail> PurchaseRequestDetails { get; set; } = new List<PurchaseRequestDetail>();

        //Relationship
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("PenggunaId")]
        public Pengguna? Pengguna { get; set; }
        [ForeignKey("BengkelId")]
        public Bengkel? Bengkel { get; set; }
    }

    [Table("TrsPurchaseRequestDetail", Schema = "dbo")]
    public class PurchaseRequestDetail : AktivitasPengguna
    {
        [Key]
        public Guid PurchaseRequestDetailId { get; set; }
        public Guid PurchaseRequestId { get; set; }
        public string NamaProduk { get; set; }
        public string KodeProduk { get; set; }        
        public string Satuan { get; set; }
        public string Principal { get; set; }        
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public int Diskon { get; set; }
        public decimal SubTotal { get; set; }

        //Relationship
        [ForeignKey("PurchaseRequestId")]
        public PurchaseRequest? PurchaseRequest { get; set; }
    }
}
