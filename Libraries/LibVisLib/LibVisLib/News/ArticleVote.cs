using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class ArticleVote
    {

        /// <summary>
        /// Voto
        /// </summary>
        int voteInt;

        /// <summary>
        /// Tipo de notícia
        /// </summary>
        public enum Vote { Undefined, Approved, DontLike, AlreadyWritten, PoorlyWritten }

        /// <summary>
        /// Usuário
        /// </summary>
        string useridInt;

        /// <summary>
        /// Data do ocorrido
        /// </summary>
        DateTime dateInt;

        /// <summary>
        /// Notícia
        /// </summary>
        Article articleInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ArticleVote(Article t)
        {

            articleInt = t;

            voteInt = 0;
            useridInt = "";
            dateInt = DateTime.Now;

        }

        /// <summary>
        /// Notícia
        /// </summary>
        public Article article
        {

            get { return articleInt; }

        }

        /// <summary>
        /// Tipo
        /// </summary>
        public Vote vote
        {

            get { return (Vote)voteInt; }
            set { voteInt = (int)value; }

        }

        /// <summary>
        /// Usuário
        /// </summary>
        public string userId
        {

            get { return useridInt; }
            set { useridInt = value; }

        }

        /// <summary>
        /// Usuário
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
        public static List<ArticleVote> LoadAllFrom(Article na)
        {
            List<ArticleVote> lst = new List<ArticleVote>();

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT userid, vote, date FROM " + Base.conf.prefix + "[newsarticlevote] WHERE id=@id ORDER BY date";
            sel.Parameters.Add(new SqlParameter("@id", na.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                // Pega as informações

                ArticleVote nl = new ArticleVote(na);

                nl.useridInt = rdr.GetString(0);
                nl.voteInt = rdr.GetInt32(1);
                nl.dateInt = rdr.GetDateTime(2);

                lst.Add(nl);

            }

            rdr.Close();
            sel.Connection.Close();

            return lst;

        }

        /// <summary>
        /// Salva a ticket
        /// </summary>
        /// <returns></returns>
        public static bool SaveAll(Article na, List<ArticleVote> lst)
        {

            bool res = false;

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticlevote] WHERE id=@id";
            del.Parameters.Add(new SqlParameter("@id", na.id));

            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();
            del.Connection.Close();

            SqlCommand ins = new SqlCommand();
            ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsarticlevote] (id, userid, vote, date) VALUES (@id, @userid, @vote, @date)";
            ins.Parameters.Add(new SqlParameter("@id", na.id));

            SqlParameter varVote = new SqlParameter("@vote", 0);
            ins.Parameters.Add(varVote);

            SqlParameter varUserId = new SqlParameter("@userid", "0");
            ins.Parameters.Add(varUserId);

            SqlParameter varGranted = new SqlParameter("@date", DateTime.Now);
            ins.Parameters.Add(varGranted);

            ins.Connection = Base.conf.Open();

            for (int i = 0; i < lst.Count; i++)
            {

                varVote.Value = lst[i].voteInt;
                varUserId.Value = lst[i].useridInt;
                varGranted.Value = lst[i].dateInt;

                ins.ExecuteNonQuery();

            }

            ins.Connection.Close();

            return res;

        }

    }

}