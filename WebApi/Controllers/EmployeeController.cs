using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = Log.Logger;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var items = await _employeeService.GetAllEmployeesAsync();
                if (!items.Any()) return NotFound();
                return Ok(_mapper.Map<IEnumerable<EmployeeResponseDto>>(items));
            }
            catch (Exception ex)
            {
                _logger.Error("An unhandled error occured", ex);
                return InternalServerError();
            }

        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("api/employee/{employeeCode}")]
        public async Task<IHttpActionResult> Get(string employeeCode)
        {
            try
            {
                var item = await _employeeService.GetEmployeeByCodeAsync(employeeCode);
                if (item == null) return NotFound();
                return Ok(_mapper.Map<EmployeeResponseDto>(item));
            }
            catch (Exception ex)
            {
                _logger.Error("An unhandled error occured", ex);
                return InternalServerError();
            }

        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] EmployeeDto employeeDto)
        {
            try
            {
                var request = _mapper.Map<EmployeeInfo>(employeeDto);
                var result = await _employeeService.AddOrUpdateEmployeeAsync(request);
                if (!result) return BadRequest("Adding new company failed!");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error("An unhandled error occured", ex);
                return InternalServerError();
            }


        }
    }
}