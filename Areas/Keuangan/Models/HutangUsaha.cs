using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.Keuangan.Models
{
    [Table("KeuHutangUsaha", Schema = "dbo")]
    public class HutangUsaha : AktivitasPengguna
    {
        [Key]
        public Guid HutangId { get; set; }
        public string HutangNumber { get; set; }
        public Guid? TransaksiId { get; set; }
        public string TransaksiNumber { get; set; }
        public Guid? BankId { get; set; }
        public string UserId { get; set; }
        public decimal Nominal { get; set; }

        //Relationship
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("BankId")]
        public Bank? Bank { get; set; }
    }
}
