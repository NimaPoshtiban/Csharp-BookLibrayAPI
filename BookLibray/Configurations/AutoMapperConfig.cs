using AutoMapper;
using BookLibray.Data;
using BookLibray.Models.Dtos.Author;

namespace BookLibray.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<AuthorCreateDto, Author>().ReverseMap();
            CreateMap<AuthorUpdateDto, Author>().ReverseMap();
            CreateMap<Author,AuthorReadOnlyDto>().ReverseMap();
        }
    }
}
