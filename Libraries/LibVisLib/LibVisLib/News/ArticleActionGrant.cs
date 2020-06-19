using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class ArticleActionGrant
    {

        /// <summary>
        /// Qual é o award específico
        /// </summary>
        string awardidInt;

        /// <summary>
        /// Designado por
        /// </summary>
        string grantedbyInt;

        /// <summary>
        /// Quando foi designado
        /// </summary>
        DateTime grantedInt;

        /// <summary>
        /// Notícia
        /// </summary>
        ArticleAction articleactionInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ArticleActionGrant(ArticleAction n)
        {

            articleactionInt = n;

            awardidInt = "";
            grantedbyInt = "";
            grantedInt = RacMsg.NullDateTime;


        }

        /// <summary>
        /// Notícia
        /// </summary>
        public ArticleAction articleAction
        {

            get { return articleactionInt; }

        }

        /// <summary>
        /// Qual é o award específico
        /// </summary>
        public string awardId
        {

            get { return awardidInt; }
            set { awardidInt = value; }

        }

        /// <summary>
        /// Designado por
        /// </summary>
        public string grantedById
        {

            get { return grantedbyInt; }
            set { grantedbyInt = value; }

        }
        
        /// <summary>
        /// Designado por
        /// </summary>
        public BaseUser grantedBy
        {

            get { return RacLib.BaseUserSource.source.LoadUser(grantedById); }

        }

        /// <summary>
        /// Retorna o nome e email de quem designou
        /// </summary>
        public string grantedByName
        {

            get
            {

                BaseUser usr = grantedBy;
                if (usr != null)
                    return usr.name;
                else
                    return "";

            }

        }

        /// <summary>
        /// Quando foi designado
        /// </summary>
        public DateTime granted
        {

            get { return grantedInt; }
            set { grantedInt = value; }

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<ArticleActionGrant> LoadAllFrom(ArticleAction na)
        {

            List<ArticleActionGrant> lst = new List<ArticleActionGrant>();

            SqlCommand sel = new SqlCommand();
            
            sel.CommandText = "SELECT awardid, grantedby, granted FROM " + Base.conf.prefix + "[newsarticleactiongrant] WHERE id=@id ORDER BY granted";
            sel.Parameters.Add(new SqlParameter("@id", na.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                // Pega as informações

                ArticleActionGrant nga = new ArticleActionGrant(na);

                nga.awardidInt = rdr.GetString(0);
                nga.grantedById = rdr.GetString(1);
                nga.granted = rdr.GetDateTime(2);

                lst.Add(nga);

            }

            rdr.Close();
            sel.Connection.Close();

            return lst;

        }

        /// <summary>
        /// Salva a ticket
        /// </summary>
        /// <returns></returns>
        public static bool SaveAll(ArticleAction na, List<ArticleActionGrant> lst)
        {

            bool res = false;

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsarticleactiongrant] WHERE id=@id";
            del.Parameters.Add(new SqlParameter("@id", na.id));

            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();
            del.Connection.Close();

            SqlCommand ins = new SqlCommand();
            ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsarticleactiongrant] (id, awardid, grantedby, granted) VALUES (@id, @awardid, @grantedby, @granted)";
            ins.Parameters.Add(new SqlParameter("@id", na.id));

            SqlParameter varAttribute = new SqlParameter("@awardid", "0");
            ins.Parameters.Add(varAttribute);

            SqlParameter varGrantedBy = new SqlParameter("@grantedby", "0");
            ins.Parameters.Add(varGrantedBy);

            SqlParameter varGranted = new SqlParameter("@granted", DateTime.Now);
            ins.Parameters.Add(varGranted);

            ins.Connection = Base.conf.Open();

            for (int i = 0; i < lst.Count; i++)
            {

                varAttribute.Value = lst[i].awardidInt;
                varGrantedBy.Value = lst[i].grantedbyInt;
                varGranted.Value = lst[i].grantedInt;

                ins.ExecuteNonQuery();

            }

            ins.Connection.Close();

            return res;

        }

    }

}