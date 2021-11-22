using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest.Models.DTO
{
    public class EmployeeDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Desgination { get; set; }

        public virtual DepartmentDTO Department { get; set; }
    }

}
