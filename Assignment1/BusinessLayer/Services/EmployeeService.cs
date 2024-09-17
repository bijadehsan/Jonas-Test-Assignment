using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, ICompanyRepository companyRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
        }
        public async Task<IEnumerable<EmployeeInfoResponse>> GetAllEmployeesAsync()
        {
            var res = await _employeeRepository.GetAllEmployeesAsync();

            //get companyList to use for mapping employee's companyName
            var companyList = await _companyRepository.GetAllAsync();
            var employeeResponses = new List<EmployeeInfoResponse>();

            foreach (var employee in res)
            {
                //get company base on companyCode to set employee's companyName
                var company = companyList.FirstOrDefault(c => c.CompanyCode == employee.CompanyCode);
                if (company != null)
                {
                    var response = MapToEmployeeInfoResponse(employee, company);
                    employeeResponses.Add(response);
                }
            }
            return employeeResponses;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesByCompanyCodeAsync(string companyCode)
        {
            var result = await _employeeRepository.GetAllEmployeesByCompanyCodeAsync(companyCode);
            return _mapper.Map<IEnumerable<EmployeeInfo>>(result);
        }

        public async Task<EmployeeInfoResponse> GetEmployeeByCodeAsync(string employeeCode)
        {
            var employee = await _employeeRepository.GetByCodeAsync(employeeCode);
            var company = await _companyRepository.GetByCodeAsync(employee.CompanyCode);
            return MapToEmployeeInfoResponse(employee, company);
        }
        public async Task<bool> AddOrUpdateEmployeeAsync(EmployeeInfo employeeInfo)
        {
            var request = _mapper.Map<Employee>(employeeInfo);
            var result = await _employeeRepository.SaveEmployeeAsync(request);
            return result;
        }


        //map 
        private EmployeeInfoResponse MapToEmployeeInfoResponse(Employee employee, Company company)
        {
            return new EmployeeInfoResponse
            {
                EmployeeCode = employee.EmployeeCode,
                EmployeeName = employee.EmployeeName,
                CompanyName = company.CompanyName,
                OccupationName = employee.Occupation,
                EmployeeStatus = employee.EmployeeStatus,
                EmailAddress = employee.EmailAddress,
                PhoneNumber = employee.Phone,
                LastModifiedDateTime = employee.LastModified.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}
