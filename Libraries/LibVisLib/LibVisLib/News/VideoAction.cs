using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class VideoAction
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
        /// Tipo
        /// </summary>
        int typeInt;

        /// <summary>
        /// Tipo de notícia
        /// </summary>
        public enum ActionType { Undefined, Created, Revised, Approved, AddedVideo, AddedAudio, IncludedInVideo }

        /// <summary>
        /// Observação
        /// </summary>
        string observationInt;

        /// <summary>
        /// Última mudança
        /// </summary>
        DateTime dateInt;

        /// <summary>
        /// Deve mostrar
        /// </summary>
        int showInt;

        /// <summary>
        /// Grants dessa ação
        /// </summary>
        List<VideoActionGrant> grantsInt;

        /// <summary>
        /// Notícia
        /// </summary>
        Video videoInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public VideoAction(Video n)
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            videoInt = n;

            useridInt = "";
            typeInt = 0;
            observationInt = "";
            dateInt = RacMsg.NullDateTime;
            showInt = 0;
            grantsInt = null;

        }

        /// <summary>
        /// Notícia
        /// </summary>
        public Video video
        {

            get { return videoInt; }

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
        public string videoId
        {

            get { return video.id; }

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
        /// Tipo
        /// </summary>
        public ActionType type
        {

            get { return (ActionType)typeInt; }
            set { typeInt = (int)value; }

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
        /// Última mudança
        /// </summary>
        public DateTime date
        {

            get { return dateInt; }
            set { dateInt = value; }

        }
        
        /// <summary>
        /// Deve mostrar
        /// </summary>
        public bool show
        {

            get { return showInt != 0; }
            set { showInt = value ? 1 : 0; }

        }

        /// <summary>
        /// Grants dessa ação
        /// </summary>
        public List<VideoActionGrant> grants
        {

            get
            {

                if (grantsInt == null)
                    grantsInt = VideoActionGrant.LoadAllFrom(this);

                return grantsInt;

            }

            set
            {

                grantsInt = value;

            }

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

            sel.CommandText = "SELECT userid, type, observation, date, show FROM " + Base.conf.prefix + "[newsvideoaction] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                useridInt = rdr.GetString(0);
                typeInt = rdr.GetInt32(1);
                observationInt = rdr.GetString(2);
                dateInt = rdr.GetDateTime(3);
                showInt = rdr.GetInt32(4);

                dbassignedidInt = true;
                res = true;

            }

            rdr.Close();
            sel.Connection.Close();

            return res;

        }

        /// <summary>
        /// Salva a ação
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();
            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsvideoaction] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsvideoaction] (id, videoid, userid, type, observation, date, show) VALUES (@id, @videoid, @userid, @type, @observation, @date, @show)";
                ins.Parameters.Add(new SqlParameter("@id", id));

                ins.Parameters.Add(new SqlParameter("@videoid", video.id));
                ins.Parameters.Add(new SqlParameter("@userid", useridInt));
                ins.Parameters.Add(new SqlParameter("@type", typeInt));
                ins.Parameters.Add(new SqlParameter("@observation", observationInt));
                ins.Parameters.Add(new SqlParameter("@date", dateInt));
                ins.Parameters.Add(new SqlParameter("@show", showInt));

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
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsvideoaction] SET videoid=@videoid, userid=@userid, type=@type, observation=@observation, date=@date, show=@show WHERE id=@id";

                upd.Parameters.Add(new SqlParameter("@videoid", video.id));
                upd.Parameters.Add(new SqlParameter("@userid", useridInt));
                upd.Parameters.Add(new SqlParameter("@type", typeInt));
                upd.Parameters.Add(new SqlParameter("@observation", observationInt));
                upd.Parameters.Add(new SqlParameter("@date", dateInt));
                upd.Parameters.Add(new SqlParameter("@show", showInt));

                upd.Parameters.Add(new SqlParameter("@id", id));

                upd.Connection = Base.conf.Open();
                upd.ExecuteNonQuery();
                upd.Connection.Close();

                res = true;
                dbassignedidInt = true;

            }

            if (res)
            {

                if (grantsInt != null)
                {

                    VideoActionGrant.SaveAll(this, grants);

                }

            }

            return res;

        }
        
    }

}