using LibVisLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibVisWeb.Models
{

    public class AccountModel
    {

        // Informação base do usuário logado

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Bitcoin { get; set; }

        public string GeneralQualification { get; set; }
        public string TotalQualification { get; set; }
        public string EditorQualification { get; set; }
        public string ReporterQualification { get; set; }

        // Informação sobre o perfil do usuário

        public int Profile { get; set; }
        public string ProfileName { get; set; }

        // Medalhas do usuário

        public List<MedalsModel> Medals { get; set; }

        // Pagamento do usuário
                
        public List<PaymentModel> Payments { get; set; }

        // Valores desse mês

        public List<ValuesPerVideoPerUser> Videos { get; set; }

        public double Value { get; set; }

        // Número de pontos

        public int Xp { get; set; }
        public int NextXp { get; set; }
        public int Ap { get; set; }
        public int NextAp { get; set; }
        public int Rp { get; set; }
        public int NextRp { get; set; }
        public int Op { get; set; }
        public int NextOp { get; set; }
        public int Qp { get; set; }
        public int NextQp { get; set; }
        public int Tp { get; set; }
        public int NextTp { get; set; }
        public int Up { get; set; }
        public int NextUp { get; set; }
        public int Sp { get; set; }
        public int NextSp { get; set; }
        public int Ep { get; set; }
        public int NextEp { get; set; }

        // Está na newsletter?
        
        public bool NewsLetter { get; set; }

        // Linguagem da interface

        public int Lang { get; set; }

        // Papeis

        public int RoleTranslatorEn { get; set; }
        public int RoleTranslatorPt { get; set; }
        public int RoleTranslatorEs { get; set; }

        public int RoleRevisorEn { get; set; }
        public int RoleRevisorPt { get; set; }
        public int RoleRevisorEs { get; set; }

        public int RoleNarratorEn { get; set; }
        public int RoleNarratorPt { get; set; }
        public int RoleNarratorEs { get; set; }

        public int RoleProducerEn { get; set; }
        public int RoleProducerPt { get; set; }
        public int RoleProducerEs { get; set; }

        public int Translator { get; set; }
        public int Revisor { get; set; }
        public int Narrator { get; set; }
        public int Producer { get; set; }
        public int Sponsor { get; set; }
        public int Staff { get; set; }
        public int Admin { get; set; }

        public int Blocked { get; set; }
        public int NotConfirmed { get; set; }

        public AccountModel()
        {

            Id = "";
            Name = "";
            Email = "";
            Bitcoin = "";

            Profile = 0;
            ProfileName = "";

            Medals = new List<MedalsModel>();
            Payments = new List<PaymentModel>();
            Videos = new List<ValuesPerVideoPerUser>();
            Value = 0;

            Xp = 0;
            NextXp = 0;
            Ap = 0;
            NextAp = 0;
            Rp = 0;
            NextRp = 0;
            Op = 0;
            NextOp = 0;
            Qp = 0;
            NextQp = 0;
            Tp = 0;
            NextTp = 0;
            Up = 0;
            NextUp = 0;
            Sp = 0;
            NextSp = 0;
            Ep = 0;
            NextEp = 0;

            NewsLetter = false;

            GeneralQualification = "";
            EditorQualification = "";
            ReporterQualification = "";

            Lang = 2;

            RoleTranslatorEn = 0;
            RoleTranslatorPt = 0;
            RoleTranslatorEs = 0;

            RoleRevisorEn = 0;
            RoleRevisorPt = 0;
            RoleRevisorEs = 0;

            RoleNarratorEn = 0;
            RoleNarratorPt = 0;
            RoleNarratorEs = 0;

            RoleProducerEn = 0;
            RoleProducerPt = 0;
            RoleProducerEs = 0;

            Translator = 0;
            Revisor = 0;
            Narrator = 0;
            Producer = 0;
            Sponsor = 0;
            Staff = 0;
            Admin = 0;

            Blocked = 0;
            NotConfirmed = 0;

        }

        public AccountModel(LibVisLib.Profile p, bool isOwn, bool isInternal)
        {

            Id = p.user.id;
            Email = p.user.email;
            Name = p.user.name;
            Profile = (int)p.user.GetProfileForInternalSystem(RacLib.InternalSystem.LibVisId);

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage(p.user.defaultLanguage);

            if (isOwn || isInternal)
                Bitcoin = p.bitcoin;
            else
                Bitcoin = "";

            ProfileName = msgs.Get(RacLib.RacMsg.Id.Anonymous);

            if (p.user.GetProfileForInternalSystem(RacLib.InternalSystem.LibVisId) >=  RacLib.BaseUser.InternalSystemProfile.Public)
                ProfileName = ProfileName = msgs.Get(RacLib.RacMsg.Id.User);

            if (p.user.GetProfileForInternalSystem(RacLib.InternalSystem.LibVisId) >= RacLib.BaseUser.InternalSystemProfile.InternalStaff)
                ProfileName = ProfileName = msgs.Get(RacLib.RacMsg.Id.Internal);

            if (p.user.GetProfileForInternalSystem(RacLib.InternalSystem.LibVisId) >= RacLib.BaseUser.InternalSystemProfile.Administrator)
                ProfileName = ProfileName = msgs.Get(RacLib.RacMsg.Id.Administrator);

            if (isOwn || isInternal)
            {

                NewsLetter = p.newsLetter;

                p.CheckMedals();

                Xp = (int)p.xp;
                NextXp = (int)p.nextXp;
                Ap = (int)p.ap;
                NextAp = (int)p.nextAp;
                Rp = (int)p.rp;
                NextRp = (int)p.nextRp;
                Op = (int)p.op;
                NextOp = (int)p.nextOp;
                Qp = (int)p.qp;
                NextQp = (int)p.nextQp;
                Tp = (int)p.tp;
                NextTp = (int)p.nextTp;
                Up = (int)p.up;
                NextUp = (int)p.nextUp;
                Sp = (int)p.sp;
                NextSp = (int)p.nextSp;
                Ep = (int)p.ep;
                NextEp = (int)p.nextEp;

            }
            else
            {

                Xp = 0;
                NextXp = 0;
                Ap = 0;
                NextAp = 0;
                Rp = 0;
                NextRp = 0;
                Op = 0;
                NextOp = 0;
                Qp = 0;
                NextQp = 0;
                Tp = 0;
                NextTp = 0;
                Up = 0;
                NextUp = 0;
                Sp = 0;
                NextSp = 0;
                Ep = 0;
                NextEp = 0;

            }

            Medals = new List<MedalsModel>();

            for (int i = 0; i < p.medals.Count; i++)
            {

                MedalsModel m = new MedalsModel(msgs, p.medals[i].medals);
                Medals.Add(m);

            }

            Payments = new List<PaymentModel>();

            if (isOwn || isInternal)
            {

                for (int i = 0; i < p.payments.Count; i++)
                {

                    PaymentModel pa = new PaymentModel(p.payments[i]);
                    Payments.Add(pa);

                }

            }

            Videos = new List<ValuesPerVideoPerUser>();
            Value = 0;
            if (isOwn || isInternal)
            {

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;

                DateTime start = new DateTime(year, month, 1);
                DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

                List<LibVisLib.Article> articles = LibVisLib.Article.ListPublishedArticlesForInterval(Id, start, end);

                double val = 0;

                for (int i = 0; i < articles.Count; i++)
                {

                    Article art = articles[i];
                    
                    ArticleAction a0 = art.GetActionForType(ArticleAction.ActionType.Created);
                                        
                    if (a0 != null && a0.userId == Id)
                    {
                        ValuesPerVideoPerUser a = new ValuesPerVideoPerUser(msgs, a0);
                        Videos.Add(a);
                        val += a.Total;
                    }

                    ArticleAction a1 = art.GetActionForType(ArticleAction.ActionType.Revised);

                    if (a1 != null && a1.userId == Id)
                    {
                        ValuesPerVideoPerUser a = new ValuesPerVideoPerUser(msgs, a1);
                        Videos.Add(a);
                        val += a.Total;
                    }

                    ArticleAction a2 = art.GetActionForType(ArticleAction.ActionType.IncludedNaration, art.finalNarratorId);

                    if (a2 != null && a2.userId == Id)
                    {
                        ValuesPerVideoPerUser a = new ValuesPerVideoPerUser(msgs, a2);
                        Videos.Add(a);
                        val += a.Total;
                    }

                    ArticleAction a3 = art.GetActionForType(ArticleAction.ActionType.Produced);

                    if (a3 != null && a3.userId == Id)
                    {
                        ValuesPerVideoPerUser a = new ValuesPerVideoPerUser(msgs, a3);
                        Videos.Add(a);
                        val += a.Total;
                    }

                }

                Value = val;

            }

            TotalQualification = p.GetTotalQualification(msgs);
            EditorQualification = p.GetEditorQualification(msgs);
            ReporterQualification = p.GetReporterQualification(msgs);

            if (EditorQualification != "" && ReporterQualification != "")
                GeneralQualification = EditorQualification + ", " + ReporterQualification;
            else
                GeneralQualification = EditorQualification + ReporterQualification;

            if (Profile >= 9)
            {
                GeneralQualification += ((GeneralQualification != "") ? " " : "") + "(Admin)";
                Staff = 1;
                Admin = 1;
            }
            else if (Profile >= 7)
            {
                GeneralQualification += ((GeneralQualification != "") ? " " : "") + "(Staff)";
                Staff = 1;
            }
            else
            {
                Staff = p.staff ? 1 : 0;
            }

            Lang = p.user.defaultLanguageId;

            RoleTranslatorEn = p.translatorEnglish ? 1 : 0;
            RoleTranslatorPt = p.translatorPortuguese ? 1 : 0;
            RoleTranslatorEs = p.translatorSpanish ? 1 : 0;

            RoleRevisorEn = p.revisorEnglish ? 1 : 0;
            RoleRevisorPt = p.revisorPortuguese ? 1 : 0;
            RoleRevisorEs = p.revisorSpanish ? 1 : 0;

            RoleNarratorEn = p.narratorEnglish ? 1 : 0;
            RoleNarratorPt = p.narratorPortuguese ? 1 : 0;
            RoleNarratorEs = p.narratorSpanish ? 1 : 0;

            RoleProducerEn = p.producerEnglish ? 1 : 0;
            RoleProducerPt = p.producerPortuguese ? 1 : 0;
            RoleProducerEs = p.producerSpanish ? 1 : 0;
            
            Sponsor = p.sponsor ? 1 : 0;

            Blocked = p.user.isBlocked || p.user.isDisabled ? 1 : 0;
            NotConfirmed = p.user.isConfirmed ? 0 : 1;

            if (p.user.defaultLanguage == RacLib.RacMsg.Language.Portugues)
            {

                Translator = p.translatorPortuguese ? 1 : 0;
                Revisor = p.revisorPortuguese ? 1 : 0;
                Narrator = p.narratorPortuguese ? 1 : 0;
                Producer = p.producerPortuguese ? 1 : 0;

            }
            else if (p.user.defaultLanguage == RacLib.RacMsg.Language.Espanol)
            {

                Translator = p.translatorSpanish ? 1 : 0;
                Revisor = p.revisorSpanish ? 1 : 0;
                Narrator = p.narratorSpanish ? 1 : 0;
                Producer = p.producerSpanish ? 1 : 0;

            }
            else
            {

                Translator = p.translatorEnglish ? 1 : 0;
                Revisor = p.revisorEnglish ? 1 : 0;
                Narrator = p.narratorEnglish ? 1 : 0;
                Producer = p.producerEnglish ? 1 : 0;

            }

        }

    }

    /// <summary>
    /// Nome e id
    /// </summary>
    public class UserNameId
    {

        public UserNameId()
        {

            Name = "";
            Email = "";
            Id = "";

        }

        public UserNameId(LibVisLib.Profile p)
        {

            Name = p.user.name;
            Email = p.user.email;
            Id = p.user.id;

        }

        public UserNameId(RacLib.BaseUser p)
        {

            Name = p.name;
            Email = p.email;
            Id = p.id;

        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }

    }


    public class SendAgainModel
    {

        public SendAgainModel()
        {

            Email = "";
            Type = 0;
            Lang = 0;

        }

        public string Email { get; set; }
        public int Type { get; set; }
        public int Lang { get; set; }

    }

    public class AccountChangeModel
    {

        public AccountChangeModel()
        {

            Email = "";
            Name = "";
            Bitcoin = "";
            Avatar = "";
            NewsLetter = false;
            Lang = 0;

        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Bitcoin { get; set; }
        public string Avatar { get; set; }
        public bool NewsLetter { get; set; }
        public int Lang { get; set; }

    }

    public class CreateAccountModel
    {

        public CreateAccountModel()
        {

            Email = "";
            Name = "";
            Password = "";
            ConfirmPassword = "";
            Lang = 0;

        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Lang { get; set; }

    }

    public class ConfirmEmailModel
    {

        public ConfirmEmailModel()
        {

            Email = "";
            Code = "";
            Lang = 0;

        }

        public string Email { get; set; }
        public string Code { get; set; }
        public int Lang { get; set; }

    }

    public class ChangePasswordModel
    {

        public ChangePasswordModel()
        {

            OldPassword = "";
            NewPassword = "";
            ConfirmNewPassword = "";
            Lang = 0;

        }
        
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public int Lang { get; set; }

    }

    public class ForgotPasswordModel
    {

        public ForgotPasswordModel()
        {

            Email = "";
            Lang = 0;

        }

        public string Email { get; set; }
        public int Lang { get; set; }

    }

    public class RecoverPasswordModel
    {

        public RecoverPasswordModel()
        {

            ChangeToken = "";
            NewPassword = "";
            ConfirmNewPassword = "";
            Email = "";
            Lang = 0;

        }

        public string Email { get; set; }
        public string ChangeToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public int Lang { get; set; }

    }

    /// <summary>
    /// Modelo de medalha
    /// </summary>
    public class MedalsModel
    {

        public MedalsModel(RacLib.RacMsg msgs, LibVisLib.Medal m)
        {

            Id = m.id;
            Name = msgs.Get(m.nameMsg);
            Description = msgs.Get(m.descriptionMsg);
            
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
    
    public class FileDataModel
    {

        public string Token { get; set; }
        public string Data { get; set; }

        public FileDataModel()
        {

            Token = "";
            Data = "";

        }

    }

    public class PaymentModel
    {

        public string Id { get; set; }
        public string MonthYearRef { get; set; }
        public int NumArticles { get; set; }
        public string Value { get; set; }
        public string Observation { get; set; }
        public string Address { get; set; }
        public string TransactionId { get; set; }
        public string Payed { get; set; }

        public PaymentModel()
        {

            Id = "";
            MonthYearRef = "";
            NumArticles = 0;
            Value = "";
            Observation = "";
            Address = "";
            TransactionId = "";
            Payed = "";

        }

        public PaymentModel(LibVisLib.Payment p)
        {

            Id = p.id;
            MonthYearRef = p.month.ToString("00") + "/" + p.year.ToString();
            NumArticles = p.numArticles;
            Value = p.value.ToString();
            Observation = p.observation;
            Address = p.address;
            TransactionId = p.transactionId;
            Payed = p.payed.ToString("dd/MM/yyyy");

        }

    }

    public class LoginRequestModel
    {

        public string Login { get; set; }
        public string Password { get; set; }
        public bool KeepLogged { get; set; }
        public int Lang { get; set; }

        public LoginRequestModel()
        {

            Login = "";
            Password = "";
            KeepLogged = false;
            Lang = 0;

        }

    }

    public class ChangeLanguageModel
    {

        public int Lang { get; set; }
        
        public ChangeLanguageModel()
        {

            Lang = 3;

        }

    }

    public class LoginResultModel
    {

        // Resultado do login

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        // Token de autenticação para ser usado em outras chamadas ao servidor

        public string Token { get; set; }

        // Informações sobre o último login

        public DateTime LastLoginDate { get; set; }
        public int NumberOfTentatives { get; set; }

        // Conta

        public AccountModel Account { get; set; }

        public LoginResultModel()
        {

            Result = 0;
            ResultComplement = "";

            LastLoginDate = DateTime.Now;
            NumberOfTentatives = 0;
            
            Token = "";
            Account = new AccountModel();

        }

        public LoginResultModel(LibVisLib.Profile p)
        {

            Result = 0;
            ResultComplement = "";

            LastLoginDate = DateTime.Now;
            NumberOfTentatives = 0;
            
            Token = "";
            Account = new AccountModel(p, true, p.user.GetProfileForInternalSystem(RacLib.InternalSystem.LibVisId) >= RacLib.BaseUser.InternalSystemProfile.InternalStaff);
            
        }

    }

    public class ValuesPerVideoPerUser
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Role { get; set; }

        public double Word { get; set; }
        public double Total { get; set; }

        public ValuesPerVideoPerUser()
        {

            Id = "";
            Title = "";
            Date = "";
            Role = "";

            Word = 0;
            Total = 0;

        }

        public ValuesPerVideoPerUser(RacLib.RacMsg msgs, LibVisLib.ArticleAction act)
        {

            Id = act.id;
            Title = act.article.title;
            Date = msgs.ShowDate(act.article.released);

            switch (act.type)
            {

                case ArticleAction.ActionType.Suggested:
                    Role = msgs.Get(RacLib.RacMsg.Id.Suggested);
                    break;

                case ArticleAction.ActionType.Created:
                    Role = msgs.Get(RacLib.RacMsg.Id.Writer);
                    break;

                case ArticleAction.ActionType.Approved:
                    Role = msgs.Get(RacLib.RacMsg.Id.Approver);
                    break;

                case ArticleAction.ActionType.Revised:
                    Role = msgs.Get(RacLib.RacMsg.Id.Revisor);
                    break;

                case ArticleAction.ActionType.IncludedNaration:
                    Role = msgs.Get(RacLib.RacMsg.Id.Narrator);
                    break;

                case ArticleAction.ActionType.Produced:
                    Role = msgs.Get(RacLib.RacMsg.Id.Producer);
                    break;

                case ArticleAction.ActionType.Published:
                    Role = msgs.Get(RacLib.RacMsg.Id.Publisher);
                    break;

            }

            Word = act.billableWords;
            Total = act.value;

        }

    }
    
    public class SearchUserModel
    {

        public string SearchString { get; set; }
        public int Lang { get; set; }

        public SearchUserModel()
        {

            SearchString = "";
            Lang = 0;

        }

    }

    public class UserListModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public int Ini { get; set; }
        public int Total { get; set; }
        public List<AccountModel> List { get; set; }

        public UserListModel()
        {
            Result = 0;
            ResultComplement = "";

            Ini = 0;
            Total = 0;
            List = new List<AccountModel>();
        }

    }
    
    public class ManagerChangePasswordModel
    {

        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public int Lang { get; set; }

        public ManagerChangePasswordModel()
        {

            Id = "";
            NewPassword = "";
            ConfirmNewPassword = "";
            Lang = 0;

        }

    }

    public class AccountForManagerModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public AccountModel Account { get; set; }

        public AccountForManagerModel()
        {

            Result = 0;
            ResultComplement = "";

            Account = null;

        }

    }

}
