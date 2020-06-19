using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class ProfileRole
    {

        /// <summary>
        /// Papel do cara
        /// </summary>
        int roleInt;

        /// <summary>
        /// Funções
        /// </summary>
        public enum Role { Undefined, Approver, Translator, Revisor, Narrator, Producer, Sponsor, Staff };

        /// <summary>
        /// Lingua
        /// </summary>
        int languageInt; // Em RacMsg.Language

        /// <summary>
        /// Perfil
        /// </summary>
        Profile profInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public ProfileRole(Profile p)
        {

            profInt = p;
            roleInt = 0;
            languageInt = 0;

        }

        /// <summary>
        /// Notícia
        /// </summary>
        public Profile profile
        {

            get { return profInt; }

        }

        /// <summary>
        /// Papel
        /// </summary>
        public Role role
        {

            get { return (Role)roleInt; }
            set { roleInt = (int)value; }

        }

        /// <summary>
        /// Papel
        /// </summary>
        public RacMsg.Language language
        {

            get { return (RacMsg.Language)languageInt; }
            set { languageInt = (int)value; }

        }
        
        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<ProfileRole> LoadAllFrom(Profile p)
        {

            List<ProfileRole> lst = new List<ProfileRole>();

            SqlCommand sel = new SqlCommand();
            
            sel.CommandText = "SELECT role, language FROM " + Base.conf.prefix + "[newsprofilerole] WHERE id=@userid";
            sel.Parameters.Add(new SqlParameter("@userid", p.user.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                // Pega as informações

                ProfileRole pm = new ProfileRole(p);

                pm.roleInt = rdr.GetInt32(0);
                pm.languageInt = rdr.GetInt32(1);

                lst.Add(pm);

            }

            rdr.Close();
            sel.Connection.Close();

            return lst;

        }

        /// <summary>
        /// Salva todo mundo
        /// </summary>
        /// <returns></returns>
        public static void SaveAll(Profile p, List<ProfileRole> lst)
        {

            // Vê se já existe

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsprofilerole] WHERE id=@userid";
            del.Parameters.Add(new SqlParameter("@userid", p.user.id));

            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();

            SqlCommand ins = new SqlCommand();
            ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsprofilerole] (id, role, language) VALUES (@userid, @role, @language)";
            ins.Parameters.Add(new SqlParameter("@userid", p.user.id));

            SqlParameter parRole = new SqlParameter("@role", 0);
            ins.Parameters.Add(parRole);

            SqlParameter parLanguage = new SqlParameter("@language", 0);
            ins.Parameters.Add(parLanguage);

            ins.Connection = del.Connection;

            for (int i = 0; i < lst.Count; i++)
            {

                parRole.Value = lst[i].roleInt;
                parLanguage.Value = lst[i].languageInt;

                ins.ExecuteNonQuery();

            }

            ins.Connection.Close();
            
        }

    }

}