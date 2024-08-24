using NoiSys.Areas.MasterData.Models;

namespace NoiSys.Areas.MasterData.ViewModels
{
    public class BankViewModel
    {
        public Guid BankId { get; set; }
        public string KodeBank { get; set; }
        public string NamaBank { get; set; }
    }
}
