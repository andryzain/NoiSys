namespace NoiSys.Areas.MasterData.ViewModels
{
    public class DetailBengkelViewModel : BengkelViewModel
    {
        public Guid BengkelId { get; set; }
        public string? BengkelPhotoPath { get; set; }
    }
}
