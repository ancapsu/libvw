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
    public class ArticleComment
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
        /// Última mudança
        /// </summary>
        DateTime dateInt;

        /// <summary>
        /// Id do usuário
        /// </summary>
        string useridInt;

        /// <summary>
        /// Comentário
        /// </summary>
        string commentInt;

        /// <summary>
        /// Notícia
        /// </summary>
        Article articleInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ArticleComment(Article n)
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            articleInt = n;

            useridInt = "";
            dateInt = RacMsg.NullDateTime;
            commentInt = "";

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
        /// Seta informação
        /// </summary>
        /// <param name="id"></param>
        public void SetId(string id)
        {

            idInt = id;

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
        /// Comentário
        /// </summary>
        public string comment
        {

            get { return commentInt; }
            set { commentInt = value; }

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
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Load(string id)
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT userid, date, comment FROM " + Base.conf.prefix + "[newsarticlecomment] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                useridInt = rdr.GetString(0);
                dateInt = rdr.GetDateTime(1);
                commentInt = rdr.GetString(2);

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
            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsarticlecomment] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsarticlecomment] (id, articleid, userid, date, comment) VALUES (@id, @articleid, @userid, @date, @comment)";
                ins.Parameters.Add(new SqlParameter("@id", id));

                ins.Parameters.Add(new SqlParameter("@articleid", article.id));
                ins.Parameters.Add(new SqlParameter("@userid", useridInt));
                ins.Parameters.Add(new SqlParameter("@date", dateInt));
                ins.Parameters.Add(new SqlParameter("@comment", commentInt));

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
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsarticlecomment] SET articleid=@articleid, userid=@userid, date=@date, comment=@comment WHERE id=@id";

                upd.Parameters.Add(new SqlParameter("@articleid", article.id));
                upd.Parameters.Add(new SqlParameter("@userid", useridInt));
                upd.Parameters.Add(new SqlParameter("@date", dateInt));
                upd.Parameters.Add(new SqlParameter("@comment", commentInt));

                upd.Parameters.Add(new SqlParameter("@id", id));

                upd.Connection = Base.conf.Open();
                upd.ExecuteNonQuery();
                upd.Connection.Close();

                res = true;
                dbassignedidInt = true;

            }

            return res;

        }
        
        /// <summary>
        /// Salva a ticket
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {

            bool res = true;

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticlecomment] WHERE id=@id";
            del.Parameters.Add(new SqlParameter("@id", id));

            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();

            del.ExecuteNonQuery();
            del.Connection.Close();

            return res;

        }
        
        /// <summary>
        /// Carrega a pauta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static Article ArticleForArticleComment(string id)
        {

            string idArt = Base.conf.GetParentString(id, "newsarticlecomment", "articleid");

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
        public static ArticleComment LoadArticleComment(string id)
        {

            Article art = ArticleForArticleComment(id);

            if (art != null)
            {

                return art.GetArticleComment(id);

            }

            return null;

        }

    }

}