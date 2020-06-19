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
    public class AvatarController : Controller
    {

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {

            byte[] res = null;
            DateTimeOffset dto = DateTimeOffset.Now;

            try
            {

                Profile prf = Profile.LoadProfile(id);
                if (prf != null)
                {

                    try
                    {

                        string path = Base.conf.tempImageFilePath + "\\u-" + id + ".jpg";
                        System.Drawing.Image img = System.Drawing.Image.FromFile(path);

                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                        res = stream.ToArray();

                    }
                    catch { }

                    dto = new DateTimeOffset(prf.lastModified);

                }

            }
            catch(Exception ex) {

            }

            if (res == null)
            {

                string imagePath = Base.conf.applicationPath.TrimEnd('\\') + "/dist/img/default-avatar.png";
                byte[] fileBytes = System.IO.File.ReadAllBytes(imagePath);
                res = fileBytes;

            }
              
            return File(res, "image/jpeg", dto, Microsoft.Net.Http.Headers.EntityTagHeaderValue.Any);

        }

        [HttpPost]
        public void Post([FromBody]FileDataModel data)
        {

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(data.Token, machine);

            Profile prf = Profile.LoadProfile(userId);

            byte[] str = Convert.FromBase64String(data.Data);

            MemoryStream ms = new MemoryStream(str, 0, str.Length);
            ms.Write(str, 0, str.Length);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

            string path = Base.conf.tempImageFilePath + "\\u-" + userId + ".jpg";

            img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        
        }

    }
    
}
