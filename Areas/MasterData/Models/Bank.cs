using NoiSys.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiSys.Areas.MasterData.Models
{
    [Table("MsdBank", Schema = "dbo")]
    public class Bank : AktivitasPengguna
    {
        [Key]
        public Guid BankId { get; set; }
        public string KodeBank { get; set; }
        public string NamaBank { get; set; }
        public ICollection<BankCabang> BankCabang { get; set; }
    }

    [Table("MsdBankCabang", Schema = "dbo")]
    public class BankCabang : AktivitasPengguna
    {
        [Key]
        public Guid BankCabangId { get; set; }
        public string KodeBankCabang { get; set; }
        public string NamaCabang { get; set; }
        public string NomorRekening { get; set; }
        public string AtasNama { get; set; }
        public Guid? BankId { get; set; }

        //Relationship
        [ForeignKey("BankId")]
        public Bank? Bank { get; set; }
    }
}
