namespace NoiSys.Areas.Administrasi.ViewModels
{
    public class InvoiceViewModel
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public Guid? BankId { get; set; }
        public string UserId { get; set; }
        public Guid? PenggunaId { get; set; }
        public string Termin { get; set; }
        public Guid? BengkelId { get; set; }
        public string Status { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Keterangan { get; set; }
    }
}
