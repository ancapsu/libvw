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

namespace LibVisWeb.Controllers
{

    [Route("api/[controller]")]
    public class TargetController : Controller
    {

        [HttpGet("[action]")]
        public TargetModel Get(string token, int lang, string id)
        {

            string userId = null;

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            Target trg = Target.LoadTarget(id);

            TargetModel m = new TargetModel();

            if (trg != null)
            {

                m = new TargetModel(msgs, trg, true, true, true, true);

                if (userId != null && userId != "")
                    m.UserVote = (int)trg.GetUserVote(userId);

            }

            return m;

        }

        [HttpGet("[action]")]
        public TargetListModel List(string token, int lang, int ini, int max)
        {

            string userId = null;

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            TargetListModel m = new TargetListModel();

            m.Ini = ini;
            m.Total = LibVisLib.Target.GetTotalLastTargets(lang, "", true, DateTime.Now.AddDays(-5));
            m.Targets = ControllerBase.LatestTargetByCategory(lang, "", true, ini, max, userId, DateTime.Now.AddDays(-5));

            return m;

        }

        [HttpGet("[action]")]
        public TargetListModel ListAll(string token, int lang, int ini, int max)
        {

            string userId = null;

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            TargetListModel m = new TargetListModel();

            m.Ini = ini;
            m.Total = LibVisLib.Target.GetTotalLastTargets(lang, "", true, DateTime.Now.AddDays(-365));
            m.Targets = ControllerBase.LatestTargetByCategory(lang, "", true, ini, max, userId, DateTime.Now.AddDays(-365));

            return m;

        }

        [HttpGet("[action]")]
        public TargetCategoryModel ByCategory(string token, int lang, string categ, int ini, int max)
        {

            string userId = null;

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            TargetCategoryModel m = new TargetCategoryModel();

            LibVisLib.Category c = LibVisLib.Category.LoadCategory(categ);
            if (c != null)
            {

                m.Title = msgs.Get(c.nameMsg);
                m.Description = msgs.Get(c.descriptionMsg);
                m.Ini = ini;
                m.Total = LibVisLib.Target.GetTotalLastTargets(lang, categ, true, DateTime.Now.AddDays(-365));
                m.Targets = ControllerBase.LatestTargetByCategory(lang, categ, true, ini, max, userId, DateTime.Now.AddDays(-365));

            }

            return m;

        }

        [HttpPost("[action]")]
        public async Task<GenericIdModel> Register(string token, [FromBody]NewTargetModel target)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            // Preciso da lingua logo no início

