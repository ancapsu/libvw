using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibVisLib;

namespace LibVisWeb.Models
{

    // -------
    //
    //   Pegar infos do TARGET
    //
    // -------

    public class TargetActionGantModel
    {

        public string AwardId { get; set; }
        public string GrantedBy { get; set; }
        public string Granted { get; set; }

        public TargetActionGantModel()
        {

            AwardId = "";
            GrantedBy = "";
            Granted = "";

        }

        public TargetActionGantModel(TargetActionGrant n)
        {

            AwardId = n.awardId;
            GrantedBy = n.grantedByNameEmail;
            Granted = n.granted.ToString();

        }

    }

    public class TargetActionModel
    {

        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Observation { get; set; }
        public string Date { get; set; }

        public List<TargetActionGantModel> Grants { get; set; }

        public TargetActionModel()
        {

            Id = "";
            UserId = "";
            UserName = "";
            Type = 0;
            Observation = "";
            Date = "";

            Grants = new List<TargetActionGantModel>();

        }

        public TargetActionModel(TargetAction n)
        {

            Id = n.id;
            UserId = n.userId;
            UserName = n.userName;
            Type = (int)n.type;
            Observation = n.observation;
            Date = n.date.ToString();

            switch (n.type)
            {

                case TargetAction.ActionType.Suggested:
                    TypeName = "Sugerido";
                    break;

                case TargetAction.ActionType.Revised:
                    TypeName = "Revisado";
                    break;

                case TargetAction.ActionType.ArticleCreated:
                    TypeName = "Artigo criado";
                    break;

                case TargetAction.ActionType.ArticleApproved:
                    TypeName = "Artigo aprovado";
                    break;

                case TargetAction.ActionType.ArticleIncludedInVideo:
                    TypeName = "Publicado";
                    break;

                case TargetAction.ActionType.Removed:
                    TypeName = "Removido";
                    break;

            }

            Grants = new List<TargetActionGantModel>();

            for (int i = 0; i < n.grants.Count; i++)
                Grants.Add(new TargetActionGantModel(n.grants[i]));

        }

    }

