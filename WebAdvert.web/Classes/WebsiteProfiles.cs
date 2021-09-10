using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.web.Models;
using WebAdvert.web.ServiceClients;

namespace WebAdvert.web.Classes
{
    public class WebsiteProfiles : Profile
    {
        public WebsiteProfiles()
        {
            CreateMap<CreateAdvertViewModel, CreateAdvertModel>().ReverseMap();
        }
    }
}
