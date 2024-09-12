using _2_Domain.IAM.Models.Commands;
using _2_Domain.IAM.Models.Entities;
using _2_Domain.IAM.Models.ValueObjects;
using _2_Domain.Publication.Models.Commands;
using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;
using _2_Domain.Search.Models.Entities;
using _2_Domain.Search.Models.Queries;
using AutoMapper;

namespace _1_API.Mapper;

public class RequestToModels : Profile
{
    public RequestToModels()
    {
        //  @AuthenticationRequest to @UserCredentials
        CreateMap<UserAuthenticationRequest, UserCredentials>()
            .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
        
        //  @RefreshTokenRequest to @RefreshTokenModel
        CreateMap<RefreshTokenCommand, CreateRefreshTokenCommand>()
            .ForMember(dest => dest.ExpiredToken, opt => opt.MapFrom(src => src.ExpiredToken))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));
        
        //  @UserRegisterRequest to @User
        CreateMap<UserRegistrationCommand, User>()
            .ForMember(
                dest => dest._UserCredentials,
                opt => opt.MapFrom(src => new UserCredentials
                    {
                        Email = src.Email,
                        HashedPassword = src.Password,
                        Username = src.Username
                    }
                )
            )
            .ForMember(
                dest => dest._UserInformation,
                opt => opt.MapFrom(src => new UserInformation()
                    {
                        Name = src.Username,
                        Lastname = src.Username,
                        PhoneNumber = src.PhoneNumber
                    }
                )
            );
        
        //  //  @SearchRequest to @SearchModel
        //  CreateMap<SearchQuery, SearchModel>()
        //      .ForMember(dest => dest.SearchInput, opt => opt.MapFrom(src => src.SearchInput))
        //      .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
        //      .ForMember(dest => dest.PriceMin, opt => opt.MapFrom(src => src.PriceMin))
        //      .ForMember(dest => dest.PriceMax, opt => opt.MapFrom(src => src.PriceMax));
        
        //  @GetPublicationRequest to @GetPublicationModel
        CreateMap<GetPublicationQuery, GetPublicationQuery>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        
        //  @PostPublicationRequest to @PublicationModel
        CreateMap<PostPublicationCommand, PublicationModel>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest._Location,
                opt => opt.MapFrom(src => new LocationModel()
                    {
                        Address = src._Location_Address
                    }
                )
            );
    }
}