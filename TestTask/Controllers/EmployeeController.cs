using Microsoft.AspNetCore.Mvc;
using TestTask.Contracts;
using TestTask.Dto;

namespace TestTask.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeForCreationDto employee)
        {
            try
            {
                var createdEmployee = await _employeeRepo.CreateEmployee(employee);
                return Ok(createdEmployee.Id);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, EmployeeForUpdateDto employee)
        {
            try
            {
                var dbEmployee = await _employeeRepo.GetEmployee(id);
                if (dbEmployee == null)
                    return NotFound();

                await _employeeRepo.UpdateEmployee(id, employee);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var dbEmployee = await _employeeRepo.GetEmployee(id);
                if (dbEmployee == null)
                    return NotFound();

                await _employeeRepo.DeleteEmployee(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetEmployeesByCompanyId")]
        public async Task<IActionResult> GetEmployeesByCompanyId(int companyId)
        {
            try
            {
                var employees = await _employeeRepo.GetEmployeesByCompanyId(companyId);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetEmployeesByDepartamentName")]
        public async Task<IActionResult> GetEmployeesByDepartamentName(string DepartamentName)
        {
            try
            {
                var employees = await _employeeRepo.GetEmployeesByDepartamentName(DepartamentName);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
