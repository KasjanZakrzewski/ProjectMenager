using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProjectMenager.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        [ValidateNever]
        public virtual IdentityUser Employee { get; set; }
        public int ProjectId { get; set; }
        [ValidateNever]
        public virtual Project Project { get; set; }
    }
}
