using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibVisWeb.Models;
using RacLib;
using Microsoft.AspNetCore.Http.Features;
using System.IO;

namespace LibVisWeb.Controllers
{

    /// <summary>
    /// Criar conta
    /// </summary>
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        [HttpPost("[action]")]
        public GenericStatusModel CreateAccount([FromBody]CreateAccountModel account)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            int lang = LibVisLib.Verify.ValidLanguage(account.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            gsm.Result = 0;
            gsm.ResultComplement = "";

            try
            {

                if (!LibVisLib.Verify.AcceptName(account.Name))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheName) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errUserName: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptEmail(account.Email))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Invalid chars or email format");
                }

                if (!LibVisLib.Verify.AcceptPassword(account.Password))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInThePassword) + "; ";                    
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptPassword(account.ConfirmPassword))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInThePasswordConfirmation) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPassConf: Invalid chars");
                }

                if (account.Password != account.ConfirmPassword)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.PasswordConfDoesNotMatch) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPassConf: Password does not match");
                }

                string nome = LibVisLib.Verify.VerifyName(account.Name);
                string email = LibVisLib.Verify.VerifyEmail(account.Email);
                string pass = LibVisLib.Verify.VerifyPassword(account.Password);

                if (RacLib.RacWebUserSource.racWebUserSource.EmailUsed(email))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.ErrorsInForm;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailUsed) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "Email already used");
                }

                if (RacLib.RacWebUserSource.racWebUserSource.NameUsed(nome))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.NameAlreadyUsed) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "Nome already used");
                }

                if (nome == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.NameIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errUserName: Required field");
                }

                if (nome.Length > 40)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.NameTooLong) + "; "; 
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errUserName: Content too big");
                }

                if (email == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Required field");
                }

                if (email.Length > 200)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailTooBig200CharsMax) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                }

                if (pass == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.PasswordIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Required field");
                }

                if (pass.Length > 20)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.PasswordTooBig20CharsMax) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Content too big");
                }

                if (!RacLib.RacWebUserSource.racWebUserSource.PasswordPolicyOk(pass))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.PasswordPolicyNotOk) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Password policy not ok");
                }

                if (gsm.Result == 0)
                {

                    RacLib.RacWebUser man = RacLib.RacWebUserSource.racWebUserSource.CreateRacWebUser();

                    man.name = nome;
                    man.email = email;
                    man.password = pass;
                    man.defaultLanguage = (RacMsg.Language)lang;
                    man.status = RacLib.BaseUser.Status.Created;
                    man.Save();

                    man.profile = RacLib.BaseUser.InternalSystemProfile.NotConfirmed;

                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Sending email confirmation to user " + man.nameEmail + " (#" + man.id.ToString() + ")");

                    RacLib.Base.conf.SendInternalEmailAlert("New user", "User " + man.nameEmail + " registered in visaolibertaria.com", "visaolibertaria.com", "visaolibertaria.com", RacLib.BaseLog.log);

                    // Email de confirmação

                    RacLib.ConfirmationCode cc = man.GenerateConfirmationCode();

                    RacLib.EmailTemplate tpl = RacLib.EmailTemplate.LoadEmailTemplate(RacLib.EmailTemplate.ConfirmNewUserCodedEmailId);
                    RacLib.Email eml = tpl.CreateNewEmail(RacLib.InternalSystem.LibVisId);
                    eml.SetParam(eml.system);
                    eml.destination.Add(new RacLib.EmailName(man.id));
                    eml.SetParam("hml", "");
                    eml.SetParam("path", "/confirm-email/");
                    eml.SetParam("emailpath", "/");
                    eml.SetParam("confirmationcode", cc.code);
                    eml.SendAll(man);

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

            return gsm;

        }

        [HttpPost("[action]")]
        public GenericStatusModel ChangeLanguage(string token, [FromBody]ChangeLanguageModel chglng)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            gsm.Result = 0;
            gsm.ResultComplement = "";

            // Verifica o token de autenticação

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            int lang = LibVisLib.Verify.ValidLanguage(chglng.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

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

                prf.user.defaultLanguage = (RacMsg.Language)lang;
                prf.user.Save();

            }

            return gsm;

        }

        [HttpPost("[action]")]
        public GenericStatusModel SendAgain([FromBody]SendAgainModel send)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            int lang = LibVisLib.Verify.ValidLanguage(send.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            gsm.Result = 0;
            gsm.ResultComplement = "";

            try
            {
                
                if (!LibVisLib.Verify.AcceptEmail(send.Email))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Invalid chars or email format");
                }

                string email = LibVisLib.Verify.VerifyEmail(send.Email);
                
                if (!RacLib.RacWebUserSource.racWebUserSource.EmailUsed(email))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.ErrorsInForm;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailNotFound) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "Email não encontrado");
                }

                if (email == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Required field");
                }

                if (email.Length > 200)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailTooBig200CharsMax) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                }

                if (gsm.Result == 0)
                {

                    RacWebUser man = RacLib.RacWebUserSource.racWebUserSource.LoadRacWebUser(email);

                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Sending (again) email confirmation to user " + man.nameEmail + " (#" + man.id.ToString() + ")");

                    RacLib.Base.conf.SendInternalEmailAlert("Sending again", "User " + man.nameEmail + " required new confirmation in visaolibertaria.com", "visaolibertaria.com", "visaolibertaria.com", RacLib.BaseLog.log);

                    // Email de confirmação

                    RacLib.ConfirmationCode cc = man.GenerateConfirmationCode();

                    if (send.Type == 0)
                    {

                        RacLib.EmailTemplate tpl = RacLib.EmailTemplate.LoadEmailTemplate(RacLib.EmailTemplate.ConfirmNewUserCodedEmailId);
                        RacLib.Email eml = tpl.CreateNewEmail(RacLib.InternalSystem.LibVisId);
                        eml.SetParam(eml.system);
                        eml.destination.Add(new RacLib.EmailName(man.id));
                        eml.SetParam("hml", "");
                        eml.SetParam("path", "/confirm-email/");
                        eml.SetParam("emailpath", "/");
                        eml.SetParam("confirmationcode", cc.code);
                        eml.SendAll(man);

                    }
                    else
                    {

                        RacLib.EmailTemplate tpl = RacLib.EmailTemplate.LoadEmailTemplate(RacLib.EmailTemplate.ConfirmNewUserNoLinkSimpleId);
                        RacLib.Email eml = tpl.CreateNewEmail(RacLib.InternalSystem.LibVisId);
                        eml.SetParam(eml.system);
                        eml.destination.Add(new RacLib.EmailName(man.id));
                        eml.SetParam("hml", "");
                        eml.SetParam("path", "/confirm-email/");
                        eml.SetParam("emailpath", "/");
                        eml.SetParam("confirmationcode", cc.code);
                        eml.SendAll(man);

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

            return gsm;

        }

        [HttpPost("[action]")]
        public GenericStatusModel ChangeAccount(string token, [FromBody]AccountChangeModel account)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            int lang = LibVisLib.Verify.ValidLanguage(account.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            gsm.Result = 0;
            gsm.ResultComplement = "";

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
                
                gsm.Result = (int)RacMsg.Id.InvalidToken;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserNotFound);

                return gsm;

            }

            try
            {

                if (!LibVisLib.Verify.AcceptName(account.Name))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheName) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errUserName: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptEmail(account.Email))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Invalid chars or email format");
                }

                if (!LibVisLib.Verify.AcceptFreeText(account.Bitcoin))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheBitcoinAddress) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errBTC: Invalid chars or bitcoin address format");
                }

                if (!LibVisLib.Verify.AcceptFreeText(account.Avatar))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidBytesInTheAvatarBytes) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errAvatar: Invalid chars or image file format");
                }

                string nome = LibVisLib.Verify.VerifyName(account.Name);
                string email = LibVisLib.Verify.VerifyEmail(account.Email);
                string btcad = LibVisLib.Verify.VerifyFreeText(account.Bitcoin);
                string avatar = LibVisLib.Verify.VerifyFreeText(account.Avatar);

                string[] avpart = avatar.Split(',');

                if (avpart.Length > 0)
                    avatar = avpart[avpart.Length - 1];

                //
                //  Mudou o email?
                //

                bool emailchanged = false;

                if (email != "" && email != prf.user.email)
                {

                    if (RacLib.RacWebUserSource.racWebUserSource.EmailUsed(email))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.ErrorsInForm;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailUsed) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "Email already used");
                    }

                    if (email.Length > 200)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailTooBig200CharsMax) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                    }

                    if (gsm.Result == 0)
                        emailchanged = true;

                }

                //
                //  Mudou o nome?
                //

                bool namechanged = false;

                if (nome != "" && nome != prf.user.name)
                {

                    if (RacLib.RacWebUserSource.racWebUserSource.NameUsed(nome))
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.NameAlreadyUsed) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "Nome already used");
                    }

                    if (nome.Length > 40)
                    {
                        gsm.Result = (int)RacLib.RacMsg.Id.Error;
                        gsm.ResultComplement += msgs.Get(RacMsg.Id.NameTooLong) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errUserName: Content too big");
                    }

                    if (gsm.Result == 0)
                        namechanged = true;

                }

                if (btcad.Length > 64)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.BitcoinAddressTooLong) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errBTC: Content too big");
                }

                if (avatar.Length > 5000000)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.AvatarDataTooBig) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errAvatar: Content too big");
                }

                if (gsm.Result == 0)
                {
                    
                    RacLib.RacWebUser man = RacLib.RacWebUserSource.racWebUserSource.LoadRacWebUser(prf.user.id);

                    if (namechanged)
                    {

                        man.name = nome;

                    }

                    if (emailchanged)
                    {

                        man.email = email;
                        man.profile = RacLib.BaseUser.InternalSystemProfile.NotConfirmed;

                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Sending email confirmation to user " + man.nameEmail + " (#" + man.id.ToString() + ")");

                        RacLib.Base.conf.SendInternalEmailAlert("User changed email", "User " + man.nameEmail + " changed email Visao Libertaria", "visaolibertaria.com", "visaolibertaria.com", RacLib.BaseLog.log);

                        // Email de confirmação

                        RacLib.ConfirmationCode cc = man.GenerateConfirmationCode();

                        RacLib.EmailTemplate tpl = RacLib.EmailTemplate.LoadEmailTemplate(RacLib.EmailTemplate.ConfirmNewUserCodedEmailId);
                        RacLib.Email eml = tpl.CreateNewEmail(RacLib.InternalSystem.LibVisId);
                        eml.SetParam(eml.system);
                        eml.destination.Add(new RacLib.EmailName(man.id));
                        eml.SetParam("hml", "");
                        eml.SetParam("path", "/confirm-email/");
                        eml.SetParam("emailpath", "/");
                        eml.SetParam("confirmationcode", cc.code);
                        eml.SendAll(man);

                    }

                    if (namechanged || emailchanged)
                        man.Save();

                    prf.bitcoin = btcad;
                    prf.newsLetter = account.NewsLetter;
                    prf.user.defaultLanguageId = lang;

                    if (avatar != "")
                    {

                        byte[] str = Convert.FromBase64String(avatar);

                        MemoryStream ms = new MemoryStream(str, 0, str.Length);
                        ms.Write(str, 0, str.Length);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                        string path = Base.conf.tempImageFilePath + "\\u-" + userId + ".jpg";

                        img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                    }

                    prf.lastModified = DateTime.Now;

                    prf.Save();

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

            return gsm;

        }

        /// <summary>
        /// Mudar linguagem
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public GenericStatusModel ChangeAccountLanguage(string token, int lang)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            gsm.Result = 0;
            gsm.ResultComplement = "";

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

                gsm.Result = (int)RacMsg.Id.InvalidToken;
                gsm.ResultComplement = msgs.Get(RacMsg.Id.UserNotFound);

                return gsm;

            }

            try
            {

                if (lang < 2 || lang > 4)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidLanguage);
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLang: Invalid language");
                }

                if (gsm.Result == 0)
                {

                    RacLib.RacWebUser man = RacLib.RacWebUserSource.racWebUserSource.LoadRacWebUser(prf.user.id);

                    man.defaultLanguageId = lang;

                    man.Save();

                    prf.lastModified = DateTime.Now;

                    prf.Save();

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

            return gsm;

        }

        /// <summary>
        /// Confirmar email
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public GenericStatusModel ConfirmEmail([FromBody]ConfirmEmailModel conf)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            int lang = LibVisLib.Verify.ValidLanguage(conf.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            gsm.Result = 0;
            gsm.ResultComplement = "";

            try
            {

                if (!LibVisLib.Verify.AcceptEmail(conf.Email))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptConfirmationCode(conf.Code))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheCode) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCode: Invalid chars on code");
                }

                string email = LibVisLib.Verify.VerifyEmail(conf.Email);
                string code = LibVisLib.Verify.VerifyConfirmationCode(conf.Code);

                if (email == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Required field");
                }

                if (email.Length > 200)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailTooBig200CharsMax) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                }

                if (code == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.CodeIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errCode: Required field");
                }

                if (code.Length > 20)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.CodeTooBig) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                }

                if (gsm.Result == 0)
                {

                    RacLib.BaseUser man = RacLib.RacWebUserSource.racWebUserSource.LoadUser(email);

                    if (RacLib.ConfirmationCode.ValidateConfirmationCode(code, man.id, true))
                    {

                        man.status = RacLib.BaseUser.Status.Confirmed;
                        man.SetProfileForInternalSystem(RacLib.InternalSystem.LibVisId, RacLib.BaseUser.InternalSystemProfile.Public, true);

                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Confirmed user " + man.nameEmail + " (#" + man.id.ToString() + ")");

                        RacLib.Base.conf.SendInternalEmailAlert("User confirmed", "User " + man.nameEmail + " confirmed in Visao Libertaria", "visaolibertaria.com", "visaolibertaria.com", RacLib.BaseLog.log);

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

            return gsm;

        }

        /// <summary>
        /// Altera a senha
        /// </summary>
        /// <param name="chgpass"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public GenericStatusModel ChangePassword(string token, [FromBody]ChangePasswordModel chgpass)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            int lang = LibVisLib.Verify.ValidLanguage(chgpass.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            gsm.Result = 0;
            gsm.ResultComplement = "";

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

            try
            {

                if (!LibVisLib.Verify.AcceptPassword(chgpass.OldPassword))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheOldPassword) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptPassword(chgpass.NewPassword))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheNewPassword) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptPassword(chgpass.ConfirmNewPassword))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheNewPasswordConfirmation) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPassConf: Invalid chars");
                }

                if (chgpass.NewPassword != chgpass.ConfirmNewPassword)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.PasswordConfDoesNotMatch) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPassConf: Password does not match");
                }

                string oldpass = LibVisLib.Verify.VerifyPassword(chgpass.OldPassword);
                string newpass = LibVisLib.Verify.VerifyPassword(chgpass.NewPassword);

                if (oldpass == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.OldPasswordIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Required field");
                }

                if (oldpass.Length > 20)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.OldPasswordTooBig) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errOldPassword: Content too big");
                }

                if (newpass == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.NewPasswordIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Required field");
                }

                if (newpass.Length > 20)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.NewPasswordTooBig) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errNewPassword: Content too big");
                }

                if (!RacLib.RacWebUserSource.racWebUserSource.PasswordPolicyOk(newpass))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.PasswordPolicyNotOk) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Password policy not ok");
                }

                if (gsm.Result == 0)
                {

                    RacWebUserSource.LogonFailure reason;
                    BaseUser usr0 = RacWebUserSource.racWebUserSource.GetUser(userId);
                    if (usr0 != null)
                    {

                        BaseUser usr = RacWebUserSource.racWebUserSource.Logon(usr0.email, oldpass, out reason);
                        if (usr == null)
                        {

                            gsm.Result = (int)RacMsg.Id.LoginFailure;
                            RacWebLog.log.Log(RacWebLog.LogType.Error, "Login failure, trying to change password");

                            if (reason == RacWebUserSource.LogonFailure.InvalidLogin)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, usr0.email, "Invalid login");
                            else if (reason == RacWebUserSource.LogonFailure.Disabled)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, usr0.email, "User disabled");
                            else if (reason == RacWebUserSource.LogonFailure.Blocked)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, usr0.email, "User blocked");
                            else if (reason == RacWebUserSource.LogonFailure.InvalidPassword)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, usr0.email, "Invalid password");
                            else if (reason == RacWebUserSource.LogonFailure.Other)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, usr0.email, "Other");
                            else
                                RacWebLog.log.Log(RacWebLog.LogType.Error, usr0.email, "Other unexpected");

                        }
                        else
                        {

                            RacWebUser man = RacWebUserSource.racWebUserSource.LoadRacWebUser(usr.id);

                            man.Unblock();
                            man.ResetFailedPass();
                            man.lastChange = DateTime.Now;
                            man.password = newpass;
                            man.Save();

                            BaseLog.log.Log(BaseLog.LogType.Informative, usr, "Changed password");

                            gsm.Result = 0;
                            gsm.ResultComplement = "";

                        }

                    }
                    else
                    {

                        gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                        gsm.ResultComplement = "";

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
                
            
            return gsm;

        }

        /// <summary>
        /// Esqueci minha senha
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public GenericStatusModel ForgotPassword([FromBody]ForgotPasswordModel account)
        {

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            GenericStatusModel gsm = new GenericStatusModel();

            int lang = LibVisLib.Verify.ValidLanguage(account.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            gsm.Result = 0;
            gsm.ResultComplement = "";

            try
            {

                if (!LibVisLib.Verify.AcceptEmail(account.Email))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Invalid chars or email format");
                }
                
                string email = LibVisLib.Verify.VerifyEmail(account.Email);

                if (email == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Required field");
                }

                if (email.Length > 200)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailTooBig200CharsMax) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                }
                
                if (gsm.Result == 0)
                {

                    RacLib.BaseUser man = BaseUserSource.source.LoadUser(email);
                    if (man == null)
                    {

                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Email " + email + " not found in user database");

                    }
                    else
                    {

                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Informative, "Sending email confirmation to user " + man.nameEmail + " (#" + man.id.ToString() + ")");

                        RacLib.Base.conf.SendInternalEmailAlert("Esqueci minha senha", "User " + man.nameEmail + " in Visao Libertaria forgot his/her password", "visaolibertaria.com", "visaolibertaria.com", RacLib.BaseLog.log);

                        // Email de confirmação

                        RacLib.SessionCode cc = SessionCode.Generate(man.id, machine);

                        RacLib.EmailTemplate tpl = RacLib.EmailTemplate.LoadEmailTemplate(RacLib.EmailTemplate.ForgotPasswordCodedEmailId);
                        RacLib.Email eml = tpl.CreateNewEmail(RacLib.InternalSystem.LibVisId);
                        eml.SetParam(eml.system);
                        eml.destination.Add(new RacLib.EmailName(man.id));
                        eml.SetParam("hml", "");
                        eml.SetParam("path", "/recover-password/");
                        eml.SetParam("emailpath", "/");
                        eml.SetParam("code", cc.code);
                        eml.SendAll(man);

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

            return gsm;

        }

        /// <summary>
        /// Recupera a senha apos esqueci minha senha
        /// </summary>
        /// <param name="chgpass"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public AccountModel UserInfo(string id)
        {

            AccountModel a = ControllerBase.GetUser(id, false, false);
            return a;

        }

        /// <summary>
        /// Retorna lista de usuários
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public List<UserNameId> List(string token)
        {

            List<UserNameId> lst = new List<UserNameId>();
            List<StringString> b = LibVisLib.Profile.listAllUsers;

            for (int i = 0; i < b.Count; i++)
            {

                UserNameId u = new UserNameId();
                u.Id = b[i].id;
                u.Name = b[i].name;

                lst.Add(u);

            }

            // Se tiver autenticado e se for interno, pode mandar o email também

            if (token != null && token != "")
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

                string userid = RacLib.SessionCode.ValidateSessionCode(token, machine);
                if (userid != null && userid != "")
                {

                    BaseUser usr = BaseUserSource.source.LoadUser(userid);
                    if (usr != null && usr.isInternal)
                    {

                        for (int i = 0; i < b.Count; i++)
                        {

                            BaseUser u = BaseUserSource.source.LoadUser(b[i].id);

                            if (u != null)
                                lst[i].Email = u.email;

                        }

                    }

                }

            }

            return lst;

        }

        /// <summary>
        /// Recupera a senha apos esqueci minha senha
        /// </summary>
        /// <param name="chgpass"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public GenericStatusModel RecoverPassword([FromBody]RecoverPasswordModel chgpass)
        {

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            GenericStatusModel gsm = new GenericStatusModel();

            int lang = LibVisLib.Verify.ValidLanguage(chgpass.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            gsm.Result = 0;
            gsm.ResultComplement = "";

            try
            {

                if (!LibVisLib.Verify.AcceptEmail(chgpass.Email))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptSessionCode(chgpass.ChangeToken))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheCode) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptPassword(chgpass.NewPassword))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheNewPassword) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Invalid chars");
                }

                if (!LibVisLib.Verify.AcceptPassword(chgpass.ConfirmNewPassword))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInTheNewPasswordConfirmation) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPassConf: Invalid chars");
                }

                if (chgpass.NewPassword != chgpass.ConfirmNewPassword)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.PasswordConfDoesNotMatch) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPassConf: Password does not match");
                }

                string email = LibVisLib.Verify.VerifyEmail(chgpass.Email);
                string token = LibVisLib.Verify.VerifySessionCode(chgpass.ChangeToken);
                string newpass = LibVisLib.Verify.VerifyPassword(chgpass.NewPassword);

                if (email == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Required field");
                }

                if (email.Length > 200)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.EmailTooBig200CharsMax) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                }

                if (token == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.CodeIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Required field");
                }

                if (token.Length > 500)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.CodeTooBig) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                }

                if (newpass == "")
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.NewPasswordIsRequired) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Required field");
                }

                if (newpass.Length > 20)
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.NewPasswordTooBig) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errEmail: Content too big");
                }

                if (!RacLib.RacWebUserSource.racWebUserSource.PasswordPolicyOk(newpass))
                {
                    gsm.Result = (int)RacLib.RacMsg.Id.Error;
                    gsm.ResultComplement += msgs.Get(RacMsg.Id.PasswordPolicyNotOk) + "; ";
                    RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Password policy not ok");
                }

                if (gsm.Result == 0)
                {
                                        
                    string id = SessionCode.ValidateSessionCode(token, machine, true);
                    if (id == null || id == "")
                    {

                        gsm.Result = (int)RacMsg.Id.InvalidConfirmationCode;
                        RacWebLog.log.Log(RacWebLog.LogType.Error, "Código inválido trying to change password");

                    }
                    else
                    {

                        RacWebUser man = RacWebUserSource.racWebUserSource.LoadRacWebUser(id);

                        man.Unblock();
                        man.ResetFailedPass();
                        man.lastChange = DateTime.Now;
                        man.password = newpass;
                        man.Save();

                        BaseLog.log.Log(BaseLog.LogType.Informative, man, "Changed password");

                        gsm.Result = 0;
                        gsm.ResultComplement = "";

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

            return gsm;

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public GenericStatusModel Put(string token, [FromBody]ChangePasswordModel account)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            gsm.Result = (int)RacMsg.Id.NotImplementedYet;

            return gsm;

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public GenericStatusModel Delete(int id)
        {

            GenericStatusModel gsm = new GenericStatusModel();

            gsm.Result = (int)RacMsg.Id.NotImplementedYet;

            return gsm;

        }

        [HttpPost("[action]")]
        public UserListModel Search(string token, int ini, int max, [FromBody]SearchUserModel search)
        {

            int lang = LibVisLib.Verify.ValidLanguage(search.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            string userId = null;

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            UserListModel m = new UserListModel();

            if (userId == "" || userId == null)
            {

                m.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                m.ResultComplement = "";

                return m;

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);

            if (prf == null)
            {

                m.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                m.ResultComplement = "";

                return m;

            }
            
            if (!prf.user.isInternal && !prf.staff)
            {
                
                m.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                m.ResultComplement = "";

                return m;
                
            }

            if (!LibVisLib.Verify.AcceptFreeText(search.SearchString))
            {
                m.Result = (int)RacLib.RacMsg.Id.Error;
                m.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInSearchString);
                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errSearchString: Invalid chars");
            }

            string srch = LibVisLib.Verify.VerifyFreeText(search.SearchString);

            m.Ini = ini;
            m.Total = LibVisLib.Profile.GetTotalUsers(srch);
            m.List = ControllerBase.GetUsers(lang, ini, max, srch);

            return m;

        }

        [HttpGet("[action]")]
        public AccountForManagerModel GetForManager(string token, int lang, string id)
        {

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            string userId = null;

            if (token != "" && token != null)
            {

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                userId = SessionCode.ValidateSessionCode(token, machine);

            }

            AccountForManagerModel m = new AccountForManagerModel();

            if (userId == "" || userId == null)
            {

                m.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                m.ResultComplement = "";

                return m;

            }

            LibVisLib.Profile prf = LibVisLib.Profile.LoadProfile(userId);

            if (prf == null)
            {

                m.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                m.ResultComplement = "";

                return m;

            }

            if (!prf.user.isInternal && !prf.staff)
            {

                m.Result = (int)RacMsg.Id.UserHasNoRightToThisItem;
                m.ResultComplement = "";

                return m;

            }

            if (!LibVisLib.Verify.AcceptGuid(id))
            {
                m.Result = (int)RacLib.RacMsg.Id.Error;
                m.ResultComplement += msgs.Get(RacMsg.Id.InvalidCharactersInId);
                RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errId: Invalid chars");
            }

            id = LibVisLib.Verify.VerifyGuid(id);

            LibVisLib.Profile prf2 = LibVisLib.Profile.LoadProfile(id);

            m.Account = new AccountModel(prf2, false, true);

            return m;

        }

    }

}
