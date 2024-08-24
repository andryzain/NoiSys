namespace NoiSys.Areas.Order.ViewModels
{
    public class PembelianViewModel
    {
        public Guid PembelianId { get; set; }
        public string PembelianNumber { get; set; }
        public Guid? PermintaanId { get; set; }
        public string PermintaanNumber { get; set; }
        public string UserId { get; set; }
        public Guid? PenggunaId { get; set; } //Sebagai Mengetahui Atasan
        public Guid? DisetujuiOlehId { get; set; }
        public Guid? BankId { get; set; }
        public string Termin { get; set; }
        public string Status { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Keterangan { get; set; }        
    }
}
