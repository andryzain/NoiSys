using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using NoiSys.Areas.MasterData.Models;

namespace NoiSys.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the AplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string KodePengguna { get; set; }
        public string NamaLengkap { get; set; }
        public Guid? LevelId { get; set; }

        //Relationship
        [ForeignKey("LevelId")]
        public LevelPengguna? LevelPengguna { get; set; }
    }    
}
