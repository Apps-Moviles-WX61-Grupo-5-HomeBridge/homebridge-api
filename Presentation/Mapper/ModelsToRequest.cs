using _1_API.Response;
using _2_Domain.IAM.Models.ValueObjects;
using AutoMapper;

namespace _1_API.Mapper;

public class ModelsToRequest : Profile
{
    public ModelsToRequest()
    {
        //  @AuthenticationResult to @AuthenticationResponse
        CreateMap<AuthenticationResults, AuthenticationResponse>()
            .ForMember(dest => dest.token, opt => opt.MapFrom(src => src.token))
            .ForMember(dest => dest.refreshToken, opt => opt.MapFrom(src => src.refreshToken))
            .ForMember(dest => dest.result, opt => opt.MapFrom(src => src.result))
            .ForMember(dest => dest.message, opt => opt.MapFrom(src => src.message));
    }
}