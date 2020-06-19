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
    public class AudioController : Controller
    {

        [HttpGet("{id}/{part}")]
        public IActionResult Get(string id, int part)
        {

            byte[] res = null;
            string mime = "audio/wav";
            DateTimeOffset dto = DateTimeOffset.Now;

            try
            {

                ArticleAction aa = ArticleAction.LoadArticleAction(id);
                if (aa != null)
                {

                    try
                    {

                        string path = aa.GetAttachFilePath(part);
                        res = System.IO.File.ReadAllBytes(path);

                        mime = aa.GetAttachFileType(part);

                    }
                    catch { }

                }

            }
            catch(Exception ex) {

            }

            return File(res, mime, dto, Microsoft.Net.Http.Headers.EntityTagHeaderValue.Any);

        }

    }
    
}
