using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace a1.Models
{
    public partial class Department
    {
        public Department()
        {
            Courses = new HashSet<Course>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int? InstructorId { get; set; }

        [JsonIgnore]
        public byte[] RowVersion { get; set; }

        public virtual Person Instructor { get; set; }

        [JsonIgnore]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
