using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.Models;
using UnitTest.Models.DTO;

namespace UnitTest
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
        }
    }
}
