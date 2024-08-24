using System.ComponentModel.DataAnnotations;

namespace NoiSys.Areas.MasterData.ViewModels
{
    public class ProdukViewModel
    {
        public Guid ProdukId { get; set; }
        public string KodeProduk { get; set; }
        public string NamaProduk { get; set; }
        [Required]
        public Guid? PrincipalId { get; set; }
        [Required]
        public Guid? KategoriId { get; set; }
        public int JumlahStok { get; set; }
        [Required]
        public Guid? SatuanId { get; set; }
        public decimal HargaBeli { get; set; }
        public decimal HargaJual { get; set; }
        public decimal Cogs { get; set; }
        [Required]
        public Guid? DiskonId { get; set; }
        public string? Catatan { get; set; }
    }
}
