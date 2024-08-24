using NoiSys.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.MasterData.Models
{
    [Table("MsdKategori", Schema = "dbo")]
    public class Kategori : AktivitasPengguna
    {
        [Key]
        public Guid KategoriId { get; set; }
        public string KodeKategori { get; set; }
        public string NamaKategori { get; set; }

        //public ICollection<Product> Product { get; set; }
    }
}
