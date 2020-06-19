using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibVisLib;

namespace LibVisWeb.Models
{

    // -------
    //
    //   ARTIGO
    //
    // -------

    public class ArticleActionGantModel
    {

        public string AwardId { get; set; }
        public string GrantedBy { get; set; }
        public string Granted { get; set; }

        public ArticleActionGantModel()
        {

            AwardId = "";
            GrantedBy = "";
            Granted = "";

        }

        public ArticleActionGantModel(RacLib.RacMsg msgs, ArticleActionGrant n)
        {

            AwardId = n.awardId;
            GrantedBy = n.grantedByName;
            Granted = msgs.ShowDate(n.granted);

        }

    }

    public class ArticleActionObservationModel
    {

        public string Observation { get; set; }
        public string IncludedBy { get; set; }
        public string Included { get; set; }
        public string IncludedById { get; set; }

        public ArticleActionObservationModel()
        {

            Observation = "";
            IncludedBy = "";
            Included = "";
            IncludedById = "";

        }

        public ArticleActionObservationModel(RacLib.RacMsg msgs, ArticleActionObservation n)
        {

            Observation = n.observation;
            IncludedBy = n.includeddByName;
            Included = msgs.ShowDate(n.included);
            IncludedById = n.userId;

        }

    }

    public class ArticleActionAudioModel
    {

        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }

