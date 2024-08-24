namespace NoiSys.Areas.Transaksi.ViewModels
{
    public class PurchaseOrderDetailViewModel
    {
        public Guid PurchaseOrderDetailId { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public string NamaProduk { get; set; }
        public string KodeProduk { get; set; }
        public string Satuan { get; set; }
        public string Principal { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public int Diskon { get; set; }
        public decimal SubTotal { get; set; }
        public bool Checked { get; set; }
    }
}
