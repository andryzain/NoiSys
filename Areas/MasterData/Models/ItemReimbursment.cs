using NoiSys.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.MasterData.Models
{
    [Table("MsdItemReimbursment", Schema = "dbo")]
    public class ItemReimbursment : AktivitasPengguna
    {
        [Key]
        public Guid ItemReimbursmentId { get; set; }
        public string KodeItemReimbursment { get; set; }
        public string NamaItemReimbursment { get; set; }
    }
}
