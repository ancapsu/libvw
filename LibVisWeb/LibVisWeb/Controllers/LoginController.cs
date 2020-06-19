using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RacLib;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using LibVisWeb.Models;

namespace LibVisWeb.Controllers
{

    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        /// <summary>
        /// POST usado para logar o usuário
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public LoginResultModel Post([FromBody]LoginRequestModel request)
        {

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            int lang = LibVisLib.Verify.ValidLanguage(request.Lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            LoginResultModel lrm = new LoginResultModel();
            if (!LibVisLib.Verify.AcceptLogin(request.Login))
            {

                lrm.Result = (int)RacMsg.Id.Error;
                lrm.ResultComplement = msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail) + "; ";
                RacWebLog.log.Log(RacWebLog.LogType.Error, "Invalid email format: " + request.Login);

            }
            else if (!LibVisLib.Verify.AcceptPassword(request.Password))
            {

                lrm.Result = (int)RacMsg.Id.Error;
                lrm.ResultComplement = msgs.Get(RacMsg.Id.InvalidCharactersInThePassword) + "; ";
                RacWebLog.log.Log(RacWebLog.LogType.Error, "Invalid password format: " + request.Password);

            }
            else
            {

                try
                {

                    string login = LibVisLib.Verify.VerifyLogin(request.Login).ToLower();
                    string password = LibVisLib.Verify.VerifyPassword(request.Password);

                    if (login.Length > 200)
                    {
                        lrm.Result = (int)RacLib.RacMsg.Id.Error;
                        lrm.ResultComplement += msgs.Get(RacMsg.Id.EmailTooBig200CharsMax) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLogin: Content too big");                                                
                    }

                    if (password.Length > 20)
                    {
                        lrm.Result = (int)RacLib.RacMsg.Id.Error;
                        lrm.ResultComplement += msgs.Get(RacMsg.Id.PasswordTooBig20CharsMax) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Content too big");
                    }

                    if (lrm.Result == 0) { 

                        RacWebUserSource.LogonFailure reason;
                        BaseUser usr = RacWebUserSource.racWebUserSource.Logon(login, password, out reason);
                        if (usr == null)
                        {

                            lrm.Result = (int)RacMsg.Id.LoginFailure;
                            RacWebLog.log.Log(RacWebLog.LogType.Error, "Login failure");

                            if (reason == RacWebUserSource.LogonFailure.InvalidLogin)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "Invalid login");
                            else if (reason == RacWebUserSource.LogonFailure.Disabled)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "User disabled");
                            else if (reason == RacWebUserSource.LogonFailure.Blocked)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "User blocked");
                            else if (reason == RacWebUserSource.LogonFailure.InvalidPassword)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "Invalid password");
                            else if (reason == RacWebUserSource.LogonFailure.Other)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "Other");
                            else
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "Other unexpected");

                        }
                        else
                        {

                            if (usr.status != BaseUser.Status.Confirmed)
                            {

                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "User not confirmed");
                                lrm.Result = (int)RacMsg.Id.UserNotConfirmed;

                            }
                            else
                            {

                                RacWebUser man = RacWebUserSource.racWebUserSource.LoadRacWebUser(usr.id);
                                LibVisLib.Profile prof = LibVisLib.Profile.LoadProfile(usr.id);

                                // Pega todas as infos do usuário

                                lrm = new LoginResultModel(prof);

                                // Coloca os resultados do login

                                lrm.Result = 0;
                                lrm.ResultComplement = "";

                                // Informações de últimos logins

                                lrm.LastLoginDate = man.lastLogon;
                                lrm.NumberOfTentatives = man.numberPasswordFails;

                                // Cria o token de sessão

                                string code = SessionCode.Generate(usr.id, machine, 30).code;
                                lrm.Token = code;

                                // Ok, fim

                                RacWebLog.log.Log(RacWebLog.LogType.Informative, login, "Successfull login: " + login + ", code = " + code);

                            }

                        }

                    }

                }
                catch (Exception ex)
                {

                    RacWebLog.log.Log(RacWebLog.LogType.InternalError, "Failed (Exception)");
                    RacWebLog.log.Log(ex);

                    lrm.Result = (int)RacMsg.Id.InternalError;
                    lrm.ResultComplement = ex.Message;

                }

            }

            return lrm;

        }
        
        /// <summary>
        /// POST usado para logar o usuário
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public LoginResultModel ApiLogin(string login, string password, int lang = 0)
        {

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            if (lang < 2 || lang > 4)
                lang = 2;

            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            LoginResultModel lrm = new LoginResultModel();
            if (!LibVisLib.Verify.AcceptLogin(login))
            {

                lrm.Result = (int)RacMsg.Id.Error;
                lrm.ResultComplement = msgs.Get(RacMsg.Id.InvalidCharactersInTheEmail) + "; ";
                RacWebLog.log.Log(RacWebLog.LogType.Error, "Invalid email format: " + login);

            }
            else if (!LibVisLib.Verify.AcceptPassword(password))
            {

                lrm.Result = (int)RacMsg.Id.Error;
                lrm.ResultComplement = msgs.Get(RacMsg.Id.InvalidCharactersInThePassword) + "; ";
                RacWebLog.log.Log(RacWebLog.LogType.Error, "Invalid password format: " + password);

            }
            else
            {

                try
                {

                    login = LibVisLib.Verify.VerifyLogin(login).ToLower();
                    password = LibVisLib.Verify.VerifyPassword(password);

                    if (login.Length > 200)
                    {
                        lrm.Result = (int)RacLib.RacMsg.Id.Error;
                        lrm.ResultComplement += msgs.Get(RacMsg.Id.EmailTooBig200CharsMax) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errLogin: Content too big");
                    }

                    if (password.Length > 20)
                    {
                        lrm.Result = (int)RacLib.RacMsg.Id.Error;
                        lrm.ResultComplement += msgs.Get(RacMsg.Id.PasswordTooBig20CharsMax) + "; ";
                        RacLib.BaseLog.log.Log(RacLib.BaseLog.LogType.Error, "errPass: Content too big");
                    }

                    if (lrm.Result == 0)
                    {

                        RacWebUserSource.LogonFailure reason;
                        BaseUser usr = RacWebUserSource.racWebUserSource.Logon(login, password, out reason);
                        if (usr == null)
                        {

                            lrm.Result = (int)RacMsg.Id.LoginFailure;
                            RacWebLog.log.Log(RacWebLog.LogType.Error, "Login failure");

                            if (reason == RacWebUserSource.LogonFailure.InvalidLogin)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "Invalid login");
                            else if (reason == RacWebUserSource.LogonFailure.Disabled)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "User disabled");
                            else if (reason == RacWebUserSource.LogonFailure.Blocked)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "User blocked");
                            else if (reason == RacWebUserSource.LogonFailure.InvalidPassword)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "Invalid password");
                            else if (reason == RacWebUserSource.LogonFailure.Other)
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "Other");
                            else
                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "Other unexpected");

                        }
                        else
                        {

                            if (usr.status != BaseUser.Status.Confirmed)
                            {

                                RacWebLog.log.Log(RacWebLog.LogType.Error, login, "User not confirmed");
                                lrm.Result = (int)RacMsg.Id.UserNotConfirmed;

                            }
                            else
                            {

                                RacWebUser man = RacWebUserSource.racWebUserSource.LoadRacWebUser(usr.id);
                                LibVisLib.Profile prof = LibVisLib.Profile.LoadProfile(usr.id);

                                // Pega todas as infos do usuário

                                lrm = new LoginResultModel(prof);

                                // Coloca os resultados do login

                                lrm.Result = 0;
                                lrm.ResultComplement = "";

                                // Informações de últimos logins

                                lrm.LastLoginDate = man.lastLogon;
                                lrm.NumberOfTentatives = man.numberPasswordFails;

                                // Cria o token de sessão

                                string code = SessionCode.Generate(usr.id, machine, 30).code;
                                lrm.Token = code;

                                // Ok, fim

                                RacWebLog.log.Log(RacWebLog.LogType.Informative, login, "Successfull login: " + login + ", code = " + code);

                            }

                        }

                    }

                }
                catch (Exception ex)
                {

                    RacWebLog.log.Log(RacWebLog.LogType.InternalError, "Failed (Exception)");
                    RacWebLog.log.Log(ex);

                    lrm.Result = (int)RacMsg.Id.InternalError;
                    lrm.ResultComplement = ex.Message;

                }

            }

            return lrm;

        }
        
        /// <summary>
        /// GET para validar o token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public LoginResultModel Get(string token, int lang)
        {

            LoginResultModel lrm = new LoginResultModel();

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacMsg msgs = RacMsg.cache.GetMessage((RacMsg.Language)lang);

            if (token == "" || token == null)
            {

                lrm.Result = (int)RacMsg.Id.InvalidToken;
                lrm.ResultComplement = msgs.Get(RacMsg.Id.InvalidToken);

            }
            else
            {

                // Verifica o token de autenticação

                string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

                string userId = SessionCode.ValidateSessionCode(token, machine);
                if (userId == null)
                {

                    lrm.Result = (int)RacMsg.Id.InvalidToken;
                    lrm.ResultComplement = msgs.Get(RacMsg.Id.InvalidToken);

                }

                if (lrm.Result == 0)
                {

                    lrm.ResultComplement = SessionCode.ValidateSessionCode(token, machine, false);
                                                         
                    RacWebUser man = RacWebUserSource.racWebUserSource.LoadRacWebUser(userId);
                    LibVisLib.Profile prof = LibVisLib.Profile.LoadProfile(userId);

                    // Pega todas as infos do usuário

                    lrm = new LoginResultModel(prof);

                    // Coloca os resultados do login

                    lrm.Result = 0;
                    lrm.ResultComplement = "";

                    // Informações de últimos logins

                    lrm.LastLoginDate = man.lastLogon;
                    lrm.NumberOfTentatives = man.numberPasswordFails;

                    // Cria o token de sessão

                    string code = SessionCode.Generate(userId, machine, 30).code;
                    lrm.Token = code;

                    // Ok, fim

                    RacWebLog.log.Log(RacWebLog.LogType.Informative, man.email, "Successfull verification: " + userId + ", code = " + code);

                }

            }

            return lrm;

        }
        
        /// <summary>
        /// DELETE para invalidar o token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete]
        public GenericStatusModel Delete(string token)
        {

            string machine = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

            string res = SessionCode.ValidateSessionCode(token, machine, true);

            GenericStatusModel gsm = new GenericStatusModel();

            if (res == null || res == "")
            {
                gsm.Result = (int)RacMsg.Id.InvalidTokenInExpression;
                gsm.ResultComplement = "";
            }
            else
            {
                gsm.Result = 0;
                gsm.ResultComplement = "";
            }

            return gsm;

        }

    }

}
