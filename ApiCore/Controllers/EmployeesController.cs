using ApiCore.Model;
using ApiCore.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using System.Net;

namespace ApiCore.Controllers
{
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesController> _logger;
        private readonly ApiResponse _apiResponse;
        public EmployeesController(ApplicationDbContext context,ILogger<EmployeesController> logger)
        {
            _context = context;
            _logger = logger;
            _apiResponse = new ApiResponse();
        }

        #endregion

        #region GetAll
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeeDto))]
        public async Task<ApiResponse> GetAll()
        {
            try
            {

                IEnumerable<EmployeeeDto> employeeeDtos = await _context.Employees
                                                                .Select(e => new EmployeeeDto { Id = e.Id, Name = e.Name, Salary = e.Salary, UAN = e.UAN }).ToListAsync();
                _logger.LogDebug("Get All has been executed successfully....");
               
                _apiResponse.Result = employeeeDtos;
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return _apiResponse;

            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess= false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessage = ex.Message;
                return _apiResponse;

            }

        }

        #endregion

        #region GetById

        [HttpGet("{id:int}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(EmployeeeDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(EmployeeeDto))]


        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Employeee? employeee = await _context.Employees.FindAsync(id);

            if (employeee == null)
            {
                return NotFound();
            }
            EmployeeeDto employeeeDto = new() { Id = employeee.Id, Name = employeee.Name, Salary = employeee.Salary, UAN = employeee.UAN };
            return Ok(employeeeDto);

        }
        #endregion

        #region Create
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<IActionResult> Create([FromBody] EmployeeeDto employeeeDto)
        {
            if (employeeeDto == null) return BadRequest();
            Employeee employeee = new()
            {
                Name = employeeeDto.Name,
                Salary = employeeeDto.Salary,
                UAN = employeeeDto.UAN,
                CreatedDate = DateTime.UtcNow
            };
            await _context.Employees.AddAsync(employeee);
            await _context.SaveChangesAsync();

            employeeeDto.Id = employeee.Id;

            return CreatedAtRoute("GetById", new { employeee.Id }, employeee);
        }
        #endregion

        #region Update
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> Update(int id, [FromBody] EmployeeeDto employeeeDto)
        {
            if (id == null || id == 0 || id != employeeeDto.Id) return BadRequest();

            Employeee? employeeeInDb = await _context.Employees.FindAsync(id);
            if (employeeeInDb == null) return BadRequest();

            employeeeInDb.Name = employeeeDto.Name;
            employeeeInDb.Salary = employeeeDto.Salary;
            employeeeInDb.UAN = employeeeDto.UAN;

            _context.Employees.Update(employeeeInDb);
            await _context.SaveChangesAsync();

            return NoContent();

        }
        #endregion

        #region Delete
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();

            Employeee? employeee = await _context.Employees.FindAsync(id);

            if (employeee == null) return NotFound();

            _context.Employees.Remove(employeee);
            await _context.SaveChangesAsync();

            return NoContent();

        }
        #endregion

        #region Patch
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartialUpdate(int id,[FromBody] JsonPatchDocument<EmployeeeDto> jsonPatch)
        {
            if(id == null || jsonPatch == null) return BadRequest();
            Employeee? employeeeInDb = await _context.Employees.AsNoTracking().SingleOrDefaultAsync(e => e.Id==id);

            if (employeeeInDb == null) return NotFound();

            EmployeeeDto employeeeDto = new()
            {
                Id = employeeeInDb.Id,
                Name = employeeeInDb.Name,
                Salary = employeeeInDb.Salary,
                UAN = employeeeInDb.UAN,
            };

            jsonPatch.ApplyTo(employeeeDto);

            Employeee employeee = new()
            {
                Id = employeeeDto.Id,
                Name = employeeeDto.Name,
                Salary= employeeeDto.Salary,
                UAN = employeeeDto.UAN,
            };

            _context.Employees.Update(employeee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion

    }
}
