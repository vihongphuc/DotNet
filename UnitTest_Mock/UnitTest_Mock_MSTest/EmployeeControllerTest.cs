using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using UnitTest_Mock.Controllers;
using UnitTest_Mock.Models;
using UnitTest_Mock.Services;

namespace UnitTest_Mock_MSTest
{
    //http://www.22bugs.co/post/Mocking-DbContext/

    [TestClass]
    public class EmployeeControllerTest
    {
        Mock<IEmployeeService> mockEmp = new Mock<IEmployeeService>();

        //[Fact]
        [TestMethod]
        public async Task GetEmployeeId_ReturnCorrect()
        {
            string expectedEmployeeName = "Hong Phuc";
            mockEmp.Setup(p => p.GetEmployeebyId(1)).ReturnsAsync(expectedEmployeeName);
            EmployeeController empCotrol = new EmployeeController(mockEmp.Object);
            var employeeName = await empCotrol.GetEmployeeById(1);
            Assert.AreEqual(expectedEmployeeName, employeeName);
        }

        [TestMethod]
        public async Task GetEmployeeId_ReturnNotNull()
        {
            string expectedEmployeeName = "Hong Phuc";
            mockEmp.Setup(p=> p.GetEmployeebyId(1)).ReturnsAsync(expectedEmployeeName);
            EmployeeController empControl = new EmployeeController(mockEmp.Object);
            var employeeName = await empControl.GetEmployeeById(1);
            Assert.IsTrue(!string.IsNullOrEmpty(employeeName) && !string.IsNullOrWhiteSpace(employeeName));
        }

        [TestMethod]
        public async Task GetEmployeeId_CallServiceOneTime()
        {
            string expectedEmployeeName = "Hong Phuc";
            mockEmp.Setup(p => p.GetEmployeebyId(1)).ReturnsAsync(expectedEmployeeName);
            EmployeeController empControl = new EmployeeController(mockEmp.Object);
            var employeeName = await empControl.GetEmployeeById(1);
            mockEmp.Verify(p => p.GetEmployeebyId(1), Times.Once);
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
            //Assert.IsTrue(employeeDTO.Equals(result));
            Assert.AreEqual<Employee>(employeeDTO, result);
        }
    }
}