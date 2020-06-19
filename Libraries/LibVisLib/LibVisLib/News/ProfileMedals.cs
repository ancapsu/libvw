using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class ProfileMedals
    {

        /// <summary>
        /// Quando foi dado
        /// </summary>
        DateTime awardedInt;

        /// <summary>
        /// Id da medalha
        /// </summary>
        string medalsInt;

        /// <summary>
        /// Perfil
        /// </summary>
        Profile profInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ProfileMedals(Profile p)
        {

            profInt = p;
            awardedInt = RacMsg.NullDateTime;
            medalsInt = "";

        }

        /// <summary>
        /// Notícia
        /// </summary>
        public Profile profile
        {

            get { return profInt; }

        }

        /// <summary>
        /// Id da medalha
        /// </summary>
        public string medalsId
        {

            get { return medalsInt; }
            set { medalsInt = value; }

        }

        /// <summary>
        /// Medalha
        /// </summary>
        public Medal medals
        {

            get { return Medal.LoadMedal(medalsId); }

            set
            {

                if (value != null)
                    medalsId = value.id;
                else
                    medalsId = null;

            }

        }

        /// <summary>
        /// Quando foi designado
        /// </summary>
        public DateTime awarded
        {

            get { return awardedInt; }
            set { awardedInt = value; }

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<ProfileMedals> LoadAllFrom(Profile p)
        {

            List<ProfileMedals> lst = new List<ProfileMedals>();

            SqlCommand sel = new SqlCommand();
            
            sel.CommandText = "SELECT medalid, awarded FROM " + Base.conf.prefix + "[newsprofilemedals] WHERE userid=@userid";
            sel.Parameters.Add(new SqlParameter("@userid", p.user.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                // Pega as informações

                ProfileMedals pm = new ProfileMedals(p);

                pm.medalsId = rdr.GetString(0);
                pm.awardedInt = rdr.GetDateTime(1);

                lst.Add(pm);

            }

            rdr.Close();
            sel.Connection.Close();

            return lst;

        }

        /// <summary>
        /// Salva o cara
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            bool res = false;

            // Vê se já existe

            SqlCommand sel = new SqlCommand();
            sel.CommandText = "SELECT awarded FROM " + Base.conf.prefix + "[newsprofilemedals] WHERE userid=@userid AND medalid=@medalid";
            sel.Parameters.Add(new SqlParameter("@userid", profile.user.id));
            sel.Parameters.Add(new SqlParameter("@medalid", medalsId));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {

                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsprofilemedals] (userid, medalid, awarded) VALUES (@userid, @medalid, @awarded)";

                ins.Parameters.Add(new SqlParameter("@userid", profile.user.id));
                ins.Parameters.Add(new SqlParameter("@medalid", medalsId));
                ins.Parameters.Add(new SqlParameter("@awarded", awardedInt));

                ins.Connection = Base.conf.Open();
                ins.ExecuteNonQuery();
                ins.Connection.Close();

                res = true;

            }
            else
            {

                SqlCommand upd = new SqlCommand();
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsprofilemedals] SET awarded=@awarded WHERE userid=@userid AND medalid=@medalid";

                upd.Parameters.Add(new SqlParameter("@userid", profile.user.id));
                upd.Parameters.Add(new SqlParameter("@medalid", medalsId));
                upd.Parameters.Add(new SqlParameter("@awarded", awardedInt));

                upd.Connection = Base.conf.Open();
                upd.ExecuteNonQuery();
                upd.Connection.Close();

                res = true;

            }

            return res;

        }

        /// <summary>
        /// Apaga a ticket
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            // Manda remover... se não existir, sem problema...

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsprofilemedals] WHERE userid=@userid AND medalid=@medalid";
            del.Parameters.Add(new SqlParameter("@userid", profile.user.id));
            del.Parameters.Add(new SqlParameter("@medalid", medalsId));

            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();

            return true;

        }

    }

}