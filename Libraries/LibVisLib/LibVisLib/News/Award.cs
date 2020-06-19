using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class Award
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
        /// Nome
        /// </summary>
        int namemsgInt;

        /// <summary>
        /// Name string
        /// </summary>
        string nameInt;

        /// <summary>
        /// Descrição
        /// </summary>
        int descriptionmsgInt;

        /// <summary>
        /// Descrição string
        /// </summary>
        string descriptionInt;

        /// <summary>
        /// Tipo de prêmio
        /// </summary>
        int typeInt;

        /// <summary>
        /// Tipos possíveis de prêmio
        /// </summary>
        public enum Type { Undefine, Positive, Negative };

        /// <summary>
        /// Aplicável a
        /// </summary>
        int appliedtoInt;

        /// <summary>
        /// Prêmio pode ser aplicado a
        /// </summary>
        public enum AppliedTo { Undefined, Target, Article };

        /// <summary>
        /// Pontos do prêmio
        /// </summary>
        List<AwardPoints> pointsInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public Award()
        {

            idInt = "";
            dbassignedidInt = false;
            typeInt = 0;
            appliedtoInt = 0;
            namemsgInt = 0;
            nameInt = "";
            descriptionmsgInt = 0;
            descriptionInt = "";

            pointsInt = null;

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
        /// Tipo
        /// </summary>
        public Type type
        {

            get { return (Type)typeInt; }
            set { typeInt = (int)value; }

        }
        
        /// <summary>
        /// Aplicável a
        /// </summary>
        public AppliedTo appliedTo
        {

            get { return (AppliedTo)appliedtoInt; }
            set { appliedtoInt = (int)value; }

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
        /// Name string
        /// </summary>
        public string name
        {

            get { return nameInt; }
            set { nameInt = value; }

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
        /// Descrição
        /// </summary>
        public string description
        {

            get { return descriptionInt; }
            set { descriptionInt = value; }

        }

        /// <summary>
        /// Pontos
        /// </summary>
        public List<AwardPoints> points
        {

            get
            {

                if (pointsInt == null)
                    pointsInt = AwardPoints.LoadAllFrom(this);

                return pointsInt;

            }

            set
            {

                pointsInt = value;

            }

        }

        /// <summary>
        /// Retorna os pontos
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        AwardPoints GetPoints(Medal.Dimension d)
        {

            for (int i = 0; i < points.Count; i++)
                if (points[i].dimension == d)
                    return points[i];

            return null;

        }

        /// <summary>
        /// Pega os pontos
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        decimal GetPointNumberForDimensions(Medal.Dimension d)
        {

            AwardPoints p = GetPoints(d);
            if (p != null)
                return p.points;
            else
                return 0;

        }

        /// <summary>
        /// Pontos de experiência
        /// </summary>
        public decimal xp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.TotalExperience); }

        }

        /// <summary>
        /// Pontos de editor
        /// </summary>
        public decimal ap
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.EditorExperience); }

        }

        /// <summary>
        /// Pontos de repórter
        /// </summary>
        public decimal rp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.ReporterExperience); }

        }
        
        /// <summary>
        /// Pontos de originalidade
        /// </summary>
        public decimal op
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Originality); }

        }

        /// <summary>
        /// Pontos de qualidade
        /// </summary>
        public decimal qp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Quality); }

        }

        /// <summary>
        /// Pontos de poliglota
        /// </summary>
        public decimal tp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Polyglot); }

        }

        /// <summary>
        /// Pontos de urgência
        /// </summary>
        public decimal up
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Speed); }

        }

        /// <summary>
        /// Pontos de adequção
        /// </summary>
        public decimal sp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Standartization); }

        }

        /// <summary>
        /// Pontos de acumulação
        /// </summary>
        public decimal ep
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Accumulator); }

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

            sel.CommandText = "SELECT type, appliedto, name, description FROM " + Base.conf.prefix + "[newsaward] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                typeInt = rdr.GetInt32(0);
                appliedtoInt = rdr.GetInt32(1);
                nameInt = rdr.GetString(2);
                descriptionInt = rdr.GetString(3);

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
        public static Award LoadAward(string id)
        {

            for(int i =0; i < awards.Count; i++)
                if (awards[i].id == id)
                    return awards[i];

            return null;

        }

        /// <summary>
        /// Prêmios lista
        /// </summary>
        static List<Award> awardsInt = null;

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Award> awards
        {

            get
            {

                if (awardsInt == null)
                {

                    List<string> ids = new List<string>();

                    SqlCommand sel = new SqlCommand();
                    sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsaward] ORDER BY position";
                    sel.Connection = Base.conf.Open();
                    SqlDataReader rdr = sel.ExecuteReader();

                    while (rdr.Read())
                    {

                        ids.Add(rdr.GetString(0));

                    }

                    rdr.Close();
                    sel.Connection.Close();

                    List<Award> lst = new List<Award>();
                    for (int i = 0; i < ids.Count; i++)
                    {

                        Award m = new Award();
                        if (m.Load(ids[i]))
                            lst.Add(m);

                    }

                    awardsInt = lst;

                }

                return awardsInt;

            }

        }

    }

}