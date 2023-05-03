using AutoMapper;
using CRUD.Domain.Employee;
using CRUDWeb.Models;
using System.Text;

namespace CRUDWeb
{
    public class EmployeeAutoMapperProfile : Profile
    {
        public EmployeeAutoMapperProfile()
        {
            CreateEmployeeMap();       
        }
        private const string Folder = "images";
        private void CreateEmployeeMap()
        {
            CreateMap<EmpRequestModel, Emp>();
            CreateMap<Emp, EmpRequestModel>()
                .ForMember(dest => dest.EmpSkills, opt => opt.MapFrom(src => GetAllSkills(src.Skills)))
                .ForMember(x => x.Image, o => o.MapFrom(x => !string.IsNullOrEmpty(x.ImageURL) ? new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("")), 0, 0, Path.GetFileName(x.ImageURL), Path.GetFileName(x.ImageURL)) : null))
                .ForMember(x => x.ImageName, o => o.MapFrom(x => !string.IsNullOrEmpty(x.ImageURL) ? Path.Combine(Folder, Path.GetFileName(x.ImageURL)): null));
        }
        private string GetAllSkills(List<SkillsList>? Skills)
        {
            if(Skills != null && Skills.Count > 0)
            {
                var skill = string.Join(",", Skills.Select(s => s.Name).ToList());
                return skill;
            }
            return "";
        }
    }
}
