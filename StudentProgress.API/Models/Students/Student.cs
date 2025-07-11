using StudentProgress.API.Models.Auth;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProgress.API.Models.Students
{
    public class Student:BaseEntity
    {
        public string FullName { get; set; }
        public string Grade { get; set; }
        [ForeignKey("Teacher")]
        public Guid? TeacherId { get; set; }
        public ApplicationUser? Teacher { get; set; }
        public List<Progress> ProgressRecords { get; set; }
    }
}
