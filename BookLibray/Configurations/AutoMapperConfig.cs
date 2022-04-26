using AutoMapper;
using BookLibrary.Data;
using BookLibrary.Models.Dtos.Auth;
using BookLibrary.Models.Dtos.Author;
using BookLibrary.Models.Dtos.Book;
using Microsoft.AspNetCore.Identity;

namespace BookLibrary.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<AuthorCreateDto, Author>().ReverseMap();
            CreateMap<AuthorUpdateDto, Author>().ReverseMap();
            CreateMap<Author, AuthorReadOnlyDto>().ReverseMap();
            CreateMap<AuthorDetailsDto,Author>().ReverseMap();

            CreateMap<Book, BookReadOnlyDto>()
               .ForMember(a => a.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
               .ReverseMap();
            CreateMap<BookCreateDto, Book>().ReverseMap();
            CreateMap<BookUpdateDto, Book>().ReverseMap();
            CreateMap<IdentityUser, AuthUserDto>().ReverseMap();
        }
    }
}
