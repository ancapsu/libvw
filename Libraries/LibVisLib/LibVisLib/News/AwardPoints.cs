using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class AwardPoints
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
        /// Prêmio
        /// </summary>
        Award awardInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public AwardPoints(Award a)
        {

            awardInt = a;

            dimensionInt = 0;
            pointsInt = 0;

        }

        /// <summary>
        /// Prêmio
        /// </summary>
        public Award award
        {

            get { return awardInt; }

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
        public static List<AwardPoints> LoadAllFrom(Award aw)
        {

            List<AwardPoints> lst = new List<AwardPoints>();

            SqlCommand sel = new SqlCommand();            
            sel.CommandText = "SELECT dimension, points FROM " + Base.conf.prefix + "[newsawardpoints] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", aw.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while(rdr.Read())
            {

                // Pega as informações

                AwardPoints pp = new AwardPoints(aw);

                pp.dimensionInt = rdr.GetInt32(0);
                pp.pointsInt = rdr.GetInt32(1);

                lst.Add(pp);

            }

            rdr.Close();
            sel.Connection.Close();

            return lst;

        }
        
    }

}