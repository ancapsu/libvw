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
    public class Article
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
        /// Situação (do texto)
        /// </summary>
        int statusInt;

        /// <summary>
        /// Possíveis situações do texto
        /// </summary>
        public enum ArticleStatus
        {
            Undefined,
            Created, // Escrito, Precisa ser aprovado
            Approved, // Aprovado, Precisa ser revisado
            Revised, // Revisado, Precisa ser narrado
            Ready, // Pronto, Precisa ser produzido
            Produced, // Produzido, Precisa ser publicado
            Published, // Publicado, fim da linha
            Removed
        } // Removido

        /// <summary>
        /// Situação (da narração)
        /// </summary>
        int narrationstatusInt;

        /// <summary>
        /// Possíveis situações
        /// </summary>
        public enum ArticleNarrationStatus { Undefined, Created, Approved, Removed }

        /// <summary>
        /// Situação
        /// </summary>
        int typeInt;

        /// <summary>
        /// Possíveis situações
        /// </summary>
        public enum ArticleType { Undefined, ShortNote, Note, Article, Chronicle, Interview, Script }

        /// <summary>
        /// Linguagem
        /// </summary>
        int languageInt;

        /// <summary>
        /// Pauta de onde originou
        /// </summary>
        string targetidInt;

        /// <summary>
        /// Titulo da matéria
        /// </summary>
        string titleInt;

        /// <summary>
        /// Chamada da matéria
        /// </summary>
        string hookInt;

        /// <summary>
        /// Texto completo
        /// </summary>
        string textInt;

        /// <summary>
        /// Liberado
        /// </summary>
        DateTime releasedInt;

        /// <summary>
        /// Última mudança
        /// </summary>
        DateTime lastchangedInt;

        /// <summary>
        /// Revisor preferencial
        /// </summary>
        string preferredrevisorInt;

        /// <summary>
        /// Narrador preferencial
        /// </summary>
        string preferrednarratorInt;

        /// <summary>
        /// Produtor preferencia
        /// </summary>
        string preferredproducerInt;

        /// <summary>
        /// Está sendo revisado
        /// </summary>
        string beingrevisedInt;

        /// <summary>
        /// Está sendo narrado
        /// </summary>
        string beingnarratedInt;

        /// <summary>
        /// Está sendo revisado
        /// </summary>
        string beingproducedInt;

        /// <summary>
        /// Narração escolhida
        /// </summary>
        string finalnarratorInt;

        /// <summary>
        /// Importância
        /// </summary>
        int importanceInt;

        /// <summary>
        /// Importância para tradução
        /// </summary>
        int translationimportanceInt;

        /// <summary>
        /// Ações
        /// </summary>
        List<ArticleAction> actionsInt;

        /// <summary>
        /// Comentários
        /// </summary>
        List<ArticleComment> commentsInt;

        /// <summary>
        /// Links
        /// </summary>
        List<ArticleLink> linksInt;

        /// <summary>
        /// Votos
        /// </summary>
        List<ArticleVote> votesInt;

        /// <summary>
        /// Categorias dessa notícia
        /// </summary>
        List<string> categoriesInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public Article()
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            statusInt = 0;
            narrationstatusInt = 0;
            typeInt = 0;
            targetidInt = null;
            languageInt = 0;
            hookInt = "";
            titleInt = "";
            textInt = "";
            releasedInt = RacMsg.NullDateTime;
            lastchangedInt = RacMsg.NullDateTime;

            preferredrevisorInt = "";
            preferrednarratorInt = "";
            preferredproducerInt = "";

            beingrevisedInt = "";
            beingnarratedInt = "";
            beingproducedInt = "";

            finalnarratorInt = "";

            actionsInt = null;
            linksInt = null;
            categoriesInt = null;
            votesInt = null;
            commentsInt = null;

            importanceInt = 10000;
            translationimportanceInt = 10000;

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
        /// Situação da notícia
        /// </summary>
        public ArticleStatus status
        {

            get { return (ArticleStatus)statusInt; }
            set { statusInt = (int)value; }

        }

        /// <summary>
        /// Situação (da narração)
        /// </summary>
        public ArticleNarrationStatus statusNarration
        {

            get { return (ArticleNarrationStatus)narrationstatusInt; }
            set { narrationstatusInt = (int)value; }

        }

        /// <summary>
        /// Possíveis situações
        /// </summary>
        public ArticleType type
        {

            get { return (ArticleType)typeInt; }
            set { typeInt = (int)value; }

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
        /// Pauta de onde originou (pode ser null)
        /// </summary>
        public string targetId
        {

            get { return targetidInt; }
            set { targetidInt = value; }

        }

        /// <summary>
        /// Pauta de onde originou (pode ser null)
        /// </summary>
        public Target target
        {

            get
            {

                return Target.LoadTarget(targetId);

            }

            set
            {

                if (value == null)
                    targetId = null;
                else
                    targetId = value.id;

            }

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
        /// Importância para tradução
        /// </summary>
        public int translationImportance
        {

            get { return translationimportanceInt; }
            set { translationimportanceInt = value; }

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
        /// Titulo da matéria
        /// </summary>
        public string title
        {

            get { return titleInt; }
            set { titleInt = value; }

        }

        /// <summary>
        /// Chamada da matéria
        /// </summary>
        string hook
        {

            get { return hookInt; }
            set { hookInt = value; }

        }

        /// <summary>
        /// Texto completo
        /// </summary>
        public string text
        {

            get { return textInt; }
            set { textInt = value; }

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
        /// Última mudança
        /// </summary>
        public DateTime lastChanged
        {

            get { return lastchangedInt; }
            set { lastchangedInt = value; }

        }

        /// <summary>
        /// Revisor preferencial
        /// </summary>
        public string preferredRevisorId
        {

            get { return preferredrevisorInt; }
            set { preferredrevisorInt = value; }

        }

        /// <summary>
        /// Narrador preferencial
        /// </summary>
        public string preferredNarratorId
        {

            get { return preferrednarratorInt; }
            set { preferrednarratorInt = value; }

        }

        /// <summary>
        /// Produtor preferencia
        /// </summary>
        public string preferredProducerId
        {

            get { return preferredproducerInt; }
            set { preferredproducerInt = value; }

        }

        /// <summary>
        /// Revisor preferencial
        /// </summary>
        public string preferredRevisorName
        {

            get { return BaseUserSource.source.GetName(preferredRevisorId); }

        }

        /// <summary>
        /// Narrador preferencial
        /// </summary>
        public string preferredNarratorName
        {

            get { return BaseUserSource.source.GetName(preferredNarratorId); }

        }

        /// <summary>
        /// Produtor preferencia
        /// </summary>
        public string preferredProducerName
        {

            get { return BaseUserSource.source.GetName(preferredProducerId); }

        }

        /// <summary>
        /// Está sendo revisado
        /// </summary>
        public string beingRevisedId
        {

            get { return beingrevisedInt; }
            set { beingrevisedInt = value; }

        }

        /// <summary>
        /// Está sendo narrado
        /// </summary>
        public string beingNarratedId
        {

            get { return beingnarratedInt; }
            set { beingnarratedInt = value; }

        }

        /// <summary>
        /// Está sendo revisado
        /// </summary>
        public string beingProducedId
        {

            get { return beingproducedInt; }
            set { beingproducedInt = value; }

        }

        /// <summary>
        /// Está sendo revisado
        /// </summary>
        public string beingRevisedName
        {

            get { return BaseUserSource.source.GetName(beingRevisedId); }

        }

        /// <summary>
        /// Está sendo narrado
        /// </summary>
        public string beingNarratedName
        {

            get { return BaseUserSource.source.GetName(beingNarratedId); }

        }

        /// <summary>
        /// Está sendo revisado
        /// </summary>
        public string beingProducedName
        {

            get { return BaseUserSource.source.GetName(beingProducedId); }

        }

        /// <summary>
        /// Narrador escolhido
        /// </summary>
        public string finalNarratorId
        {

            get { return finalnarratorInt; }
            set { finalnarratorInt = value; }

        }

        /// <summary>
        /// Está sendo revisado
        /// </summary>
        public string finalNarratorName
        {

            get { return BaseUserSource.source.GetName(finalNarratorId); }

        }

        /// <summary>
        /// Ações
        /// </summary>
        public List<ArticleAction> actions
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
        List<ArticleAction> ActionForNews()
        {

            List<string> ids = Base.conf.LoadStringList(id, "newsarticleaction", "articleid", "id", "date");
            List<ArticleAction> lst = new List<ArticleAction>();

            for (int i = 0; i < ids.Count; i++)
            {

                ArticleAction npc = new ArticleAction(this);
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
        public ArticleAction GetArticleAction(string id)
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
        public int GetPosition(ArticleAction n)
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
        public ArticleAction GetActionForType(ArticleAction.ActionType t, string userId = "")
        {

            if (userId == "")
            {

                for (int i = 0; i < actions.Count; i++)
                    if (actions[i].type == t)
                        return actions[i];

            }
            else
            {
                
                for (int i = 0; i < actions.Count; i++)
                    if (actions[i].type == t && actions[i].userId == userId)
                        return actions[i];

            }

            return null;

        }

        /// <summary>
        /// Pega as ações específicas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ArticleAction> GetActionsForType(ArticleAction.ActionType t)
        {

            List<ArticleAction> lst = new List<ArticleAction>();

            for (int i = 0; i < actions.Count; i++)
                if (actions[i].type == t)
                    lst.Add(actions[i]);

            return lst;

        }

        /// <summary>
        /// Comentários
        /// </summary>
        public List<ArticleComment> comments
        {

            get
            {

                if (commentsInt == null)
                    commentsInt = CommentsForArticle();

                return commentsInt;

            }

            set
            {

                commentsInt = value;

            }

        }

        /// <summary>
        /// Carrega a lista de classes de licença para este sistema
        /// </summary>
        /// <returns></returns>
        List<ArticleComment> CommentsForArticle()
        {

            List<string> ids = Base.conf.LoadStringList(id, "newsarticlecomment", "articleid", "id", "date");
            List<ArticleComment> lst = new List<ArticleComment>();

            for (int i = 0; i < ids.Count; i++)
            {

                ArticleComment npc = new ArticleComment(this);
                if (npc.Load(ids[i]))
                    lst.Add(npc);

            }

            return lst;

        }

        /// <summary>
        /// Pega o comentário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ArticleComment GetArticleComment(string id)
        {

            for (int i = 0; i < comments.Count; i++)
                if (comments[i].id == id)
                    return comments[i];

            return null;

        }

        /// <summary>
        /// Remove o comentário da list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void RemoveArticleComment(string id)
        {

            for (int i = 0; i < comments.Count; i++)
            {

                if (comments[i].id == id)
                {

                    comments.RemoveAt(i);
                    return;

                }

            }

        }

        /// <summary>
        /// Posição do call
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int GetPosition(ArticleComment n)
        {

            for (int i = 0; i < comments.Count; i++)
                if (comments[i].id == n.id)
                    return i;

            return -1;

        }

        /// <summary>
        /// Votos
        /// </summary>
        public List<ArticleVote> votes
        {

            get
            {

                if (votesInt == null)
                    votesInt = ArticleVote.LoadAllFrom(this);

                return votesInt;

            }

            set { votesInt = value; }

        }

        /// <summary>
        /// Calcula os votos
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int CalcVotes(ArticleVote.Vote t)
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
        /// Votos aprovado
        /// </summary>
        public int voteApproved
        {

            get { return CalcVotes(ArticleVote.Vote.Approved); }

        }

        /// <summary>
        /// Votos não gostei
        /// </summary>
        public int voteDontLike
        {

            get { return CalcVotes(ArticleVote.Vote.DontLike); }

        }

        /// <summary>
        /// Votos já escrito
        /// </summary>
        public int voteAlreadyWritten
        {

            get { return CalcVotes(ArticleVote.Vote.AlreadyWritten); }

        }

        /// <summary>
        /// Votos poorly written
        /// </summary>
        public int votePoorlyWritten
        {

            get { return CalcVotes(ArticleVote.Vote.PoorlyWritten); }

        }

        /// <summary>
        /// Pega o voto para o usuário
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ArticleVote.Vote GetUserVote(string userid)
        {

            for (int i = 0; i < votes.Count; i++)
                if (votes[i].userId == userid)
                    return votes[i].vote;

            return ArticleVote.Vote.Undefined;

        }

        /// <summary>
        /// Pega o voto para o usuário
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool SetUserVote(string userid, ArticleVote.Vote v)
        {

            ArticleVote.Vote vo = GetUserVote(userid);
            if (vo == v)
                v = ArticleVote.Vote.Undefined;

            if (v == ArticleVote.Vote.Undefined)
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

                ArticleVote vote = new ArticleVote(this);
                vote.userId = userid;
                vote.vote = v;
                vote.date = DateTime.Now;

                votes.Add(vote);

                return true;

            }

            return false;

        }

        /// <summary>
        /// Autor
        /// </summary>
        public string authored
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.Created);
                if (n != null && n.show)
                    return n.userName;
                else
                    return "N/D";

            }

        }

        public string authoredId
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.Created);
                if (n != null && n.show)
                    return n.userId;
                else
                    return "";

            }

        }

        public string suggested
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.Suggested);
                if (n != null && n.show)
                    return n.userName;
                else
                    return "N/D";

            }

        }

        public string suggestedId
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.Suggested);
                if (n != null && n.show)
                    return n.userId;
                else
                    return "";

            }

        }

        public string revised
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.Revised);
                if (n != null && n.show)
                    return n.userName;
                else
                    return "N/D";

            }

        }

        public string revisedId
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.Revised);
                if (n != null && n.show)
                    return n.userId;
                else
                    return "";

            }

        }

        public string narrated
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.IncludedNaration, finalNarratorId);
                if (n != null && n.show)
                    return n.userName;
                else
                    return "N/D";

            }

        }

        public string narratedId
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.IncludedNaration, finalNarratorId);
                if (n != null && n.show)
                    return n.userId;
                else
                    return "";

            }

        }

        public string produced
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.Produced);
                if (n != null && n.show)
                    return n.userName;
                else
                    return "N/D";

            }

        }

        public string producedId
        {

            get
            {

                ArticleAction n = GetActionForType(ArticleAction.ActionType.Produced);
                if (n != null && n.show)
                    return n.userId;
                else
                    return "";

            }

        }

        /// <summary>
        /// Ações
        /// </summary>
        public List<ArticleLink> links
        {

            get
            {

                if (linksInt == null)
                    linksInt = ArticleLink.LoadAllFrom(this);

                return linksInt;

            }

            set
            {

                linksInt = value;

            }

        }

        /// <summary>
        /// Posição do call
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int GetPosition(ArticleLink n)
        {

            for (int i = 0; i < links.Count; i++)
                if (links[i] == n)
                    return i;

            return -1;

        }

        /// <summary>
        /// Normaliza a lista de categorias
        /// </summary>
        public void NormalizeMain()
        {

            List<string> rc = new List<string>();

            // Primeiro inclui a verdadeira!

            if (type == ArticleType.ShortNote || type == ArticleType.Note || type == ArticleType.Article)
            {

                if (textSizeType == TextSizeType.TooShort || textSizeType == TextSizeType.ShortNote)
                    rc.Add("shortnote");
                else if (textSizeType == TextSizeType.Note)
                    rc.Add("note");
                else if (textSizeType == TextSizeType.Article || textSizeType == TextSizeType.TooLong)
                    rc.Add("article");

            }
            else if (type == ArticleType.Script)
            {

                rc.Add("script");

            }
            else if (type == ArticleType.Chronicle)
            {

                rc.Add("chronicle");

            }
            else if (type == ArticleType.Interview)
            {

                rc.Add("interview");

            }

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

                    categoriesInt = Base.conf.LoadStringList(id, "newsarticlecategory", "id", "label");
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

            sel.CommandText = "SELECT status, narrationstatus, type, targetid, lang, title, hook, text, released, lastchanged, preferredrevisor, preferrednarrator, preferredproducer, beingrevised, beingnarrated, beingproduced, finalnarrator, importance, translationimportance FROM " + Base.conf.prefix + "[newsarticle] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                statusInt = rdr.GetInt32(0);
                narrationstatusInt = rdr.GetInt32(1);
                typeInt = rdr.GetInt32(2);
                targetidInt = rdr[3].ToString() == "" ? null : rdr.GetString(3);
                languageInt = rdr.GetInt32(4);
                titleInt = rdr.GetString(5);
                hookInt = rdr.GetString(6);
                textInt = rdr.GetString(7);
                releasedInt = rdr.GetDateTime(8);
                lastchangedInt = rdr.GetDateTime(9);
                preferredrevisorInt = rdr.GetString(10);
                preferrednarratorInt = rdr.GetString(11);
                preferredproducerInt = rdr.GetString(12);
                beingrevisedInt = rdr.GetString(13);
                beingnarratedInt = rdr.GetString(14);
                beingproducedInt = rdr.GetString(15);
                finalnarratorInt = rdr.GetString(16);
                importanceInt = rdr.GetInt32(17);
                translationimportanceInt = rdr.GetInt32(18); 

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
                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE id=@id OR title=@title";
                sel.Parameters.Add(new SqlParameter("@id", id));
                sel.Parameters.Add(new SqlParameter("@title", title));

                sel.Connection = Base.conf.Open();
                SqlDataReader rdr = sel.ExecuteReader();

                if (!rdr.Read())
                {

                    rdr.Close();
                    sel.Connection.Close();

                    SqlCommand ins = new SqlCommand();
                    ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsarticle] (id, targetid, lang, title, hook, text, status, narrationstatus, type, released, lastchanged, preferredrevisor, preferrednarrator, preferredproducer, beingrevised, beingnarrated, beingproduced, finalnarrator, importance, translationimportance) VALUES (@id, @targetid, @lang, @title, @hook, @text, @status, @narrationstatus, @type, @released, @lastchanged, @preferredrevisor, @preferrednarrator, @preferredproducer, @beingrevised, @beingnarrated, @beingproduced, @finalnarrator, @importance, @translationimportance)";
                    ins.Parameters.Add(new SqlParameter("@id", id));

                    if (targetidInt != null && targetidInt != "" && targetidInt != "0")
                        ins.Parameters.Add(new SqlParameter("@targetid", targetidInt));
                    else
                        ins.Parameters.Add(new SqlParameter("@targetid", DBNull.Value));

                    ins.Parameters.Add(new SqlParameter("@lang", languageInt));
                    ins.Parameters.Add(new SqlParameter("@title", titleInt));
                    ins.Parameters.Add(new SqlParameter("@hook", hookInt));
                    ins.Parameters.Add(new SqlParameter("@text", textInt));
                    ins.Parameters.Add(new SqlParameter("@status", statusInt));
                    ins.Parameters.Add(new SqlParameter("@narrationstatus", narrationstatusInt));
                    ins.Parameters.Add(new SqlParameter("@type", typeInt));
                    ins.Parameters.Add(new SqlParameter("@released", releasedInt));
                    ins.Parameters.Add(new SqlParameter("@lastchanged", lastchangedInt));
                    ins.Parameters.Add(new SqlParameter("@preferredrevisor", preferredrevisorInt));
                    ins.Parameters.Add(new SqlParameter("@preferrednarrator", preferrednarratorInt));
                    ins.Parameters.Add(new SqlParameter("@preferredproducer", preferredproducerInt));
                    ins.Parameters.Add(new SqlParameter("@beingrevised", beingrevisedInt));
                    ins.Parameters.Add(new SqlParameter("@beingnarrated", beingnarratedInt));
                    ins.Parameters.Add(new SqlParameter("@beingproduced", beingproducedInt));
                    ins.Parameters.Add(new SqlParameter("@finalnarrator", finalnarratorInt));
                    ins.Parameters.Add(new SqlParameter("@importance", importanceInt));
                    ins.Parameters.Add(new SqlParameter("@translationimportance", translationimportanceInt));

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
                    upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsarticle] SET targetid=@targetid, lang=@lang, title=@title, hook=@hook, text=@text, status=@status, narrationstatus=@narrationstatus, type=@type, released=@released, lastchanged=@lastchanged, preferredrevisor=@preferredrevisor, preferrednarrator=@preferrednarrator, preferredproducer=@preferredproducer, beingrevised=@beingrevised, beingnarrated=@beingnarrated, beingproduced=@beingproduced, finalnarrator=@finalnarrator, importance=@importance, translationimportance=@translationimportance WHERE id=@id";

                    if (targetidInt != null && targetidInt != "" && targetidInt != "0")
                        upd.Parameters.Add(new SqlParameter("@targetid", targetidInt));
                    else
                        upd.Parameters.Add(new SqlParameter("@targetid", DBNull.Value));

                    upd.Parameters.Add(new SqlParameter("@lang", languageInt));
                    upd.Parameters.Add(new SqlParameter("@title", titleInt));
                    upd.Parameters.Add(new SqlParameter("@hook", hookInt));
                    upd.Parameters.Add(new SqlParameter("@text", textInt));
                    upd.Parameters.Add(new SqlParameter("@status", statusInt));
                    upd.Parameters.Add(new SqlParameter("@narrationstatus", narrationstatusInt));
                    upd.Parameters.Add(new SqlParameter("@type", typeInt));
                    upd.Parameters.Add(new SqlParameter("@released", releasedInt));
                    upd.Parameters.Add(new SqlParameter("@lastchanged", lastchangedInt));
                    upd.Parameters.Add(new SqlParameter("@preferredrevisor", preferredrevisorInt));
                    upd.Parameters.Add(new SqlParameter("@preferrednarrator", preferrednarratorInt));
                    upd.Parameters.Add(new SqlParameter("@preferredproducer", preferredproducerInt));
                    upd.Parameters.Add(new SqlParameter("@beingrevised", beingrevisedInt));
                    upd.Parameters.Add(new SqlParameter("@beingnarrated", beingnarratedInt));
                    upd.Parameters.Add(new SqlParameter("@beingproduced", beingproducedInt));
                    upd.Parameters.Add(new SqlParameter("@finalnarrator", finalnarratorInt));
                    upd.Parameters.Add(new SqlParameter("@importance", importanceInt));
                    upd.Parameters.Add(new SqlParameter("@translationimportance", translationimportanceInt));

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

                        foreach (ArticleAction ta in actions) { ta.Save(); }

                    }

                    if (commentsInt != null)
                    {

                        foreach (ArticleComment ac in comments) { ac.Save(); }

                    }

                    if (votesInt != null)
                    {

                        ArticleVote.SaveAll(this, votes);

                    }

                    if (categoriesInt != null)
                    {

                        Base.conf.UpdateStringList(id, categories, "newsarticlecategory", "id", "label");

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
            del0.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticleactiongrant] WHERE id IN (SELECT id from " + Base.conf.prefix + "[newstargetaction] WHERE articleid=@id)";
            del0.Parameters.Add(new SqlParameter("@id", id));
            del0.Connection = Base.conf.Open();
            del0.ExecuteNonQuery();
            del0.Connection.Close();

            // Apaga todas as ações

            SqlCommand del1 = new SqlCommand();
            del1.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticleaction] WHERE articleid=@id";
            del1.Parameters.Add(new SqlParameter("@id", id));
            del1.Connection = Base.conf.Open();
            del1.ExecuteNonQuery();
            del1.Connection.Close();

            // Apaga todas as categorias

            SqlCommand del2 = new SqlCommand();
            del2.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE id=@id";
            del2.Parameters.Add(new SqlParameter("@id", id));
            del2.Connection = Base.conf.Open();
            del2.ExecuteNonQuery();
            del2.Connection.Close();

            // Apaga o target em si

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticle] WHERE id=@id";
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
        public static Article LoadArticle(string id)
        {

            Article n = new Article();
            if (n.Load(id))
                return n;

            return null;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Article> GetLastArticles(int lang, string userid, string category, int ini, int max, int sts, string search = "")
        {

            List<string> ids = new List<string>();

            SqlCommand sel = new SqlCommand();

            RacLib.BaseUser usr = RacLib.BaseUserSource.source.LoadUser(userid);
            if (usr != null && usr.isInternal && sts == 0)
            {

                if (category == "")
                {

                    if (search == "")
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] ORDER BY importance DESC, released DESC";

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }
                    else
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }

                }
                else if (category != "")
                {

                    if (search == "")
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }
                    else
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }

                }

            }
            else if (sts == 0)
            {

                if (usr == null)
                    userid = "";

                if (category == "" && userid == "")
                {

                    if (search == "")
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 ORDER BY importance DESC, released DESC";

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }
                    else
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=lang AND status=6 AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }

                }
                else if (category != "" && userid == "")
                {

                    if (search == "")
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }
                    else
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }

                }
                else if (category == "" && userid != "")
                {

                    if (search == "")
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@userid", userid));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@userid", userid));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }
                    else
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@userid", userid));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@userid", userid));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }

                }
                else if (category != "" && userid != "")
                {

                    if (search == "")
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE (status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)) AND (id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@userid", userid));
                            sel.Parameters.Add(new SqlParameter("@label", category));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND (status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)) AND (id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@userid", userid));
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }
                    else
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE (status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)) AND (id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@userid", userid));
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND (status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)) AND (id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@userid", userid));
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }

                }

            }
            else
            {

                if (usr == null)
                    userid = "";

                if (category == "")
                {

                    if (sts == 2)
                    {

                        if (search == "")
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=2 AND (lastchanged < @datelimit OR preferredrevisor=@preferred OR preferredrevisor='') ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=2 AND (lastchanged < @datelimit OR preferredrevisor=@preferred OR preferredrevisor='') ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }
                        else
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=2 AND (lastchanged < @datelimit OR preferredrevisor=@preferred OR preferredrevisor='') AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=2 AND (lastchanged < @datelimit OR preferredrevisor=@preferred OR preferredrevisor='') AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }

                    }
                    else if (sts == 3)
                    {

                        if (search == "")
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=3 AND (lastchanged < @datelimit OR preferrednarrator=@preferred OR preferrednarrator='') ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=3 AND (lastchanged < @datelimit OR preferrednarrator=@preferred OR preferrednarrator='') ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }
                        else
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=3 AND (lastchanged < @datelimit OR preferrednarrator=@preferred OR preferrednarrator='') AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=3 AND (lastchanged < @datelimit OR preferrednarrator=@preferred OR preferrednarrator='') AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }

                    }
                    else if (sts == 4)
                    {

                        if (search == "")
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=4 AND (lastchanged < @datelimit OR preferredproducer=@preferred OR preferredproducer='') ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=4 AND (lastchanged < @datelimit OR preferredproducer=@preferred OR preferredproducer='') ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }
                        else
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=4 AND (lastchanged < @datelimit OR preferredproducer=@preferred OR preferredproducer='') AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=4 AND (lastchanged < @datelimit OR preferredproducer=@preferred OR preferredproducer='') AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }

                    }
                    else if (sts == 5)
                    {

                        if (search == "")
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=5 ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=5 ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }
                        else
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=5 AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=5 AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                sel.Parameters.Add(new SqlParameter("@preferred", userid));
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

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@status", sts));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@status", sts));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }
                        else
                        {

                            if (lang < 2)
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@status", sts));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                            }
                            else
                            {

                                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                                sel.Parameters.Add(new SqlParameter("@status", sts));
                                sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                sel.Parameters.Add(new SqlParameter("@lang", lang));

                            }

                        }

                    }

                }
                else
                {

                    if (search == "")
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@status", sts));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@status", sts));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }
                    else
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@status", sts));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                        }
                        else
                        {

                            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search) ORDER BY importance DESC, released DESC";
                            sel.Parameters.Add(new SqlParameter("@label", category));
                            sel.Parameters.Add(new SqlParameter("@status", sts));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

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
                else if (count >= ini + max)
                    break;

                count++;

            }

            rdr.Close();
            sel.Connection.Close();

            List<Article> lst = new List<Article>();
            for (int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadArticle(ids[i]));

            }

            return lst;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Article> GetLastArticlesForTranslation(int lang, int ini, int max)
        {

            List<string> ids = new List<string>();

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE status >= @status_min AND status <= @status_max AND lang <> @lang ORDER BY translationimportance DESC, released DESC";
            sel.Parameters.Add(new SqlParameter("@status_min", 3));
            sel.Parameters.Add(new SqlParameter("@status_max", 7));
            sel.Parameters.Add(new SqlParameter("@lang", lang));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            int count = 0;

            while (rdr.Read())
            {

                if (count >= ini && count < ini + max)
                    ids.Add(rdr.GetString(0));
                else if (count >= ini + max)
                    break;

                count++;

            }

            rdr.Close();
            sel.Connection.Close();

            List<Article> lst = new List<Article>();
            for (int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadArticle(ids[i]));

            }

            return lst;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetTotalLastArticles(int lang, string userid, string category, int sts, string search = "")
        {

            try
            {

                List<string> ids = new List<string>();

                SqlCommand sel = new SqlCommand();

                if (userid == "0")
                {

                    if (search == "")
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status";
                            sel.Parameters.Add(new SqlParameter("@status", sts));

                        }
                        else
                        {

                            sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status";
                            sel.Parameters.Add(new SqlParameter("@status", sts));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }
                    else
                    {

                        if (lang < 2)
                        {

                            sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status AND (title LIKE @search OR text LIKE @search)";
                            sel.Parameters.Add(new SqlParameter("@status", sts));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                        }
                        else
                        {

                            sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status AND (title LIKE @search OR text LIKE @search)";
                            sel.Parameters.Add(new SqlParameter("@status", sts));
                            sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                            sel.Parameters.Add(new SqlParameter("@lang", lang));

                        }

                    }

                }
                else
                {

                    RacLib.BaseUser usr = RacLib.BaseUserSource.source.LoadUser(userid);
                    if (usr != null && usr.isInternal && sts == 0)
                    {

                        if (category == "")
                        {

                            if (search == "")
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle]";

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang";
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }
                            else
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }

                        }
                        else if (category != "")
                        {

                            if (search == "")
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }
                            else
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }

                        }

                    }
                    else if (sts == 0)
                    {

                        if (usr == null)
                            userid = "";

                        if (category == "" && userid == "")
                        {

                            if (search == "")
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6";

                                }
                                else
                                {
                                    
                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6";
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }
                            else
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }


                        }
                        else if (category != "" && userid == "")
                        {

                            if (search == "")
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }
                            else
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }

                        }
                        else if (category == "" && userid != "")
                        {

                            if (search == "")
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)";
                                    sel.Parameters.Add(new SqlParameter("@userid", userid));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)";
                                    sel.Parameters.Add(new SqlParameter("@userid", userid));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }
                            else
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@userid", userid));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@userid", userid));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }

                        }
                        else if (category != "" && userid != "")
                        {

                            if (search == "")
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE (status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)) AND (id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label))";
                                    sel.Parameters.Add(new SqlParameter("@userid", userid));
                                    sel.Parameters.Add(new SqlParameter("@label", category));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND (status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)) AND (id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label))";
                                    sel.Parameters.Add(new SqlParameter("@userid", userid));
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }
                            else
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE (status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)) AND (id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@userid", userid));
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND (status=6 OR id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)) AND (id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@userid", userid));
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }

                        }

                    }
                    else
                    {

                        if (usr == null)
                            userid = "";

                        if (category == "")
                        {

                            if (sts == 2)
                            {

                                if (search == "")
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=2 AND (lastchanged < @datelimit OR preferredrevisor=@preferred OR preferredrevisor='')";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=2 AND (lastchanged < @datelimit OR preferredrevisor=@preferred OR preferredrevisor='')";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }
                                else
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=2 AND (lastchanged < @datelimit OR preferredrevisor=@preferred OR preferredrevisor='') AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=2 AND (lastchanged < @datelimit OR preferredrevisor=@preferred OR preferredrevisor='') AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }

                            }
                            else if (sts == 3)
                            {

                                if (search == "")
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=3 AND (lastchanged < @datelimit OR preferrednarrator=@preferred OR preferrednarrator='')";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=3 AND (lastchanged < @datelimit OR preferrednarrator=@preferred OR preferrednarrator='')";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }
                                else
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=3 AND (lastchanged < @datelimit OR preferrednarrator=@preferred OR preferrednarrator='') AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=3 AND (lastchanged < @datelimit OR preferrednarrator=@preferred OR preferrednarrator='') AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }

                            }
                            else if (sts == 4)
                            {

                                if (search == "")
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=4 AND (lastchanged < @datelimit OR preferredproducer=@preferred OR preferredproducer='')";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=4 AND (lastchanged < @datelimit OR preferredproducer=@preferred OR preferredproducer='')";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }
                                else
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=4 AND (lastchanged < @datelimit OR preferredproducer=@preferred OR preferredproducer='') AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=4 AND (lastchanged < @datelimit OR preferredproducer=@preferred OR preferredproducer='') AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }

                            }
                            else if (sts == 5)
                            {

                                if (search == "")
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=5";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=5";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }
                                else
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=5 AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=5 AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@datelimit", DateTime.Now.AddDays(-1)));
                                        sel.Parameters.Add(new SqlParameter("@preferred", userid));
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

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status";
                                        sel.Parameters.Add(new SqlParameter("@status", sts));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status";
                                        sel.Parameters.Add(new SqlParameter("@status", sts));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }
                                else
                                {

                                    if (lang < 2)
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@status", sts));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                    }
                                    else
                                    {

                                        sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status AND (title LIKE @search OR text LIKE @search)";
                                        sel.Parameters.Add(new SqlParameter("@status", sts));
                                        sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                        sel.Parameters.Add(new SqlParameter("@lang", lang));

                                    }

                                }

                            }

                        }
                        else
                        {

                            if (search == "")
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@status", sts));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@status", sts));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }
                            else
                            {

                                if (lang < 2)
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status=@status AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@status", sts));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));

                                }
                                else
                                {

                                    sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE lang=@lang AND status=@status AND id IN (SELECT id FROM " + Base.conf.prefix + "[newsarticlecategory] WHERE label=@label) AND (title LIKE @search OR text LIKE @search)";
                                    sel.Parameters.Add(new SqlParameter("@label", category));
                                    sel.Parameters.Add(new SqlParameter("@status", sts));
                                    sel.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
                                    sel.Parameters.Add(new SqlParameter("@lang", lang));

                                }

                            }

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
            catch (Exception ex)
            {

                return 0;

            }

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetTotalArticlesForTranslation(int lang)
        {

            try
            {

                List<string> ids = new List<string>();

                SqlCommand sel = new SqlCommand();

                sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE status >= @status_min AND status <= @status_max AND lang <> @lang ORDER BY translationimportance DESC, released DESC";
                sel.Parameters.Add(new SqlParameter("@status_min", 3));
                sel.Parameters.Add(new SqlParameter("@status_max", 7));
                sel.Parameters.Add(new SqlParameter("@lang", lang));

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
            catch (Exception ex)
            {

                return 0;

            }

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Article> GetLastArticlesForUser(string userid, int ini, int max)
        {

            List<string> ids = new List<string>();

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid) ORDER BY released DESC";
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
                else if (count >= ini + max)
                    break;

                count++;

            }

            rdr.Close();
            sel.Connection.Close();

            List<Article> lst = new List<Article>();
            for (int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadArticle(ids[i]));

            }

            return lst;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetTotalArticlesForUser(string userid)
        {

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid)";
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

        /// <summary>
        /// Já existe (mesmo link ou mesmo título)
        /// </summary>
        /// <param name="link"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool AlreadyExists(string title)
        {

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE title=@title";
            sel.Parameters.Add(new SqlParameter("@title", title));

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
        /// Pega o número
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int NumberArticlesRegisteredSince(string userid, DateTime date)
        {

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[newsarticle] WHERE id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid AND date >= @date AND type = 1)";
            sel.Parameters.Add(new SqlParameter("@userid", userid));
            sel.Parameters.Add(new SqlParameter("@date", date));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            int n = 0;

            if (rdr.Read())
            {

                n = rdr.GetInt32(0);

            }

            rdr.Close();
            sel.Connection.Close();

            return n;

        }

        /// <summary>
        /// Quantas palavras tem?
        /// </summary>
        public int wordCount
        {

            get
            {

                int c = 0;

                if (Char.IsLetterOrDigit(text[0]))
                    c++;

                for (int i = 1; i < text.Length; i++)
                    if (Char.IsWhiteSpace(text[i - 1]) && Char.IsLetterOrDigit(text[i]))
                        c++;

                return c;

            }

        }

        public enum TextSizeType { Undefined, TooShort, ShortNote, Note, Article, TooLong };

        public TextSizeType textSizeType
        {

            get
            {

                int s = wordCount;

                if (s < 100)
                    return TextSizeType.TooShort;   // Curto de mais

                if (s < 350)
                    return TextSizeType.ShortNote;  // Tapa libertário

                if (s < 700)
                    return TextSizeType.Note;       // Nota... não usamos por hora

                if (s < 2000)
                    return TextSizeType.Article;    // Resumo libertario

                return TextSizeType.TooLong;

            }

        }

        /// <summary>
        /// Esse usuário já mexeu nesse artigo?
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

        /// <summary>
        /// Foi narrado pelo solicitado
        /// </summary>
        /// <returns></returns>
        public bool NarratedByPreferred()
        {

            for (int i = 0; i < actions.Count; i++)
                if (actions[i].type == ArticleAction.ActionType.IncludedNaration && actions[i].userId == preferredNarratorId)
                    return true;

            return false;

        }
        
        /// <summary>
        /// Foi narrado pelo solicitado
        /// </summary>
        /// <returns></returns>
        public bool NarratedByNarrator()
        {

            for (int i = 0; i < actions.Count; i++)
                if (actions[i].type == ArticleAction.ActionType.IncludedNaration && actions[i].profile.narrator)
                    return true;

            return false;

        }

        /// <summary>
        /// Tem pelo menos uma narração
        /// </summary>
        /// <returns></returns>
        public bool HasAtLeastNarration(int c)
        {

            int n = 0;

            for (int i = 0; i < actions.Count; i++)
            {

                if (actions[i].type == ArticleAction.ActionType.IncludedNaration)
                {

                    bool have = false;

                    for (int j = 0; j < i; j++)
                    {

                        if (actions[j].type == ArticleAction.ActionType.IncludedNaration)
                        {

                            if (actions[i].userId == actions[j].userId)
                                have = true;

                        }

                    }

                    if (!have)
                        n++;

                }

            }

            if (n >= c)
                return true;

            return false;

        }
        
        /// <summary>
        /// Pega o artigo dessa pauta
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public static Article ArticleFromTarget(string targetid)
        {

            Article art = null;
            
            SqlCommand sel = new SqlCommand();

            if (targetid != "")
            {

                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE targetid=@targetid";
                sel.Parameters.Add(new SqlParameter("@targetid", targetid));

                sel.Connection = Base.conf.Open();
                SqlDataReader rdr = sel.ExecuteReader();

                string id = "";

                if (rdr.Read())
                {

                    id = rdr.GetString(0);

                }

                rdr.Close();
                sel.Connection.Close();

                if (id != "")
                {

                    art = LoadArticle(id);

                }

            }

            return art;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Article> ListPublishedArticlesForInterval(string userid, DateTime start, DateTime end)
        {

            List<string> ids = new List<string>();

            SqlCommand sel = new SqlCommand();

            if (userid != "")
            {

                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE id IN (SELECT articleid FROM " + Base.conf.prefix + "[newsarticleaction] WHERE userid=@userid) AND released >= @mindate AND released < @maxdate AND status=6 ORDER BY released";
                sel.Parameters.Add(new SqlParameter("@userid", userid));
                sel.Parameters.Add(new SqlParameter("@mindate", start));
                sel.Parameters.Add(new SqlParameter("@maxdate", end));

            }
            else
            {

                sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticle] WHERE released >= @mindate AND released < @maxdate AND status=6 ORDER BY released";
                sel.Parameters.Add(new SqlParameter("@mindate", start));
                sel.Parameters.Add(new SqlParameter("@maxdate", end));

            }

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                ids.Add(rdr.GetString(0));
            
            }

            rdr.Close();
            sel.Connection.Close();

            List<Article> lst = new List<Article>();
            for (int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadArticle(ids[i]));

            }

            return lst;

        }

    }

}