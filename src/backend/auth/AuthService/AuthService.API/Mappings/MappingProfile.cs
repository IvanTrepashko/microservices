using AuthService.API.ApiModels;
using AuthService.Application.Commands.Authentication;
using AutoMapper;

namespace AuthService.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginCommandResponse, LoginResponseApiModel>();
        CreateMap<RegisterCommandResponse, RegisterResponseApiModel>();
    }
}
