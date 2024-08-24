using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.Transaksi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Administrasi.Models
{
    [Table("AdmPembayaran", Schema = "dbo")]
    public class Pembayaran : AktivitasPengguna
    {
        [Key]
        public Guid PaymentId { get; set; }
        public string PaymentNumber { get; set; }
        public Guid? InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid? BankId { get; set; }
        public string UserId { get; set; }
        public Guid? PenggunaId { get; set; }
        public string Termin { get; set; }
        public Guid? BengkelId { get; set; }
        public string Status { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Keterangan { get; set; }

        //Relationship
        [ForeignKey("InvoiceId")]
        public Invoice? Invoice { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("PenggunaId")]
        public Pengguna? Pengguna { get; set; }
        [ForeignKey("BengkelId")]
        public Bengkel? Bengkel { get; set; }
        [ForeignKey("BankId")]
        public Bank? Bank { get; set; }        
    }
}
