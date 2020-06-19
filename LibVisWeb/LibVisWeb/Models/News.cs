using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibVisLib;

namespace LibVisWeb.Models
{
    
    public class ListItem
    {

        public string Id { get; set; }
        public string Text { get; set; }

        public ListItem()
        {

            Id = "";
            Text = "";
 
        }
        
        public ListItem(string i, string t)
        {

            Id = i;
            Text = t;

        }

    }

    public class SiteWarningModel
    {

        public int Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public SiteWarningModel()
        {

            Type = 0;
            Title = "";
            Text = "";

        }

        public SiteWarningModel(LibVisLib.SiteWarning s)
        {

            Type = (int)s.type;
            Title = s.title;
            Text = s.text;

        }

    }

    public class HomePageModel
    {

        public List<VideoModel> Videos { get; set; }
        public List<NewsStatisticModel> Statistics { get; set; }
        public List<ArticleRowModel> Articles { get; set; }
        public List<SiteWarningModel> Warnings { get; set; }

        public int NumTargets { get; set; }
        public int NumApproval { get; set; }
        public int NumRevision { get; set; }
        public int NumNarration { get; set; }
        public int NumProduction { get; set; }

        public int Seq { get; set; }

        public HomePageModel()
        {

            Videos = new List<VideoModel>();
            Statistics = new List<NewsStatisticModel>();
            Articles = new List<ArticleRowModel>();
            Warnings = new List<SiteWarningModel>();
            
            NumTargets = 0;
            NumApproval = 0;
            NumRevision = 0;
            NumNarration = 0;
            NumProduction = 0;

            Seq = 0;

        }

    }

    public class MainPageModel
    {

        public List<TargetModel> Targets { get; set; }
        public List<ArticleModel> Articles { get; set; }
        public List<VideoModel> Videos { get; set; }

        public int NumArticles { get; set; }
        public int NumShortNote { get; set; }
        public int NumSatoshis { get; set; }

        public MainPageModel()
        {

            Targets = new List<TargetModel>();
            Articles = new List<ArticleModel>();
            Videos = new List<VideoModel>();
            NumArticles = 0;
            NumShortNote = 0;
            NumSatoshis = 0;

        }

    }

    public class UserPageModel
    {

        public AccountModel User { get; set; }
        public List<TargetModel> Targets { get; set; }
        public List<ArticleModel> Articles { get; set; }
        public List<VideoModel> Videos { get; set; }
        
        public UserPageModel()
        {

            User = new AccountModel();
            Targets = new List<TargetModel>();
            Articles = new List<ArticleModel>();
            Videos = new List<VideoModel>();

        }

    }
    
    public class CategInfo
    {

        public NewsCategory MainCategory { get; set; }
        public List<NewsCategory> Categories { get; set; }

        public CategInfo()
        {

            MainCategory = new NewsCategory();
            Categories = new List<NewsCategory>();

        }

    }

    public class AuthorInfo 
    {

        public string SuggestedLabel { get; set; }
        public string AuthoredLabel { get; set; }
        public string RevisedLabel { get; set; }
        public string NarratedLabel { get; set; }
        public string ProducedLabel { get; set; }
                
        public UserIdModel Suggested { get; set; }
        public UserIdModel Authored { get; set; }
        public UserIdModel Revised { get; set; }
        public UserIdModel Narrated { get; set; }
        public UserIdModel Produced { get; set; }

        public string DateLabel { get; set; }
        public string Date { get; set; }

        public string StatusText { get; set; }
        
        public AuthorInfo()
        {

            SuggestedLabel = "";
            AuthoredLabel = "";
            RevisedLabel = "";
            NarratedLabel = "";
            ProducedLabel = "";

            Suggested = new UserIdModel();
            Authored = new UserIdModel();            
            Revised = new UserIdModel();
            Narrated = new UserIdModel();
            Produced = new UserIdModel();

            DateLabel = "";
            Date = "";

            StatusText = "";

        }

    }

    public class NewsCategory
    {

        public string Label { get; set; }
        public string Category { get; set; }

        public NewsCategory()
        {

            Label = "";
            Category = "";

        }

        public NewsCategory(RacLib.RacMsg msgs, LibVisLib.Category c)
        {

            Label = c.label;
            Category = msgs.Get(c.nameMsg);

        }

    }

    public class NewsAward
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public NewsAward()
        {

            Id = "";
            Name = "";
            Description = "";

        }

        public NewsAward(LibVisLib.Award a)
        {

            Id = a.id;
            Name = a.name;
            Description = a.description;

        }

    }

    public class NewsStatisticModel
    {

        public string Realm { get; set; }
        public string Parameter { get; set; }
        public string ImageLink { get; set; }
        public int Value { get; set; }
        public string Units { get; set; }
        public string Link { get; set; }

        public NewsStatisticModel(RacLib.RacMsg msgs, Statistics ns)
        {

            switch (ns.realm)
            {

                case Statistics.Realm.Youtube:
                    Realm = "Youtube";
                    break;

                case Statistics.Realm.Bitchute:
                    Realm = "Bitchute";
                    break;

                case Statistics.Realm.Facebook:
                    Realm = "Facebook";
                    break;

                case Statistics.Realm.Minds:
                    Realm = "Minds";
                    break;

                case Statistics.Realm.Twitter:
                    Realm = "Twitter";
                    break;

                case Statistics.Realm.Gabai:
                    Realm = "Gab";
                    break;

                case Statistics.Realm.Site:
                    Realm = msgs.Get(RacLib.RacMsg.Id.LibertarianViewSiteName);
                    break;

            }

            switch (ns.parameter)
            {

                case Statistics.Parameter.Subscribers:
                    Parameter = msgs.Get(RacLib.RacMsg.Id.Subscribers);
                    break;

                case Statistics.Parameter.Views:
                    Parameter = msgs.Get(RacLib.RacMsg.Id.Views);
                    break;

            }

            ImageLink = ns.icon;
            Value = ns.value;
            Units = ns.units;
            Link = ns.link;

        }

    }

    public class UserIdModel
    {

        public string Name { get; set; }
        public string Id { get; set; }

        public UserIdModel()
        {

            Name = "";
            Id = "";

        }

        public UserIdModel(string i, string n)
        {

            Name = n;
            Id = i;

        }

    }

    public class RegisterGrant
    {

        public string ActionId { get; set; }
        public string AwardId { get; set; }
        public int Add { get; set; }
        public int Lang { get; set; }

        public RegisterGrant()
        {

            ActionId = "";
            AwardId = "";
            Add = 0;
            Lang = 0;

        }

    }
    
    /// <summary>
    /// Registro da newsletter
    /// </summary>
    public class NewsLetterRegister
    {

        public int Type { get; set; }
        public string Data { get; set; }
        public int Lang { get; set; }

        public NewsLetterRegister()
        {

            Type = 0;
            Data = "";
            Lang = 0;

        }

    }

    public class ChangeLanguage
    {

        public string Id { get; set; }
        public int Lang { get; set; }
        public int NewLang { get; set; }

        public ChangeLanguage()
        {

            Id = "";
            Lang = 0;
            NewLang = 0;

        }

    }
    
}

