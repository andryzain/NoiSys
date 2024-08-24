namespace NoiSys.Areas.MasterData.ViewModels
{
    public class BankCabangViewModel
    {
        public Guid BankCabangId { get; set; }
        public string KodeBankCabang { get; set; }
        public string NamaCabang { get; set; }
        public string NomorRekening { get; set; }
        public string AtasNama { get; set; }
        public Guid? BankId { get; set; }
    }
}
