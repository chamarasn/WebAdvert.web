﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAdvert.web.Models;

namespace WebAdvert.web.ServiceClients
{
    public class SearchApiClient : ISearchApiClient
    {
        private readonly HttpClient _client;
        private readonly string BaseAddress = string.Empty;
        public SearchApiClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            BaseAddress = configuration.GetSection("SearchApi").GetValue<string>("url");
        }

        public async Task<List<AdvertType>> Search(string keyword)
        {
            var result = new List<AdvertType>();
            var callUrl = $"{BaseAddress}/search/v1/{keyword}";
            var httpResponse = await _client.GetAsync(new Uri(callUrl)).ConfigureAwait(false);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var allAdverts = await httpResponse.Content.ReadAsAsync<List<AdvertType>>().ConfigureAwait(false);
                result.AddRange(allAdverts);
            }
            return result;
        }

    }
}
