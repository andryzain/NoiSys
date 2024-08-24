namespace NoiSys.Areas.MasterData.ViewModels
{
    public class BengkelViewModel
    {
        public Guid BengkelId { get; set; }
        public string KodeBengkel { get; set; }
        public string NamaBengkel { get; set; }
        public string PenanggungJawab { get; set; }
        public string NamaPemilik { get; set; }
        public string Alamat { get; set; }
        public string NomorTelepon { get; set; }
        public string Email { get; set; }
        public string? Keterangan { get; set; }
        public IFormFile? Foto { get; set; }
    }
}
