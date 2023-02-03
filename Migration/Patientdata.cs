namespace HosipitalWebApplication.Migration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Patientdata
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string HealthIssues { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        public DateTime CreatedOnDateTime { get; set; }

        public DateTime LastVisitDateTime { get; set; }

        public int CreatedByUserId { get; set; }

        public int UpdatedByUserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
