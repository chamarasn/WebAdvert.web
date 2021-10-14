using AdvertApi.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAdvert.web.Models;

namespace WebAdvert.web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _configuration = configuration;
            _client = client;
            _mapper = mapper;

            var createUrl = _configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
            _client.BaseAddress = new Uri(createUrl);
        }

        public async Task<AdvertResponse> Create(CreateAdvertModel model)
        {
            var advertApiModel = _mapper.Map<AdvertModel>(model); 

            var jsonModel = JsonConvert.SerializeObject(advertApiModel);

            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "relativeAddress");
            //request.Content = new StringContent(jsonModel,
            //                                    Encoding.UTF8,
            //                                    "application/json");

            //var responseJson = await _client.SendAsync(request).ConfigureAwait(false);

             var response = await _client.PostAsync(_client.BaseAddress + "/Create", new StringContent(jsonModel, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);
            
            var advertResponse = _mapper.Map<AdvertResponse>(createAdvertResponse);

            return advertResponse;
        }

        public async Task<bool> Confirm(ConfirmAdvertRequest model)
        {
            var advertModel = _mapper.Map<ConfirmAdverModel>(model);
            var jsonModel = JsonConvert.SerializeObject(advertModel);
            var response = await _client
                .PutAsync($"{_client.BaseAddress}/confirm", new StringContent(jsonModel, Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<List<Advertisement>> GetAllAsync()
        {
            var apiCallResponse = await _client.GetAsync(new Uri($"{_client.BaseAddress}/all")).ConfigureAwait(false);
            var allAdvertModels = await apiCallResponse.Content.ReadAsAsync<List<AdvertModel>>().ConfigureAwait(false);

            return allAdvertModels.Select(x => _mapper.Map<Advertisement>(x)).ToList();
        }

        public async Task<Advertisement> GetAsync(string advertId)
        {
            var apiCallResponse = await _client.GetAsync(new Uri($"{_client.BaseAddress}/{advertId}")).ConfigureAwait(false);
            var fullAdvertModels = await apiCallResponse.Content.ReadAsAsync<AdvertModel>().ConfigureAwait(false);

            return _mapper.Map<Advertisement>(fullAdvertModels);
        }
    }
}
