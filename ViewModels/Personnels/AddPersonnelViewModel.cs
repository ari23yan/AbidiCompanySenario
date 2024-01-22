using System.ComponentModel.DataAnnotations;

namespace AbidiCompanySenario.ViewModels.Personnels
{
    public class AddPersonnelViewModel
    {

        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        [Required]
        public long Personnel_Code { get; set; }
        [Required]
        public string National_Code { get; set; }
    }
}
