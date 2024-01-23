﻿using System.ComponentModel.DataAnnotations;

namespace AbidiCompanySenario.Models.Entities
{
    public class AcademicDegree
    {
        public long Id { get; set; }
        [Required]

        public long PersonnelId { get; set; }
        [Required]
        [MaxLength(256)]
        public string FileName { get; set; }
        [MaxLength(64)]
        public string FileType { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public Personnel Personnel { get; set; }
    }
}
