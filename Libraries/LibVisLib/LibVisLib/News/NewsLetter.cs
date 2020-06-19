using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class NewsLetter
    {

        /// <summary>
        /// Tipo
        /// </summary>
        int typeInt;

        /// <summary>
        /// Possíveis formas de newsletter
        /// </summary>
        public enum NewsLetterType { Undefined, Email }

        /// <summary>
        /// Dados
        /// </summary>
        string dataInt;

        /// <summary>
        /// Tempo de registro
        /// </summary>
        DateTime registeredInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public NewsLetter()
        {

            typeInt = 0;
            dataInt = "";
            registeredInt = RacMsg.NullDateTime;

        }

        /// <summary>
        /// Tipo de aviso
        /// </summary>
        public NewsLetterType type
        {

            get { return (NewsLetterType)typeInt; }
            set { typeInt = (int)value; }

        }

        /// <summary>
        /// Tipo de email
        /// </summary>
        public string data
        {

            get { return dataInt; }
            set { dataInt = value; }

        }

        /// <summary>
        /// Texto completo
        /// </summary>
        public DateTime registered
        {

            get { return registeredInt; }
            set { registeredInt = value; }

        }
        
        /// <summary>
        /// Pega a lista de todas as newsletter
        /// </summary>
        public static List<NewsLetter> newsLetter
        {

            get
            {

                List<NewsLetter> lst = new List<NewsLetter>();

                SqlCommand sel = new SqlCommand();

                sel.CommandText = "SELECT type, data, registered FROM " + Base.conf.prefix + "[newsletter] ORDER BY registered";
                
                sel.Connection = Base.conf.Open();
                SqlDataReader rdr = sel.ExecuteReader();

                if (rdr.Read())
                {

                    NewsLetter s = new NewsLetter();
                    s.typeInt = rdr.GetInt32(0);
                    s.dataInt = rdr.GetString(1);
                    s.registeredInt = rdr.GetDateTime(2);

                    lst.Add(s);

                }

                rdr.Close();
                sel.Connection.Close();

                return lst;

            }

        }

        /// <summary>
        /// Inclui uma nova newsletter
        /// </summary>
        /// <param name="email"></param>
        public static void AddNewsLetterEmail(string email)
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT registered FROM " + Base.conf.prefix + "[newsletter] WHERE type=1 AND data=@data";
            sel.Parameters.Add(new SqlParameter("@data", email));
            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
                res = true;

            rdr.Close();
            sel.Connection.Close();

            if (!res)
            {

                SqlCommand inc = new SqlCommand();

                inc.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsletter] (type, data, registered) VALUES (1, @data, @registered)";
                inc.Parameters.Add(new SqlParameter("@data", email));
                inc.Parameters.Add(new SqlParameter("@registered", DateTime.Now));
                inc.Connection = Base.conf.Open();

                inc.ExecuteNonQuery();

                inc.Connection.Close();

            }

        }
 
        /// <summary>
        /// Está registrado?
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmailInNewsLetter(string email)
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT registered FROM " + Base.conf.prefix + "[newsletter] WHERE type=1 AND data=@data";
            sel.Parameters.Add(new SqlParameter("@data", email));
            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
                res = true;

            rdr.Close();
            sel.Connection.Close();

            return res;

        }

        /// <summary>
        /// Remove um email da newsletter
        /// </summary>
        /// <param name="email"></param>
        public static void RemoveNewsLetterEmail(string email)
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT registered FROM " + Base.conf.prefix + "[newsletter] WHERE type=1 AND data=@data";
            sel.Parameters.Add(new SqlParameter("@data", email));
            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
                res = true;

            rdr.Close();
            sel.Connection.Close();

            if (res)
            {

                SqlCommand del = new SqlCommand();

                del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsletter] WHERE type=1 AND data=@data";
                del.Parameters.Add(new SqlParameter("@data", email));
                del.Connection = Base.conf.Open();

                del.ExecuteNonQuery();

                del.Connection.Close();

            }

        }

    }

}