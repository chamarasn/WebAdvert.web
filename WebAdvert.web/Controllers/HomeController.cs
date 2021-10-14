using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.web.Models;
using WebAdvert.web.ServiceClients;

namespace WebAdvert.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISearchApiClient SearchApiClient { get; }
        private IAdvertApiClient AdvertApiClient { get; }
        public IMapper Mapper { get; }

        public HomeController(ISearchApiClient searchApiClient, IMapper mapper, IAdvertApiClient advertApiClient, ILogger<HomeController> logger)
        {
            SearchApiClient = searchApiClient;
            AdvertApiClient = advertApiClient;
            Mapper = mapper;
            _logger = logger;
        }

        //[Authorize]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index()
        {
            var allAds = await AdvertApiClient.GetAllAsync().ConfigureAwait(false);
            var allViewModels = allAds.Select(x => Mapper.Map<IndexViewModel>(x));

            return View(allViewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            var viewModel = new List<SearchViewModel>();

            var searchResult = await SearchApiClient.Search(keyword).ConfigureAwait(false);
            searchResult.ForEach(advertDoc =>
            {
                var viewModelItem = Mapper.Map<SearchViewModel>(advertDoc);
                viewModel.Add(viewModelItem);
            });

            return View("Search", viewModel);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    //return View(new )
        //}
    }
}
