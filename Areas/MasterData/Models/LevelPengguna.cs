using NoiSys.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.MasterData.Models
{
    [Table("MsdLevelPengguna", Schema = "dbo")]
    public class LevelPengguna : AktivitasPengguna
    {
        [Key]
        public Guid LevelId { get; set; }
        public string KodeLevel { get; set; }
        public string NamaLevel { get; set; }
        public int Persentase { get; set; }
        public string? Keterangan { get; set; }
    }
}
