using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//using NUnit.Framework;
using System.Threading.Tasks;
using UnitTest_Mock.Controllers;
using UnitTest_Mock.Models;
using UnitTest_Mock.Services;
//using Xunit;

namespace UnitTest_Mock_Test
{
    public class EmployeeTest
    {
        Mock<IEmployeeService> mockEmp = new Mock<IEmployeeService>();

        //[Fact]
        [TestMethod]
        public async Task GetEmployeeId()
        {
            string expectedEmployeeName = "Phuc";
            mockEmp.Setup(p => p.GetEmployeebyId(1)).ReturnsAsync(expectedEmployeeName);
            EmployeeController empCotrol = new EmployeeController(mockEmp.Object);
            var employeeName = await empCotrol.GetEmployeeById(1);
            Assert.Equals(expectedEmployeeName, employeeName);
        }

        [TestMethod]
        public async Task GetEmployeeDetails()
        {
            var employeeDTO = new Employee()
            {
                Id = 1,
                Name = "JK",
                Desgination = "SDE"
            };
            mockEmp.Setup(p => p.GetEmployeeDetails(1)).ReturnsAsync(employeeDTO);
            EmployeeController emp = new EmployeeController(mockEmp.Object);
            var result = await emp.GetEmployeeDetails(1);
            Assert.IsTrue(employeeDTO.Equals(result));
        }
    }
}