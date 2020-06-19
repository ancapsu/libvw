using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class Medal
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
        /// Dimension
        /// </summary>
        int dimensionInt;

        /// <summary>
        /// Tipo de notícia
        /// </summary>
        public enum Dimension { Undefined,
            TotalExperience,
            EditorExperience=100, ReporterExperience, RevisorExperience, DiagrammerExperience, CameraExperience, AudioExperience, ColumnistExperience,
            Originality=200, Quality, Polyglot, Speed, Standartization, Accumulator
        }

        /// <summary>
        /// Pontos necessarios
        /// </summary>
        decimal requiredpointsInt;

        /// <summary>
        /// Nome
        /// </summary>
        int namemsgInt;
        
        /// <summary>
        /// Descrição
        /// </summary>
        int descriptionmsgInt;
        
        /// <summary>
        /// Cria novo
        /// </summary>
        public Medal()
        {

            idInt = "";
            dbassignedidInt = false;
            dimensionInt = 0;
            requiredpointsInt = 0;
            namemsgInt = 0;
            descriptionmsgInt = 0;

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
        /// Dimension
        /// </summary>
        public Dimension dimension
        {

            get { return (Dimension)dimensionInt; }
            set { dimensionInt = (int)value; }

        }
        
        /// <summary>
        /// Pontos necessarios
        /// </summary>
        public decimal requiredPoints
        {

            get { return requiredpointsInt; }
            set { requiredpointsInt = value; }

        }

        /// <summary>
        /// Nome
        /// </summary>
        public int nameMsg
        {

            get { return namemsgInt; }
            set { namemsgInt = value; }

        }
        
        /// <summary>
        /// Descrição
        /// </summary>
        public int descriptionMsg
        {

            get { return descriptionmsgInt; }
            set { descriptionmsgInt = value; }

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

            sel.CommandText = "SELECT dimension, points, name, description FROM " + Base.conf.prefix + "[newsmedals] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                dimensionInt = rdr.GetInt32(0);
                requiredpointsInt = rdr.GetDecimal(1);
                namemsgInt = rdr.GetInt32(2);
                descriptionmsgInt = rdr.GetInt32(3);

                dbassignedidInt = true;
                res = true;

            }

            rdr.Close();
            sel.Connection.Close();

            return res;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Medal LoadMedal(string id)
        {

            for(int i =0; i < medals.Count; i++)
                if (medals[i].id == id)
                    return medals[i];

            return null;

        }

        /// <summary>
        /// Todas as medalhas para pontos
        /// </summary>
        /// <param name="d"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static Medal MajorMedalForPoints(Dimension d, decimal pts)
        {

            Medal m = null;

            for (int i = 0; i < medals.Count - 1; i++)
                if (medals[i].dimension == d && medals[i].requiredPoints <= pts)
                    m = medals[i];

            return m;

        }

        /// <summary>
        /// Todas as medalhas para pontos
        /// </summary>
        /// <param name="d"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static List<Medal> AllMedalsForPoints(Dimension d, decimal pts)
        {

            List<Medal> lst = new List<Medal>();

            for (int i = 0; i < medals.Count - 1; i++) 
                if (medals[i].dimension == d && medals[i].requiredPoints <= pts)
                        lst.Add(medals[i]);

            return lst;

        }

        /// <summary>
        /// Todas as medalhas para pontos
        /// </summary>
        /// <param name="d"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static Medal NextMedalsForPoints(Dimension d, decimal pts)
        {

            List<Medal> lst = new List<Medal>();

            for (int i = 0; i < medals.Count - 1; i++)
                if (medals[i].dimension == d && medals[i].requiredPoints > pts)
                    return medals[i];

            return null;

        }

        /// <summary>
        /// Medalhas lista
        /// </summary>
        static List<Medal> medalsInt = null;

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Medal> medals
        {

            get
            {

                if (medalsInt == null)
                {

                    List<string> ids = new List<string>();

                    SqlCommand sel = new SqlCommand();
                    sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsmedals] ORDER BY dimension, points";
                    sel.Connection = Base.conf.Open();
                    SqlDataReader rdr = sel.ExecuteReader();

                    while (rdr.Read())
                    {

                        ids.Add(rdr.GetString(0));

                    }

                    rdr.Close();
                    sel.Connection.Close();

                    List<Medal> lst = new List<Medal>();
                    for (int i = 0; i < ids.Count; i++)
                    {

                        Medal m = new Medal();
                        if (m.Load(ids[i]))
                            lst.Add(m);

                    }

                    medalsInt = lst;

                }

                return medalsInt;

            }

        }

    }

}