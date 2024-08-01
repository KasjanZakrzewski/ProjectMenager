using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProjectMenager.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public int TaskId { get; set; }
        [ValidateNever]
        public virtual Task Task { get; set; }
    }
}
