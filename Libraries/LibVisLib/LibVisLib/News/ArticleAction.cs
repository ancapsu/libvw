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
    public class ArticleAction
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
        /// Id do usuário
        /// </summary>
        string useridInt;

        /// <summary>
        /// Tipo
        /// </summary>
        int typeInt;

        /// <summary>
        /// Tipo de notícia
        /// </summary>
        public enum ActionType { Undefined, Suggested, Created, Approved, Revised, IncludedNaration, Produced, Published, Removed };

        /// <summary>
        /// Observação
        /// </summary>
        string observationInt;

        /// <summary>
        /// Última mudança
        /// </summary>
        DateTime dateInt;

        /// <summary>
        /// Deve mostrar
        /// </summary>
        int showInt;

        /// <summary>
        /// Número de palavras
        /// </summary>
        int billablewordsInt;

        /// <summary>
        /// Texto
        /// </summary>
        string textInt;

        /// <summary>
        /// Grants dessa ação
        /// </summary>
        List<ArticleActionGrant> grantsInt;

        /// <summary>
        /// Observações desse 
        /// </summary>
        List<ArticleActionObservation> observationsInt;

        /// <summary>
        /// Notícia
        /// </summary>
        Article articleInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ArticleAction(Article n)
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            articleInt = n;

            useridInt = "";
            typeInt = 0;
            observationInt = "";
            dateInt = RacMsg.NullDateTime;
            showInt = 0;
            billablewordsInt = 0;
            textInt = "";
            grantsInt = null;
            observationsInt = null;

        }

        /// <summary>
        /// Notícia
        /// </summary>
        public Article article
        {

            get { return articleInt; }

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
        /// Id da notícia
        /// </summary>
        public string articleId
        {

            get { return article.id; }

        }

        /// <summary>
        /// Id do usuário
        /// </summary>
        public string userId
        {

            get { return useridInt; }
            set { useridInt = value; }

        }

        /// <summary>
        /// Usuário
        /// </summary>
        public Profile profile
        {

            get { return Profile.LoadProfile(userId); }

        }

        /// <summary>
        /// Usuário
        /// </summary>
        public BaseUser user
        {

            get { return RacWebUserSource.source.LoadUser(userId); }

        }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string userName
        {

            get
            {

                BaseUser u = user;

                if (user != null)
                    return user.name;
                else
                    return "";

            }

        }

        /// <summary>
        /// Tipo
        /// </summary>
        public ActionType type
        {

            get { return (ActionType)typeInt; }
            set { typeInt = (int)value; }

        }

        /// <summary>
        /// Observação
        /// </summary>
        public string observation
        {

            get { return observationInt; }
            set { observationInt = value; }

        }

        /// <summary>
        /// Última mudança
        /// </summary>
        public DateTime date
        {

            get { return dateInt; }
            set { dateInt = value; }

        }
        
        /// <summary>
        /// Deve mostrar
        /// </summary>
        public bool show
        {

            get { return showInt != 0; }
            set { showInt = value ? 1 : 0; }

        }

        /// <summary>
        /// Número de palavras
        /// </summary>
        public int billableWords
        {

            get
            {

                if (billablewordsInt == 0)
                {

                    billablewordsInt = article.wordCount;

                    for (int i = 0; i < article.actions.Count; i++)
                    {
                        
                        if (article.actions[i].id == id)
                            break;

                        if (article.actions[i].billablewordsInt > 0)
                            billablewordsInt = article.actions[i].billablewordsInt;

                    }

                }

                return billablewordsInt;

            }

            set { billablewordsInt = value; }

        }

        /// <summary>
        /// Número de palavras
        /// </summary>
        public int minimumBillableWords
        {

            get
            {

                int min = article.wordCount;

                for (int i = 0; i < article.actions.Count; i++)
                {

                    if (article.actions[i].billableWords > 0 && article.actions[i].billableWords < min)
                        min = article.actions[i].billableWords;

                }

                return min;

            }

        }

        public static double CalcValue(int words, ActionType type)
        {

            switch (type)
            {

                case ActionType.Suggested:
                    return 0;

                case ActionType.Created:
                    return words * 0.0000008;   // 80 satoshis por palavra no mínimo do artigo

                case ActionType.Approved:
                    return 0;

                case ActionType.Revised:
                    return words * 0.0000008;   // 80 satoshis por palavra para revisão

                case ActionType.IncludedNaration:
                    return words * 0.0000008;   // 80 satoshis por palavra para narração

                case ActionType.Produced:
                    return words * 0.0000008;   // 80 satoshis por palavra para produção

                case ActionType.Published:
                    return 0;

                case ActionType.Removed:
                    return 0;

            }

            return 0;

        }

        /// <summary>
        /// <summary>
        /// Número de palavras
        /// </summary>
        public double value
        {

            get
            {

                return CalcRevenue(article, type, minimumBillableWords, billableWords);

            }

        }

        /// <summary>
        /// TABELA PRINCIPAL DE CÁLCULO
        /// </summary>
        /// <param name="a"></param>
        /// <param name="at"></param>
        /// <param name="minBillableWords"></param>
        /// <param name="billableWords"></param>
        /// <returns></returns>
        public static double CalcRevenue(Article a, ArticleAction.ActionType at, int minBillableWords, int billableWords)
        {

            if (a.status == Article.ArticleStatus.Published)
            {

                switch (at)
                {

                    case ActionType.Suggested:
                        return 0;

                    case ActionType.Created:
                        return minBillableWords * 0.0000008;   // 80 satoshis por palavra no mínimo do artigo

                    case ActionType.Approved:
                        return 0;

                    case ActionType.Revised:
                        return billableWords * 0.0000008;   // 80 satoshis por palavra para revisão

                    case ActionType.IncludedNaration:
                        return billableWords * 0.0000008;   // 80 satoshis por palavra para narração

                    case ActionType.Produced:
                        return billableWords * 0.0000008;   // 80 satoshis por palavra para produção

                    case ActionType.Published:
                        return 0;

                    case ActionType.Removed:
                        return 0;

                }

            }

            return 0;

        }

        /// <summary>
        /// Texto
        /// </summary>
        public string text
        {

            get { return textInt; }
            set { textInt = value; }

        }

        /// <summary>
        /// Grants dessa ação
        /// </summary>
        public List<ArticleActionGrant> grants
        {

            get
            {

                if (grantsInt == null)
                    grantsInt = ArticleActionGrant.LoadAllFrom(this);

                return grantsInt;

            }

            set
            {

                grantsInt = value;

            }

        }
        
        /// <summary>
        /// Grants dessa ação
        /// </summary>
        public List<ArticleActionObservation> observations
        {

            get
            {

                if (observationsInt == null)
                    observationsInt = ArticleActionObservation.LoadAllFrom(this);

                return observationsInt;

            }

            set
            {

                observationsInt = value;

            }

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

            sel.CommandText = "SELECT userid, type, observation, date, show, billablewords, text FROM " + Base.conf.prefix + "[newsarticleaction] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                useridInt = rdr.GetString(0);
                typeInt = rdr.GetInt32(1);
                observationInt = rdr.GetString(2);
                dateInt = rdr.GetDateTime(3);
                showInt = rdr.GetInt32(4);
                billablewordsInt = rdr.GetInt32(5);
                textInt = rdr.GetString(6);

                dbassignedidInt = true;
                res = true;

            }

            rdr.Close();
            sel.Connection.Close();

            return res;

        }

        /// <summary>
        /// Salva a ticket
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();
            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticleaction] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsarticleaction] (id, articleid, userid, type, observation, date, show, billablewords, text) VALUES (@id, @articleid, @userid, @type, @observation, @date, @show, @billablewords, @text)";
                ins.Parameters.Add(new SqlParameter("@id", id));

                ins.Parameters.Add(new SqlParameter("@articleid", article.id));
                ins.Parameters.Add(new SqlParameter("@userid", useridInt));
                ins.Parameters.Add(new SqlParameter("@type", typeInt));
                ins.Parameters.Add(new SqlParameter("@observation", observationInt));
                ins.Parameters.Add(new SqlParameter("@date", dateInt));
                ins.Parameters.Add(new SqlParameter("@show", showInt));
                ins.Parameters.Add(new SqlParameter("@billablewords", billablewordsInt));
                ins.Parameters.Add(new SqlParameter("@text", textInt));

                ins.Connection = Base.conf.Open();
                ins.ExecuteNonQuery();
                ins.Connection.Close();

                res = true;
                dbassignedidInt = true;

            }
            else
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand upd = new SqlCommand();
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsarticleaction] SET articleid=@articleid, userid=@userid, type=@type, observation=@observation, date=@date, show=@show, billablewords=@billablewords, text=@text WHERE id=@id";

                upd.Parameters.Add(new SqlParameter("@articleid", article.id));
                upd.Parameters.Add(new SqlParameter("@userid", useridInt));
                upd.Parameters.Add(new SqlParameter("@type", typeInt));
                upd.Parameters.Add(new SqlParameter("@observation", observationInt));
                upd.Parameters.Add(new SqlParameter("@date", dateInt));
                upd.Parameters.Add(new SqlParameter("@show", showInt));
                upd.Parameters.Add(new SqlParameter("@billablewords", billablewordsInt));
                upd.Parameters.Add(new SqlParameter("@text", textInt));

                upd.Parameters.Add(new SqlParameter("@id", id));

                upd.Connection = Base.conf.Open();
                upd.ExecuteNonQuery();
                upd.Connection.Close();

                res = true;
                dbassignedidInt = true;

            }

            if (res)
            {

                if (grantsInt != null)
                {

                    ArticleActionGrant.SaveAll(this, grants);

                }

                if (observationsInt != null)
                {

                    ArticleActionObservation.SaveAll(this, observations);

                }

            }

            return res;

        }
        
        /// <summary>
        /// Salva a ticket
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {

            bool res = false;

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticleaction] WHERE id=@id";
            del.Parameters.Add(new SqlParameter("@id", id));

            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();
            del.Connection.Close();

            return res;

        }
        
        /// <summary>
        /// Carrega a pauta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static Article ArticleForArticleAction(string id)
        {

            string idArt = Base.conf.GetParentString(id, "newsarticleaction", "articleid");

            if (idArt != null)
            {

                Article art = Article.LoadArticle(idArt);
                return art;

            }

            return null;

        }

        /// <summary>
        /// Carrega o action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ArticleAction LoadArticleAction(string id)
        {

            Article art = ArticleForArticleAction(id);

            if (art != null)
            {

                return art.GetArticleAction(id);

            }

            return null;

        }

        /// <summary>
        /// Remove um grant
        /// </summary>
        /// <param name="awardId"></param>
        public void RemoveGrant(string awardId)
        {

            for (int i = 0; i < grants.Count; i++)
            {

                if (grants[i].awardId == awardId)
                {

                    profile.RemoveAward(awardId);

                    grants.RemoveAt(i);
                    i--;

                }

            }

        }

        /// <summary>
        /// Adiciona um grant pelo usuário
        /// </summary>
        /// <param name="awardId"></param>
        /// <param name="userId"></param>
        public void AddGrant(string awardId, string userId)
        {

            for (int i = 0; i < grants.Count; i++)
            {

                if (grants[i].awardId == awardId)
                {

                    return;

                }

            }

            ArticleActionGrant tag = new ArticleActionGrant(this);
            tag.awardId = awardId;
            tag.grantedById = userId;
            tag.granted = DateTime.Now;

            grants.Add(tag);

            profile.AddAward(awardId);

        }

        /// <summary>
        /// Calcula o número de arquivos de audio
        /// </summary>
        /// <returns></returns>
        public int CheckNumberAudioFiles()
        {

            string obs = observation;
            if (obs == "" || obs.Length < 10)
                return 0;

            if (obs.StartsWith("Inclui arquivos: "))
                obs = obs.Substring(17);

            string[] files = obs.Split(',');
            int n = 0;

            for (int i = 0; i < files.Length; i++)
            {

                string ext = "wav";
                if (files[i].Length > 3)
                    ext = files[i].Substring(files[i].Length - 3);

                string path = Base.conf.tempImageFilePath + "\\c-" + id + "-" + i.ToString("00") + "." + ext;

                if (File.Exists(path))
                    n++;

            }

            return n;

        }

        /// <summary>
        /// Pega o caminho do arquivo "n"
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string GetAttachFilePath(int n)
        {

            string obs = observation;
            if (obs == "" || obs.Length < 10 || n < 0)
                return "";

            if (obs.StartsWith("Inclui arquivos: "))
                obs = obs.Substring(17);

            string[] files = obs.Split(',');
            if (files.Length <= n)
                return "";

            string ext = "wav";
            if (files[n].Length > 3)
                ext = files[n].Substring(files[n].Length - 3);

            string path = Base.conf.tempImageFilePath + "\\c-" + id + "-" + n.ToString("00") + "." + ext;

            if (File.Exists(path))
                return path;

            return "";

        }

        /// <summary>
        /// Pega o nome do arquivo "n"
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string GetAttachFileName(int n)
        {

            string obs = observation;
            if (obs == "" || obs.Length < 10 || n < 0)
                return "";

            if (obs.StartsWith("Inclui arquivos: "))
                obs = obs.Substring(17);

            string[] files = obs.Split(',');
            if (files.Length <= n)
                return "";

            return files[n];

        }


        /// <summary>
        /// Pega o caminho do arquivo "n"
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string GetAttachFileType(int n)
        {

            string obs = observation;
            if (obs == "" || obs.Length < 10 || n < 0)
                return "";

            if (obs.StartsWith("Inclui arquivos: "))
                obs = obs.Substring(17);

            string[] files = obs.Split(',');
            if (files.Length <= n)
                return "";

            string ext = "wav";
            if (files[n].Length > 3)
                ext = files[n].Substring(files[n].Length - 3);

            if (ext == "wav")
                return "audio/wav";

            if (ext == "mp3")
                return "audio/mp3";

            if (ext == "mp4")
                return "audio/mp4";

            if (ext == "m4a")
                return "audio/m4a";

            if (ext == "ogg")
                return "audio/ogg";

            return "audio/wav";

        }

    }

}