        public ArticleActionAudioModel()
        {

            FileName = "";
            MimeType = "";
            Extension = "";

        }

    }

    public class ArticleActionModel
    {

        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Observation { get; set; }
        public string Date { get; set; }
        public int BillableWords { get; set; }

        public List<ArticleActionAudioModel> Audios { get; set; }
        public List<ArticleActionGantModel> Grants { get; set; }
        public List<ArticleActionObservationModel> Observations { get; set; }

        public ArticleActionModel()
        {

            Id = "";
            UserId = "";
            UserName = "";
            Type = 0;
            Observation = "";
            Date = "";
            BillableWords = 0;

            Audios = new List<ArticleActionAudioModel>();
            Grants = new List<ArticleActionGantModel>();
            Observations = new List<ArticleActionObservationModel>();

        }

        public ArticleActionModel(RacLib.RacMsg msgs, ArticleAction n)
        {

            Id = n.id;
            UserId = n.userId;
            UserName = n.userName;
            Type = (int)n.type;
            Observation = n.observation;
            Date = msgs.ShowDate(n.date);
            BillableWords = n.billableWords;

            Audios = new List<ArticleActionAudioModel>();

            if (n.type == ArticleAction.ActionType.IncludedNaration)
            {

                int na = n.CheckNumberAudioFiles();
                for (int i = 0; i < na; i++)
                {

                    ArticleActionAudioModel aaa = new ArticleActionAudioModel();

                    aaa.FileName = n.GetAttachFileName(i);
                    aaa.MimeType = n.GetAttachFileType(i);
                    aaa.Extension = "." + (aaa.FileName.Length > 3 ? aaa.FileName.Substring(aaa.FileName.Length - 3) : aaa.FileName);

                    Audios.Add(aaa);

                }

            }

            switch (n.type)
            {

                case ArticleAction.ActionType.Suggested:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Suggested);
                    break;

                case ArticleAction.ActionType.Created:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Created);
                    break;

                case ArticleAction.ActionType.Approved:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Approved);
                    break;

                case ArticleAction.ActionType.Revised:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Revised);
                    break;

                case ArticleAction.ActionType.IncludedNaration:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Narrated);
                    break;

                case ArticleAction.ActionType.Produced:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Produced);
                    break;

                case ArticleAction.ActionType.Published:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Published);
                    break;

                case ArticleAction.ActionType.Removed:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Removed);
                    break;

            }

            Grants = new List<ArticleActionGantModel>();

            for (int i = 0; i < n.grants.Count; i++)
                Grants.Add(new ArticleActionGantModel(msgs, n.grants[i]));

            Observations = new List<ArticleActionObservationModel>();

            for (int i = 0; i < n.observations.Count; i++)
                Observations.Add(new ArticleActionObservationModel(msgs, n.observations[i]));

        }

    }

    public class ArticleLinkModel
    {

        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        public ArticleLinkModel(RacLib.RacMsg msgs, ArticleLink n)
        {

            Type = (int)n.type;
            TypeName = "";
            Link = n.link;
            Description = n.description;

            switch (n.type)
            {

                case ArticleLink.LinkType.ExternalLink:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.ExternalLink);
                    break;

                case ArticleLink.LinkType.Image:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Image);
                    break;

                case ArticleLink.LinkType.Video:
                    TypeName = msgs.Get(RacLib.RacMsg.Id.Video);
                    break;

            }

        }

        public ArticleLinkModel()
        {

            Type = 0;
            TypeName = "";
            Link = "";

        }

    }

    public class ArticleCommentModel
    {

        public string Id { get; set; }
        public string Comment { get; set; }
        public string IncludedBy { get; set; }
        public string Included { get; set; }
        public string IncludedById { get; set; }

        public ArticleCommentModel()
        {

            Id = "";
            Comment = "";
            IncludedBy = "";
            Included = "";
            IncludedById = "";

        }

        public ArticleCommentModel(RacLib.RacMsg msgs, ArticleComment n)
        {

            Id = n.id;
            Comment = n.comment;
            IncludedBy = n.userName;
            Included = msgs.ShowDate(n.date);
            IncludedById = n.userId;

        }

    }

    public class ArticleModel
    {

        public string Id { get; set; }
        public string Title { get; set; }

        public string Text { get; set; }
        public List<string> Paragraphs { get; set; }
        public string StartingText { get; set; }

        public AuthorInfo Authors { get; set; }

        public TargetModel Target { get; set; }

        public CategInfo Categories { get; set; }

        public List<ArticleActionModel> Actions;
        public List<ArticleLinkModel> Links;
        public List<ArticleCommentModel> Comments;

        public int Status { get; set; }
        public int StatusNarration { get; set; }
        public int Type { get; set; }
        public int ArticleType { get; set; }
        public string StatusName { get; set; }
        public string StatusNarrationName { get; set; }
        public string TypeName { get; set; }
        public string ArticleTypeName { get; set; }

        public int VoteApprove { get; set; }
        public int VoteNot { get; set; }
        public int VoteAlready { get; set; }
        public int VotePoorly { get; set; }

        public int UserVote { get; set; }

        public string preferredRevisor { get; set; }
        public string preferredNarrator { get; set; }
        public string preferredProducer { get; set; }

        public string preferredRevisorName { get; set; }
        public string preferredNarratorName { get; set; }
        public string preferredProducerName { get; set; }

        public string beingRevised { get; set; }
        public string beingNarrated { get; set; }
        public string beingProduced { get; set; }

        public string beingRevisedName { get; set; }
        public string beingNarratedName { get; set; }
        public string beingProducedName { get; set; }

        public ArticleModel()
        {

            Id = "";
            Title = "";
            StartingText = "";

            Text = "";
            Paragraphs = new List<string>();

            Authors = new AuthorInfo();
            Target = new TargetModel();
            Categories = new CategInfo();

            Actions = new List<ArticleActionModel>();
            Links = new List<ArticleLinkModel>();
            Comments = new List<ArticleCommentModel>();

            Status = 0;
            StatusNarration = 0;
            Type = 0;
            ArticleType = 0;
            StatusName = "";
            StatusNarrationName = "";
            TypeName = "";
            ArticleTypeName = "";

            VoteApprove = 0;
            VoteNot = 0;
            VoteAlready = 0;
            VotePoorly = 0;

            UserVote = -1;

            preferredRevisor = "";
            preferredNarrator = "";
            preferredProducer = "";
            preferredRevisorName = "";
            preferredNarratorName = "";
            preferredProducerName = "";

            beingRevised = "";
            beingNarrated = "";
            beingProduced = "";
            beingRevisedName = "";
            beingNarratedName = "";
            beingProducedName = "";

        }

        public ArticleModel(RacLib.RacMsg msgs, Article nv, bool incText, bool incComment, bool incTarg, bool incAuth, bool incCateg, bool incLink, bool incAct)
        {

            Id = nv.id;
            Title = nv.title;

            StartingText = (nv.text.Length < 300) ? nv.text : nv.text.Substring(0, 300);

            if (incText)
            {
                Text = nv.text;
                Paragraphs = nv.text.Split('\n').ToList();
            }
            else
            {
                Text = "";
                Paragraphs = new List<string>();
            }

            if (incTarg && nv.target != null)
            {
                Target = new TargetModel(msgs, nv.target, false, false, false, false);
            }
            else
            {
                Target = new TargetModel();
            }

            Comments = new List<ArticleCommentModel>();

            if (incComment)
            {

                for (int i = 0; i < nv.comments.Count; i++)
                    Comments.Add(new ArticleCommentModel(msgs, nv.comments[i]));

            }

            Authors = new AuthorInfo();

            if (incAuth)
            {

                Authors.Date = msgs.ShowDate(nv.released);
                Authors.DateLabel = msgs.Get(RacLib.RacMsg.Id.In);
                Authors.StatusText = "";
                
                Authors.SuggestedLabel = msgs.Get(RacLib.RacMsg.Id.Suggested);
                Authors.AuthoredLabel = msgs.Get(RacLib.RacMsg.Id.Written);
                Authors.RevisedLabel = msgs.Get(RacLib.RacMsg.Id.Revised);
                Authors.NarratedLabel = msgs.Get(RacLib.RacMsg.Id.Narrated);
                Authors.ProducedLabel = msgs.Get(RacLib.RacMsg.Id.Produced);

                Authors.Suggested = new UserIdModel(nv.suggestedId, nv.suggested);
                Authors.Authored = new UserIdModel(nv.authoredId, nv.authored);
                Authors.Revised = new UserIdModel(nv.revisedId, nv.revised);
                Authors.Narrated = new UserIdModel(nv.narratedId, nv.narrated);
                Authors.Produced = new UserIdModel(nv.producedId, nv.produced);

            }

            Links = new List<ArticleLinkModel>();

            if (incLink)
            {

                for (int i = 0; i < nv.links.Count; i++)
                    Links.Add(new ArticleLinkModel(msgs, nv.links[i]));

            }

            Actions = new List<ArticleActionModel>();
            if (incAct)
            {

                for (int i = 0; i < nv.actions.Count; i++)
                    Actions.Add(new ArticleActionModel(msgs, nv.actions[i]));

            }

            preferredRevisor = nv.preferredRevisorId;
            preferredNarrator = nv.preferredNarratorId;
            preferredProducer = nv.preferredProducerId;

            beingRevised = nv.beingRevisedId;
            beingNarrated = nv.beingNarratedId;
            beingProduced = nv.beingProducedId;

            preferredRevisorName = nv.preferredRevisorName;
            preferredNarratorName = nv.preferredNarratorName;
            preferredProducerName = nv.preferredProducerName;

            beingRevisedName = nv.beingRevisedName;
            beingNarratedName = nv.beingNarratedName;
            beingProducedName = nv.beingProducedName;

            Categories = new CategInfo();
            if (incCateg)
            {

                Categories.Categories = new List<NewsCategory>();
                for (int i = 0; i < nv.categories.Count; i++)
                {

                    NewsCategory nc = new NewsCategory();
                    nc.Label = nv.categories[i];
                    nc.Category = msgs.Get(Category.GetNameForLabel(nc.Label));

                    Categories.Categories.Add(nc);

                }

                Categories.MainCategory = new NewsCategory();
                if (Categories.Categories.Count > 0)
                    Categories.MainCategory = Categories.Categories[0];

            }

            VoteApprove = nv.voteApproved;
            VoteNot = nv.voteDontLike;
            VoteAlready = nv.voteAlreadyWritten;
            VotePoorly = nv.votePoorlyWritten;

            UserVote = 0;

            Status = (int)nv.status;
            StatusNarration = (int)nv.statusNarration;
            Type = (int)nv.type;
            ArticleType = (int)nv.textSizeType;

            switch (nv.status)
            {

                case Article.ArticleStatus.Created:
                    StatusName = msgs.Get(RacLib.RacMsg.Id.WaitingApproval);
                    Authors.StatusText = msgs.Get(RacLib.RacMsg.Id.Written);
                    break;

                case Article.ArticleStatus.Approved:
                    StatusName = msgs.Get(RacLib.RacMsg.Id.WaitingRevision);
                    Authors.StatusText = msgs.Get(RacLib.RacMsg.Id.Approved);
                    break;

                case Article.ArticleStatus.Revised:
                    StatusName = msgs.Get(RacLib.RacMsg.Id.WaitingNarration);
                    Authors.StatusText = msgs.Get(RacLib.RacMsg.Id.Revised);
                    break;

                case Article.ArticleStatus.Ready:
                    StatusName = msgs.Get(RacLib.RacMsg.Id.WaitingProduction);
                    Authors.StatusText = msgs.Get(RacLib.RacMsg.Id.Narrated);
                    break;

                case Article.ArticleStatus.Produced:
                    StatusName = msgs.Get(RacLib.RacMsg.Id.WaitingPublishing);
                    Authors.StatusText = msgs.Get(RacLib.RacMsg.Id.Produced);
                    break;

                case Article.ArticleStatus.Published:
                    StatusName = msgs.Get(RacLib.RacMsg.Id.Published);
                    Authors.StatusText = msgs.Get(RacLib.RacMsg.Id.Published);
                    break;

                case Article.ArticleStatus.Removed:
                    StatusName = msgs.Get(RacLib.RacMsg.Id.Removed);
                    Authors.StatusText = msgs.Get(RacLib.RacMsg.Id.Removed);
                    break;

            }

            switch (nv.statusNarration)
            {

                case Article.ArticleNarrationStatus.Undefined:
                    StatusNarrationName = "Aguardando";
                    break;


                case Article.ArticleNarrationStatus.Created:
                    StatusNarrationName = "Submetido";
                    break;

                case Article.ArticleNarrationStatus.Approved:
                    StatusNarrationName = "Aprovado";
                    break;

            }

            switch (nv.type)
            {

                case Article.ArticleType.ShortNote:
                    TypeName = "Tapa Libertário";
                    break;

                case Article.ArticleType.Note:
                    TypeName = "Nota Libertária";
                    break;

                case Article.ArticleType.Article:
                    TypeName = "Artigo";
                    break;

                case Article.ArticleType.Script:
                    TypeName = "Script";
                    break;

                case Article.ArticleType.Chronicle:
                    TypeName = "Crônica";
                    break;

                case Article.ArticleType.Interview:
                    TypeName = "Entrevista";
                    break;

            }

            switch (nv.textSizeType)
            {

                case Article.TextSizeType.TooShort:
                case Article.TextSizeType.ShortNote:
                    ArticleTypeName = "Tapa Libertário";
                    break;

                case Article.TextSizeType.Note:
                    ArticleTypeName = "Nota Libertária";
                    break;

                case Article.TextSizeType.Article:
                case Article.TextSizeType.TooLong:
                    ArticleTypeName = "Artigo";
                    break;

                default:
                    ArticleTypeName = "N/D";
                    break;

            }

        }

    }

    // -------
    //
    //   Modelos genéricos de artigo
    //
    // -------

    public class ArticleListModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public int Sts { get; set; }
        public int Ini { get; set; }
        public int Total { get; set; }
        public List<ArticleModel> Articles { get; set; }

        public ArticleListModel()
        {

            Sts = 0;
            Ini = 0;
            Total = 0;
            Articles = new List<ArticleModel>();

            Result = 0;
            ResultComplement = "";

        }

    }

    public class ArticleCategoryModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int Ini { get; set; }
        public int Total { get; set; }
        public List<ArticleModel> Articles { get; set; }

        public ArticleCategoryModel()
        {

            Title = "";
            Description = "";

            Ini = 0;
            Total = 0;
            Articles = new List<ArticleModel>();

            Result = 0;
            ResultComplement = "";

        }

    }

    public class TargetForNewArticleModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public List<TargetModel> Targets { get; set; }
        public List<NewsCategory> Categories { get; set; }

        public TargetForNewArticleModel()
        {

            Result = 0;
            ResultComplement = "";

            Targets = new List<TargetModel>();
            Categories = new List<NewsCategory>();

        }

    }

    public class ArticleRowModel
    {

        public List<ArticleModel> Articles { get; set; }

        public ArticleRowModel()
        {

            Articles = new List<ArticleModel>();

        }

    }

    // -------
    //
    //   Alterar infos do artigo
    //
    // -------

    public class NewArticleModel
    {

        public string TargetId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public int Type { get; set; }
        public string Categ { get; set; }
        public int Lang { get; set; }

        public NewArticleModel()
        {

            TargetId = "";
            Title = "";
            Text = "";
            Image = "";
            Type = 0;
            Categ = "";
            Lang = 0;

        }

    }

    public class ChangeArticleModel
    {

        public string Id { get; set; }
        public string TargetId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public int Type { get; set; }
        public string Categ { get; set; }
        public int Status { get; set; }
        public int Action { get; set; }
        public int Lang { get; set; }

        public ChangeArticleModel()
        {

            Id = "";
            TargetId = "";
            Title = "";
            Text = "";
            Image = "";
            Type = 0;
            Categ = "";
            Status = 0;
            Action = 0;
            Lang = 0;

        }

    }

    public class EditArticleBaseModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public List<TargetModel> Targets { get; set; }
        public List<NewsCategory> Categories { get; set; }
        public List<NewsAward> Awards { get; set; }
        public List<UserIdModel> Revisors { get; set; }
        public List<UserIdModel> Narrators { get; set; }
        public List<UserIdModel> Producers { get; set; }

        public EditArticleBaseModel()
        {

            Result = 0;
            ResultComplement = "";

            Targets = new List<TargetModel>();
            Categories = new List<NewsCategory>();
            Awards = new List<NewsAward>();
            Revisors = new List<UserIdModel>();
            Narrators = new List<UserIdModel>();
            Producers = new List<UserIdModel>();

        }

    }

    public class ActionFile
    {

        public string FileName { get; set; }
        public string Content { get; set; }

        public ActionFile()
        {

            FileName = "";
            Content = "";

        }

    }

    public class Publish
    {

        public string Id { get; set; }
        public int Status { get; set; }
        public string Info { get; set; }
        public int Lang { get; set; }

        public Publish()
        {

            Id = "";
            Status = 0;
            Info = "";
            Lang = 0;

        }

    }

    public class IncludeActionWithFile
    {

        public string Id { get; set; }
        public int Status { get; set; }
        public string Info { get; set; }
        public List<ActionFile> Files { get; set; }
        public int Lang { get; set; }

        public IncludeActionWithFile()
        {

            Id = "";
            Status = 0;
            Info = "";
            Lang = 0;

            Files = new List<ActionFile>();

        }

    }

    public class IncludeObservaton
    {

        public string Id { get; set; }
        public string CommentId { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Included { get; set; }
        public int Lang { get; set; }

        public IncludeObservaton()
        {

            Id = "";
            CommentId = "";
            Comment = "";
            UserName = "";
            UserId = "";
            Included = "";
            Lang = 0;

        }

    }

    public class RegisterPriority
    {

        public int Priority { get; set; }
        public int Define { get; set; }
        public string Id { get; set; }
        public int Lang { get; set; }

        public RegisterPriority()
        {

            Id = "";
            Priority = 0;
            Define = 0;
            Lang = 0;

        }

    }

    public class SearchArticleModel
    {

        public string SearchString { get; set; }
        public int Lang { get; set; }

        public SearchArticleModel()
        {

            SearchString = "";
            Lang = 0;

        }

    }

    public class UserValue
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public UserValue()
        {

            Id = "";
            Name = "";
            Value = 0.0;

        }

        public UserValue(ArticleAction aa)
        {

            Id = "";
            Name = "";
            Value = 0.0;

            if (aa != null)
            {

                Id = aa.userId;
                Name = aa.userName;
                Value = aa.value;

            }

        }

        public UserValue(Article a, Profile prf, double val)
        {

            Id = "";
            Name = "";
            Value = 0.0;

            if (a != null)
            {

                Id = prf.user.id;
                Name = prf.user.name;
                Value = val;

            }

        }

    }

    public class ValuesPerVideo
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Month { get; set; }

        public UserValue Author { get; set; }
        public UserValue Revisor { get; set; }
        public UserValue Narrator { get; set; }
        public UserValue Producer { get; set; }

        public double Total { get; set; }

        public ValuesPerVideo()
        {

            Id = "";
            Title = "";
            Date = "";
            Month = "";

            Author = new UserValue();
            Revisor = new UserValue();
            Narrator = new UserValue();
            Producer = new UserValue();

            Total = 0;

        }
        
        public ValuesPerVideo(RacLib.RacMsg msgs, LibVisLib.Article art, string userid)
        {

            Id = art.id;
            Title = art.title;
            Date = msgs.ShowDate(art.released);
            Month = art.released.Year.ToString("0000") + art.released.Month.ToString("00");

            Total = 0;

            ArticleAction a0 = art.GetActionForType(ArticleAction.ActionType.Created);

            Author = new UserValue();
            if (a0 != null && (userid == "" || a0.userId == userid))
            {
                Author = new UserValue(a0);
                Total += Author.Value;
            }
            else
            {
                double val = ArticleAction.CalcRevenue(art, ArticleAction.ActionType.Created, art.wordCount, art.wordCount);
                Author = new UserValue(art, Profile.Administrator, val);
                Total += val;
            }

            ArticleAction a1 = art.GetActionForType(ArticleAction.ActionType.Revised);

            Revisor = new UserValue();
            if (a1 != null && (userid == "" || a1.userId == userid))
            {
                Revisor = new UserValue(a1);
                Total += Revisor.Value;
            }
            else
            {
                double val = ArticleAction.CalcRevenue(art, ArticleAction.ActionType.Revised, art.wordCount, art.wordCount);
                Revisor = new UserValue(art, Profile.Administrator, val);
                Total += val;
            }

            ArticleAction a2 = art.GetActionForType(ArticleAction.ActionType.IncludedNaration, art.finalNarratorId);

            Narrator = new UserValue();
            if (a2 != null && (userid == "" || a2.userId == userid))
            {
                Narrator = new UserValue(a2);
                Total += Narrator.Value;
            }
            else
            {
                double val = ArticleAction.CalcRevenue(art, ArticleAction.ActionType.IncludedNaration, art.wordCount, art.wordCount);
                Narrator = new UserValue(art, Profile.Administrator, val);
                Total += val;
            }

            ArticleAction a3 = art.GetActionForType(ArticleAction.ActionType.Produced);

            Producer = new UserValue();
            if (a3 != null && (userid == "" || a3.userId == userid))
            {
                Producer = new UserValue(a3);
                Total += Producer.Value;
            }
            else
            {
                double val = ArticleAction.CalcRevenue(art, ArticleAction.ActionType.Produced, art.wordCount, art.wordCount);
                Producer = new UserValue(art, Profile.Administrator, val);
                Total += val;
            }

        }

    }

    public class VideoValue
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Role { get; set; }
        public double Value { get; set; }

        public VideoValue()
        {

            Id = "";
            Title = "";
            Date = "";
            Role = "";
            Value = 0.0;

        }

        public VideoValue(RacLib.RacMsg msgs, ArticleAction aa)
        {

            Id = "";
            Title = "";
            Date = "";
            Role = "";
            Value = 0.0;

            if (aa != null)
            {

                Id = aa.article.id;
                Title = aa.article.title;
                Date = msgs.ShowDate(aa.article.released);

                switch (aa.type)
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
                                
                Value = aa.value;

            }

        }

        public VideoValue(RacLib.RacMsg msgs, Article a, ArticleAction.ActionType at, double val)
        {

            Id = "";
            Title = "";
            Date = "";
            Role = "";
            Value = 0.0; 

            if (a != null)
            {

                Id = a.id;
                Title = a.title;
                Date = msgs.ShowDate(a.released);

                switch (at)
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

                Value = val;

            }

        }

    }

    public class ValuesPerUser
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Bicoin { get; set; }
        public string Month { get; set; }
        public string Description { get; set; }

        public List<VideoValue> Values { get; set; }

        public double Total { get; set; }

        public ValuesPerUser()
        {

            Id = "";
            Name = "";
            Bicoin = "";
            Month = "";
            Description = "";
            Values = new List<VideoValue>();
            Total = 0;

        }

        public ValuesPerUser(RacLib.RacMsg msgs, ArticleAction aa)
        {

            Id = "";
            Name = "";
            Bicoin = "";
            Month = "";
            Description = "";
            Values = new List<VideoValue>();

            if (aa != null)
            {

                Id = aa.userId;
                Name = aa.userName;
                Bicoin = aa.profile.bitcoin;
                Month = msgs.ShowDate(aa.date);

                VideoValue vv = new VideoValue(msgs, aa);
                Values.Add(vv);

            }

            Total = 0;

        }

        public ValuesPerUser(RacLib.RacMsg msgs, Article a, ArticleAction.ActionType at, double val, Profile prf)
        {

            Id = "";
            Name = "";
            Bicoin = "";
            Month = "";
            Description = "";
            Values = new List<VideoValue>();

            Id = prf.user.id;
            Name = prf.user.name;
            Bicoin = prf.bitcoin;
            Month = msgs.ShowDate(a.released);

            VideoValue vv = new VideoValue(msgs, a, at, val);
            Values.Add(vv);

            Total = 0;

        }

    }

    public class MonthValueDiscrimination
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public string Month { get; set; }

        public List<ValuesPerUser> Users { get; set; }
        public List<ValuesPerVideo> Videos { get; set; }

        public double Total { get; set; }

        public MonthValueDiscrimination()
        {

            Result = 0;
            ResultComplement = "";

            Month = "";

            Users = new List<ValuesPerUser>();
            Videos = new List<ValuesPerVideo>();

            Total = 0;

        }

    }

}



