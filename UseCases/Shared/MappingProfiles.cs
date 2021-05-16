using AutoMapper;
using Domain;
using UseCases.Account;

namespace UseCases.Shared
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
        }
    }
}