            int lang = LibVisLib.Verify.ValidLanguage(target.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);
            
            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.InvalidToken);

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserNotFound);

            }

            if (gsm.Result == 0)
            {

                int n = LibVisLib.Target.GetTotalLastTargetsForUser(prf.user.id, DateTime.Now.AddDays(-1));
                if (n >= 3 && prf.user.profile < BaseUser.InternalSystemProfile.InternalStaff)
                {

                    gsm.Result = (int)RacMsg.Id.MaximunOf3SubmissionPerDay;
                    gsm.ResultComplement = msgs.Get(RacMsg.Id.MaximunOf3SubmissionPerDay) + "; ";

                }

                try
                {

                    if (!LibVisLib.Verify.AcceptInteger(target.Categ))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharsInCategories) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCateg: Invalid chars");
                    }

                    target.Title = LibVisLib.Verify.NonPritableToSpace(target.Title);
                    target.Text = LibVisLib.Verify.NonPritableToSpace(target.Text);

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(target.Title, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTitle) + " (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: Invalid chars (" + r0 + ")");
                    }

                    string r1 = "";
                    if (!LibVisLib.Verify.AcceptMultilineFreeText(target.Text, ref r1))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInText) + " (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errText: Invalid chars (" + r1 + ")");
                    }

                    string r2 = "";
                    if (!LibVisLib.Verify.AcceptUrl(target.Link, ref r2))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInLink) + " (" + r2 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLink: Invalid chars or url format (" + r2 + ")");
                    }

                    string imagem = "";

                    if (target.ImageType == 1)
                    {

                        r1 = "";
                        if (!LibVisLib.Verify.AcceptUrl(target.Image, ref r1))
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInImageUrl) + " (" + r1 + "); ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Invalid chars (url) (" + r1 + "); ");

                        }

                        imagem = LibVisLib.Verify.VerifyUrl(target.Image);

                    }
                    else if (target.ImageType == 2)
                    {

                        r1 = "";
                        if (!LibVisLib.Verify.AcceptFreeText(target.Image, ref r1))
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInImageData) + " (" + r1 + "); ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Invalid chars (imagem base64) (" + r1 + "); ");

                        }

                        imagem = LibVisLib.Verify.VerifyFreeText(target.Image);

                        string[] imagempart = imagem.Split(',');

                        if (imagempart.Length > 0)
                            imagem = imagempart[imagempart.Length - 1];

                    }

                    string title = LibVisLib.Verify.VerifyFreeText(target.Title);
                    string text = LibVisLib.Verify.VerifyMultilineFreeText(target.Text);
                    string link = LibVisLib.Verify.VerifyUrl(target.Link);
                    string categ = LibVisLib.Verify.VerifyInteger(target.Categ);

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

                    if (text.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TextTooShort) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: required at least 10 chars");
                    }

                    if (text.Length > 10000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TextTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: Content too big");
                    }

                    if (link.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.LinkTooShort) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLink: required at least 10 chars");
                    }

                    if (link.Length > 500)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.LinkTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLink: Content too big");
                    }

                    if (imagem.Length > 5000000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.ImageTooLarge) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Content too big");
                    }

                    if (Target.AlreadyExists(lang, link, title))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.AgendaAlreadyRegistered) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTarget: Already exists");
                    }

                    if (gsm.Result == 0)
                    {

                        LibVisLib.Target trg = new LibVisLib.Target();

                        trg.title = title;
                        trg.text = text;
                        trg.link = link;
                        trg.status = LibVisLib.Target.NewsTargetStatus.Created;
                        trg.registered = DateTime.Now;
                        trg.language = (RacMsg.Language)lang;

                        trg.categories = new List<string>();

                        int ic = 0;
                        for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                            if (!LibVisLib.Category.categories[i].main)
                                if (ic < categ.Length && categ[ic++] == '1')
                                    trg.categories.Add(Category.categories[i].label);

                        trg.NormalizeMain();

                        if (trg.Save())
                        {

                            try
                            {

                                if (target.ImageType == 2)
                                {

                                    byte[] str = Convert.FromBase64String(imagem);

                                    MemoryStream ms = new MemoryStream(str, 0, str.Length);
                                    ms.Write(str, 0, str.Length);
                                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                                    string path = Base.conf.tempImageFilePath + "\\t-" + trg.id + ".jpg";

                                    img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                                }
                                else if (target.ImageType == 1)
                                {

                                    System.Drawing.Image img = null;

                                    try
                                    {

                                        HttpClient client = new HttpClient();
                                        HttpResponseMessage response = await client.GetAsync(imagem);
                                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                        ms.Write(imageBytes, 0, imageBytes.Length);
                                        img = System.Drawing.Image.FromStream(ms, true);

                                    }
                                    catch (Exception ex)
                                    {

                                        gsm.Result = (int)RacLib.RacMsg.Id.InternalError;
                                        gsm.ResultComplement += ex.Message;
                                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.InternalError, "Exception getting http image");
                                        RacLib.BaseLog.log.Log(ex);

                                    }

                                    if (img != null)
                                    {

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

                                                byte[] str = stream.ToArray();

                                                MemoryStream ms2 = new MemoryStream(str, 0, str.Length);
                                                ms2.Write(str, 0, str.Length);
                                                System.Drawing.Image img2 = System.Drawing.Image.FromStream(ms2, true);

                                                string path = Base.conf.tempImageFilePath + "\\t-" + trg.id + ".jpg";

                                                img2.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                                            }

                                        }

                                    }

                                }

                            }
                            catch (Exception ex)
                            {

                                gsm.Result = (int)RacLib.RacMsg.Id.InternalError;
                                gsm.ResultComplement += ex.Message;
                                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.InternalError, "Exception treating image");
                                RacLib.BaseLog.log.Log(ex);

                            }

                        }

                        LibVisLib.TargetAction act = new LibVisLib.TargetAction(trg);

                        act.date = trg.registered;
                        act.type = LibVisLib.TargetAction.ActionType.Suggested;
                        act.userId = userId;
                        act.show = true;
                        act.observation = "";

                        act.Save();

                        gsm.Id = trg.id;

                        prf.RegisterAction(Profile.ProfileAction.RegisterTarget);

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
        public GenericIdModel Change(string token, [FromBody]ChangeTargetModel target)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            // Preciso da lingua logo no início

            int lang = 2;

            if (target.Lang == 3 || target.Lang == 4)
                lang = target.Lang;

            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.InvalidToken);

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserNotFound);

            }

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuid(target.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInId) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptInteger(target.Categ))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharsInCategories) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCateg: Invalid chars");
                    }

                    target.Title = LibVisLib.Verify.NonPritableToSpace(target.Title);
                    target.Text = LibVisLib.Verify.NonPritableToSpace(target.Text);

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(target.Title, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTitle) + " (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: Invalid chars (" + r0 + ")");
                    }

                    string r1 = "";
                    if (!LibVisLib.Verify.AcceptMultilineFreeText(target.Text, ref r1))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInText) + " (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errText: Invalid chars (" + r1 + ")");
                    }

                    string r2 = "";
                    if (!LibVisLib.Verify.AcceptUrl(target.Link, ref r2))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInLink) + " (" + r2 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLink: Invalid chars or url format (" + r2 + ")");
                    }

                    string imagem = "";

                    r1 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(target.Image, ref r1))
                    {

                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInImageData) + " (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Invalid chars (imagem base64) (" + r1 + ")");

                    }

                    imagem = LibVisLib.Verify.VerifyFreeText(target.Image);

                    string[] imagempart = imagem.Split(',');

                    if (imagempart.Length > 0)
                        imagem = imagempart[imagempart.Length - 1];

                    string targetId = LibVisLib.Verify.VerifyGuid(target.Id);
                    string title = LibVisLib.Verify.VerifyFreeText(target.Title);
                    string text = LibVisLib.Verify.VerifyMultilineFreeText(target.Text);
                    string link = LibVisLib.Verify.VerifyUrl(target.Link);
                    string categ = LibVisLib.Verify.VerifyInteger(target.Categ);

                    if (targetId == null || targetId.Length < 36)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidId) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Id inválido");
                    }

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

                    if (text.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TextTooShort) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: required at least 10 chars");
                    }

                    if (text.Length > 10000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TextTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: Content too big");
                    }

                    if (link.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.LinkTooShort) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLink: required at least 10 chars");
                    }

                    if (link.Length > 500)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.LinkTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLink: Content too big");
                    }

                    if (imagem.Length > 5000000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.ImageTooLarge) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Content too big");
                    }
                    if (gsm.Result == 0)
                    {

                        LibVisLib.Target trg = Target.LoadTarget(targetId);

                        trg.title = title;
                        trg.text = text;
                        trg.link = link;

                        trg.categories = new List<string>();

                        int ic = 0;
                        for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                            if (!LibVisLib.Category.categories[i].main)
                                if (ic < categ.Length && categ[ic++] == '1')
                                    trg.categories.Add(Category.categories[i].label);

                        trg.NormalizeMain();

                        byte[] str = Convert.FromBase64String(imagem);

                        MemoryStream ms = new MemoryStream(str, 0, str.Length);
                        ms.Write(str, 0, str.Length);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                        string path = Base.conf.tempImageFilePath + "\\t-" + trg.id + ".jpg";

                        img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                        if (trg.Save())
                        {

                            gsm.Id = trg.id;

                            if (!trg.HasAlreadyRevised(userId))
                                prf.RegisterAction(Profile.ProfileAction.ReviseTarget);

                            if (userId != trg.authorId)
                            {

                                LibVisLib.TargetAction act = new LibVisLib.TargetAction(trg);

                                act.date = DateTime.Now;
                                act.type = LibVisLib.TargetAction.ActionType.Revised;
                                act.userId = userId;
                                act.show = true;
                                act.observation = "";

                                act.Save();

                            }

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

        [HttpPost("[action]")]
        public GenericStatusModel RegisterVote(string token, [FromBody]RegisterVote vote)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";

            // Preciso da lingua logo no início

            int lang = 2;

            if (vote.Lang == 3 || vote.Lang == 4)
                lang = vote.Lang;

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

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(vote.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInId) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");
                    }

                    if (vote.Vote < 0 || vote.Vote > 6)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidVote) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errVote: Invalid value");
                    }

                    if (gsm.Result == 0)
                    {

                        string targetId = LibVisLib.Verify.VerifyGuid(vote.Id);

                        LibVisLib.Target trg = LibVisLib.Target.LoadTarget(targetId);
                        if (trg == null)
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidId) + "; ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");

                        }
                        else
                        {

                            if (trg.SetUserVote(userId, (LibVisLib.TargetVote.Vote)vote.Vote))
                                prf.RegisterAction(Profile.ProfileAction.VoteTarget);

                            trg.Save();

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

        [HttpPost("[action]")]
        public GenericStatusModel ChangeLanguage(string token, [FromBody]ChangeLanguage chg)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";

            // Preciso da lingua logo no início
            
            int lang = LibVisLib.Verify.ValidLanguage(chg.Lang);
            int newlang = LibVisLib.Verify.ValidLanguage(chg.NewLang);
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

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(chg.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInId) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");
                    }

                    if (gsm.Result == 0)
                    {

                        string targetId = LibVisLib.Verify.VerifyGuid(chg.Id);

                        LibVisLib.Target trg = LibVisLib.Target.LoadTarget(targetId);
                        if (trg == null)
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidId) + "; ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");

                        }
                        else
                        {

                            trg.language = (RacMsg.Language)newlang;
                            trg.Save();

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
        public EditTargetBaseModel EditBase(string token, int lang)
        {

            EditTargetBaseModel gsm = new EditTargetBaseModel();

            // Preciso da lingua logo no início

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            // Verifica o token de autenticação

            string userId = null;
            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            gsm.Categories = new List<NewsCategory>();

            for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                if (!LibVisLib.Category.categories[i].main)
                    gsm.Categories.Add(new NewsCategory(msgs, LibVisLib.Category.categories[i]));

            gsm.Awards = new List<NewsAward>();

            for (int i = 0; i < LibVisLib.Award.awards.Count; i++)
                if (LibVisLib.Award.awards[i].type == Award.Type.Positive && LibVisLib.Award.awards[i].appliedTo == Award.AppliedTo.Target)
                    gsm.Awards.Add(new NewsAward(LibVisLib.Award.awards[i]));

            return gsm;

        }

        [HttpPost("[action]")]
        public async Task<LinkResultModel> Link(string token, [FromBody]LinkModel data)
        {

            Console.WriteLine("Entrei");

            LinkResultModel lrm = new LinkResultModel();

            int lang = LibVisLib.Verify.ValidLanguage(data.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                lrm.Result = (int)RacMsg.Id.InvalidToken;
                lrm.ResultComplement = msgs.Get(RacMsg.Id.InvalidToken);

                return lrm;

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                lrm.Result = (int)RacMsg.Id.UserNotFound;
                lrm.ResultComplement = msgs.Get(RacMsg.Id.UserNotFound);

                return lrm;

            }

            try
            {

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
                HttpResponseMessage response = await client.GetAsync(data.Link);
                string pageContents = await response.Content.ReadAsStringAsync();
                HtmlDocument pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);

                lrm.Link = data.Link;

                lrm.Title = "";

                // Checa primeiro o og:title...
                HtmlNodeCollection ogt = pageDocument.DocumentNode.SelectNodes("//meta[@property='og:title']");
                if (ogt != null)
                {

                    foreach (HtmlNode n in ogt)
                    {

                        if (n.Attributes["content"] != null)
                        {

                            lrm.Title = RemoveEnter(n.Attributes["content"].Value);
                            break;

                        }

                    }

                }

                if (lrm.Title == "")
                {

                    // Checa todas as h...
                    HtmlNodeCollection h1col = pageDocument.DocumentNode.SelectNodes("//article//h1");
                    if (h1col != null)
                    {

                        foreach (HtmlNode n in h1col)
                        {

                            lrm.Title = RemoveEnter(n.InnerHtml);
                            break;

                        }

                    }

                }

                if (lrm.Title == "")
                {

                    // Checa todas as h...
                    HtmlNodeCollection h1colm = pageDocument.DocumentNode.SelectNodes("//main//h1");
                    if (h1colm != null)
                    {

                        foreach (HtmlNode n in h1colm)
                        {

                            lrm.Title = RemoveEnter(n.InnerHtml);
                            break;

                        }

                    }


                }

                if (lrm.Title == "")
                {

                    HtmlNodeCollection h2col = pageDocument.DocumentNode.SelectNodes("//article//h2");
                    if (h2col != null)
                    {

                        foreach (HtmlNode n in h2col)
                        {

                            lrm.Title = RemoveEnter(n.InnerHtml);
                            break;

                        }

                    }

                }

                if (lrm.Title == "")
                {

                    HtmlNodeCollection h3col = pageDocument.DocumentNode.SelectNodes("//article//h3");
                    if (h3col != null)
                    {

                        foreach (HtmlNode n in h3col)
                        {

                            lrm.Title = RemoveEnter(n.InnerHtml);
                            break;

                        }

                    }

                }

                if (lrm.Title == "")
                {

                    // Checa todas as h...
                    HtmlNodeCollection h1col = pageDocument.DocumentNode.SelectNodes("//h1");
                    if (h1col != null)
                    {

                        foreach (HtmlNode n in h1col)
                        {

                            lrm.Title = RemoveEnter(n.InnerHtml);
                            break;

                        }

                    }

                }

                if (lrm.Title == "")
                {

                    // Checa todas as h...
                    HtmlNodeCollection h2col = pageDocument.DocumentNode.SelectNodes("//h2");
                    if (h2col != null)
                    {

                        foreach (HtmlNode n in h2col)
                        {

                            lrm.Title = RemoveEnter(n.InnerHtml);
                            break;

                        }

                    }

                }

                if (lrm.Title == "")
                {

                    // Checa todas as h...
                    HtmlNodeCollection h3col = pageDocument.DocumentNode.SelectNodes("//h3");
                    if (h3col != null)
                    {

                        foreach (HtmlNode n in h3col)
                        {

                            lrm.Title = RemoveEnter(n.InnerHtml);
                            break;

                        }

                    }

                }

                lrm.Text = "";

                HtmlNodeCollection pcol = pageDocument.DocumentNode.SelectNodes("//article//p");
                if (pcol != null)
                {

                    foreach (HtmlNode n in pcol)
                    {

                        string text = RemoveTags(n.InnerHtml);
                        if (LooksLikeText(text))
                            lrm.Text += text + "\r\n";

                        if (lrm.Text.Length > 1000)
                            break;

                    }

                }

                if (lrm.Text == "")
                {

                    HtmlNodeCollection pdes = pageDocument.DocumentNode.SelectNodes("//meta[@property='og:description']");
                    if (pdes != null)
                    {

                        foreach (HtmlNode n in pdes)
                        {

                            if (n.Attributes["content"] != null)
                            {
                                string text = RemoveTags(n.Attributes["content"].Value);

                                if (LooksLikeText(text))
                                    lrm.Text += text + "\r\n";

                            }

                            if (lrm.Text.Length > 1000)
                                break;

                        }

                    }

                }

                lrm.Image = new List<string>();

                // Checa primeiro o og:image...
                HtmlNodeCollection ogi = pageDocument.DocumentNode.SelectNodes("//meta[@property='og:image']");
                if (ogi != null)
                {

                    foreach (HtmlNode n in ogi)
                    {

                        if (n.Attributes["content"] != null)
                            lrm.Image.Add(n.Attributes["content"].Value);

                        if (lrm.Image.Count >= 3)
                            break;

                    }

                }

                if (lrm.Image.Count < 3)
                {

                    HtmlNodeCollection pimg = pageDocument.DocumentNode.SelectNodes("//article//img");
                    if (pimg != null)
                    {

                        foreach (HtmlNode n in pimg)
                        {

                            if (n.Attributes["src"] != null)
                                lrm.Image.Add(n.Attributes["src"].Value);

                            if (lrm.Image.Count >= 3)
                                break;

                        }

                    }

                }

                if (lrm.Image.Count < 3)
                {

                    HtmlNodeCollection pimg2 = pageDocument.DocumentNode.SelectNodes("//article//img");
                    if (pimg2 != null)
                    {

                        foreach (HtmlNode n in pimg2)
                        {

                            if (n.Attributes["src"] != null)
                                lrm.Image.Add(n.Attributes["src"].Value);

                            if (lrm.Image.Count >= 3)
                                break;

                        }

                    }

                }

                //
                //   Já existe a pauta?
                //

                if (Target.AlreadyExists(lang, lrm.Link, lrm.Title))
                {

                    lrm = new LinkResultModel();
                    lrm.Result = (int)RacMsg.Id.AlreadyExists;
                    lrm.ResultComplement = "Pauta conhecida";

                }


            }
            catch (Exception ex)
            {

                lrm.Result = (int)RacMsg.Id.InternalError;
                lrm.ResultComplement = ex.Message;

            }

            return lrm;

        }
        
        [HttpPost("[action]")]
        public GenericStatusModel RegisterGrant(string token, [FromBody]RegisterGrant grant)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";

            int lang = LibVisLib.Verify.ValidLanguage(grant.Lang);
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

            if (!prf.user.isInternal)
            {

                gsm.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserHasNoRightToThisItem); 

            }

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(grant.ActionId))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInId) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errActionId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptGuidOrNull(grant.AwardId))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInId) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errAwardId: Invalid chars");
                    }

                    if (grant.Add < 0 || grant.Add > 1)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidAction) + "; ";
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
                            gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidAction) + "; ";
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

                    Target target = Target.LoadTarget(id);
                    if (target != null)
                    {

                        try
                        {

                            string path = Base.conf.tempImageFilePath + "\\t-" + id + ".jpg";
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

        string RemoveEnter(string src)
        {

            string c0 = RemoveTags(src);
            string c1 = c0.Replace("\n", " ");
            string c2 = c1.Replace("\r", " ");
            string c3 = c2.Replace("\t", " ");
            string c4 = c3.Replace("\xa0", " ");
            string c5 = LibVisLib.Verify.VerifyFreeText(c4);

            return c5.Trim();

        }

        string RemoveTags(string src)
        {

            string saida = "";
            int cont_tag = 0;
            int cont_amp = 0;

            for (int i = 0; i < src.Length; i++)
            {

                if (src[i] == '<')
                    cont_tag++;

                if (src[i] == '&')
                {

                    bool valid = false;

                    for (int j = 1; j < 8; j++)
                    {

                        if (src[i + j] == '&' || src[i + j] == ' ')
                            break;

                        if (src[i + j] == ';')
                        {
                            valid = true;
                            break;
                        }

                    }

                    if (valid)
                        cont_amp++;

                }

                if (cont_tag == 0 && cont_amp == 0)
                    saida += src[i];

                if (src[i] == ';' && cont_amp > 0)
                    cont_amp--;

                if (src[i] == '>')
                    cont_tag--;

            }

            return LibVisLib.Verify.VerifyMultilineFreeText(saida);

        }

        bool LooksLikeText(string src)
        {

            int len = src.Length;

            if (len < 30)
                return false;

            int nw = src.Split(' ').Length;
            int nlpw = len / nw;

            if (nlpw < 2 || nlpw > 30)
                return false;

            return true;


        }

        [HttpPost("[action]")]
        public TargetListModel Search(string token, int lang, int ini, int max, [FromBody]SearchTargetModel search)
        {

            string userId = null;

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            TargetListModel m = new TargetListModel();

            if (!LibVisLib.Verify.AcceptFreeText(search.SearchString))
            {
                m.Result = (int)RacLib.RacMsg.Id.Error;
                m.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInSearchString);
                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errSearchString: Invalid chars");
            }

            string srch = LibVisLib.Verify.VerifyFreeText(search.SearchString);

            m.Ini = ini;
            m.Total = LibVisLib.Target.GetTotalLastTargets(lang, "", true, DateTime.Now.AddDays(-365), srch);
            m.Targets = ControllerBase.LatestTargetByCategory(lang, "", true, ini, max, userId, DateTime.Now.AddDays(-365), srch);

            return m;

        }

    }

}
