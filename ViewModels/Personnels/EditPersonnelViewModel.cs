using System.ComponentModel.DataAnnotations;

namespace AbidiCompanySenario.ViewModels.Personnels
{
    public class EditPersonnelViewModel
    {
        public long Personnel_Id { get; set; }

        [MaxLength(64)]
        public string First_Name { get; set; }

        [MaxLength(64)]
        public string Last_Name { get; set; }

        public List<IFormFile>? AcademicDegrees { get; set; }

    }
}
