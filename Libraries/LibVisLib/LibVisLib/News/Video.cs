using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class Video
    {

        /// <summary>
        /// Id do item
        /// </summary>
        protected string idInt;

        /// <summary>
        /// Designado?
        /// </summary>
        protected bool dbassignedidInt;

        /// <summary>
        /// Nome do vídeo
        /// </summary>
        string titleInt;

        /// <summary>
        /// Texto completo
        /// </summary>
        string descriptionInt;

        /// <summary>
        /// Linguagem
        /// </summary>
        int languageInt;

        /// <summary>
        /// Tags de busca
        /// </summary>
        string tagsInt;

        /// <summary>
        /// Link do vídeo no youtube
        /// </summary>
        string youtubeInt;

        /// <summary>
        /// Link do vídeo no bitchute
        /// </summary>
        string bitchuteInt;

        /// <summary>
        /// Liberado
        /// </summary>
        DateTime releasedInt;

        /// <summary>
        /// Script do vídeo
        /// </summary>
        string scriptInt;

        /// <summary>
        /// Ações
        /// </summary>
        List<VideoAction> actionsInt;

        /// <summary>
        /// Categorias dessa notícia
        /// </summary>
        List<string> categoriesInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public Video()
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            titleInt = "";
            descriptionInt = "";
            languageInt = 0;
            tagsInt = "";
            youtubeInt = "";
            bitchuteInt = "";
            releasedInt = RacMsg.NullDateTime;
            scriptInt = "";

            actionsInt = null;
            categoriesInt = null;

        }

        /// <summary>
        /// Id do artigo
        /// </summary>
        public string id
        {

            get { return idInt; }

        }

        /// <summary>
        /// Já foi registrado no BD
        /// </summary>
        public bool dbAssigned
        {

            get { return dbassignedidInt; }

        }

        /// <summary>
        /// Lingua do artigo
        /// </summary>
        public RacMsg.Language language
        {

            get { return (RacMsg.Language)languageInt; }
            set { languageInt = (int)value; }

        }

        /// <summary>
        /// Nome do vídeo
        /// </summary>
        public string title
        {

            get { return titleInt; }
            set { titleInt = value; }

        }

        /// <summary>
        /// Descrição
        /// </summary>
        public string description
        {

            get { return descriptionInt; }
            set { descriptionInt = value; }

        }

        /// <summary>
        /// Tags
        /// </summary>
        public string tags
        {

            get { return tagsInt; }
            set { tagsInt = value; }

        }

        /// <summary>
        /// Link principal do conteúdo (vídeo)
        /// </summary>
        public string linkYoutube
        {

            get { return youtubeInt; }
            set { youtubeInt = value; }

        }

        /// <summary>
        /// Link principal do conteúdo (vídeo)
        /// </summary>
        public string linkBitchute
        {

            get { return bitchuteInt; }
            set { bitchuteInt = value; }

        }

        /// <summary>
        /// Liberado
        /// </summary>
        public DateTime released
        {

            get { return releasedInt; }
            set { releasedInt = value; }

        }

        /// <summary>
        /// Script do vídeo
        /// </summary>
        public string script
        {

            get { return scriptInt; }
            set { scriptInt = value; }

        }

        /// <summary>
        /// Categoria principal da notícia
        /// </summary>
        public string mainLabel
        {

            get
            {

                if (categories.Count > 0)
                    return categories[0];
                else
                    return "none";

            }

        }

        /// <summary>
        /// Ações
        /// </summary>
        public List<VideoAction> actions
        {

            get
            {

                if (actionsInt == null)
                    actionsInt = ActionForNews();

                return actionsInt;

            }

            set
            {

                actionsInt = value;

            }

        }

        /// <summary>
        /// Carrega a lista de classes de licença para este sistema
        /// </summary>
        /// <returns></returns>
        List<VideoAction> ActionForNews()
        {

            List<string> ids = Base.conf.LoadStringList(id, "newsvideoaction", "videoid", "id", "date");
            List<VideoAction> lst = new List<VideoAction>();

            for (int i = 0; i < ids.Count; i++)
            {

                VideoAction npc = new VideoAction(this);
                if (npc.Load(ids[i]))
                    lst.Add(npc);

            }

            return lst;

        }

        /// <summary>
        /// Licenças
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VideoAction GetNewsAction(string id)
        {

            for (int i = 0; i < actions.Count; i++)
                if (actions[i].id == id)
                    return actions[i];

            return null;

        }

        /// <summary>
        /// Posição do call
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int GetPosition(VideoAction n)
        {

            for (int i = 0; i < actions.Count; i++)
                if (actions[i].id == n.id)
                    return i;

            return -1;

        }

        /// <summary>
        /// Pega a ação específica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VideoAction GetNewsActionForType(VideoAction.ActionType t)
        {

            for (int i = 0; i < actions.Count; i++)
                if (actions[i].type == t)
                    return actions[i];

            return null;

        }

        public string author
        {

            get
            {

                VideoAction n = GetNewsActionForType(VideoAction.ActionType.Created);
                if (n != null && n.show)
                    return n.userName;
                else
                    return "N/D";

            }

        }

        public string authorId
        {

            get
            {

                VideoAction n = GetNewsActionForType(VideoAction.ActionType.Created);
                if (n != null && n.show)
                    return n.userId;
                else
                    return "";

            }

        }

        public List<string> othersInvolved
        {

            get
            {

                List<string> lst = new List<string>();

                // Tem que descartar o autor
                VideoAction n = GetNewsActionForType(VideoAction.ActionType.Created);

                // Verifica todos os demais
                for (int i = 0; i < actions.Count; i++)
                {

                    if (n != null && actions[i].userId != n.userId && actions[i].show)
                    {

                        bool jatem = false;

                        for (int j = 0; j < lst.Count; j++)
                            if (lst[j] == actions[i].userName)
                                jatem = true;

                        if (!jatem)
                            lst.Add(actions[i].userName);

                    }

                }

                return lst;

            }

        }

        public List<string> othersInvolvedIds
        {

            get
            {

                List<string> lst = new List<string>();

                // Tem que descartar o autor
                VideoAction n = GetNewsActionForType(VideoAction.ActionType.Created);

                // Verifica todos os demais
                for (int i = 0; i < actions.Count; i++)
                {

                    if (n != null && actions[i].userId != n.userId && actions[i].show)
                    {

                        bool jatem = false;

                        for (int j = 0; j < lst.Count; j++)
                            if (lst[j] == actions[i].userId)
                                jatem = true;

                        if (!jatem)
                            lst.Add(actions[i].userId);

                    }

                }

                return lst;

            }

        }

        /// <summary>
        /// Normaliza a lista de categorias
        /// </summary>
        public void NormalizeMain()
        {

            List<string> rc = new List<string>();

            // Primeiro inclui a verdadeira!
            rc.Add("video");

            for (int i = 0; i < categories.Count; i++)
            {

                if (!Category.IsMain(categories[i]))
                    rc.Add(categories[i]);

            }

            categories = rc;

        }

        /// <summary>
        /// Categorias dessa notícia
        /// </summary>
        public List<string> categories
        {

            get
            {

                if (categoriesInt == null)
                {
                    categoriesInt = Base.conf.LoadStringList(id, "newsvideocategory", "id", "label");
                    NormalizeMain();
                }

                return categoriesInt;

            }

            set { categoriesInt = value; }

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Load(string id)
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();
            
            sel.CommandText = "SELECT title, description, lang, tags, youtube, bitchute, released, script FROM " + Base.conf.prefix + "[newsvideo] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                titleInt = rdr.GetString(0);
                descriptionInt = rdr.GetString(1);
                languageInt = rdr.GetInt32(2);
                tagsInt = rdr.GetString(3);
                youtubeInt = rdr.GetString(4);
                bitchuteInt = rdr.GetString(5);
                releasedInt = rdr.GetDateTime(6);
                scriptInt = rdr.GetString(7);

                dbassignedidInt = true;
                res = true;

            }

            rdr.Close();
            sel.Connection.Close();

            return res;

        }
        
        /// <summary>
        /// Salva a parada
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            bool res = false;

            lock (RacLib.Base.conf)
            {

                SqlCommand sel = new SqlCommand();
                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE id=@id OR title=@title";
                sel.Parameters.Add(new SqlParameter("@id", id));
                sel.Parameters.Add(new SqlParameter("@title", title));

                sel.Connection = Base.conf.Open();
                SqlDataReader rdr = sel.ExecuteReader();

                if (!rdr.Read())
                {
                    
                    rdr.Close();
                    sel.Connection.Close();

                    SqlCommand ins = new SqlCommand();
                    ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsvideo] (id, title, description, lang, tags, youtube, bitchute, released, script) VALUES (@id, @title, @description, @lang, @tags, @youtube, @bitchute, @released, @script)";
                    ins.Parameters.Add(new SqlParameter("@id", id));

                    ins.Parameters.Add(new SqlParameter("@title", titleInt));
                    ins.Parameters.Add(new SqlParameter("@description", descriptionInt));
                    ins.Parameters.Add(new SqlParameter("@lang", languageInt));
                    ins.Parameters.Add(new SqlParameter("@tags", tagsInt));
                    ins.Parameters.Add(new SqlParameter("@youtube", youtubeInt));
                    ins.Parameters.Add(new SqlParameter("@bitchute", bitchuteInt));
                    ins.Parameters.Add(new SqlParameter("@released", releasedInt));
                    ins.Parameters.Add(new SqlParameter("@script", scriptInt));

                    ins.Connection = Base.conf.Open();
                    ins.ExecuteNonQuery();
                    ins.Connection.Close();

                    res = true;
                    dbassignedidInt = true;

                }
                else
                {

                    idInt = rdr.GetString(0);

                    rdr.Close();
                    sel.Connection.Close();

                    SqlCommand upd = new SqlCommand();
                    upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsvideo] SET title=@title, description=@description, lang=@lang, tags=@tags, youtube=@youtube, bitchute=@bitchute, released=@released, script=@script WHERE id=@id";

                    upd.Parameters.Add(new SqlParameter("@title", titleInt));
                    upd.Parameters.Add(new SqlParameter("@description", descriptionInt));
                    upd.Parameters.Add(new SqlParameter("@lang", languageInt));
                    upd.Parameters.Add(new SqlParameter("@tags", tagsInt));
                    upd.Parameters.Add(new SqlParameter("@youtube", youtubeInt));
                    upd.Parameters.Add(new SqlParameter("@bitchute", bitchuteInt));
                    upd.Parameters.Add(new SqlParameter("@released", releasedInt));
                    upd.Parameters.Add(new SqlParameter("@script", scriptInt));

                    upd.Parameters.Add(new SqlParameter("@id", id));

                    upd.Connection = Base.conf.Open();
                    upd.ExecuteNonQuery();
                    upd.Connection.Close();

                    res = true;
                    dbassignedidInt = true;

                }

                if (res)
                {

                    if (actionsInt != null)
                    {

                        foreach (VideoAction ta in actions) { ta.Save(); }

                    }

                    if (categoriesInt != null)
                    {

                        Base.conf.UpdateStringList(id, categories, "newsvideocategory", "id", "label");

                    }

                }

            }

            return res;

        }

        /// <summary>
        /// Apaga a ticket
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            // Apaga todos os grants

            SqlCommand del0 = new SqlCommand();
            del0.CommandText = "DELETE FROM " + Base.conf.prefix + "[newstargetactiongrant] WHERE id IN (SELECT id from " + Base.conf.prefix + "[newstargetaction] WHERE targetid=@id)";
            del0.Parameters.Add(new SqlParameter("@id", id));
            del0.Connection = Base.conf.Open();
            del0.ExecuteNonQuery();
            del0.Connection.Close();

            // Apaga todas as ações

            SqlCommand del1 = new SqlCommand();
            del1.CommandText = "DELETE FROM " + Base.conf.prefix + "[newstargetaction] WHERE targetid=@id";
            del1.Parameters.Add(new SqlParameter("@id", id));
            del1.Connection = Base.conf.Open();
            del1.ExecuteNonQuery();
            del1.Connection.Close();

            // Apaga todas as categorias

            SqlCommand del2 = new SqlCommand();
            del2.CommandText = "DELETE FROM " + Base.conf.prefix + "[newstargetcategory] WHERE id=@id";
            del2.Parameters.Add(new SqlParameter("@id", id));
            del2.Connection = Base.conf.Open();
            del2.ExecuteNonQuery();
            del2.Connection.Close();

            // Apaga todos os comentários

            SqlCommand del3 = new SqlCommand();
            del3.CommandText = "DELETE FROM " + Base.conf.prefix + "[newstargetcomment] WHERE id=@id";
            del3.Parameters.Add(new SqlParameter("@id", id));
            del3.Connection = Base.conf.Open();
            del3.ExecuteNonQuery();
            del3.Connection.Close();

            // Apaga todos os votos

            SqlCommand del4 = new SqlCommand();
            del4.CommandText = "DELETE FROM " + Base.conf.prefix + "[newstargetvote] WHERE id=@id";
            del4.Parameters.Add(new SqlParameter("@id", id));
            del4.Connection = Base.conf.Open();
            del4.ExecuteNonQuery();
            del4.Connection.Close();

            // Apaga o target em si

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newstarget] WHERE id=@id";
            del.Parameters.Add(new SqlParameter("@id", id));
            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();
            del.Connection.Close();

            return true;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Video LoadVideo(string id)
        {

            Video n = new Video();
            if (n.Load(id))
                return n;

            return null;

        }
        
        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Video> GetLastVideos(int lang, string categ, int ini, int max, string search = "")
        {

            List<string> ids = new List<string>();
            SqlCommand sel = new SqlCommand();

            if (categ == "")
            {

                if (search == "")
                {

                    if (lang < 2)
                    {
                         
                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] ORDER BY released DESC";

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE lang=@lang ORDER BY released DESC";
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE (title LIKE @search OR description LIKE @search OR script LIKE @search OR tags LIKE @search) ORDER BY released DESC";
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE lang=@lang AND (title LIKE @search OR description LIKE @search OR script LIKE @search OR tags LIKE @search) ORDER BY released DESC";
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            else
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE id IN (SELECT id FROM " + Base.conf.prefix + "[newsvideocategory] WHERE label=@label) ORDER BY released DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE lang=@lang AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsvideocategory] WHERE label=@label) ORDER BY released DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE id IN (SELECT id FROM " + Base.conf.prefix + "[newsvideocategory] WHERE label=@label) AND (title LIKE @search OR description LIKE @search OR script LIKE @search OR tags LIKE @search) ORDER BY released DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE lang=@lang AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsvideocategory] WHERE label=@label) AND (title LIKE @search OR description LIKE @search OR script LIKE @search OR tags LIKE @search) ORDER BY released DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            
            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            int count = 0;

            while (rdr.Read())
            {

                if (count >= ini && count < ini + max)
                    ids.Add(rdr.GetString(0));
                if (count >= ini + max)
                    break;

                count++;

            }

            rdr.Close();
            sel.Connection.Close();

            List<Video> lst = new List<Video>();
            for(int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadVideo(ids[i]));

            }

            return lst;
            
        }


        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetTotalLastVideos(int lang, string categ, string search = "")
        {

            SqlCommand sel = new SqlCommand();

            if (categ == "")
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo]";

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo] WHERE lang=@lang";
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo] WHERE (title LIKE @search OR description LIKE @search OR script LIKE @search OR tags LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo] WHERE lang=@lang AND (title LIKE @search OR description LIKE @search OR script LIKE @search OR tags LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            else
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo] WHERE id IN (SELECT id FROM " + Base.conf.prefix + "[newsvideocategory] WHERE label=@label)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo] WHERE lang=@lang AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsvideocategory] WHERE label=@label)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo] WHERE id IN (SELECT id FROM " + Base.conf.prefix + "[newsvideocategory] WHERE label=@label) AND (title LIKE @search OR description LIKE @search OR script LIKE @search OR tags LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo] WHERE lang=@lang AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsvideocategory] WHERE label=@label) AND (title LIKE @search OR description LIKE @search OR script LIKE @search OR tags LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            int count = 0;

            if (rdr.Read())
            {

                count = rdr.GetInt32(0);

            }

            rdr.Close();
            sel.Connection.Close();

            return count;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Video> GetLastVideosForUser(string userid, int ini, int max)
        {

            List<string> ids = new List<string>();
            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideo] WHERE id IN (SELECT videoid FROM " + Base.conf.prefix + "[newsvideoaction] WHERE userid=@userid) ORDER BY released DESC";
            sel.Parameters.Add(new SqlParameter("@userid", userid));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (max <= ini)
                max = 30000000;

            int count = 0;

            while (rdr.Read())
            {

                if (count >= ini && count < ini + max)
                    ids.Add(rdr.GetString(0));
                if (count >= ini + max)
                    break;

                count++;

            }

            rdr.Close();
            sel.Connection.Close();

            List<Video> lst = new List<Video>();
            for (int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadVideo(ids[i]));

            }

            return lst;

        }
        
        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetTotalLastVideosForUser(string userid)
        {

            List<string> ids = new List<string>();
            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsvideo] WHERE id IN (SELECT videoid FROM " + Base.conf.prefix + "[newsvideoaction] WHERE userid=@userid) ORDER BY released DESC";
            sel.Parameters.Add(new SqlParameter("@userid", userid));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();
            
            int count = 0;

            if (rdr.Read())
            {

                count = rdr.GetInt32(0);

            }

            rdr.Close();
            sel.Connection.Close();

            return count;

        }

    }

}