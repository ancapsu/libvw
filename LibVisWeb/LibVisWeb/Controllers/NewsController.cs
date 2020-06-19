using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibVisWeb.Models;
using Microsoft.AspNetCore.Http.Features;
using RacLib;
using System.Net.Http;
using LibVisLib;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace LibVisWeb.Controllers
{

    [Route("api/[controller]")]
    public class NewsController : Controller
    {

        [HttpGet("[action]")]
        public HomePageModel HomePage(int lang, int seq)
        {

            lang = LibVisLib.Verify.ValidLanguage(lang);

            HomePageModel m = new HomePageModel();

            try
            {

                m.Videos = ControllerBase.LatestVideosByCategory(lang, "", 0, 4);
                m.Statistics = ControllerBase.GetStatistics(lang);
                m.Articles = ControllerBase.LatestArticlesInRow(lang, 9, 3);

                m.Warnings = new List<SiteWarningModel>();

                for (int i = 0; i < LibVisLib.SiteWarning.warnings.Count; i++)
                {

                    m.Warnings.Add(new SiteWarningModel(LibVisLib.SiteWarning.warnings[i]));

                }

                m.NumTargets = LibVisLib.Target.GetTotalLastTargets(lang, "", true, DateTime.Now.AddDays(-5));
                m.NumApproval = LibVisLib.Article.GetTotalLastArticles(lang, "0", "", 1);
                m.NumRevision = LibVisLib.Article.GetTotalLastArticles(lang, "0", "", 2);
                m.NumNarration = LibVisLib.Article.GetTotalLastArticles(lang, "0", "", 3);
                m.NumProduction = LibVisLib.Article.GetTotalLastArticles(lang, "0", "", 4);

                m.Seq = seq;

            }
            catch (Exception ex)
            {

                BaseLog.log.Log(BaseLog.LogType.InternalError, "Exception in homepage controller");
                BaseLog.log.Log(ex);

            }
            
            return m;

        }

        [HttpGet("[action]")]
        public MainPageModel MainPage(string token, int lang)
        {

            MainPageModel m = new MainPageModel();

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userid = RacLib.SessionCode.ValidateSessionCode(token, machine);
            if (userid != null && userid != "")
            {

                m.Targets = ControllerBase.LatestTargetForUser(userid, lang, 0, 5, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                m.Articles = ControllerBase.LatestArticlesForUser(userid, lang, 0, 5);
                m.Videos = ControllerBase.LatestVideosForUser(userid, lang, 0, 5);

            }

            return m;

        }

        [HttpGet("[action]")]
        public UserPageModel UserPage(string token, int lang, string id)
        {

            UserPageModel m = new UserPageModel();

            string userId = null;

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }
            
            m.User = ControllerBase.GetUser(id, id == userId, false);
            m.Targets = ControllerBase.LatestTargetForUser(id, lang, 0, 5, DateTime.Now.AddDays(-5));
            m.Articles = ControllerBase.LatestArticlesForUser(id, lang, 0, 5);
            m.Videos = ControllerBase.LatestVideosForUser(id, lang, 0, 5);

            return m;

        }
              
        /// <summary>
        /// Registra um email para newsletter
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("[action]")]
        public GenericStatusModel NewsLetterRegister([FromBody]NewsLetterRegister request)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            int lang = 2;
            if (request.Lang >= 2 && request.Lang <= 4)
                lang = request.Lang;

            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            if (request.Type == 1)
            {

                if (!LibVisLib.Verify.AcceptEmail(request.Data))
                {

                    gsm.Result = (int)RacMsg.Id.Error;
                    gsm.ResultComplement = msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail); 
                    RacWebLog.log.Log(RacWebLog.LogType.Error, "Invalid email format: " + request.Data);

                } else
                {

                    RacWebLog.log.Log(RacWebLog.LogType.Informative, "Registro de email para newsletter: " + request.Data);
                    NewsLetter.AddNewsLetterEmail(request.Data);

                    gsm.Result = 0;
                    gsm.ResultComplement = "";

                }

            }
            else
            {

                gsm.Result = (int)RacMsg.Id.Error;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.InvalidType);
                RacWebLog.log.Log(RacWebLog.LogType.Error, "Tipo de registro inválido: " + request.Type.ToString());

            }

            return gsm;

        }

    }

}
