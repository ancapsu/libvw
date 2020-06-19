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
    public class Target
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
        string textInt;

        /// <summary>
        /// Linguagem
        /// </summary>
        int languageInt;

        /// <summary>
        /// Situação
        /// </summary>
        int statusInt;

        /// <summary>
        /// Possíveis situações
        /// </summary>
        public enum NewsTargetStatus { Undefined, Created, Used, Removed }

        /// <summary>
        /// Link
        /// </summary>
        string linkInt;

        /// <summary>
        /// Liberado
        /// </summary>
        DateTime registeredInt;

        /// <summary>
        /// Índice de importância
        /// </summary>
        int importanceInt;

        /// <summary>
        /// Ações
        /// </summary>
        List<TargetAction> actionsInt;

        /// <summary>
        /// Votos
        /// </summary>
        List<TargetVote> votesInt;

        /// <summary>
        /// Categorias dessa notícia
        /// </summary>
        List<string> categoriesInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public Target()
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            titleInt = "";
            textInt = "";
            languageInt = 0;
            statusInt = 0;
            linkInt = "";
            registeredInt = DateTime.Now;
            importanceInt = 10000;

            actionsInt = null;
            votesInt = null;
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
        public string text
        {

            get { return textInt; }
            set { textInt = value; }

        }

        /// <summary>
        /// Situação
        /// </summary>
        public NewsTargetStatus status
        {

            get { return (NewsTargetStatus)statusInt; }
            set { statusInt = (int)value; }

        }

        /// <summary>
        /// Tags
        /// </summary>
        public string link
        {

            get { return linkInt; }
            set { linkInt = value; }

        }

        /// <summary>
        /// Liberado
        /// </summary>
        public DateTime registered
        {

            get { return registeredInt; }
            set { registeredInt = value; }

        }

        /// <summary>
        /// Importância
        /// </summary>
        public int importance
        {

            get { return importanceInt; }
            set { importanceInt = value; }

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
        public List<TargetAction> actions
        {

            get
            {

                if (actionsInt == null)
                    actionsInt = ActionForTarget();

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
        List<TargetAction> ActionForTarget()
        {

            List<string> ids = Base.conf.LoadStringList(id, "newstargetaction", "targetid", "id", "date");
            List<TargetAction> lst = new List<TargetAction>();

            for (int i = 0; i < ids.Count; i++)
            {

                TargetAction npc = new TargetAction(this);
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
        public TargetAction GetTargetAction(string id)
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
        public int GetPosition(TargetAction n)
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
        public TargetAction GetActionForType(TargetAction.ActionType t)
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

                TargetAction n = GetActionForType(TargetAction.ActionType.Suggested);
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

                TargetAction n = GetActionForType(TargetAction.ActionType.Suggested);
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
                TargetAction n = GetActionForType(TargetAction.ActionType.Suggested);

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
                TargetAction n = GetActionForType(TargetAction.ActionType.Suggested);

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
        /// Tem artigo associado?
        /// </summary>
        public Article associatedArticle
        {

            get { return Article.ArticleFromTarget(id); }

        }

        /// <summary>
        /// Votos
        /// </summary>
        public List<TargetVote> votes
        {

            get
            {

                if (votesInt == null)
                    votesInt = TargetVote.LoadAllFrom(this);

                return votesInt;

            }

            set { votesInt = value; }

        }

        /// <summary>
        /// Calcula os votos
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int CalcVotes(TargetVote.Vote t)
        {

            int count = 0;

            votesInt = null;

            for (int i = 0; i < votes.Count; i++)
            {

                if (votes[i].vote == t)
                    count++;

            }

            return count;

        }
        
        /// <summary>
        /// Votos por escrever
        /// </summary>
        public int voteWillWriteArticle
        {

            get { return CalcVotes(TargetVote.Vote.WillWriteArticle); }

        }

        /// <summary>
        /// Votos muito bom
        /// </summary>
        public int voteVeryGood
        {

            get { return CalcVotes(TargetVote.Vote.VeryGood); }

        }

        /// <summary>
        /// Votos bom
        /// </summary>
        public int voteGood
        {

            get { return CalcVotes(TargetVote.Vote.Good); }

        }

        /// <summary>
        /// Votos não interessante
        /// </summary>
        public int voteNotInteresting
        {

            get { return CalcVotes(TargetVote.Vote.NotInteresting); }

        }

        /// <summary>
        /// Votos notícia antiga
        /// </summary>
        public int voteOldNews
        {

            get { return CalcVotes(TargetVote.Vote.OldNews); }

        }

        /// <summary>
        /// Votos notícia falsa
        /// </summary>
        public int voteFalseFact
        {

            get { return CalcVotes(TargetVote.Vote.FalseFact); }

        }

        /// <summary>
        /// Pega o voto para o usuário
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public TargetVote.Vote GetUserVote(string userid)
        {

            for (int i = 0; i < votes.Count; i++)
                if (votes[i].userId == userid)
                    return votes[i].vote;

            return TargetVote.Vote.Undefined;

        }

        /// <summary>
        /// Pega o voto para o usuário
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool SetUserVote(string userid, TargetVote.Vote v)
        {

            TargetVote.Vote vo = GetUserVote(userid);
            if (vo == v) 
                v = TargetVote.Vote.Undefined;

            if (v == TargetVote.Vote.Undefined)
            {

                for (int i = 0; i < votes.Count; i++)
                {

                    if (votes[i].userId == userid)
                    {

                        votes.RemoveAt(i);
                        return false;

                    }

                }

            }
            else
            {

                for (int i = 0; i < votes.Count; i++)
                {

                    if (votes[i].userId == userid)
                    {
                        votes[i].vote = v;
                        votes[i].date = DateTime.Now;
                        return false;
                    }

                }

                TargetVote vote = new TargetVote(this);
                vote.userId = userid;
                vote.vote = v;
                vote.date = DateTime.Now;

                votes.Add(vote);

                return true;

            }

            return false;

        }

        /// <summary>
        /// Categorias dessa notícia
        /// </summary>
        public List<string> categories
        {

            get
            {

                if (categoriesInt == null)
                    categoriesInt = Base.conf.LoadStringList(id, "newstargetcategory", "id", "label");

                return categoriesInt;

            }

            set { categoriesInt = value; }

        }

        /// <summary>
        /// Normaliza a lista de categorias
        /// </summary>
        public void NormalizeMain()
        {

            List<string> rc = new List<string>();

            // Primeiro inclui a verdadeira!
            rc.Add("suggested");

            for (int i = 0; i < categories.Count; i++)
            {

                if (!Category.IsMain(categories[i]))
                    rc.Add(categories[i]);

            }

            categories = rc;

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Load(string id)
        {

            if (id == null)
                return false;

            bool res = false;

            SqlCommand sel = new SqlCommand();
            
            sel.CommandText = "SELECT link, lang, title, text, status, registered, importance FROM " + Base.conf.prefix + "[newstarget] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                linkInt = rdr.GetString(0);
                languageInt = rdr.GetInt32(1);
                titleInt = rdr.GetString(2);
                textInt = rdr.GetString(3);
                statusInt = rdr.GetInt32(4);
                registeredInt = rdr.GetDateTime(5);
                importanceInt = rdr.GetInt32(6);

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

            lock (RacLib.Base.conf)
            {

                SqlCommand sel = new SqlCommand();
                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE id=@id OR title=@title OR link=@link";
                sel.Parameters.Add(new SqlParameter("@id", id));
                sel.Parameters.Add(new SqlParameter("@title", title));
                sel.Parameters.Add(new SqlParameter("@link", link));

                sel.Connection = Base.conf.Open();
                SqlDataReader rdr = sel.ExecuteReader();

                if (!rdr.Read())
                {

                    rdr.Close();
                    sel.Connection.Close();

                    SqlCommand ins = new SqlCommand();
                    ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newstarget] (id, link, lang, title, text, status, registered, importance) VALUES (@id, @link, @lang, @title, @text, @status, @registered, @importance)";
                    ins.Parameters.Add(new SqlParameter("@id", id));

                    ins.Parameters.Add(new SqlParameter("@link", linkInt));
                    ins.Parameters.Add(new SqlParameter("@title", titleInt));
                    ins.Parameters.Add(new SqlParameter("@lang", languageInt));
                    ins.Parameters.Add(new SqlParameter("@text", textInt));
                    ins.Parameters.Add(new SqlParameter("@status", statusInt));
                    ins.Parameters.Add(new SqlParameter("@registered", registeredInt));
                    ins.Parameters.Add(new SqlParameter("@importance", importanceInt));

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
                    upd.CommandText = "UPDATE " + Base.conf.prefix + "[newstarget] SET link=@link, lang=@lang, title=@title, text=@text, status=@status, registered=@registered, importance=@importance WHERE id=@id";

                    upd.Parameters.Add(new SqlParameter("@link", linkInt));
                    upd.Parameters.Add(new SqlParameter("@lang", languageInt));
                    upd.Parameters.Add(new SqlParameter("@title", titleInt));
                    upd.Parameters.Add(new SqlParameter("@text", textInt));
                    upd.Parameters.Add(new SqlParameter("@status", statusInt));
                    upd.Parameters.Add(new SqlParameter("@registered", registeredInt));
                    upd.Parameters.Add(new SqlParameter("@importance", importanceInt));

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

                        foreach (TargetAction ta in actions) { ta.Save(); }

                    }

                    if (votesInt != null)
                    {

                        TargetVote.SaveAll(this, votes);

                    }

                    if (categoriesInt != null)
                    {

                        Base.conf.UpdateStringList(id, categories, "newstargetcategory", "id", "label");

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
        public static Target LoadTarget(string id)
        {

            Target n = new Target();
            if (n.Load(id))
                return n;

            return null;

        }
        
        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Target> GetLastTargets(int lang, string categ, bool onlyValid, int ini, int max, DateTime lastdate, string search = "")
        {

            List<string> ids = new List<string>();
            SqlCommand sel = new SqlCommand();

            if (categ == "" && !onlyValid)
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @last ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @last ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @last AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @last AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            else if (categ != "" && !onlyValid)
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE registered > @last AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered > @last AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE registered > @last AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered > @last AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            else if (categ == "" && onlyValid)
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE registered > @last AND status=1 ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered > @last AND status=1 ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE registered > @last AND status=1 AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=lang AND registered > @last AND status=1 AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            else if (categ != "" && onlyValid)
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE registered > @last AND status=1 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered > @last AND status=1 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE registered > @last AND status=1 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered > @last AND status=1 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, registered DESC";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@last", lastdate));
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

            List<Target> lst = new List<Target>();
            for(int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadTarget(ids[i]));

            }

            return lst;
            
        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetTotalLastTargets(int lang, string categ, bool onlyValid, DateTime dt, string search = "")
        {

            List<string> ids = new List<string>();
            SqlCommand sel = new SqlCommand();

            if (categ == "" && !onlyValid)
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @registered";
                        sel.Parameters.Add(new SqlParameter("@registered", dt));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @registered";
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @registered AND (title LIKE @search OR text LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @registered AND (title LIKE @search OR text LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            else if (categ != "" && !onlyValid)
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @registered AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@registered", dt));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @registered AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @registered AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @registered AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            else if (categ == "" && onlyValid)
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @registered AND status=1";
                        sel.Parameters.Add(new SqlParameter("@registered", dt));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @registered AND status=1";
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @registered AND status=1 AND (title LIKE @search OR text LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @registered AND status=1 AND (title LIKE @search OR text LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }

            }
            else if (categ != "" && onlyValid)
            {

                if (search == "")
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @registered AND status=1 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@registered", dt));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @registered AND status=1 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                    }

                }
                else
                {

                    if (lang < 2)
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE registered >= @registered AND status=1 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                    }
                    else
                    {

                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND registered >= @registered AND status=1 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newstargetcategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                        sel.Parameters.Add(new SqlParameter("@label", categ));
                        sel.Parameters.Add(new SqlParameter("@registered", dt));
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
        public static List<Target> GetLastTargetsForUser(string userid, int ini, int max, DateTime dt)
        {

            List<string> ids = new List<string>();
            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE id IN (SELECT targetid FROM " + Base.conf.prefix + "[newstargetaction] WHERE userid=@userid) AND registered >= @registered ORDER BY registered DESC";
            sel.Parameters.Add(new SqlParameter("@userid", userid));
            sel.Parameters.Add(new SqlParameter("@registered", dt));

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

            List<Target> lst = new List<Target>();
            for (int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadTarget(ids[i]));

            }

            return lst;

        }

        /// <summary>
        /// Pega o número
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetTotalLastTargetsForUser(string userid, DateTime date)
        {

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newstarget] WHERE id IN (SELECT targetid FROM " + Base.conf.prefix + "[newstargetaction] WHERE userid=@userid) AND registered >= @registered";
            sel.Parameters.Add(new SqlParameter("@userid", userid));
            sel.Parameters.Add(new SqlParameter("@registered", date));

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
        /// Já existe (mesmo link ou mesmo título)
        /// </summary>
        /// <param name="link"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool AlreadyExists(int lang, string link, string title)
        {

            SqlCommand sel = new SqlCommand();

            if (lang < 2)
            {

                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE link=@link OR title=@title";
                sel.Parameters.Add(new SqlParameter("@link", link));
                sel.Parameters.Add(new SqlParameter("@title", title));

            }
            else
            {

                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstarget] WHERE lang=@lang AND (link=@link OR title=@title)";
                sel.Parameters.Add(new SqlParameter("@link", link));
                sel.Parameters.Add(new SqlParameter("@title", title));
                sel.Parameters.Add(new SqlParameter("@lang", lang));

            }

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            bool res = false;

            if (rdr.Read())
                res = true;

            rdr.Close();
            sel.Connection.Close();

            return res;

        }

        /// <summary>
        /// Esse usuário já mexeu nessa pauta?
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool HasAlreadyRevised(string userid)
        {

            bool res = false;

            for (int i = 0; i < actions.Count; i++)
                if (actions[i].userId == userid)
                    res = true;

            return res;

        }

    }

}