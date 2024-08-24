using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.Transaksi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Order.Models
{
    [Table("OrdPembelian", Schema = "dbo")]
    public class Pembelian : AktivitasPengguna
    {
        [Key]
        public Guid PembelianId { get; set; }
        public string PembelianNumber { get; set; }
        public Guid? PermintaanId { get; set; }
        public string PermintaanNumber { get; set; }
        public string UserId { get; set; }
        public Guid? PenggunaId { get; set; } //Sebagai Mengetahui Atasan
        public Guid? DisetujuiOlehId { get; set; }
        public string Termin { get; set; }
        public string Status { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Keterangan { get; set; }
        public List<PembelianDetail> PembelianDetails { get; set; } = new List<PembelianDetail>();

        //Relationship
        [ForeignKey("PermintaanId")]
        public Permintaan? Permintaan { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("PenggunaId")]
        public Pengguna? Pengguna { get; set; }
        [ForeignKey("DisetujuiOlehId")]
        public Pengguna? DisetujuiOleh { get; set; }
    }

    [Table("OrdPembelianDetail", Schema = "dbo")]
    public class PembelianDetail : AktivitasPengguna
    {
        [Key]
        public Guid PembelianDetailId { get; set; }
        public Guid PembelianId { get; set; }
        public string NamaProduk { get; set; }
        public string KodeProduk { get; set; }
        public string Satuan { get; set; }
        public string Principal { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public int Diskon { get; set; }
        public decimal SubTotal { get; set; }

        //Relationship
        [ForeignKey("PembelianId")]
        public Pembelian? Pembelian { get; set; }
    }
}
