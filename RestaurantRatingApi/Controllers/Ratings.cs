using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantRatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Ratings : ControllerBase
    {
        private ServiceBusSender _serviceBusSender;
        private IHttpClientFactory _clientFactory;
        public Ratings(ServiceBusSender serviceBus, IHttpClientFactory httpClientFactory)
        {
            _serviceBusSender = serviceBus;
            _clientFactory = httpClientFactory;
        }

        [HttpPost]
        [Route("/uploadJson")]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
           
            string filePath = string.Empty;
            string fileName = string.Empty;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    filePath = Path.GetTempFileName();
                    fileName = formFile.FileName;

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

              await _serviceBusSender.UploadFile(filePath, "BlobStorage");

              await _serviceBusSender.SendMessage("BlobStorage");
      
            return Ok();
        }

    }
}
