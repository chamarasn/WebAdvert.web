using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.web.Models;
using WebAdvert.web.Services;

namespace WebAdvert.web.Controllers
{
    public class AdvertManagement : Controller
    {
        private readonly IFileUploader _fileUploader;

        public AdvertManagement(IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
        }

        public IActionResult Create(CreateViewModel model)
        {
            return View(model);
        }

        public async Task<IActionResult> Create(CreateViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var id = "11111";
                //You must make a call to Advert Api, create the advertisement in the database and return id

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

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                return null;
            }
        }
    }
}
