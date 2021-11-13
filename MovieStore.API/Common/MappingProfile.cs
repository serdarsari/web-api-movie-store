using AutoMapper;
using MovieStore.DTO;
using MovieStore.DTO.ActorDTO;
using MovieStore.DTO.AwardDTO;
using MovieStore.DTO.DirectorDTO;
using MovieStore.DTO.MovieDTO;
using MovieStore.Entity;

namespace MovieStore.API.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Movie
            CreateMap<Movie, GetMovieDetailResponse>().ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Month + ", " + src.Year));
            CreateMap<CreateMovieRequest, Movie>().ReverseMap();

            //Actor
            CreateMap<Actor, GetActorDetailResponse>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Born, opt => opt.MapFrom(src => src.DateOfBirth.ToString("MMMM") + " " +src.DateOfBirth.ToString("dd") + ", " +src.PlaceOfBirth));
            CreateMap<CreateActorRequest, Actor>().ReverseMap();

            //Director
            CreateMap<Director, GetDirectorDetailResponse>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Born, opt => opt.MapFrom(src => src.DateOfBirth.ToString("MMMM") + " " + src.DateOfBirth.ToString("dd") + ", " + src.PlaceOfBirth));
            CreateMap<CreateDirectorRequest, Director>().ReverseMap();

            //Award
            CreateMap<CreateAwardRequest, Award>().ReverseMap();
        }
    }
}
