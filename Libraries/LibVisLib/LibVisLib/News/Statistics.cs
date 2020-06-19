using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class Statistics
    {
                
        /// <summary>
        /// Onde
        /// </summary>
        int realmInt;

        /// <summary>
        /// Tipos de onde
        /// </summary>
        public enum Realm { Undefined, Youtube, Bitchute, Facebook, Minds, Twitter, Gabai, Site }

        /// <summary>
        /// Parâmetro
        /// </summary>
        int parameterInt;

        /// <summary>
        /// Página principal
        /// </summary>
        public enum Parameter { Undefined, Subscribers, Views }

        /// <summary>
        /// Link principal do conteúdo (vídeo)
        /// </summary>
        string iconInt;

        /// <summary>
        /// Link principal do conteúdo (vídeo)
        /// </summary>
        string linkInt;

        /// <summary>
        /// Valor
        /// </summary>
        int valueInt;

        /// <summary>
        /// Unidade
        /// </summary>
        string unitsInt;

        /// <summary>
        /// Última atualização
        /// </summary>
        DateTime lastupdateInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public Statistics()
        {

            realmInt = 0;
            parameterInt = 0;
            iconInt = "";
            linkInt = "";
            valueInt = 0;
            unitsInt = "";
            lastupdateInt = RacMsg.NullDateTime;

        }
        
        /// <summary>
        /// Onde
        /// </summary>
        public Realm realm
        {

            get { return (Realm)realmInt; }
            set { realmInt = (int)value; }

        }

        /// <summary>
        /// Tipos de página
        /// </summary>
        public Parameter parameter
        {

            get { return (Parameter)parameterInt; }
            set { parameterInt = (int)value; }

        }

        /// <summary>
        /// Ícone
        /// </summary>
        public string icon
        {

            get { return iconInt; }
            set { iconInt = value; }

        }

        /// <summary>
        /// Link
        /// </summary>
        public string link
        {

            get { return linkInt; }
            set { linkInt = value; }

        }

        /// <summary>
        /// Valor
        /// </summary>
        public int value
        {

            get { return valueInt; }
            set { valueInt = value; }

        }

        /// <summary>
        /// Unidade
        /// </summary>
        public string units
        {

            get { return unitsInt; }
            set { unitsInt = value; }

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Load(int lang, Realm realm, Parameter parameter)
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();
            
            sel.CommandText = "SELECT icon, link, value, units, lastupdate FROM " + Base.conf.prefix + "[newsstatistics] WHERE realm=@realm AND parameter=@parameter AND lang=@lang ORDER BY lastupdate DESC";
            sel.Parameters.Add(new SqlParameter("@realm", (int)realm));
            sel.Parameters.Add(new SqlParameter("@parameter", (int)parameter));
            sel.Parameters.Add(new SqlParameter("@lang", lang));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                realmInt = (int)realm;
                parameterInt = (int)parameter;
                iconInt = rdr.GetString(0);
                linkInt = rdr.GetString(1);
                valueInt = rdr.GetInt32(2);
                unitsInt = rdr.GetString(3);
                lastupdateInt = rdr.GetDateTime(4);

                res = true;

            }

            rdr.Close();
            sel.Connection.Close();

            return res;

        }

        /// <summary>
        /// Carrega a estaísticas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Statistics LoadNewsStatistics(int lang, Realm real, Parameter parameter)
        {

            Statistics n = new Statistics();
            if (n.Load(lang, real, parameter))
                return n;

            return null;

        }

        /// <summary>
        /// Definir estatística
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public static void SetStatistic(int lang, Realm realm, Parameter parameter, int value)
        {


            SqlCommand upd = new SqlCommand();

            upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsstatistics] SET value=@value, lastupdate=@lastupdate WHERE lang=@lang AND realm=@realm AND parameter=@parameter";
            upd.Parameters.Add(new SqlParameter("@lang", lang));
            upd.Parameters.Add(new SqlParameter("@realm", (int)realm));
            upd.Parameters.Add(new SqlParameter("@parameter", (int)parameter));
            upd.Parameters.Add(new SqlParameter("@value", value));
            upd.Parameters.Add(new SqlParameter("@lastupdate", DateTime.Now));

            upd.Connection = Base.conf.Open();
            upd.ExecuteReader();
            upd.Connection.Close();

        }

        /// <summary>
        /// Pega as últimas estatísticas
        /// </summary>
        public static List<Statistics> GetStatistics(int lang)
        {

            List<Statistics> lst = new List<Statistics>();

            SqlCommand sel = new SqlCommand();
            sel.CommandText = "SELECT realm, parameter FROM " + Base.conf.prefix + "[newsstatistics] WHERE lang=@lang ORDER BY position";
            sel.Parameters.Add(new SqlParameter("@lang", lang));
            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                // Pega as informações

                Realm r = (Realm)rdr.GetInt32(0);
                Parameter p = (Parameter)rdr.GetInt32(1);

                Statistics ns = LoadNewsStatistics(lang, r, p);
                if (ns != null)
                    lst.Add(ns);

            }

            rdr.Close();
            sel.Connection.Close();

            return lst;

        }

    }

}