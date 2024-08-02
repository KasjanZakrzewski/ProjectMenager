using System.ComponentModel.DataAnnotations;

namespace ProjectMenager.Models.DTO
{
    public class ProjectDTO
    {
        public string Name { get; set; }
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Manager { get; set; }
    }
}
