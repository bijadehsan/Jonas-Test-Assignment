using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;

namespace BusinessLayer
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<DataEntity, BaseInfo>();
            CreateMap<Company, CompanyInfo>();
            CreateMap<ArSubledger, ArSubledgerInfo>();
            CreateMap<Employee, EmployeeInfo>();
            CreateMap<EmployeeInfo, EmployeeInfoResponse>()
            .ForMember(dest => dest.CompanyName, opt => opt.Ignore()) // Assuming CompanyName is not in Employee
            .ForMember(dest => dest.OccupationName, opt => opt.MapFrom(src => src.Occupation))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.LastModifiedDateTime, opt => opt.MapFrom(src => src.LastModified.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }

}