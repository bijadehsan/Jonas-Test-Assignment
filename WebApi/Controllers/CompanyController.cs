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
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        //private readonly ILogger<CompanyController> _logger;
        private readonly ILogger _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = Log.Logger;
        }
        // GET api/<controller>
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var items = await _companyService.GetAllCompaniesAsync();
                if (!items.Any()) return NotFound(); // when there is no company to return
                return Ok(_mapper.Map<IEnumerable<CompanyDto>>(items));
            }
            catch (Exception ex)
            {
                _logger.Information("An unhandled error occured", ex);
                return InternalServerError();
            }

        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("api/company/{companyCode}")]
        public async Task<IHttpActionResult> Get(string companyCode)
        {
            try
            {
                var item = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (item == null) return NotFound();
                return Ok(_mapper.Map<CompanyDto>(item));
            }
            catch (Exception ex)
            {
                _logger.Error("An unhandled error occured", ex);
                return InternalServerError();
            }

        }

        // POST api/<controller>
        [HttpPost, HttpPut]
        public async Task<IHttpActionResult> PostOrPut([FromBody] CompanyDto companyDto)
        {
            try
            {
                var request = _mapper.Map<CompanyInfo>(companyDto);
                var result = await _companyService.AddOrUpdateCompanyAsync(request);
                if (!result) return BadRequest("Creating new company failed!");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error("An unhandled error occured", ex);
                return InternalServerError();
            }
        }

        // PUT api/<controller>/5
        //[HttpPut]
        //public async Task<IHttpActionResult> Put([FromBody] CompanyDto companyDto)
        //{
        //    try
        //    {
        //        var request = _mapper.Map<CompanyInfo>(companyDto);
        //        var result = await _companyService.AddOrUpdateCompanyAsync(request);
        //        if (!result) return BadRequest("Updating data has issue!");
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("An unhandled error occured", ex);
        //        return InternalServerError();
        //    }
        //}

        // DELETE api/<controller>/5
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string companyCode)
        {
            try
            {
                var item = await _companyService.DeleteAsync(companyCode);
                if (!item) return BadRequest("Deleting data has issue");
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