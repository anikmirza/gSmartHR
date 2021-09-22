using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gSmartHR.DAL.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeIdNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? JoiningDate { get; set; }
	    public string Email { get; set; }
	    public string ContactNo { get; set; }
	    public string NationalId { get; set; }
	    public string OfficeName { get; set; }
	    public string Department { get; set; }
	    public string Designaton { get; set; }
	    public string EmployeeImagePath { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
