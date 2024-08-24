using NoiSys.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.MasterData.Models
{
    [Table("MsdSatuan", Schema = "dbo")]
    public class Satuan : AktivitasPengguna
    {
        [Key]
        public Guid SatuanId { get; set; }
        public string KodeSatuan { get; set; }
        public string NamaSatuan { get; set; }
    }
}
