using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Order.Models
{
    [Table("OrdPembayaranBarang", Schema = "dbo")]
    public class PembayaranBarang : AktivitasPengguna
    {
        [Key]
        public Guid PaymentId { get; set; }
        public string PaymentNumber { get; set; }
        public Guid? PembelianId { get; set; }
        public string PembelianNumber { get; set; }
        public Guid? BankId { get; set; }
        public string UserId { get; set; }
        public Guid? PenggunaId { get; set; }
        public Guid? DisetujuiOlehId { get; set; }
        public string Termin { get; set; }
        public string Status { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Keterangan { get; set; }

        //Relationship
        [ForeignKey("PembelianId")]
        public Pembelian? Pembelian { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("DisetujuiOlehId")]
        public Pengguna? DisetujuiOleh { get; set; }
        [ForeignKey("PenggunaId")]
        public Pengguna? Pengguna { get; set; }
        [ForeignKey("BankId")]
        public Bank? Bank { get; set; }
    }
}
