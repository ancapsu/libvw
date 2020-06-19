using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class SiteWarning
    {

        /// <summary>
        /// Tipo
        /// </summary>
        int typeInt;

        /// <summary>
        /// Possíveis situações
        /// </summary>
        public enum WarningType { Undefined, Urgent }

        /// <summary>
        /// Titulo do aviso
        /// </summary>
        string titleInt;

        /// <summary>
        /// Texto completo
        /// </summary>
        string textInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public SiteWarning()
        {

            typeInt = 0;
            titleInt = "";
            textInt = "";

        }

        /// <summary>
        /// Tipo de aviso
        /// </summary>
        public WarningType type
        {

            get { return (WarningType)typeInt; }
            set { typeInt = (int)value; }

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
        /// Texto completo
        /// </summary>
        public string text
        {

            get { return textInt; }
            set { textInt = value; }

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SiteWarning> warnings
        {

            get
            {

                List<SiteWarning> lst = new List<SiteWarning>();

                SqlCommand sel = new SqlCommand();

                sel.CommandText = "SELECT type, title, text FROM " + Base.conf.prefix + "[newssitewarning] ORDER BY type";
                
                sel.Connection = Base.conf.Open();
                SqlDataReader rdr = sel.ExecuteReader();

                if (rdr.Read())
                {

                    SiteWarning s = new SiteWarning();
                    s.typeInt = rdr.GetInt32(0);
                    s.titleInt = rdr.GetString(1);
                    s.textInt = rdr.GetString(2);

                    lst.Add(s);

                }

                rdr.Close();
                sel.Connection.Close();

                return lst;

            }

        }

    }

}