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
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace LibVisWeb.Controllers
{

    [Route("api/[controller]")]
    public class VideoController : Controller
    {

        [HttpGet("[action]")]
        public VideoModel Get(string token, int lang, string id)
        {

            string userId = null;

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            Video vid = Video.LoadVideo(id);

            VideoModel m = new VideoModel();

            if (vid != null)
            {

                m = new VideoModel(msgs, vid, true, true, true, true);

            }

            return m;

        }

        [HttpGet("[action]")]
        public VideoListModel List(string token, int lang, int ini, int max)
        {

            string userId = "";

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            VideoListModel m = new VideoListModel();

            m.Ini = ini;
            m.Total = LibVisLib.Video.GetTotalLastVideos(lang, "");
            m.Videos = ControllerBase.LatestVideosByCategory(lang, "", ini, max);

            return m;

        }

        [HttpGet("[action]")]
        public VideoCategoryModel ByCategory(string token, int lang, string categ, int ini, int max)
        {

            string userId = "";

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            VideoCategoryModel m = new VideoCategoryModel();

            LibVisLib.Category c = LibVisLib.Category.LoadCategory(categ);
            if (c != null)
            {

                m.Title = msgs.Get(c.nameMsg);
                m.Description = msgs.Get(c.descriptionMsg);
                m.Ini = ini;
                m.Total = Video.GetTotalLastVideos(lang, categ);
                m.Videos = ControllerBase.LatestVideosByCategory(lang, categ, ini, max);

            }

            return m;

        }

        [HttpGet("[action]")]
        public VideoModel VideoInfo(string id, int lang)
        {

            VideoModel m = new VideoModel();

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            LibVisLib.Video n = LibVisLib.Video.LoadVideo(id);
            if (n != null)
                m = new VideoModel(msgs, n, true, true, true, true);

            return m;

        }
        
        [HttpPost("[action]")]
        public GenericIdModel Register(string token, [FromBody]NewVideoModel video)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            int lang = LibVisLib.Verify.ValidLanguage(video.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidToken;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.InvalidToken);

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.UserNotFound;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserNotFound);

            }

            if (gsm.Result == 0)
            {

                if (prf.user.profile < BaseUser.InternalSystemProfile.InternalStaff)
                {

                    gsm.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                    gsm.ResultComplement = msgs.Get(RacMsg.Id.UserHasNoRightToThisItem); 

                }

                try
                {
                                        
                    if (!LibVisLib.Verify.AcceptInteger(video.Categ))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharsInCategories) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCateg: Invalid chars");
                    }

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(video.Title, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTitle) + " (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: Invalid chars");
                    }

                    string r1 = "";
                    if (!LibVisLib.Verify.AcceptMultilineFreeText(video.Description, ref r1))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInText) + " (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errDescription: Invalid chars  (" + r1 + "); ");
                    }

                    string r2 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(video.Tags, ref r2))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTags) + " (" + r2 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTags: Invalid chars");
                    }

                    r1 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(video.Image, ref r1))
                    {

                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInImageData) + " (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Invalid chars (imagem base64) (" + r1 + "); ");

                    }

                    if (!LibVisLib.Verify.AcceptUrl(video.YoutubeLink))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInYoutubeUrl) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errYoutubeLink: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptUrl(video.BitchuteLink))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInBitchuteUrl) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errBitchuteLink: Invalid chars");
                    }

                    r1 = "";
                    if (!LibVisLib.Verify.AcceptMultilineFreeText(video.Script, ref r1))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheVideoScript) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errDescription: Invalid chars  (" + r1 + "); ");
                    }

                    string imagem = video.Image;

                    string[] imagempart = imagem.Split(',');

                    if (imagempart.Length > 0)
                        imagem = imagempart[imagempart.Length - 1];
                        
                    string title = LibVisLib.Verify.VerifyFreeText(video.Title);
                    string description = LibVisLib.Verify.VerifyMultilineFreeText(video.Description);
                    string tags = LibVisLib.Verify.VerifyFreeText(video.Tags);
                    string categ = LibVisLib.Verify.VerifyInteger(video.Categ);
                    string youtubelink = LibVisLib.Verify.VerifyUrl(video.YoutubeLink);
                    string bitchutelink = LibVisLib.Verify.VerifyUrl(video.BitchuteLink);
                    string script = LibVisLib.Verify.VerifyMultilineFreeText(video.Script);

                    if (title.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TitleTooShort) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: required at least 10 chars");
                    }

                    if (title.Length > 150)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TitleTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: Content too big");
                    }

                    if (description.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TextTooShort) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: required at least 10 chars");
                    }

                    if (description.Length > 5000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TextTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errDescription: Content too big");
                    }

                    if (script.Length > 100000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.ScriptTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errScript: Content too big");
                    }

                    if (imagem.Length > 5000000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.ImageTooLarge) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Content too big");
                    }

                    if (gsm.Result == 0)
                    {

                        LibVisLib.Video vid = new Video();

                        vid.title = title;
                        vid.description = description;                   
                        vid.released = DateTime.Now;
                        vid.script = script;
                        vid.linkBitchute = bitchutelink;
                        vid.linkYoutube = youtubelink;
                        vid.tags = tags;
                        vid.language = (RacMsg.Language)lang;
                        
                        vid.categories = new List<string>();

                        int ic = 0;
                        for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                            if (!LibVisLib.Category.categories[i].main)
                                if (ic < categ.Length && categ[ic++] == '1')
                                    vid.categories.Add(Category.categories[i].label);

                        vid.NormalizeMain();
                        vid.Save();
                        

                        byte[] str = Convert.FromBase64String(imagem);

                        MemoryStream ms = new MemoryStream(str, 0, str.Length);
                        ms.Write(str, 0, str.Length);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                        string path = Base.conf.tempImageFilePath + "\\v-" + vid.id + ".jpg";

                        img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);


                        VideoAction act = new VideoAction(vid);

                        act.date = vid.released;
                        act.type = VideoAction.ActionType.Created;
                        act.userId = userId;
                        act.show = true;
                        act.observation = "";

                        act.Save();

                        //prf.RegisterAction(Profile.ProfileAction.Reg);

                        gsm.Id = vid.id;

                    }

                }
                catch (Exception ex)
                {

                    if (!ex.Message.StartsWith("Thread was being aborted"))
                    {

                        gsm.Result = (int)RacLib.RacMsg.Id.InternalError;
                        gsm.ResultComplement += ex.Message;
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.InternalError, "Exception in form");
                        RacLib.BaseLog.log.Log(ex);

                    }

                }

            }

            return gsm;

        }
        
        [HttpPost("[action]")]
        public GenericIdModel Change(string token, [FromBody]ChangeVideoModel video)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            int lang = video.Lang;

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidToken;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.InvalidToken);

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.UserNotFound;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserNotFound);

            }

            if (gsm.Result == 0)
            {
                
                if (prf.user.profile < BaseUser.InternalSystemProfile.InternalStaff)
                {

                    gsm.Result = (int)RacMsg.Id.Number;
                    gsm.ResultComplement = "Você não tem direitos para cadastrar vídeos; ";

                }

                try
                {

                    if (!LibVisLib.Verify.AcceptGuid(video.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no Id do vídeo; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptInteger(video.Categ))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos nas categorias; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCateg: Invalid chars");
                    }

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(video.Title, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no título (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: Invalid chars");
                    }

                    string r1 = "";
                    if (!LibVisLib.Verify.AcceptMultilineFreeText(video.Description, ref r1))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos na descrição (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errDescription: Invalid chars  (" + r1 + "); ");
                    }

                    string r2 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(video.Tags, ref r2))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos na tag (" + r2 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTags: Invalid chars");
                    }

                    r1 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(video.Image, ref r1))
                    {

                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos nos dados da imagem (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Invalid chars (imagem base64) (" + r1 + "); ");

                    }

                    if (!LibVisLib.Verify.AcceptUrl(video.YoutubeLink))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no link do youtube; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errYoutubeLink: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptUrl(video.BitchuteLink))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no link do bitchute; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errBitchuteLink: Invalid chars");
                    }

                    r1 = "";
                    if (!LibVisLib.Verify.AcceptMultilineFreeText(video.Script, ref r1))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos na descrição (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errDescription: Invalid chars  (" + r1 + "); ");
                    }

                    string imagem = video.Image;

                    string[] imagempart = imagem.Split(',');

                    if (imagempart.Length > 0)
                        imagem = imagempart[imagempart.Length - 1];

                    string videoId = LibVisLib.Verify.VerifyGuid(video.Id);
                    string title = LibVisLib.Verify.VerifyFreeText(video.Title);
                    string tags = LibVisLib.Verify.VerifyFreeText(video.Tags);
                    string description = LibVisLib.Verify.VerifyMultilineFreeText(video.Description);
                    string categ = LibVisLib.Verify.VerifyInteger(video.Categ);
                    string youtubelink = LibVisLib.Verify.VerifyUrl(video.YoutubeLink);
                    string bitchutelink = LibVisLib.Verify.VerifyUrl(video.BitchuteLink);
                    string script = LibVisLib.Verify.VerifyMultilineFreeText(video.Script);

                    if (videoId == null || videoId.Length < 36)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Id do vídeo inválido; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Id inválido");
                    }

                    if (title.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Titulo muito pequeno (mínimo 10 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: required at least 10 chars");
                    }

                    if (title.Length > 150)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Titulo muito grande (máximo 150 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: Content too big");
                    }

                    if (description.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Texto muito pequeno (mínimo 10 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: required at least 10 chars");
                    }

                    if (description.Length > 5000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Descrição muito grande (máximo 5k caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errDescription: Content too big");
                    }

                    if (script.Length > 100000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Script muito grande (máximo 100k caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errScript: Content too big");
                    }

                    if (imagem.Length > 5000000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Imagem muito grande (máximo 500k caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Content too big");
                    }

                    if (gsm.Result == 0)
                    {

                        LibVisLib.Video vid = Video.LoadVideo(videoId);

                        vid.title = title;
                        vid.description = description;
                        vid.released = DateTime.Now;
                        vid.script = script;
                        vid.linkBitchute = bitchutelink;
                        vid.linkYoutube = youtubelink;
                        vid.tags = tags;
                        vid.language = (RacMsg.Language)lang;

                        vid.categories = new List<string>();

                        int ic = 0;
                        for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                            if (!LibVisLib.Category.categories[i].main)
                                if (ic < categ.Length && categ[ic++] == '1')
                                    vid.categories.Add(Category.categories[i].label);

                        vid.NormalizeMain();
                        vid.Save();

                        byte[] str = Convert.FromBase64String(imagem);

                        MemoryStream ms = new MemoryStream(str, 0, str.Length);
                        ms.Write(str, 0, str.Length);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                        string path = Base.conf.tempImageFilePath + "\\v-" + vid.id + ".jpg";

                        img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                        VideoAction act = new VideoAction(vid);

                        act.date = vid.released;
                        act.type = VideoAction.ActionType.Created;
                        act.userId = userId;
                        act.show = true;
                        act.observation = "";

                        act.Save();

                        gsm.Id = vid.id;

                    }

                }
                catch (Exception ex)
                {

                    if (!ex.Message.StartsWith("Thread was being aborted"))
                    {

                        gsm.Result = (int)RacLib.RacMsg.Id.InternalError;
                        gsm.ResultComplement += ex.Message;
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.InternalError, "Exception in form");
                        RacLib.BaseLog.log.Log(ex);

                    }

                }

            }

            return gsm;

        }

        [HttpGet("[action]")]
        public EditVideoBaseModel EditBase(string token, int lang)
        {

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            EditVideoBaseModel gsm = new EditVideoBaseModel();

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                string userId = SessionCode.ValidateSessionCode(token, machine);

            }

            gsm.Categories = new List<NewsCategory>();

            for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                if (!LibVisLib.Category.categories[i].main)
                    gsm.Categories.Add(new NewsCategory(msgs, LibVisLib.Category.categories[i]));

            return gsm;

        }

        [HttpPost("[action]")]
        public async Task<YoutubeResultModel> Link(string token, [FromBody]YoutubeModel data)
        {

            int lang = data.Lang;

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);
            
            YoutubeResultModel yrm = new YoutubeResultModel();

            try
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                string userId = SessionCode.ValidateSessionCode(token, machine);
                if (userId != null)
                {

                    Profile prf = Profile.LoadProfile(userId);
                    if (prf != null)
                    {

                        string videoCode = "";

                        if (data.Link.Contains("youtu.be/"))
                        {

                            int idx = data.Link.IndexOf("youtu.be/");

                            int s = idx + 9;

                            for (int i = s; i < data.Link.Length; i++)
                            {

                                if (data.Link[i] == '/' || data.Link[i] == '&' || data.Link[i] == '?')
                                    break;

                                videoCode += data.Link[i];

                            }

                        }
                        else
                        {

                            int idx = data.Link.IndexOf("?");

                            string pars = data.Link.Substring(idx + 1);
                            string[] p = pars.Split('&');

                            for (int i = 0; i < p.Length; i++)
                            {

                                if (p[i].StartsWith("v="))
                                {

                                    videoCode = p[i].Substring(2);
                                    break;

                                }

                            }

                        }

                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "videoCode = '" + videoCode + "'");

                        // Links do youtube e do bitchute

                        yrm.YoutubeLink = data.Link;
                        yrm.BitchuteLink = "";

                        // Pega a figura

                        string figlink = "https://i.ytimg.com/vi/" + videoCode + "/maxresdefault.jpg";
                                               
                        HttpClient client_fig = new HttpClient();
                        HttpResponseMessage response_fig = await client_fig.GetAsync(figlink);
                        byte[] imageBytes = await response_fig.Content.ReadAsByteArrayAsync();

                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Got bits");

                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                        int cw = img.Width;
                        int ch = img.Height;

                        int dw = 1280;
                        int dh = 720;

                        float ew = (float)cw / (float)dw;
                        float eh = (float)ch / (float)dh;

                        int px = 0, py = 0;
                        int lw = 0, lh = 0;

                        if (ew == eh) // Nenhum crop necessário, só resize
                        {

                            px = 0;
                            py = 0;
                            lw = cw;
                            lh = ch;

                        }
                        else if (ew > eh) // Tem que cropar na largura
                        {

                            int totw = (int)(cw / eh);
                            px = (totw - dw) / 2;
                            py = 0;

                            lw = (int)(dw * eh);
                            lh = ch;

                        }
                        else if (ew < eh)// Tem que cropar na altura
                        {

                            int toth = (int)(ch / ew);
                            px = 0;
                            py = (toth - dh) / 2;

                            lw = cw;
                            lh = (int)(dh * ew);

                        }

                        ImageAttributes ia = new ImageAttributes();
                        ia.SetWrapMode(WrapMode.TileFlipXY);

                        using (System.Drawing.Bitmap _bitmap = new System.Drawing.Bitmap(dw, dh, PixelFormat.Format32bppPArgb))
                        {

                            _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                            using (Graphics _graphic = Graphics.FromImage(_bitmap))
                            {

                                _graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                _graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                _graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                                _graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                                _graphic.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                                _graphic.Clear(Color.White);

                                _graphic.DrawImage(img, new Rectangle(0, 0, dw, dh), new Rectangle(px, py, lw, lh), GraphicsUnit.Pixel);

                                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                                _bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                yrm.Image = Convert.ToBase64String(stream.ToArray());

                            }

                        }

                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Got image");

                        string apiCall = "https://www.googleapis.com/youtube/v3/videos?part=snippet&id=" + videoCode + "&key=AIzaSyDZyxrWNvN0MLWexTptTPDHzhnXHwebnCU";
                        
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
                        HttpResponseMessage response = await client.GetAsync(apiCall);
                        string pageContents = await response.Content.ReadAsStringAsync();

                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Got stuff");

                        dynamic stuff = JsonConvert.DeserializeObject(pageContents);

                        if (stuff.items.Count > 0)
                        {

                            yrm.Title = stuff.items[0].snippet.title;
                            yrm.Description = stuff.items[0].snippet.description;
                            yrm.Tags = "";

                            try
                            {

                                for (int i = 0; i < stuff.items[0].snippet.tags.Count; i++)
                                {

                                    if (yrm.Tags != "")
                                        yrm.Tags += ", ";

                                    yrm.Tags += stuff.items[0].snippet.tags[i];

                                }

                            }
                            catch { }

                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Got all");

                        }
                        else
                        {
                            
                            yrm.Result = (int)RacMsg.Id.InternalError;
                            yrm.ResultComplement = "Não encontrei o vídeo no youtube";

                        }

                    }

                }

            }
            catch (Exception ex)
            {

                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Exception");
                RacLib.BaseLog.log.Log(ex);

                yrm.Result = (int)RacMsg.Id.InternalError;
                yrm.ResultComplement = ex.Message;

            }

            return yrm;

        }

        [HttpPost("[action]")]
        public GenericStatusModel RegisterGrant(string token, [FromBody]RegisterGrant grant)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Token inválido";

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Usuário não encontrado";

            }

            if (!prf.user.isInternal)
            {

                gsm.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                gsm.ResultComplement = "Usuário não tem direitos para essa ação";

            }

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(grant.ActionId))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id da ação; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errActionId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptGuidOrNull(grant.AwardId))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id do premio; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errActionId: Invalid chars");
                    }

                    if (grant.Add < 0 || grant.Add > 1)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Ação inválida; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errAdd: Invalid value");
                    }

                    if (gsm.Result == 0)
                    {

                        string actionId = LibVisLib.Verify.VerifyGuid(grant.ActionId);
                        string awardId = LibVisLib.Verify.VerifyGuid(grant.AwardId);

                        LibVisLib.TargetAction trga = LibVisLib.TargetAction.LoadTargetAction(actionId);
                        if (trga == null)
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += "Ação inválida; ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid target");

                        }
                        else
                        {

                            if (grant.Add == 0)
                                trga.RemoveGrant(grant.AwardId);
                            else
                                trga.AddGrant(grant.AwardId, userId);

                            trga.Save();

                        }

                    }

                }
                catch (Exception ex)
                {

                    if (!ex.Message.StartsWith("Thread was being aborted"))
                    {

                        gsm.Result = (int)RacLib.RacMsg.Id.InternalError;
                        gsm.ResultComplement += ex.Message;
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.InternalError, "Exception in form");
                        RacLib.BaseLog.log.Log(ex);

                    }

                }

            }

            return gsm;

        }

        [HttpGet("[action]")]
        public FileContentResult Image(string id)
        {

            byte[] res = null;

            try
            {

                id = LibVisLib.Verify.VerifyGuid(id);

                if (id != null)
                {

                    LibVisLib.Video video = LibVisLib.Video.LoadVideo(id);
                    if (video != null)
                    {

                        try
                        {

                            string path = Base.conf.tempImageFilePath + "\\v-" + id + ".jpg";
                            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

                            System.IO.MemoryStream stream = new System.IO.MemoryStream();
                            img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                            res = stream.ToArray();

                        }
                        catch { }

                    }

                }

            }
            catch { }

            if (res == null)
            {

                string imagePath = Base.conf.applicationPath.TrimEnd('\\') + "/wwwroot/dist/img/no-image.jpg";
                byte[] fileBytes = System.IO.File.ReadAllBytes(imagePath);
                res = fileBytes;

            }

            return File(res, "image/jpeg");

        }
        
        [HttpPost("[action]")]
        public VideoListModel Search(string token, int lang, int ini, int max, [FromBody]SearchVideoModel search)
        {

            string userId = null;

            if (lang < 3 || lang > 4)
                lang = 2;

            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            VideoListModel m = new VideoListModel();

            if (!LibVisLib.Verify.AcceptFreeText(search.SearchString))
            {
                m.Result = (int)RacLib.RacMsg.Id.Error;
                m.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInSearchString);
                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errSearchString: Invalid chars");
            }

            string srch = LibVisLib.Verify.VerifyFreeText(search.SearchString);

            m.Ini = ini;
            m.Total = LibVisLib.Video.GetTotalLastVideos(lang, "", srch);
            m.Videos = ControllerBase.LatestVideosByCategory(lang, "", ini, max, srch);

            return m;

        }

    }

}
