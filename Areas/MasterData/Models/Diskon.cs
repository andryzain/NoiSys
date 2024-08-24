using NoiSys.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.MasterData.Models
{
    [Table("MsdDiskon", Schema = "dbo")]
    public class Diskon : AktivitasPengguna
    {
        public Guid DiskonId { get; set; }
        public string KodeDiskon { get; set; }
        public int Nilai { get; set; }
    }
}