    public class TargetModel
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }

        public string StartingText { get; set; }
        public string Text { get; set; }
        public List<string> Paragraphs { get; set; }

        public CategInfo Categories { get; set; }

        public AuthorInfo Authors { get; set; }

        public int VoteWrite { get; set; }
        public int VoteVery { get; set; }
        public int VoteGood { get; set; }
        public int VoteNot { get; set; }
        public int VoteOld { get; set; }
        public int VoteFake { get; set; }

        public int UserVote { get; set; }

        public List<TargetActionModel> Actions { get; set; }

        public int Status { get; set; }
        public string StatusName { get; set; }

        public string AssociatedArticleId { get; set; }
        public string AssociatedArticleTitle { get; set; }

        public TargetModel()
        {

            Id = "";
            Title = "";
            Link = "";
            StartingText = "";

            Text = "";
            Paragraphs = new List<string>();

            Categories = new CategInfo();

            Authors = new AuthorInfo();

            VoteWrite = 0;
            VoteVery = 0;
            VoteGood = 0;
            VoteNot = 0;
            VoteOld = 0;
            VoteFake = 0;
            UserVote = -1;

            Actions = new List<TargetActionModel>();

            Status = 0;
            StatusName = "";

            AssociatedArticleId = "";
            AssociatedArticleTitle = "";

        }

        public TargetModel(RacLib.RacMsg msgs, Target nt, bool incText, bool incCateg, bool incAuth, bool incAct)
        {

            Id = nt.id;
            Title = nt.title;
            Link = nt.link;
            StartingText = (nt.text.Length < 300) ? nt.text : nt.text.Substring(0, 300);

            if (incText)
            {
                Text = nt.text;
                Paragraphs = nt.text.Split('\n').ToList(); 
            }
            else
            {
                Text = "";
                Paragraphs = new List<string>();
            }

            Authors = new AuthorInfo();

            if (incAuth)
            {

                Authors.SuggestedLabel = msgs.Get(RacLib.RacMsg.Id.SuggestedBy);
                Authors.AuthoredLabel = msgs.Get(RacLib.RacMsg.Id.SuggestedBy);

                Authors.Date = msgs.ShowDate(nt.registered);
                Authors.DateLabel = msgs.Get(RacLib.RacMsg.Id.Registered2);
                Authors.StatusText = "";

                Authors.Authored = new UserIdModel(nt.authorId, nt.author);

            }

            Actions = new List<TargetActionModel>();
            if (incAct)
            {

                for (int i = 0; i < nt.actions.Count; i++)
                    Actions.Add(new TargetActionModel(nt.actions[i]));

            }

            Categories = new CategInfo();
            if (incCateg)
            {

                Categories.Categories = new List<NewsCategory>();
                for (int i = 0; i < nt.categories.Count; i++)
                {

                    NewsCategory nc = new NewsCategory();
                    nc.Label = nt.categories[i];
                    nc.Category = msgs.Get(Category.GetNameForLabel(nc.Label));

                    Categories.Categories.Add(nc);

                }

                Categories.MainCategory = new NewsCategory();
                if (Categories.Categories.Count > 0)
                    Categories.MainCategory = Categories.Categories[0];

            }

            VoteWrite = nt.voteWillWriteArticle;
            VoteVery = nt.voteVeryGood;
            VoteGood = nt.voteGood;
            VoteNot = nt.voteNotInteresting;
            VoteOld = nt.voteOldNews;
            VoteFake = nt.voteFalseFact;

            UserVote = 0;

            Status = 1;
            StatusName = "Registrado";

            AssociatedArticleId = "";
            AssociatedArticleTitle = "";

            Article art = Article.ArticleFromTarget(nt.id);

            if (art != null)
            {

                Status = 2;
                StatusName = "Já usado";

                AssociatedArticleId = art.id;
                AssociatedArticleTitle = art.title;

            }

        }

    }

    // -------
    //
    //   Modelos genéricos da pauta
    //
    // -------

    public class TargetListModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public int Ini { get; set; }
        public int Total { get; set; }
        public List<TargetModel> Targets { get; set; }

        public TargetListModel()
        {

            Ini = 0;
            Total = 0;
            Targets = new List<TargetModel>();

            Result = 0;
            ResultComplement = "";

        }

    }

    public class TargetCategoryModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int Ini { get; set; }
        public int Total { get; set; }
        public List<TargetModel> Targets { get; set; }

        public TargetCategoryModel()
        {

            Title = "";
            Description = "";

            Ini = 0;
            Total = 0;
            Targets = new List<TargetModel>();

            Result = 0;
            ResultComplement = "";

        }

    }

    public class EditTargetBaseModel
    {
        
        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public List<NewsCategory> Categories { get; set; }
        public List<NewsAward> Awards { get; set; }

        public EditTargetBaseModel()
        {

            Result = 0;
            ResultComplement = "";

            Categories = new List<NewsCategory>();
            Awards = new List<NewsAward>();

        }

    }

    // -------
    //
    //   Alterar infos da pauta
    //
    // -------

    public class NewTargetModel
    {

        public string Title { get; set; }
        public string Link { get; set; }
        public string Text { get; set; }
        public int ImageType { get; set; }
        public string Image { get; set; }
        public string Categ { get; set; }
        public int Lang { get; set; }

        public NewTargetModel()
        {

            Title = "";
            Link = "";
            Text = "";
            ImageType = 0;
            Image = "";
            Categ = "";
            Lang = 0;

        }

    }

    public class ChangeTargetModel
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Categ { get; set; }
        public int Status { get; set; }
        public int Action { get; set; }
        public int Lang { get; set; }

        public ChangeTargetModel()
        {

            Id = "";
            Title = "";
            Link = "";
            Text = "";
            Image = "";
            Categ = "";
            Status = 0;
            Action = 0;
            Lang = 0;

        }

    }

    public class RegisterVote
    {

        public string Id { get; set; }
        public int Vote { get; set; }
        public int Lang { get; set; }

        public RegisterVote()
        {

            Id = "";
            Vote = 0;
            Lang = 0;

        }

    }

    public class TargetActionGrantModel
    {

        public string TargetActionId { get; set; }
        public int GrantType { get; set; }

        public TargetActionGrantModel()
        {

            TargetActionId = "";
            GrantType = 0;

        }

    }

    public class SearchTargetModel
    {

        public string SearchString;

        public SearchTargetModel()
        {

            SearchString = "";

        }

    }

}

