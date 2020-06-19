using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class ArticleActionObservation
    {

        /// <summary>
        /// Usuário que comentou
        /// </summary>
        string useridInt;

        /// <summary>
        /// Quando foi designado
        /// </summary>
        DateTime includedInt;

        /// <summary>
        /// Observação em si
        /// </summary>
        string observationInt;

        /// <summary>
        /// Notícia
        /// </summary>
        ArticleAction articleactionInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ArticleActionObservation(ArticleAction n)
        {

            articleactionInt = n;

            useridInt = "";
            observationInt = "";
            includedInt = RacMsg.NullDateTime;


        }

        /// <summary>
        /// Notícia
        /// </summary>
        public ArticleAction articleAction
        {

            get { return articleactionInt; }

        }

        /// <summary>
        /// Qual é o usuário
        /// </summary>
        public string userId
        {

            get { return useridInt; }
            set { useridInt = value; }

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
        /// Designado por
        /// </summary>
        public BaseUser includeddBy
        {

            get { return RacLib.BaseUserSource.source.LoadUser(userId); }

        }

        /// <summary>
        /// Retorna o nome e email de quem designou
        /// </summary>
        public string includeddByName
        {

            get
            {

                BaseUser usr = includeddBy;
                if (usr != null)
                    return usr.name;
                else
                    return "";

            }

        }

        /// <summary>
        /// Quando foi designado
        /// </summary>
        public DateTime included
        {

            get { return includedInt; }
            set { includedInt = value; }

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<ArticleActionObservation> LoadAllFrom(ArticleAction na)
        {

            List<ArticleActionObservation> lst = new List<ArticleActionObservation>();

            SqlCommand sel = new SqlCommand();
            
            sel.CommandText = "SELECT userid, included, observation FROM " + Base.conf.prefix + "[newsarticleactionobservation] WHERE id=@id ORDER BY included";
            sel.Parameters.Add(new SqlParameter("@id", na.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                // Pega as informações

                ArticleActionObservation ngo = new ArticleActionObservation(na);

                ngo.useridInt = rdr.GetString(0);                
                ngo.includedInt = rdr.GetDateTime(1);
                ngo.observationInt = rdr.GetString(2);

                lst.Add(ngo);

            }

            rdr.Close();
            sel.Connection.Close();

            return lst;

        }

        /// <summary>
        /// Salva a ticket
        /// </summary>
        /// <returns></returns>
        public static bool SaveAll(ArticleAction na, List<ArticleActionObservation> lst)
        {

            bool res = false;

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticleactionobservation] WHERE id=@id";
            del.Parameters.Add(new SqlParameter("@id", na.id));

            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();
            del.Connection.Close();

            SqlCommand ins = new SqlCommand();
            ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsarticleactionobservation] (id, userid, included, observation) VALUES (@id, @userid, @included, @observation)";
            ins.Parameters.Add(new SqlParameter("@id", na.id));

            SqlParameter varObservation = new SqlParameter("@observation", "0");
            ins.Parameters.Add(varObservation);

            SqlParameter varUserId = new SqlParameter("@userid", "0");
            ins.Parameters.Add(varUserId);

            SqlParameter varIncluded = new SqlParameter("@included", DateTime.Now);
            ins.Parameters.Add(varIncluded);

            ins.Connection = Base.conf.Open();

            for (int i = 0; i < lst.Count; i++)
            {

                varObservation.Value = lst[i].observationInt;
                varUserId.Value = lst[i].useridInt;
                varIncluded.Value = lst[i].includedInt;

                ins.ExecuteNonQuery();

            }

            ins.Connection.Close();

            return res;

        }

    }

}