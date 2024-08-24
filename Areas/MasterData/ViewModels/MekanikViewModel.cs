namespace NoiSys.Areas.MasterData.ViewModels
{
    public class MekanikViewModel
    {
        public Guid MekanikId { get; set; }
        public string KodeMekanik { get; set; }
        public string NamaMekanik { get; set; }
        public Guid? BengkelId { get; set; }
    }
}
