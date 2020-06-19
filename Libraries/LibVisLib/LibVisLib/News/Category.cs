using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class Category
    {

        /// <summary>
        /// Label
        /// </summary>
        string labelInt;

        /// <summary>
        /// Nome
        /// </summary>
        int namemsgInt;

        /// <summary>
        /// Descrição
        /// </summary>
        int descriptionmsgInt;
        
        /// <summary>
        /// É categoria principal
        /// </summary>
        int mainInt;
        
        /// <summary>
        /// Cria novo
        /// </summary>
        public Category()
        {

            labelInt = "";
            namemsgInt = 0;            
            descriptionmsgInt = 0;            
            mainInt = 0;

        }

        /// <summary>
        /// Label da categoria
        /// </summary>
        public string label
        {

            get { return labelInt; }
        
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
        /// É categoria principal
        /// </summary>
        public bool main
        {

            get { return mainInt != 0; }
            set { mainInt = value ? 1 : 0; }

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Load(string label)
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT name, description, main FROM " + Base.conf.prefix + "[newscategory] WHERE label=@label";
            sel.Parameters.Add(new SqlParameter("@label", label));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações
                                
                labelInt = label;
                namemsgInt = rdr.GetInt32(0);
                descriptionmsgInt = rdr.GetInt32(1);
                mainInt = rdr.GetInt32(2);

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
        public static Category LoadCategory(string label)
        {

            for (int i = 0; i < categories.Count; i++)
                if (categories[i].label == label)
                    return categories[i];

            return null;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsMain(string label)
        {

            for (int i = 0; i < categories.Count; i++)
                if (categories[i].label == label)
                    return categories[i].main;

            return false;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetNameForLabel(string label)
        {

            for (int i = 0; i < categories.Count; i++)
                if (categories[i].label == label)
                    return categories[i].nameMsg;

            return (int)RacMsg.Id.Undefined;

        }

        /// <summary>
        /// Medalhas lista
        /// </summary>
        static List<Category> categoriesInt = null;

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Category> categories
        {

            get
            {

                if (categoriesInt == null)
                {

                    List<string> ids = new List<string>();

                    SqlCommand sel = new SqlCommand();
                    sel.CommandText = "SELECT label FROM " + Base.conf.prefix + "[newscategory] ORDER BY position";
                    sel.Connection = Base.conf.Open();
                    SqlDataReader rdr = sel.ExecuteReader();

                    while (rdr.Read())
                    {

                        ids.Add(rdr.GetString(0));

                    }

                    rdr.Close();
                    sel.Connection.Close();

                    List<Category> lst = new List<Category>();
                    for (int i = 0; i < ids.Count; i++)
                    {

                        Category m = new Category();
                        if (m.Load(ids[i]))
                            lst.Add(m);

                    }

                    categoriesInt = lst;

                }

                return categoriesInt;

            }

        }

    }

}