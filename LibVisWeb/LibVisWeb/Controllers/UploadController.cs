using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using RacLib;
using LibVisLib;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using LibVisWeb.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Drawing;

namespace LibVisWeb.Controllers
{

    [Route("api/[controller]")]
    public class UploadController : Controller
    {

        [HttpPost]
        public string Post(Microsoft.AspNetCore.Http.IFormFile file)
        {

            string ext = "wav";
            if (file.FileName.Length > 3)
                ext = file.FileName.Substring(file.FileName.Length - 3);

            string id = Guid.NewGuid().ToString();

            string path = Base.conf.tempImageFilePath + "\\d-" + id + "." + ext;

            Stream read = file.OpenReadStream();

            using (var fileStream = System.IO.File.Create(path))
            {
                read.CopyTo(fileStream);
            }

            return id;

        }

    }
    
}
