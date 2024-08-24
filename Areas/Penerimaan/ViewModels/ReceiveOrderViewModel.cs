using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Penerimaan.Models;
using NoiSys.Areas.Transaksi.Models;

namespace NoiSys.Areas.Penerimaan.ViewModels
{
    public class ReceiveOrderViewModel
    {
        public Guid ReceiveOrderId { get; set; }
        public string ReceiveOrderNumber { get; set; }
        public Guid? PembelianId { get; set; }
        public string ReceiveById { get; set; }
        public string Status { get; set; }
        public string? Catatan { get; set; }
        public List<PembelianDetail> PembelianDetails { get; set; } = new List<PembelianDetail>();
        public List<ReceiveOrderDetail> ReceiveOrderDetails { get; set; } = new List<ReceiveOrderDetail>();
    }
}
