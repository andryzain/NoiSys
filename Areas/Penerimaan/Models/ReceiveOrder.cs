using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Transaksi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Penerimaan.Models
{
    [Table("RcvReceiveOrder", Schema = "dbo")]
    public class ReceiveOrder : AktivitasPengguna
    {
        [Key]
        public Guid ReceiveOrderId { get; set; }
        public string ReceiveOrderNumber { get; set; }
        public Guid? PembelianId { get; set; }
        public string ReceiveById { get; set; }
        public string Status { get; set; }
        public string? Catatan { get; set; }
        public List<PembelianDetail> PembelianDetails { get; set; } = new List<PembelianDetail>();
        public List<ReceiveOrderDetail> ReceiveOrderDetails { get; set; } = new List<ReceiveOrderDetail>();

        //Relationship        
        [ForeignKey("PembelianId")]
        public Pembelian? Pembelian { get; set; }
        [ForeignKey("ReceiveById")]
        public ApplicationUser? ApplicationUser { get; set; }
    }

    [Table("RcvReceiveOrderDetail", Schema = "dbo")]
    public class ReceiveOrderDetail : AktivitasPengguna
    {
        [Key]
        public Guid ReceivedOrderDetailId { get; set; }
        public Guid ReceiveOrderId { get; set; }
        public string NamaProduk { get; set; }
        public string KodeProduk { get; set; }
        public string Satuan { get; set; }
        public int QtyDiOrder { get; set; }
        public int QtyDiTerima { get; set; }

        //Relationship        
        [ForeignKey("ReceiveOrderId")]
        public ReceiveOrder? ReceiveOrder { get; set; }
    }
}
