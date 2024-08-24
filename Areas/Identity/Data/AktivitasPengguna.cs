using System.ComponentModel.DataAnnotations;

namespace NoiSys.Areas.Identity.Data
{
    public class AktivitasPengguna
    {
        public DateTime CreateDateTime { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public Guid UpdateBy { get; set; }
        public bool IsDelete { get; set; }
        public DateTime DeleteDateTime { get; set; }
        public Guid DeleteBy { get; set; }
    }
}
