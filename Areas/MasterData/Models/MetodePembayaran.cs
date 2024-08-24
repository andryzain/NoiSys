using NoiSys.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.MasterData.Models
{
    [Table("MsdMetodePembayaran", Schema = "dbo")]
    public class MetodePembayaran : AktivitasPengguna
    {
        [Key]
        public Guid MetodePembayaranId { get; set; }
        public string KodeMetodePembayaran { get; set; }
        public string NamaMetodePembayaran { get; set; }
    }
}
