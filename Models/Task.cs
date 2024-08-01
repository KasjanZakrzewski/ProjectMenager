using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ProjectMenager.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        public int ItemId { get; set; }
        [ValidateNever]
        public virtual Item Item { get; set; }
        public string EmployeeId { get; set; }
        [ValidateNever]
        public virtual IdentityUser Employee { get; set; }
    }
}
