using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Test.Enum;
using Test.model;
using Test.Repository;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository _repository;

        public EmployeeController(
            IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Employee
        [HttpGet]
    
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string names = "dj";
                var result = await _repository.GetAll();
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/Employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _repository.GetById(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Employee not found"
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Employee data is required"
                    });
                }
                //employee.Id = Guid.NewGuid();
                //var result = await _repository.Add(employee);
                var  id= await _repository.Add(employee);
                employee.Id = id;

                
                return Ok(new
                {
                    success = true,
                    message = "Employee created successfully",
                    data=employee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Employee data is required"
                    });
                }

                bool result =
                    await _repository.Update(id, employee);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Employee not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Employee updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result =
                    await _repository.Delete(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Employee not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Employee deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
