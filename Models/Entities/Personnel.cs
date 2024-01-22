using System.ComponentModel.DataAnnotations;

namespace AbidiCompanySenario.Models.Entities
{
    public class Personnel
    {
        [Key]
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
        [MaxLength(64)]
        public string National_Code { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
