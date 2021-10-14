using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.web.Models;

namespace WebAdvert.web.ServiceClients
{
    public interface ISearchApiClient
    {
        Task<List<AdvertType>> Search(string keyword);
    }
}
