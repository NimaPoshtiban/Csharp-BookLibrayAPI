using AutoMapper;
using BookLibray.Data;
using BookLibray.Models.Dtos.Author;
using BookLibray.Models.Dtos.Book;

namespace BookLibray.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<AuthorCreateDto, Author>().ReverseMap();
            CreateMap<AuthorUpdateDto, Author>().ReverseMap();
            CreateMap<Author, AuthorReadOnlyDto>().ReverseMap();
            CreateMap<Book, BookReadOnlyDto>()
               .ForMember(a => a.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
               .ReverseMap();
            CreateMap<BookCreateDto,Book>().ReverseMap();
            CreateMap<BookUpdateDto, Book>().ReverseMap();

        }
    }
}
