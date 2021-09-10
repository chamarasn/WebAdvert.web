using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.web.Models;
using WebAdvert.web.ServiceClients;
using WebAdvert.web.Services;

namespace WebAdvert.web.Controllers
{
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _advertApiCient;
        private readonly IMapper _mapper;

        public AdvertManagementController(IFileUploader fileUploader, IAdvertApiClient advertApiCient, IMapper mapper)
        {
            _fileUploader = fileUploader;
            _advertApiCient = advertApiCient;
            _mapper = mapper;
        }

        public IActionResult Create(CreateAdvertViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var createAdvertModel = _mapper.Map<CreateAdvertModel>(model);
                var apiCallResponse = await _advertApiCient.Create(createAdvertModel);
                var id = apiCallResponse.Id;

                var fileName = "";

                if (imageFile != null)
                {
                    fileName = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using(var readStream = imageFile.OpenReadStream())
                        {
                            var result = await _fileUploader.UploadFileAsync(filePath, readStream)
                                .ConfigureAwait(false);

                            if (!result)
                                throw new Exception(
                                    "Coluld not upload the image to the repository. Please see logs");
                        }

                        var confirmModel = new ConfirmAdvertRequest
                        {
                            Id = id,
                            FilePath = filePath,
                            Status = AdvertApi.Models.AdvertStatus.Active
                        };

                        var canConfirm = await _advertApiCient.Confirm(confirmModel);
                        if (!canConfirm)
                        {
                            throw new Exception($"Cannot confirm advert of id = {id}");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        var confirmModel = new ConfirmAdvertRequest
                        {
                            Id = id,
                            FilePath = filePath,
                            Status = AdvertApi.Models.AdvertStatus.Pending
                        };

                         await _advertApiCient.Confirm(confirmModel);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return null;
        }
    }
}
