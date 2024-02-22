using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;

namespace PlatformService.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<PlatformPublishedDTO, Platform>()
                .ForMember(
                    dest => dest.ExternalId,
                    opt => opt.MapFrom(src => src.Id)
                );
        }
    }
}