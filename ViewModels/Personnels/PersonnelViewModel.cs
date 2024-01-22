using System.ComponentModel.DataAnnotations;

namespace AbidiCompanySenario.ViewModels.Personnels
{
    public class PersonnelViewModel
    {
        public long Personnel_Id { get; set; }
        [Required]

        [MaxLength(64)]
        public string First_Name { get; set; }
        [Required]

        [MaxLength(64)]
        public string Last_Name { get; set; }
        [Required]

        public long Personnel_Code { get; set; }
        [Required]

        public long National_Code { get; set; }
        public DateTime CreateDate { get; set; } 
        public bool IsDeleted { get; set; } 
    }
}
