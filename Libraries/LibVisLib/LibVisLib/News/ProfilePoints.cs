using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class ProfilePoints
    {
        
        /// <summary>
        /// Dimension
        /// </summary>
        int dimensionInt;

        /// <summary>
        /// Pontos
        /// </summary>
        decimal pointsInt;

        /// <summary>
        /// Perfil
        /// </summary>
        Profile profInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ProfilePoints(Profile p)
        {

            profInt = p;

            dimensionInt = 0;
            pointsInt = 0;

        }

        /// <summary>
        /// Notícia
        /// </summary>
        public Profile profile
        {

            get { return profInt; }

        }
        
        /// <summary>
        /// Dimension
        /// </summary>
        public Medal.Dimension dimension
        {

            get { return (Medal.Dimension)dimensionInt; }
            set { dimensionInt = (int)value; }

        }

        /// <summary>
        /// Pontos
        /// </summary>
        public decimal points
        {

            get { return pointsInt; }
            set { pointsInt = value; }

        }
        
        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<ProfilePoints> LoadAllFrom(Profile pr)
        {

            List<ProfilePoints> lst = new List<ProfilePoints>();

            SqlCommand sel = new SqlCommand();            
            sel.CommandText = "SELECT dimension, points FROM " + Base.conf.prefix + "[newsprofilepoints] WHERE userid=@id";
            sel.Parameters.Add(new SqlParameter("@id", pr.user.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while(rdr.Read())
            {

                // Pega as informações

                ProfilePoints pp = new ProfilePoints(pr);

                pp.dimensionInt = rdr.GetInt32(0);
                pp.pointsInt = rdr.GetDecimal(1);

                lst.Add(pp);

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
            sel.CommandText = "SELECT points FROM " + Base.conf.prefix + "[newsprofilepoints] WHERE userid=@userid AND dimension=@dimension";
            sel.Parameters.Add(new SqlParameter("@userid", profile.user.id));
            sel.Parameters.Add(new SqlParameter("@dimension", (int)dimension));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {
                
                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsprofilepoints] (userid, dimension, points) VALUES (@userid, @dimension, @points)";

                ins.Parameters.Add(new SqlParameter("@userid", profile.user.id));
                ins.Parameters.Add(new SqlParameter("@dimension", (int)dimension));
                ins.Parameters.Add(new SqlParameter("@points", points));

                ins.Connection = Base.conf.Open();
                ins.ExecuteNonQuery();
                ins.Connection.Close();

                res = true;

            }
            else
            {

                SqlCommand upd = new SqlCommand();
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsprofilepoints] SET points=@points WHERE userid=@userid AND dimension=@dimension";

                upd.Parameters.Add(new SqlParameter("@userid", profile.user.id));
                upd.Parameters.Add(new SqlParameter("@dimension", (int)dimension));
                upd.Parameters.Add(new SqlParameter("@points", points));

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
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsprofilepoints] WHERE userid=@userid AND dimension=@dimension";
            del.Parameters.Add(new SqlParameter("@userid", profile.user.id));
            del.Parameters.Add(new SqlParameter("@dimension", (int)dimension));

            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();

            return true;

        }

    }

}