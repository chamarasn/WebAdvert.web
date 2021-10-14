using AdvertApi.Models;
using AutoMapper;
using System;
using WebAdvert.web.Models;
using WebAdvert.web.ServiceClients;

namespace WebAdvert.web
{
    public class AdvertApiProfile : Profile
    {
        public AdvertApiProfile()
        {
            CreateMap<AdvertModel, CreateAdvertModel>().ReverseMap();

            CreateMap<AdvertModel, Advertisement>()
                .ReverseMap();

            CreateMap<Advertisement, IndexViewModel>()
                .ForMember(
                    dest => dest.Id, src => Guid.NewGuid()).ReverseMap();

            CreateMap<AdvertModel, IndexViewModel>()
                .ForMember(
                    dest => dest.Title, src => src.MapFrom(field => field.Title));

            CreateMap<CreateAdvertResponse, AdvertResponse>().ReverseMap();
            CreateMap<ConfirmAdvertRequest, ConfirmAdverModel>().ReverseMap();
            CreateMap<AdvertType, SearchViewModel>().ReverseMap();
        }
    }
}
