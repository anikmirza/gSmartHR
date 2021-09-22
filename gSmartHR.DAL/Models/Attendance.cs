using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gSmartHR.DAL.Models
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
	    public DateTime StartDateTime { get; set; }
	    public DateTime? EndDateTime { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
