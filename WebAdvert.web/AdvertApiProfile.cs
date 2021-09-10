using AdvertApi.Models;
using AutoMapper;
using WebAdvert.web.ServiceClients;

namespace WebAdvert.web
{
    public class AdvertApiProfile : Profile
    {
        public AdvertApiProfile()
        {
            CreateMap<AdverModel, CreateAdvertModel>().ReverseMap();
            CreateMap<CreateAdvertResponse, AdvertResponse>().ReverseMap();
            CreateMap<ConfirmAdvertRequest, ConfirmAdverModel>().ReverseMap();
        }
    }
}
