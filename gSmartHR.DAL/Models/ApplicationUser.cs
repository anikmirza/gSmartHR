using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gSmartHR.DAL.Models
{
    [Table("ApplicationUser")]
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Employee Employee { get; set; }
    }
}
