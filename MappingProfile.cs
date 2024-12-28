using AutoMapper;
using PetAdminApi.Models;

namespace PetAdminApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Admin, AdminDto>().ReverseMap(); // Двусторонний маппинг между Admin и AdminDto
        }
    }
}
