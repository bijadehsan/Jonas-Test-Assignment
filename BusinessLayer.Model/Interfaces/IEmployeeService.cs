using BusinessLayer.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Model.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeInfoResponse>> GetAllEmployeesAsync();
        Task<IEnumerable<EmployeeInfo>> GetAllEmployeesByCompanyCodeAsync(string companyCode);
        Task<EmployeeInfoResponse> GetEmployeeByCodeAsync(string employeeCode);
        Task<bool> AddOrUpdateEmployeeAsync(EmployeeInfo employeeInfo);
    }
}
