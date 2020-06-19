using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibVisLib;

namespace LibVisWeb.Models
{

    // -------
    //
    //   VIDEO
    //
    // -------

    public class VideoActionGantModel
    {

        public string AwardId { get; set; }
        public string GrantedBy { get; set; }
        public string Granted { get; set; }

        public VideoActionGantModel()
        {

            AwardId = "";
            GrantedBy = "";
            Granted = "";

        }

        public VideoActionGantModel(VideoActionGrant n)
        {

            AwardId = n.awardId;
            GrantedBy = n.grantedByNameEmail;
            Granted = n.granted.ToString();

        }

    }

    public class VideoActionModel
    {

        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Observation { get; set; }
        public string Date { get; set; }

        public List<VideoActionGantModel> Grants { get; set; }

        public VideoActionModel()
        {

            Id = "";
            UserId = "";
            UserName = "";
            Type = 0;
            Observation = "";
            Date = "";

            Grants = new List<VideoActionGantModel>();

        }

        public VideoActionModel(VideoAction n)
        {

            Id = n.id;
            UserId = n.userId;
            UserName = n.userName;
            Type = (int)n.type;
            Observation = n.observation;
            Date = n.date.ToString();

            switch (n.type)
            {

                case VideoAction.ActionType.AddedAudio:
                    TypeName = "Incluiu áudio";
                    break;

                case VideoAction.ActionType.AddedVideo:
                    TypeName = "Incluiu vídeo";
                    break;

                case VideoAction.ActionType.Approved:
                    TypeName = "Aprovado";
                    break;

                case VideoAction.ActionType.Created:
                    TypeName = "Criado";
                    break;

                case VideoAction.ActionType.IncludedInVideo:
                    TypeName = "Incluído em vídeo";
                    break;

                case VideoAction.ActionType.Revised:
                    TypeName = "Revisado";
                    break;

            }

            Grants = new List<VideoActionGantModel>();

            for (int i = 0; i < n.grants.Count; i++)
                Grants.Add(new VideoActionGantModel(n.grants[i]));

        }

    }

    public class VideoModel
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string YoutubeLink { get; set; }
        public string BitchuteLink { get; set; }

        public string StartingDescription { get; set; }
        public string Description { get; set; }
        public List<string> DescriptionPars { get; set; }
        public string Script { get; set; }
        public List<string> ScriptPars { get; set; }

        public string Tags { get; set; }

        public CategInfo Categories { get; set; }

        public AuthorInfo Authors { get; set; }

        public List<VideoActionModel> Actions { get; set; }

        public int Status { get; set; }
        public string StatusName { get; set; }

        public VideoModel()
        {

            Id = "";
            Title = "";
            YoutubeLink = "";
            BitchuteLink = "";

            StartingDescription = "";
            Description = "";
            DescriptionPars = new List<string>();
            Script = "";
            ScriptPars = new List<string>();

            Tags = "";

            Categories = new CategInfo();

            Authors = new AuthorInfo();

            Actions = new List<VideoActionModel>();

            Status = 0;
            StatusName = "";

        }

        public VideoModel(RacLib.RacMsg msgs, Video nv, bool incText, bool incCateg, bool incAuth, bool incAct)
        {

            Id = nv.id;
            Title = nv.title;

            YoutubeLink = nv.linkYoutube;
            BitchuteLink = nv.linkBitchute;

            StartingDescription = (nv.description.Length < 300) ? nv.description : nv.description.Substring(0, 300);

            if (incText)
            {
                Description = nv.description;
                Script = nv.script;
                DescriptionPars = nv.description.Split('\n').ToList();
                ScriptPars = nv.script.Split('\n').ToList();
            }
            else
            {
                Description = "";
                Script = "";
                DescriptionPars = new List<string>();
                ScriptPars = new List<string>();
            }

            Authors = new AuthorInfo();

            if (incAuth)
            {

                Authors.AuthoredLabel = "Produzido por";
                
                Authors.Date = nv.released.ToString("dd/MM/yyyy");
                Authors.DateLabel = "em";

                Authors.Authored = new UserIdModel(nv.authorId, nv.author);

            }

            Actions = new List<VideoActionModel>();
            if (incAct)
            {

                for (int i = 0; i < nv.actions.Count; i++)
                    Actions.Add(new VideoActionModel(nv.actions[i]));

            }

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

            Status = (int)1;
            StatusName = "Publicado";

        }

    }

    // -------
    //
    //   Modelos genéricos de vídeo
    //
    // -------

    public class VideoListModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public int Ini { get; set; }
        public int Total { get; set; }
        public List<VideoModel> Videos { get; set; }

        public VideoListModel()
        {

            Ini = 0;
            Total = 0;
            Videos = new List<VideoModel>();

            Result = 0;
            ResultComplement = "";

        }

    }

    public class VideoCategoryModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int Ini { get; set; }
        public int Total { get; set; }
        public List<VideoModel> Videos { get; set; }

        public VideoCategoryModel()
        {

            Title = "";
            Description = "";

            Ini = 0;
            Total = 0;
            Videos = new List<VideoModel>();

            Result = 0;
            ResultComplement = "";

        }

    }

    public class YoutubeModel
    {

        public string Link { get; set; }
        public int Lang { get; set; }

        public YoutubeModel()
        {

            Link = "";
            Lang = 0;

        }

    }

    public class YoutubeResultModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string YoutubeLink { get; set; }
        public string BitchuteLink { get; set; }
        public string Image { get; set; }

        public YoutubeResultModel()
        {

            Result = 0;
            ResultComplement = "";

            Title = "";
            Description = "";
            Tags = "";
            YoutubeLink = "";
            BitchuteLink = "";
            Image = "";

        }

    }

    public class EditVideoBaseModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public List<NewsCategory> Categories { get; set; }

        public EditVideoBaseModel()
        {

            Result = 0;
            ResultComplement = "";

            Categories = new List<NewsCategory>();

        }

    }
    
    // -------
    //
    //   Alterar infos do vídeo
    //
    // -------

    public class NewVideoModel
    {

        public string Title { get; set; }        
        public string Description { get; set; }
        public string Tags { get; set; }
        public string YoutubeLink { get; set; }
        public string BitchuteLink { get; set; }
        public string Image { get; set; }
        public string Categ { get; set; }
        public string Script { get; set; }
        public int Lang { get; set; }

        public NewVideoModel()
        {

            Title = "";
            Description = "";
            Tags = "";
            YoutubeLink = "";
            BitchuteLink = "";
            Image = "";
            Categ = "";
            Script = "";
            Lang = 0;

        }

    }

    public class ChangeVideoModel
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string YoutubeLink { get; set; }
        public string BitchuteLink { get; set; }
        public string Image { get; set; }
        public string Categ { get; set; }
        public string Script { get; set; }
        public int Status { get; set; }
        public int Action { get; set; }
        public int Lang { get; set; }

        public ChangeVideoModel()
        {

            Id = "";
            Title = "";
            Description = "";
            Tags = "";
            YoutubeLink = "";
            BitchuteLink = "";
            Image = "";
            Categ = "";
            Script = "";
            Status = 0;
            Action = 0;
            Lang = 0;

        }

    }

    public class SearchVideoModel
    {

        public string SearchString { get; set; }
        public int Lang { get; set; }

        public SearchVideoModel()
        {

            SearchString = "";
            Lang = 0;

        }

    }

}

