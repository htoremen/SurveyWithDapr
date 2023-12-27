using AutoMapper;

namespace Application;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();
    }
}