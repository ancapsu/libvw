using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class ArticleLink
    {

        /// <summary>
        /// Tipo
        /// </summary>
        int typeInt;

        /// <summary>
        /// Tipo de notícia
        /// </summary>
        public enum LinkType { Undefined, Image, Video, ExternalLink }

        /// <summary>
        /// Links
        /// </summary>
        string linkInt;

        /// <summary>
        /// Descrição
        /// </summary>
        string descriptionInt;

        /// <summary>
        /// Notícia
        /// </summary>
        Article newsInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ArticleLink(Article n)
        {

            newsInt = n;

            typeInt = 0;
            linkInt = "";
            descriptionInt = "";

        }

        /// <summary>
        /// Notícia
        /// </summary>
        public Article news
        {

            get { return newsInt; }

        }

        /// <summary>
        /// Tipo
        /// </summary>
        public LinkType type
        {

            get { return (LinkType)typeInt; }
            set { typeInt = (int)value; }

        }

        /// <summary>
        /// Links
        /// </summary>
        public string link
        {

            get { return linkInt; }
            set { linkInt = value; }

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
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<ArticleLink> LoadAllFrom(Article na)
        {
            List<ArticleLink> lst = new List<ArticleLink>();

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT type, link, description FROM " + Base.conf.prefix + "[newsarticlelink] WHERE id=@id ORDER BY position";
            sel.Parameters.Add(new SqlParameter("@id", na.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                // Pega as informações

                ArticleLink nl = new ArticleLink(na);

                nl.type = (LinkType)rdr.GetInt32(0);
                nl.link = rdr.GetString(1);
                nl.description = rdr.GetString(2);

                lst.Add(nl);

            }

            rdr.Close();
            sel.Connection.Close();

            return lst;

        }
        
    }

}