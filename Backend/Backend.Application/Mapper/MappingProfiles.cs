using AutoMapper;
using Backend.Application.DTO.Entity;
using Backend.Domain.Entites;

namespace Backend.Application.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductDTO,Product> (); // source to destination
        }
    }
    
}
