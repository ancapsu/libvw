using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class Payment
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
        /// Mês
        /// </summary>
        int monthInt;

        /// <summary>
        /// Ano
        /// </summary>
        int yearInt;

        /// <summary>
        /// Número de artigos
        /// </summary>
        int numarticlesInt;

        /// <summary>
        /// Valor total
        /// </summary>
        double valueInt;

        /// <summary>
        /// Observação
        /// </summary>
        string observationInt;
        
        /// <summary>
        /// Endereço
        /// </summary>
        string addressInt;

        /// <summary>
        /// Pago
        /// </summary>
        DateTime payedInt;

        /// <summary>
        /// Transação
        /// </summary>
        string transidInt;

        /// <summary>
        /// Perfil do usuário
        /// </summary>
        Profile profileInt;

        /// <summary>
        /// Cria novo
        /// </summary>
        public Payment(Profile p)
        {

            idInt = Guid.NewGuid().ToString();
            dbassignedidInt = false;

            profileInt = p;

            monthInt = 1;
            yearInt = 2019;
            numarticlesInt = 0;
            valueInt = 0;
            observationInt = "";
            addressInt = "";
            payedInt = RacMsg.NullDateTime;
            transidInt = "";

        }

        /// <summary>
        /// Perfil de usuário
        /// </summary>
        public Profile profile
        {

            get { return profileInt; }

        }

        /// <summary>
        /// Id do perfil de usuário
        /// </summary>
        public string profileId
        {

            get { return profile.user.id; }

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
        /// Nome do usuário
        /// </summary>
        public string userName
        {

            get
            {

                if (profile != null && profile.user != null)
                    return profile.user.name;
                else
                    return "";

            }

        }

        /// <summary>
        /// Mês
        /// </summary>
        public int month
        {

            get { return monthInt; }
            set { monthInt = value; }

        }

        /// <summary>
        /// Ano
        /// </summary>
        public int year
        {

            get { return yearInt; }
            set { yearInt = value; }

        }

        /// <summary>
        /// Número de artigos
        /// </summary>
        public int numArticles
        {

            get { return numarticlesInt; }
            set { numarticlesInt = value; }

        }

        /// <summary>
        /// Valor total
        /// </summary>
        public double value
        {

            get { return valueInt; }
            set { valueInt = value; }

        }

        /// <summary>
        /// Endereço
        /// </summary>
        public string address
        {

            get { return addressInt; }
            set { addressInt = value; }

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
        /// Quando foi pago
        /// </summary>
        public DateTime payed
        {

            get { return payedInt; }
            set { payedInt = value; }

        }
        
        /// <summary>
        /// ID de transação bitcoin
        /// </summary>
        public string transactionId
        {

            get { return transidInt; }
            set { transidInt = value; }

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

            sel.CommandText = "SELECT month, year, numberarticles, value, observation, address, payed, transid FROM " + Base.conf.prefix + "[newspayment] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                idInt = id;
                monthInt = rdr.GetInt32(0);
                yearInt = rdr.GetInt32(1);
                numarticlesInt = rdr.GetInt32(2);
                valueInt = rdr.GetDouble(3);
                observationInt = rdr.GetString(4);
                addressInt= rdr.GetString(5);
                payedInt = rdr.GetDateTime(6);
                transidInt = rdr.GetString(7);
                
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
            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newspayment] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newspayment] (id, userid, month, year, numberarticles, value, observation, address, payed, transid) VALUES (@id, @userid, @month, @year, @numberarticles, @value, @observation, @address, @payed, @transid)";
                ins.Parameters.Add(new SqlParameter("@id", id));

                ins.Parameters.Add(new SqlParameter("@userid", profileId));
                ins.Parameters.Add(new SqlParameter("@month", monthInt));
                ins.Parameters.Add(new SqlParameter("@year", yearInt));
                ins.Parameters.Add(new SqlParameter("@numberarticles", numarticlesInt));
                ins.Parameters.Add(new SqlParameter("@value", valueInt));
                ins.Parameters.Add(new SqlParameter("@observation", observationInt));
                ins.Parameters.Add(new SqlParameter("@payed", payedInt));
                ins.Parameters.Add(new SqlParameter("@address", addressInt));
                ins.Parameters.Add(new SqlParameter("@transid", transidInt));

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
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newspayment] SET userid=@userid, month=@month, year=@year, numberarticles=@numberarticles, value=@value, observation=@observation, address=@address, payed=@payed, transid=@transid WHERE id=@id";

                upd.Parameters.Add(new SqlParameter("@userid", profileId));
                upd.Parameters.Add(new SqlParameter("@month", monthInt));
                upd.Parameters.Add(new SqlParameter("@year", yearInt));
                upd.Parameters.Add(new SqlParameter("@numberarticles", numarticlesInt));
                upd.Parameters.Add(new SqlParameter("@value", valueInt));
                upd.Parameters.Add(new SqlParameter("@observation", observationInt));
                upd.Parameters.Add(new SqlParameter("@payed", payedInt));
                upd.Parameters.Add(new SqlParameter("@address", addressInt));
                upd.Parameters.Add(new SqlParameter("@transid", transidInt));

                upd.Parameters.Add(new SqlParameter("@id", id));

                upd.Connection = Base.conf.Open();
                upd.ExecuteNonQuery();
                upd.Connection.Close();

                res = true;
                dbassignedidInt = true;

            }

            return res;

        }
        
    }

}