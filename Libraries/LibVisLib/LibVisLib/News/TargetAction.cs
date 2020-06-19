using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class TargetAction
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
        public enum ActionType { Undefined, Suggested, Revised, ArticleCreated, ArticleApproved, ArticleIncludedInVideo, Removed }

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
        List<TargetActionGrant> grantsInt;

        /// <summary>
        /// Notícia
        /// </summary>
        Target targetInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public TargetAction(Target n)
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            targetInt = n;

            useridInt = "";
            typeInt = 0;
            observationInt = "";
            dateInt = DateTime.Now;
            showInt = 0;
            grantsInt = null;

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

                if (u != null)
                    return u.name;
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
        public List<TargetActionGrant> grants
        {

            get
            {

                if (grantsInt == null)
                    grantsInt = TargetActionGrant.LoadAllFrom(this);

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

            sel.CommandText = "SELECT userid, type, observation, date, show FROM " + Base.conf.prefix + "[newstargetaction] WHERE id=@id";
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
        /// Salva a ticket
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();
            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newstargetaction] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newstargetaction] (id, targetid, userid, type, observation, date, show) VALUES (@id, @targetid, @userid, @type, @observation, @date, @show)";
                ins.Parameters.Add(new SqlParameter("@id", id));

                ins.Parameters.Add(new SqlParameter("@targetid", target.id));
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
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newstargetaction] SET targetid=@targetid, userid=@userid, type=@type, observation=@observation, date=@date, show=@show WHERE id=@id";

                upd.Parameters.Add(new SqlParameter("@targetid", target.id));
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

                    TargetActionGrant.SaveAll(this, grants);

                }
                
            }

            return res;

        }

        /// <summary>
        /// Carrega a pauta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static Target TargetForTargetAction(string id)
        {

            string idTrg = Base.conf.GetParentString(id, "newstargetaction", "targetid");

            if (idTrg != null)
            {

                Target trg = Target.LoadTarget(idTrg);
                return trg;

            }

            return null;

        }

        /// <summary>
        /// Carrega o action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TargetAction LoadTargetAction(string id)
        {

            Target trg = TargetForTargetAction(id);

            if (trg != null)
            {

                return trg.GetTargetAction(id);

            }

            return null;

        }

        /// <summary>
        /// Remove um grant
        /// </summary>
        /// <param name="awardId"></param>
        public void RemoveGrant(string awardId)
        {

            for(int i = 0; i < grants.Count; i++)
            {

                if (grants[i].awardId == awardId)
                {

                    profile.RemoveAward(awardId);

                    grants.RemoveAt(i);
                    i--;

                }

            }
            
        }

        /// <summary>
        /// Adiciona um grant pelo usuário
        /// </summary>
        /// <param name="awardId"></param>
        /// <param name="userId"></param>
        public void AddGrant(string awardId, string userId)
        {

            for (int i = 0; i < grants.Count; i++)
            {

                if (grants[i].awardId == awardId)
                {

                    return;

                }

            }

            TargetActionGrant tag = new TargetActionGrant(this);
            tag.awardId = awardId;
            tag.grantedById = userId;
            tag.granted = DateTime.Now;

            grants.Add(tag);

            profile.AddAward(awardId);

        }

    }

}