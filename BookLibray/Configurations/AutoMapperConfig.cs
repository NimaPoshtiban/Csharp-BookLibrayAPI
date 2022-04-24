using AutoMapper;
using BookLibrary.Data;
using BookLibrary.Models.Dtos.Author;
using BookLibrary.Models.Dtos.Book;

namespace BookLibrary.Configurations
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
