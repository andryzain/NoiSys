namespace NoiSys.Areas.MasterData.ViewModels
{
    public class PrincipalViewModel
    {
        public Guid PrincipalId { get; set; }
        public string KodePrincipal { get; set; }
        public string NamaPrincipal { get; set; }
        public string Alamat { get; set; }
        public string NomorTelepon { get; set; }
        public string Email { get; set; }
        public string? Keterangan { get; set; }
    }
}
