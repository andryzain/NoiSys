using NoiSys.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace NoiSys.Areas.MasterData.Models
{
    [Table("MsdProduk", Schema = "dbo")]
    public class Produk : AktivitasPengguna
    {
        [Key]
        public Guid ProdukId { get; set; }
        public string KodeProduk { get; set; }
        public string NamaProduk { get; set; }        
        public Guid? PrincipalId { get; set; }
        public Guid? KategoriId { get; set; }
        public int JumlahStok { get; set; }
        public Guid? SatuanId { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal HargaBeli { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal HargaJual { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal Cogs { get; set; }
        public Guid? DiskonId { get; set; }
        public string? Catatan { get; set; }
        
        //Relationship        
        [ForeignKey("PrincipalId")]
        public Principal? Principal { get; set; }
        [ForeignKey("KategoriId")]
        public Kategori? Kategori { get; set; }
        [ForeignKey("SatuanId")]
        public Satuan? Satuan { get; set; }
        [ForeignKey("DiskonId")]
        public Diskon? Diskon { get; set; }
    }
}
