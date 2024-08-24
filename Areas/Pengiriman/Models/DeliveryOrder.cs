using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.Transaksi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Pengiriman.Models
{
    [Table("DlvDeliveryOrder", Schema = "dbo")]
    public class DeliveryOrder : AktivitasPengguna
    {
        [Key]
        public Guid DeliveryOrderId { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string UserId { get; set; }
        public Guid? PenggunaId { get; set; }
        public string Termin { get; set; }
        public Guid? BengkelId { get; set; }
        public string Status { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Keterangan { get; set; }
        public List<DeliveryOrderDetail> DeliveryOrderDetails { get; set; } = new List<DeliveryOrderDetail>();

        //Relationship
        [ForeignKey("PurchaseOrderId")]
        public PurchaseOrder? PurchaseOrder { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("PenggunaId")]
        public Pengguna? Pengguna { get; set; }
        [ForeignKey("BengkelId")]
        public Bengkel? Bengkel { get; set; }
    }

    [Table("DlvDeliveryOrderDetail", Schema = "dbo")]
    public class DeliveryOrderDetail : AktivitasPengguna
    {
        [Key]
        public Guid DeliveryOrderDetailId { get; set; }
        public Guid? DeliveryOrderId { get; set; }
        public string NamaProduk { get; set; }
        public string KodeProduk { get; set; }
        public string Satuan { get; set; }
        public string Principal { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public int Diskon { get; set; }
        public decimal SubTotal { get; set; }
        public bool Checked { get; set; }

        //Relationship
        [ForeignKey("DeliveryOrderId")]
        public DeliveryOrder? DeliveryOrder { get; set; }
    }
}
