namespace NoiSys.Areas.MasterData.ViewModels
{
    public class LevelPenggunaViewModel
    {
        public Guid LevelId { get; set; }
        public string KodeLevel { get; set; }
        public string NamaLevel { get; set; }
        public int Persentase { get; set; }
        public string? Keterangan { get; set; }
    }
}
