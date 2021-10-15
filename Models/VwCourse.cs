using System;
using System.Collections.Generic;

#nullable disable

namespace a1.Models
{
    public partial class VwCourse
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
