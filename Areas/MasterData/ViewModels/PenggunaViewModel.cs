namespace NoiSys.Areas.MasterData.ViewModels
{
    public class PenggunaViewModel
    {
        public Guid PenggunaId { get; set; }
        public string KodePengguna { get; set; }
        public string NamaLengkap { get; set; }
        public string NomorIdentitas { get; set; }
        public Guid? LevelId { get; set; }
        public string? LevelPengguna { get; set; }
        public string TempatLahir { get; set; }
        public DateTime TanggalLahir { get; set; } = DateTime.Now;
        public string JenisKelamin { get; set; }
        public string AlamatLengkap { get; set; }
        public string NomorHandphone { get; set; }
        public string Email { get; set; }
        public IFormFile? Foto { get; set; }
    }
}
