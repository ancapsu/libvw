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
    public class ArticleController : Controller
    {

        [HttpGet("[action]")]
        public ArticleModel Get(string token, int lang, string id)
        {

            string userId = null;

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            LibVisLib.Article art = LibVisLib.Article.LoadArticle(id);

            ArticleModel m = new ArticleModel();

            if (art != null)
            {

                m = new ArticleModel(msgs, art, true, true, true, true, true, true, true);

                if (userId != null && userId != "")
                    m.UserVote = (int)art.GetUserVote(userId);

            }

            return m;

        }

        [HttpGet("[action]")]
        public ArticleListModel List(string token, int lang, int ini, int max, int sts)
        {

            string userId = "";

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            if (sts < 0 || sts > 7)
                sts = 0;

            ArticleListModel m = new ArticleListModel();

            m.Sts = sts;
            m.Ini = ini;
            m.Total = LibVisLib.Article.GetTotalLastArticles(lang, userId, "", sts);
            m.Articles = ControllerBase.LatestArticlesByCategory(lang, userId, "", ini, max, sts);

            return m;

        }

        [HttpGet("[action]")]
        public ArticleListModel ListForTranslation(string token, int lang, int ini, int max)
        {

            string userId = "";

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            ArticleListModel m = new ArticleListModel();
            
            m.Ini = ini;
            m.Total = LibVisLib.Article.GetTotalArticlesForTranslation(lang);
            m.Articles = ControllerBase.LatestArticlesForTranslation(lang, userId, ini, max);

            return m;

        }

        [HttpGet("[action]")]
        public ArticleCategoryModel ByCategory(string token, int lang, string categ, int ini, int max)
        {

            string userId = "";

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            ArticleCategoryModel m = new ArticleCategoryModel();

            LibVisLib.Category c = LibVisLib.Category.LoadCategory(categ);
            if (c != null)
            {

                m.Title = msgs.Get(c.nameMsg);
                m.Description = msgs.Get(c.descriptionMsg);
                m.Ini = ini;
                m.Total = LibVisLib.Article.GetTotalLastArticles(lang, userId, categ, 0);
                m.Articles = ControllerBase.LatestArticlesByCategory(lang, userId, categ, ini, max, 0);

            }

            return m;

        }
        
        [HttpGet("[action]")]
        public ArticleModel ArticleInfo(string id, int lang)
        {

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            ArticleModel m = new ArticleModel();

            LibVisLib.Article n = LibVisLib.Article.LoadArticle(id);
            if (n != null)
                m = new ArticleModel(msgs, n, true, true, false, true, true, false, false);

            return m;

        }

        [HttpPost("[action]")]
        public GenericIdModel Register(string token, [FromBody]NewArticleModel article)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            int lang = LibVisLib.Verify.ValidLanguage(article.Lang);
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

                int n = LibVisLib.Article.NumberArticlesRegisteredSince(prf.user.id, DateTime.Now.AddDays(-1));
                if (n >= 1 && prf.user.profile < BaseUser.InternalSystemProfile.InternalStaff)
                {

                    gsm.Result = (int)RacMsg.Id.Number;
                    gsm.ResultComplement = msgs.Get(RacMsg.Id.TooManyArticlesMaximum1PerDay) + "; ";

                }

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(article.TargetId))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInId) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptInteger(article.Categ))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharsInCategories) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCateg: Invalid chars");
                    }

                    article.Title = LibVisLib.Verify.NonPritableToSpace(article.Title);
                    article.Text = LibVisLib.Verify.NonPritableToSpace(article.Text);

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(article.Title, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTitle) + " (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: Invalid chars (" + r0 + ")");
                    }

                    string r1 = "";
                    if (!LibVisLib.Verify.AcceptMultilineFreeText(article.Text, ref r1))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInText) + " (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errText: Invalid chars (" + r1 + ")");
                    }

                    string imagem = "";

                    r1 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(article.Image, ref r1))
                    {

                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInImageData) + " (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Invalid chars (imagem base64) (" + r1 + ")");

                    }

                    imagem = article.Image;

                    string[] imagempart = imagem.Split(',');

                    if (imagempart.Length > 0)
                        imagem = imagempart[imagempart.Length - 1];

                    string targetId = LibVisLib.Verify.VerifyGuidOrNull(article.TargetId);
                    string title = LibVisLib.Verify.VerifyFreeText(article.Title);
                    string text = LibVisLib.Verify.VerifyMultilineFreeText(article.Text);
                    string categ = LibVisLib.Verify.VerifyInteger(article.Categ);

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

                    if (text.Length > 100000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.TextTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: Content too big");
                    }

                    if (imagem.Length > 5000000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.ImageTooLarge) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Content too big");
                    }

                    if (LibVisLib.Article.AlreadyExists(title))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.ArticleAlreadyRegisteredWithThisTitle) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTarget: Already exists");
                    }

                    if (gsm.Result == 0)
                    {
                        
                        lock (RacLib.BaseLog.log)
                        {

                            LibVisLib.Article art = new LibVisLib.Article();

                            art.targetId = targetId;
                            art.title = title;
                            art.text = text;
                            art.status = LibVisLib.Article.ArticleStatus.Created;
                            art.released = DateTime.Now;
                            art.lastChanged = art.released;
                            art.type = (LibVisLib.Article.ArticleType)article.Type;
                            art.language = (RacMsg.Language)lang;

                            art.categories = new List<string>();

                            int ic = 0;
                            for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                                if (!LibVisLib.Category.categories[i].main)
                                    if (ic < categ.Length && categ[ic++] == '1')
                                        art.categories.Add(Category.categories[i].label);

                            art.NormalizeMain();

                            art.Save();

                            try
                            {

                                byte[] str = Convert.FromBase64String(imagem);

                                MemoryStream ms = new MemoryStream(str, 0, str.Length);
                                ms.Write(str, 0, str.Length);
                                System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                                string path = Base.conf.tempImageFilePath + "\\a-" + art.id + ".jpg";

                                img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                            }
                            catch(Exception ex)
                            {

                                gsm.Result = (int)RacLib.RacMsg.Id.InternalError;
                                gsm.ResultComplement += ex.Message;
                                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.InternalError, "Exception saving image");
                                RacLib.BaseLog.log.Log(ex);

                            }

                            if (art.target != null)
                            {

                                ArticleAction act0 = new ArticleAction(art);

                                act0.date = art.target.registered;
                                act0.type = ArticleAction.ActionType.Suggested;
                                act0.userId = art.target.authorId;
                                act0.show = true;
                                act0.observation = "";
                                
                                act0.Save();

                                Profile prft = Profile.LoadProfile(art.target.authorId);
                                prft.RegisterAction(Profile.ProfileAction.TargetUsed);

                            }
                            else
                            {

                                ArticleAction act0 = new ArticleAction(art);

                                act0.date = art.released;
                                act0.type = ArticleAction.ActionType.Suggested;
                                act0.userId = userId;
                                act0.show = true;
                                act0.observation = "";
                                
                                act0.Save();

                            }

                            ArticleAction act = new ArticleAction(art);

                            act.date = art.released;
                            act.type = ArticleAction.ActionType.Created;
                            act.userId = userId;
                            act.show = true;
                            act.observation = "";
                            act.billableWords = art.wordCount;
                            act.text = text;

                            act.Save();

                            prf.RegisterAction(Profile.ProfileAction.RegisterArticle);

                            gsm.Id = art.id;

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
        public GenericIdModel IncludeObservation(string token, [FromBody]IncludeObservaton obs)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            int lang = obs.Lang;

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

                    gsm.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                    gsm.ResultComplement = msgs.Get(RacMsg.Id.UserHasNoRightToThisItem);

                }

                try
                {

                    if (!LibVisLib.Verify.AcceptGuid(obs.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInId) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errActionId: Invalid chars");
                    }

                    obs.Comment = LibVisLib.Verify.NonPritableToSpace(obs.Comment);

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(obs.Comment, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no comentário (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: Invalid chars (" + r0 + ")");
                    }

                    string id = LibVisLib.Verify.VerifyGuid(obs.Id);
                    string comment = LibVisLib.Verify.VerifyFreeText(obs.Comment);

                    if (comment.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Commentário muito pequeno (mínimo 10 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: required at least 10 chars");
                    }

                    if (comment.Length > 1000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Commentário muito grande (máximo 1000 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: Content too big");
                    }

                    LibVisLib.ArticleAction aa = LibVisLib.ArticleAction.LoadArticleAction(id);
                    if (aa == null)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Action not found; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Article not found");
                    }

                    if (gsm.Result == 0)
                    {

                        ArticleActionObservation aao = new ArticleActionObservation(aa);

                        aao.userId = userId;
                        aao.observation = comment;
                        aao.included = DateTime.Now;

                        aa.observations.Add(aao);
                        aa.Save();

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
        public GenericIdModel IncludeComment(string token, [FromBody]IncludeObservaton obs)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Token inválido; ";

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Usuário não encontrado; ";

            }

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuid(obs.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id da ação/artigo; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errActionId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptGuid(obs.CommentId))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id do comentario; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCommentId: Invalid chars");
                    }

                    obs.Comment = LibVisLib.Verify.NonPritableToSpace(obs.Comment);

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(obs.Comment, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no comentário (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: Invalid chars (" + r0 + ")");
                    }

                    string id = LibVisLib.Verify.VerifyGuid(obs.Id);
                    string comment = LibVisLib.Verify.VerifyFreeText(obs.Comment);

                    if (comment.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Commentário muito pequeno (mínimo 10 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: required at least 10 chars");
                    }

                    if (comment.Length > 1000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Commentário muito grande (máximo 1000 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: Content too big");
                    }

                    LibVisLib.Article aa = LibVisLib.Article.LoadArticle(id);
                    if (aa == null)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Article not found; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Article not found");
                    }

                    if (gsm.Result == 0)
                    {

                        ArticleComment ac = new ArticleComment(aa);

                        ac.SetId(obs.CommentId);
                        ac.userId = userId;
                        ac.comment = comment;
                        ac.date = DateTime.Now;

                        aa.comments.Add(ac);
                        aa.Save();

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
        public GenericIdModel ChangeComment(string token, [FromBody]IncludeObservaton obs)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Token inválido; ";

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Usuário não encontrado; ";

            }

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuid(obs.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id da ação; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errActionId: Invalid chars");
                    }

                    obs.Comment = LibVisLib.Verify.NonPritableToSpace(obs.Comment);

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(obs.Comment, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no comentário (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: Invalid chars (" + r0 + ")");
                    }

                    string id = LibVisLib.Verify.VerifyGuid(obs.Id);
                    string comment = LibVisLib.Verify.VerifyFreeText(obs.Comment);

                    if (comment.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Commentário muito pequeno (mínimo 10 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: required at least 10 chars");
                    }

                    if (comment.Length > 1000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Commentário muito grande (máximo 1000 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errComment: Content too big");
                    }

                    LibVisLib.ArticleComment ac = LibVisLib.ArticleComment.LoadArticleComment(id);
                    if (ac == null)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Comentário não encotnrado; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Comment not found");
                    }

                    if (ac.userId != userId && ((int)(prf.user.profile) < 7))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Não tem direito de alterar esse comentário; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Comment insuficient rights");
                    }

                    if (gsm.Result == 0)
                    {

                        ac.comment = comment;                        
                        ac.Save();

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
        public GenericIdModel RemoveComment(string token, string id)
        {

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Token inválido; ";

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Usuário não encontrado; ";

            }

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuid(id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id da ação; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errActionId: Invalid chars");
                    }

                    id = LibVisLib.Verify.VerifyGuid(id);
                    
                    LibVisLib.ArticleComment ac = LibVisLib.ArticleComment.LoadArticleComment(id);
                    if (ac == null)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Comment not found; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Comment not found");
                    }

                    if (gsm.Result == 0)
                    {

                        ac.article.RemoveArticleComment(ac.id);
                        ac.Remove();

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
        public GenericIdModel Change(string token, [FromBody]ChangeArticleModel article)
        { 

            GenericIdModel gsm = new GenericIdModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";
            gsm.Id = "";

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Token inválido; ";

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "Usuário não encontrado; ";

            }

            if (gsm.Result == 0)
            {
                
                try
                {

                    if (!LibVisLib.Verify.AcceptGuid(article.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no Id da pauta; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptGuidOrNull(article.TargetId))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id do target; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptInteger(article.Categ))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos nas categorias; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCateg: Invalid chars");
                    }

                    article.Title = LibVisLib.Verify.NonPritableToSpace(article.Title);
                    article.Text = LibVisLib.Verify.NonPritableToSpace(article.Text);

                    string r0 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(article.Title, ref r0))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no título (" + r0 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTitle: Invalid chars (" + r0 + ");");
                    }

                    string r1 = "";
                    if (!LibVisLib.Verify.AcceptMultilineFreeText(article.Text, ref r1))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no texto (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errText: Invalid chars (" + r1 + ");");
                    }

                    string imagem = "";

                    r1 = "";
                    if (!LibVisLib.Verify.AcceptFreeText(article.Image, ref r1))
                    {

                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos nos dados da imagem (" + r1 + "); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Invalid chars (imagem base64) (" + r1 + ");");

                    }

                    imagem = article.Image;

                    string[] imagempart = imagem.Split(',');

                    if (imagempart.Length > 0)
                        imagem = imagempart[imagempart.Length - 1];

                    string articleId = LibVisLib.Verify.VerifyGuid(article.Id);
                    string targetId = LibVisLib.Verify.VerifyGuidOrNull(article.TargetId);
                    string title = LibVisLib.Verify.VerifyFreeText(article.Title);
                    string text = LibVisLib.Verify.VerifyMultilineFreeText(article.Text);
                    string categ = LibVisLib.Verify.VerifyInteger(article.Categ);

                    if (articleId == null || articleId.Length < 36)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Id do artigo inválido; ";
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

                    if (text.Length < 10)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Texto muito pequeno (mínimo 10 caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: required at least 10 chars");
                    }

                    if (text.Length > 100000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Texto muito grande (máximo 100k caracteres); ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTexto: Content too big");
                    }

                    if (imagem.Length > 5000000)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Imagem muito grande; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errImage: Content too big");
                    }

                    if (gsm.Result == 0)
                    {

                        lock (RacLib.BaseLog.log)
                        {

                            bool chgtxt = false;

                            LibVisLib.Article art = LibVisLib.Article.LoadArticle(articleId);

                            art.targetId = targetId;
                            art.title = title;

                            if (art.text != text)
                                chgtxt = true;

                            art.text = text;
                            art.type = (LibVisLib.Article.ArticleType)article.Type;
                            art.released = DateTime.Now;
                            art.lastChanged = art.released;

                            art.categories = new List<string>();

                            int ic = 0;
                            for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                                if (!LibVisLib.Category.categories[i].main)
                                    if (ic < categ.Length && categ[ic++] == '1')
                                        art.categories.Add(Category.categories[i].label);

                            art.NormalizeMain();

                            byte[] str = Convert.FromBase64String(imagem);

                            MemoryStream ms = new MemoryStream(str, 0, str.Length);
                            ms.Write(str, 0, str.Length);
                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                            string path = Base.conf.tempImageFilePath + "\\a-" + art.id + ".jpg";

                            img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                            if (art.Save())
                            {

                                gsm.Id = art.id;

                                if (!art.HasAlreadyRevised(userId))
                                    prf.RegisterAction(Profile.ProfileAction.ReviseArticle);

                                if (userId != art.authoredId)
                                {

                                    ArticleAction act = new ArticleAction(art);

                                    act.date = art.released;
                                    act.type = ArticleAction.ActionType.Revised;
                                    act.userId = userId;
                                    act.show = true;
                                    act.observation = "";
                                    act.billableWords = art.wordCount;

                                    if (chgtxt)
                                        act.text = text;

                                    act.Save();

                                }
                                else
                                {

                                    ArticleAction act = art.GetActionForType(ArticleAction.ActionType.Created);
                                    if (art != null)
                                    {

                                        act.billableWords = art.wordCount;

                                        if (chgtxt)
                                            act.text = text;

                                        act.Save();

                                    }

                                }

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

                        LibVisLib.ArticleAction arta = LibVisLib.ArticleAction.LoadArticleAction(actionId);
                        if (arta == null)
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += "Ação inválida; ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid target");

                        }
                        else
                        {

                            if (grant.Add == 0)
                                arta.RemoveGrant(grant.AwardId);
                            else
                                arta.AddGrant(grant.AwardId, userId);

                            arta.Save();

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
        public GenericStatusModel Publish(string token, [FromBody]Publish data)
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

            //if (!prf.user.isInternal && ((data.Status == 3 && !prf.revisor && !prf.narrator) || (data.Status == 5 && !prf.producer)) || (data.Status == 6 && !prf.revisor))
            //{ 

//                gsm.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
//                gsm.ResultComplement = "Usuário não tem direitos para essa ação";

 //           }

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(data.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id do artigo; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errArticleId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptGuidOrNull(data.Info))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id do narrador; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errNarratorId: Invalid chars");
                    }
                    
                    if (data.Status < 1 || data.Status > 7)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Situação inválida; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errStatus: Invalid value");
                    }

                    if (gsm.Result == 0)
                    {

                        string articleId = LibVisLib.Verify.VerifyGuid(data.Id);
                        string preferredId = LibVisLib.Verify.VerifyGuid(data.Info);

                        LibVisLib.Article art = LibVisLib.Article.LoadArticle(articleId);
                        if (art == null)
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += "Ação inválida; ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errArticleId: Invalid article");

                        }
                        else
                        {

                            LibVisLib.Article.ArticleStatus newStatus = (LibVisLib.Article.ArticleStatus)data.Status;

                            // Se for igual, ignora

                            if (art.status != newStatus) {

                                art.status = newStatus;
                                art.lastChanged = DateTime.Now;
                                art.Save();

                                if (newStatus == LibVisLib.Article.ArticleStatus.Approved)
                                {

                                    ArticleAction act = new ArticleAction(art);

                                    act.date = DateTime.Now;
                                    act.type = ArticleAction.ActionType.Approved;
                                    act.userId = userId;
                                    act.show = true;
                                    act.observation = "";
                                                                       
                                    act.Save();

                                    if (preferredId != null && preferredId != "" & preferredId != "0")
                                    {

                                        art.preferredRevisorId = preferredId;
                                        art.Save();

                                    }

                                }
                                else if (newStatus == LibVisLib.Article.ArticleStatus.Revised)
                                {

                                    ArticleAction act = new ArticleAction(art);

                                    act.date = DateTime.Now;
                                    act.type = ArticleAction.ActionType.Revised;
                                    act.userId = userId;
                                    act.show = true;
                                    act.observation = "";
                                    
                                    act.Save();

                                    prf.RegisterAction(Profile.ProfileAction.ReviseArticle);

                                    if (preferredId != null && preferredId != "" & preferredId != "0")
                                    {

                                        art.preferredNarratorId = preferredId;
                                        art.Save();
                                        
                                        RacLib.EmailTemplate tpl = RacLib.EmailTemplate.LoadEmailTemplate(RacLib.EmailTemplate.ArticleNarrationEmailId);
                                        RacLib.Email eml = tpl.CreateNewEmail(RacLib.InternalSystem.LibVisId);
                                        eml.SetParam(eml.system);
                                        eml.destination.Add(new RacLib.EmailName(preferredId));
                                        eml.SetParam("title", art.title);
                                        eml.SetParam("hml", "");
                                        eml.SetParam("path", "/article/");
                                        eml.SetParam("id", art.id);
                                        eml.SendAll(prf.user);

                                    }

                                }
                                else if (newStatus == LibVisLib.Article.ArticleStatus.Ready)
                                {

                                    ArticleAction act = new ArticleAction(art);

                                    act.date = DateTime.Now;
                                    act.type = ArticleAction.ActionType.IncludedNaration;
                                    act.userId = userId;
                                    act.show = true;
                                    act.observation = "";

                                    act.Save();

                                    //prf.RegisterAction(Profile.ProfileAction.Narrated);

                                    if (preferredId != null && preferredId != "" & preferredId != "0")
                                    {

                                        art.preferredProducerId = preferredId;
                                        art.Save();
                                        
                                        RacLib.EmailTemplate tpl = RacLib.EmailTemplate.LoadEmailTemplate(RacLib.EmailTemplate.ArticleProductionEmailId);
                                        RacLib.Email eml = tpl.CreateNewEmail(RacLib.InternalSystem.LibVisId);
                                        eml.SetParam(eml.system);
                                        eml.destination.Add(new RacLib.EmailName(preferredId));
                                        eml.SetParam("title", art.title);
                                        eml.SetParam("hml", "");
                                        eml.SetParam("path", "/article/");
                                        eml.SetParam("id", art.id);
                                        eml.SendAll(prf.user);

                                    }

                                }                                
                                else if (newStatus == LibVisLib.Article.ArticleStatus.Produced)
                                {

                                    ArticleAction act = new ArticleAction(art);

                                    act.date = DateTime.Now;
                                    act.type = ArticleAction.ActionType.Produced;
                                    act.userId = userId;
                                    act.show = true;
                                    act.observation = "";
                                    
                                    act.Save();
                                    
                                    art.status = LibVisLib.Article.ArticleStatus.Produced;
                                    art.finalNarratorId = LibVisLib.Verify.VerifyGuid(data.Info);
                                    if (art.finalNarratorId == null)
                                        art.finalNarratorId = "";

                                    art.Save();

                                }
                                else if (newStatus == LibVisLib.Article.ArticleStatus.Published)
                                {

                                    ArticleAction act = new ArticleAction(art);

                                    act.date = DateTime.Now;
                                    act.type = ArticleAction.ActionType.Published;
                                    act.userId = userId;
                                    act.show = true;
                                    act.observation = "";
                                    
                                    act.Save();

                                    ArticleAction auth_act = art.GetActionForType(ArticleAction.ActionType.Created);
                                    if (auth_act != null && auth_act.profile != null)
                                        auth_act.profile.RegisterAction(Profile.ProfileAction.ArticleApproved);

                                    ArticleAction revs_act = art.GetActionForType(ArticleAction.ActionType.Revised);
                                    if (revs_act != null && revs_act.profile != null)
                                        revs_act.profile.RegisterAction(Profile.ProfileAction.ArticleApproved);

                                    art.released = DateTime.Now;
                                    art.Save();

                                }
                                else if (newStatus == LibVisLib.Article.ArticleStatus.Removed)
                                {

                                    ArticleAction act = new ArticleAction(art);

                                    act.date = DateTime.Now;
                                    act.type = ArticleAction.ActionType.Removed;
                                    act.userId = userId;
                                    act.show = true;
                                    act.observation = "";

                                    act.Save();

                                }

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
        [RequestSizeLimit(1000000000)]
        public GenericStatusModel PublishWithFiles(string token, [FromBody]IncludeActionWithFile data)
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

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(data.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id do artigo; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errArticleId: Invalid chars");
                    }

                    if (!LibVisLib.Verify.AcceptGuidOrNull(data.Info))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id do narrador; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errNarratorId: Invalid chars");
                    }

                    if (data.Status < 1 || data.Status > 6)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Situação inválida; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errStatus: Invalid value");
                    }

                    if (gsm.Result == 0)
                    {

                        string articleId = LibVisLib.Verify.VerifyGuid(data.Id);

                        LibVisLib.Article art = LibVisLib.Article.LoadArticle(articleId);
                        if (art == null)
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += "Ação inválida; ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errArticleId: Invalid article");

                        }
                        else
                        {

                            art.lastChanged = DateTime.Now;
                            art.Save();

                            LibVisLib.Article.ArticleStatus newStatus = (LibVisLib.Article.ArticleStatus)data.Status;

                            // Se estiver na narração

                            if (art.status == LibVisLib.Article.ArticleStatus.Revised && (newStatus == LibVisLib.Article.ArticleStatus.Revised || newStatus == LibVisLib.Article.ArticleStatus.Ready))
                            {

                                ArticleAction act = new ArticleAction(art);

                                act.date = DateTime.Now;
                                act.type = ArticleAction.ActionType.IncludedNaration;
                                act.userId = userId;
                                act.show = true;
                                
                                string files_obs = "";

                                for (int i = 0; i < data.Files.Count; i++)
                                {

                                    string ext = "wav";
                                    if (data.Files[i].FileName.Length > 3)
                                        ext = data.Files[i].FileName.Substring(data.Files[i].FileName.Length - 3);

                                    string path = Base.conf.tempImageFilePath + "\\c-" + act.id + "-" + i.ToString("00") + "." + ext;

                                    string cts = data.Files[i].Content;
                                    string[] ct = data.Files[i].Content.Split(',');

                                    if (ct.Length > 1)
                                        cts = ct[ct.Length - 1];

                                    byte[] str = Convert.FromBase64String(cts);

                                    System.IO.File.WriteAllBytes(path, str);

                                    files_obs += data.Files[i].FileName + ",";

                                }

                                if (files_obs.Length > 0)
                                    files_obs = files_obs.Substring(0, files_obs.Length - 1);

                                act.observation = "Inclui arquivos: " + files_obs;
                                act.Save();

                                art.status = LibVisLib.Article.ArticleStatus.Ready;
                                art.Save();

                            }

                            // Se estiver na produção

                            if (art.status == LibVisLib.Article.ArticleStatus.Ready && newStatus == LibVisLib.Article.ArticleStatus.Produced)
                            {

                                ArticleAction act = new ArticleAction(art);

                                act.date = DateTime.Now;
                                act.type = ArticleAction.ActionType.Published;
                                act.userId = userId;
                                act.show = true;

                                string files_obs = "";

                                for (int i = 0; i < data.Files.Count; i++)
                                {

                                    string ext = "mp4";
                                    if (data.Files[i].FileName.Length > 3)
                                        ext = data.Files[i].FileName.Substring(data.Files[i].FileName.Length - 3);

                                    string path = Base.conf.tempImageFilePath + "\\v-" + act.id + "-" + i.ToString("00") + "." + ext;

                                    if (ext == "jpg" || ext == "png")
                                        path = Base.conf.tempImageFilePath + "\\t-" + act.id + "-" + i.ToString("00") + "." + ext;

                                    string cts = data.Files[i].Content;
                                    string[] ct = data.Files[i].Content.Split(',');

                                    if (ct.Length > 1)
                                        cts = ct[ct.Length - 1];

                                    byte[] str = Convert.FromBase64String(cts);

                                    System.IO.File.WriteAllBytes(path, str);

                                    files_obs += data.Files[i].FileName + ",";

                                }

                                if (files_obs.Length > 0)
                                    files_obs = files_obs.Substring(0, files_obs.Length - 1);

                                act.observation = act.observation + ", " + files_obs;

                                act.Save();

                                art.status = LibVisLib.Article.ArticleStatus.Produced;
                                art.finalNarratorId = LibVisLib.Verify.VerifyGuid(data.Info);
                                if (art.finalNarratorId == null)
                                    art.finalNarratorId = "";

                                art.Save();

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

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(vote.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");
                    }

                    if (vote.Vote < 0 || vote.Vote > 2)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Voto inválido; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errVote: Invalid value");
                    }

                    if (gsm.Result == 0)
                    {

                        string articleId = LibVisLib.Verify.VerifyGuid(vote.Id);

                        LibVisLib.Article art = LibVisLib.Article.LoadArticle(articleId);
                        if (art == null)
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += "Id com formato inválido; ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");

                        }
                        else
                        {

                            if (art.SetUserVote(userId, (LibVisLib.ArticleVote.Vote)vote.Vote))
                                prf.RegisterAction(Profile.ProfileAction.VoteArticle);

                            art.Save();

                            if (art.voteApproved - art.voteDontLike >= 5 && art.status == LibVisLib.Article.ArticleStatus.Created)
                            {

                                art.status = LibVisLib.Article.ArticleStatus.Approved;
                                art.Save();

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

        [HttpGet("[action]")]
        public FileContentResult Image(string id)
        {

            byte[] res = null;

            try
            {

                id = LibVisLib.Verify.VerifyGuid(id);

                if (id != null)
                {

                    LibVisLib.Article article = LibVisLib.Article.LoadArticle(id);
                    if (article != null)
                    {

                        try
                        {

                            string path = Base.conf.tempImageFilePath + "\\a-" + id + ".jpg";
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
        
        [HttpGet("[action]")]
        public EditArticleBaseModel EditBase(string token, int lang, string articleId, string targetId)
        {

            BaseUser usr = null;

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            EditArticleBaseModel gsm = new EditArticleBaseModel();

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                string userId = SessionCode.ValidateSessionCode(token, machine);

                usr = BaseUserSource.source.LoadUser(userId);

            }

            gsm.Categories = new List<NewsCategory>();

            for (int i = 0; i < LibVisLib.Category.categories.Count; i++)
                if (!LibVisLib.Category.categories[i].main)
                    gsm.Categories.Add(new NewsCategory(msgs, LibVisLib.Category.categories[i]));

            gsm.Awards = new List<NewsAward>();

            for (int i = 0; i < LibVisLib.Award.awards.Count; i++)
                if (LibVisLib.Award.awards[i].type == Award.Type.Positive && LibVisLib.Award.awards[i].appliedTo == Award.AppliedTo.Article)
                    gsm.Awards.Add(new NewsAward(LibVisLib.Award.awards[i]));

            gsm.Targets = new List<TargetModel>();

            List<Target> lst = Target.GetLastTargets(lang, "", true, 0, 50, DateTime.Now.AddDays(-5));

            for (int i = 0; i < lst.Count; i++)
                gsm.Targets.Add(new TargetModel(msgs, lst[i], true, true, false, false));

            if (articleId != "" && articleId != null && articleId != "0")
            {

                LibVisLib.Article pa = LibVisLib.Article.LoadArticle(articleId);
                if (pa != null)
                {

                    if (pa.targetId != null)
                        targetId = pa.targetId;
                    
                }

            }

            if (targetId != "" && targetId != null && targetId != "0")
            {

                Target ta = Target.LoadTarget(targetId);
                if (ta != null)
                {

                    bool jafoi = false;

                    for (int i = 0; i < lst.Count; i++)
                        if (lst[i].id == targetId)
                            jafoi = true;

                    if (!jafoi)
                    {

                        gsm.Targets.Add(new TargetModel(msgs, ta, true, true, false, false));

                    }

                }

            }

            if (usr != null)
            {

                Profile prf = Profile.Load(usr);

                if (prf.user.isInternal)
                {

                    List<BaseUser> lstn = Profile.GetUsersWithRole(ProfileRole.Role.Revisor);

                    for (int i = 0; i < lstn.Count; i++)
                        gsm.Revisors.Add(new UserIdModel(lstn[i].id, lstn[i].name));

                }

                if (prf.user.isInternal || prf.revisor)
                {

                    List<BaseUser> lstn = Profile.GetUsersWithRole(ProfileRole.Role.Narrator);

                    for (int i = 0; i < lstn.Count; i++)
                        gsm.Narrators.Add(new UserIdModel(lstn[i].id, lstn[i].name));

                }

                if (prf.user.isInternal || prf.narrator)
                {

                    List<BaseUser> lstn = Profile.GetUsersWithRole(ProfileRole.Role.Producer);

                    for (int i = 0; i < lstn.Count; i++)
                        gsm.Producers.Add(new UserIdModel(lstn[i].id, lstn[i].name));

                }

            }

            return gsm;

        }

        [HttpGet("[action]")]
        public GenericStatusModel RemoveAction(string token, string id)
        {

            BaseUser usr = null;

            GenericStatusModel gsm = new GenericStatusModel();

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                string userId = SessionCode.ValidateSessionCode(token, machine);

                usr = BaseUserSource.source.LoadUser(userId);

            }

            if (!usr.isInternal)
            {

                gsm.Result = (int)RacMsg.Id.InvalidAccess;
                gsm.ResultComplement = "Usuário não tem direitos para essa ação";

            }

            if (!LibVisLib.Verify.AcceptGuid(id))
            {
                gsm.Result = (int)RacLib.RacMsg.Id.Error;
                gsm.ResultComplement += "Caracteres inválidos no id da ação; ";
                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errActionId: Invalid chars");
            }

            if (gsm.Result == 0)
            {

                ArticleAction act = ArticleAction.LoadArticleAction(id);
                if (act != null)
                {

                    act.Remove();

                }
                else
                {

                    gsm.Result = (int)RacMsg.Id.NotFound;
                    gsm.ResultComplement = "Ação não encontrada";

                }

            }

            return gsm;

        }

        [HttpPost("[action]")]
        public GenericStatusModel RegisterPriority(string token, [FromBody]RegisterPriority prio)
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

            if (gsm.Result == 0)
            {

                try
                {

                    if (!LibVisLib.Verify.AcceptGuidOrNull(prio.Id))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Caracteres inválidos no id; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");
                    }

                    if (prio.Define < 0 || prio.Define > 1)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Definição de prioridade inválida; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errDefine: Invalid value define");
                    }

                    if (prio.Priority < 2 || prio.Define > 4)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Prioridade inválida; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPriority: Invalid priority");
                    }

                    if (gsm.Result == 0)
                    {

                        string articleId = LibVisLib.Verify.VerifyGuid(prio.Id);

                        LibVisLib.Article art = LibVisLib.Article.LoadArticle(articleId);
                        if (art == null)
                        {

                            gsm.Result = (int)RacLib.RacMsg.Id.Error;
                            gsm.ResultComplement += "Id com formato inválido; ";
                            RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errTargetId: Invalid chars");

                        }
                        else
                        {

                            if (prio.Define == 1)
                            {

                                switch (prio.Priority)
                                {

                                    case 2:
                                        art.beingRevisedId = userId;
                                        break;

                                    case 3:
                                        art.beingNarratedId = userId;
                                        break;

                                    case 4:
                                        art.beingProducedId = userId;
                                        break;

                                }

                            }
                            else
                            {

                                switch (prio.Priority)
                                {

                                    case 2:
                                        art.beingRevisedId = "";
                                        break;

                                    case 3:
                                        art.beingNarratedId = "";
                                        break;

                                    case 4:
                                        art.beingProducedId = "";
                                        break;

                                }

                            }

                            art.Save();
                            
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
        public ArticleListModel Search(string token, int ini, int max, [FromBody]SearchArticleModel search)
        {

            int lang = LibVisLib.Verify.ValidLanguage(search.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            string userId = null;

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            ArticleListModel m = new ArticleListModel();

            if (!LibVisLib.Verify.AcceptFreeText(search.SearchString))
            {
                m.Result = (int)RacLib.RacMsg.Id.Error;
                m.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInSearchString);
                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errSearchString: Invalid chars");
            }

            string srch = LibVisLib.Verify.VerifyFreeText(search.SearchString);
                        
            m.Sts = 0;
            m.Ini = ini;
            m.Total = LibVisLib.Article.GetTotalLastArticles(lang, userId, "", 0, srch);
            m.Articles = ControllerBase.LatestArticlesByCategory(lang, userId, "", ini, max, 0, srch);

            return m;

        }

        [HttpGet("[action]")]
        public MonthValueDiscrimination GetMonthValueDiscrimination(string token, int lang, int year, int month)
        {

            MonthValueDiscrimination gsm = new MonthValueDiscrimination();

            gsm.Result = 0;
            gsm.ResultComplement = "";

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string userId = SessionCode.ValidateSessionCode(token, machine);
            if (userId == null)
            {

                gsm.Result = (int)RacMsg.Id.InvalidToken;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.InvalidToken);

                return gsm;

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);
            if (prf == null)
            {

                gsm.Result = (int)RacMsg.Id.UserNotFound;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserNotFound);

                return gsm;

            }

            if (prf != null && prf.user != null && prf.user.profile != BaseUser.InternalSystemProfile.Administrator)
            {
                
                gsm.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserHasNoRightToThisItem);

                return gsm;

            }

            if (gsm.Result == 0)
            {

                try
                {

                    if (year < 2020 && year > DateTime.Now.Year)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Ano invalido; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errYear: ano inválido");
                    }

                    if (month < 1 && ((year < DateTime.Now.Year && month > 12) || (year == DateTime.Now.Year && month > DateTime.Now.Month)))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += "Mês invalido; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errMonth: mês inválido");
                    }

                    if (gsm.Result == 0)
                    {

                        string monthdesc = year.ToString("0000") + month.ToString("00");

                        gsm.Month = monthdesc;

                        DateTime start = new DateTime(year, month, 1);
                        DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

                        List<LibVisLib.Article> articles = LibVisLib.Article.ListPublishedArticlesForInterval("", start, end);

                        gsm.Videos = new List<ValuesPerVideo>();
                        double val = 0;

                        for (int i = 0; i < articles.Count; i++)
                        {

                            ValuesPerVideo vpv = new ValuesPerVideo(msgs, articles[i], "");
                            gsm.Videos.Add(vpv);

                            val += vpv.Total;

                        }

                        gsm.Total = val;

                        gsm.Users = new List<ValuesPerUser>();
                        
                        for (int i = 0; i < articles.Count; i++)
                        {
                            
                            ArticleAction a0 = articles[i].GetActionForType(ArticleAction.ActionType.Created);
                            if (a0 != null)
                                AddUserToList(msgs, gsm.Users, a0);
                            else
                                AddAdmToList(msgs, gsm.Users, articles[i], ArticleAction.ActionType.Created, ArticleAction.CalcRevenue(articles[i], ArticleAction.ActionType.Created, articles[i].wordCount, articles[i].wordCount));

                            ArticleAction a1 = articles[i].GetActionForType(ArticleAction.ActionType.Revised);
                            if (a1 != null)
                                AddUserToList(msgs, gsm.Users, a1);
                            else
                                AddAdmToList(msgs, gsm.Users, articles[i], ArticleAction.ActionType.Revised, ArticleAction.CalcRevenue(articles[i], ArticleAction.ActionType.Revised, articles[i].wordCount, articles[i].wordCount));

                            ArticleAction a2 = articles[i].GetActionForType(ArticleAction.ActionType.IncludedNaration, articles[i].finalNarratorId);
                            if (a2 != null)
                                AddUserToList(msgs, gsm.Users, a2);
                            else
                                AddAdmToList(msgs, gsm.Users, articles[i], ArticleAction.ActionType.IncludedNaration, ArticleAction.CalcRevenue(articles[i], ArticleAction.ActionType.IncludedNaration, articles[i].wordCount, articles[i].wordCount));

                            ArticleAction a3 = articles[i].GetActionForType(ArticleAction.ActionType.Produced);
                            if (a3 != null)
                                AddUserToList(msgs, gsm.Users, a3);
                            else
                                AddAdmToList(msgs, gsm.Users, articles[i], ArticleAction.ActionType.Produced, ArticleAction.CalcRevenue(articles[i], ArticleAction.ActionType.Produced, articles[i].wordCount, articles[i].wordCount));

                        }
                                                
                        for (int i = 0; i < gsm.Users.Count; i++)
                        {

                            double vu = 0;
                            string ds = "";

                            for(int j = 0; j < gsm.Users[i].Values.Count; j++)
                            {

                                ds += gsm.Users[i].Values[j].Role + " em \"" + gsm.Users[i].Values[j].Title + "\", " + gsm.Users[i].Values[j].Value.ToString("0.000000") + "; ";
                                vu += gsm.Users[i].Values[j].Value;

                            }

                            gsm.Users[i].Total = vu;
                            gsm.Users[i].Description = ds;

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

        void AddUserToList(RacLib.RacMsg msgs, List<ValuesPerUser>  lst, ArticleAction aa)
        {

            for(int i = 0; i < lst.Count; i++)
            {

                if (lst[i].Id == aa.userId)
                {

                    VideoValue vv = new VideoValue(msgs, aa);
                    lst[i].Values.Add(vv);

                    return;

                }

            }

            ValuesPerUser vu = new ValuesPerUser(msgs, aa);
            lst.Add(vu);

        }

        void AddAdmToList(RacLib.RacMsg msgs, List<ValuesPerUser> lst, LibVisLib.Article a, ArticleAction.ActionType at, double val)
        {

            for (int i = 0; i < lst.Count; i++)
            {

                if (lst[i].Id == Profile.AdministratorId)
                {

                    VideoValue vv = new VideoValue(msgs, a, at, val);
                    lst[i].Values.Add(vv);

                    return;

                }

            }

            ValuesPerUser vu = new ValuesPerUser(msgs, a, at, val, Profile.Administrator);
            lst.Add(vu);

        }

    }

}
