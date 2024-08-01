using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ProjectMenager.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ManagerId { get; set; }
        [ValidateNever]
        public virtual IdentityUser Manager { get; set; }
    }
}
