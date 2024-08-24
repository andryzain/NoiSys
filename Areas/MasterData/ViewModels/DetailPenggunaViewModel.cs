namespace NoiSys.Areas.MasterData.ViewModels
{
    public class DetailPenggunaViewModel : PenggunaViewModel
    {
        public Guid PenggunaId { get; set; }
        public string? PenggunaPhotoPath { get; set; }
    }
}
