using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.Models.DTO;

namespace UnitTest.Models.DTO
{
    public class DepartmentDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public virtual IEnumerable<EmployeeDTO> Employees { get; set; }
    }
}
