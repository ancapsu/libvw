using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class TargetComment
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
        /// Texto do comentário
        /// </summary>
        string textInt;

        /// <summary>
        /// Última mudança
        /// </summary>
        DateTime dateInt;

        /// <summary>
        /// Notícia
        /// </summary>
        Target targetInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public TargetComment(Target n)
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            targetInt = n;

            useridInt = "";
            textInt = "";
            dateInt = DateTime.Now;

        }

        /// <summary>
        /// Notícia
        /// </summary>
        public Target target
        {

            get { return targetInt; }

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
        public string targetId
        {

            get { return target.id; }

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
        /// Id do usuário
        /// </summary>
        public RacLib.BaseUser user
        {

            get { return RacLib.BaseUserSource.source.LoadUser(userId); }
            
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
        /// Texto
        /// </summary>
        public string text
        {

            get { return textInt; }
            set { textInt = value; }

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

            sel.CommandText = "SELECT userid, text, date FROM " + Base.conf.prefix + "[newstargetcomment] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                useridInt = rdr.GetString(0);
                textInt = rdr.GetString(1);
                dateInt = rdr.GetDateTime(2);

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
            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstargetcomment] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newstargetcomment] (id, targetid, userid, text, date) VALUES (@id, @targetid, @userid, @text, @date)";
                ins.Parameters.Add(new SqlParameter("@id", id));

                ins.Parameters.Add(new SqlParameter("@targetid", target.id));
                ins.Parameters.Add(new SqlParameter("@userid", useridInt));
                ins.Parameters.Add(new SqlParameter("@text", textInt));
                ins.Parameters.Add(new SqlParameter("@date", dateInt));

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
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newstargetcomment] SET targetid=@targetid, userid=@userid, text=@text, date=@date WHERE id=@id";

                upd.Parameters.Add(new SqlParameter("@targetid", target.id));
                upd.Parameters.Add(new SqlParameter("@userid", useridInt));
                upd.Parameters.Add(new SqlParameter("@text", textInt));
                upd.Parameters.Add(new SqlParameter("@date", dateInt));

                upd.Parameters.Add(new SqlParameter("@id", id));

                upd.Connection = Base.conf.Open();
                upd.ExecuteNonQuery();
                upd.Connection.Close();

                res = true;
                dbassignedidInt = true;

            }

            return res;

        }

    }

}