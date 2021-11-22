using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desgination { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
