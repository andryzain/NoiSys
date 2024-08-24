namespace NoiSys.Areas.Order.ViewModels
{
    public class PembayaranBarangViewModel
    {
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
    }
}
