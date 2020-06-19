using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Summary description for article
    /// </summary>
    public class Profile
    {

        /// <summary>
        /// Endereço de bitcoin
        /// </summary>
        string bitcoinInt;

        /// <summary>
        /// Última modificação
        /// </summary>
        DateTime lastmodifiedInt;

        /// <summary>
        /// Papeis do cara
        /// </summary>
        List<ProfileRole> rolesInt;

        /// <summary>
        /// Pontos do cara
        /// </summary>
        List<ProfilePoints> pointsInt;

        /// <summary>
        /// Medalhas do cara
        /// </summary>
        List<ProfileMedals> medalsInt;

        /// <summary>
        /// Pagamentos
        /// </summary>
        List<Payment> paymentsInt;

        /// <summary>
        /// Usuário
        /// </summary>
        BaseUser usrInt;

        /// <summary>
        /// Id do adm
        /// </summary>
        public static string AdministratorId = "23b0f439-72a2-4ea1-8f2f-f31e44d8fc2e";

        /// <summary>
        /// Perfil do admin
        /// </summary>
        public static Profile Administrator
        {

            get { return Profile.LoadProfile(AdministratorId); }

        }

        /// <summary>
        /// Cria novo
        /// </summary>
        public Profile(BaseUser u)
        {

            usrInt = u;

            bitcoinInt = "";
            lastmodifiedInt = RacMsg.NullDateTime;

            pointsInt = null;
            medalsInt = null;
            paymentsInt = null;
            rolesInt = null;

        }

        /// <summary>
        /// Usuário
        /// </summary>
        public BaseUser user
        {

            get { return usrInt; }

        }

        /// <summary>
        /// Endereço de bitcoin
        /// </summary>
        public string bitcoin
        {

            get { return bitcoinInt; }
            set { bitcoinInt = value; }

        }

        /// <summary>
        /// Está na lista de emails de newsletter
        /// </summary>
        public bool newsLetter
        {

            get { return NewsLetter.IsEmailInNewsLetter(user.email); }

            set
            {

                if (value)
                    NewsLetter.AddNewsLetterEmail(user.email);
                else
                    NewsLetter.RemoveNewsLetterEmail(user.email);

            }

        }

        /// <summary>
        /// Última modificação
        /// </summary>
        public DateTime lastModified
        {

            get { return lastmodifiedInt; }
            set { lastmodifiedInt = value; }

        }

        /// <summary>
        /// Retorna os pontos
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        ProfilePoints GetPoints(Medal.Dimension d)
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

            ProfilePoints p = GetPoints(d);
            if (p != null)
                return p.points;
            else
                return 0;

        }

        /// <summary>
        /// Pega os pontos
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        void SetPointNumberForDimensions(Medal.Dimension d, decimal num)
        {

            ProfilePoints p = GetPoints(d);
            if (p != null)
            {

                p.points = num;
                p.Save();

            }
            else
            {

                ProfilePoints pp = new ProfilePoints(this);
                pp.dimension = d;
                pp.points = num;
                points.Add(pp);
                pp.Save();

            }

        }

        /// <summary>
        /// Pontos de experiência
        /// </summary>
        public decimal xp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.TotalExperience); }
            set { SetPointNumberForDimensions(Medal.Dimension.TotalExperience, value); }

        }

        /// <summary>
        /// Pontos de experiência para o próximo nível
        /// </summary>
        public decimal nextXp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.TotalExperience, xp);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Medalha de qualificação total
        /// </summary>
        public Medal totalQualification
        {

            get { return Medal.MajorMedalForPoints(Medal.Dimension.TotalExperience, xp); }

        }

        /// <summary>
        /// Nome da medalha de qualificação total
        /// </summary>
        public string GetTotalQualification(RacMsg msgs)
        {

            Medal m = totalQualification;

            if (m != null) return msgs.Get(m.nameMsg);

            return ""; 

        }

        /// <summary>
        /// Medalha de qualificação de editor
        /// </summary>
        public Medal editorQualification
        {

            get { return Medal.MajorMedalForPoints(Medal.Dimension.EditorExperience, ap); }

        }

        /// <summary>
        /// Nome da medalha de qualificação de editor
        /// </summary>
        public string GetEditorQualification(RacMsg msgs)
        {

            Medal m = editorQualification;

            if (m != null) return msgs.Get(m.nameMsg);

            return "";

        }

        /// <summary>
        /// Medalha de qualificação de reporter
        /// </summary>
        public Medal reporterQualification
        {

            get { return Medal.MajorMedalForPoints(Medal.Dimension.ReporterExperience, rp); }

        }

        /// <summary>
        /// Nome da medalha de qualificação de reporter
        /// </summary>
        public string GetReporterQualification(RacMsg msgs)
        {

            Medal m = reporterQualification;

            if (m != null) return msgs.Get(m.nameMsg);

            return "";

        }

        /// <summary>
        /// Pontos de editor
        /// </summary>
        public decimal ap
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.EditorExperience); }
            set { SetPointNumberForDimensions(Medal.Dimension.EditorExperience, value); }

        }

        /// <summary>
        /// Pontos de editor para o próximo nível
        /// </summary>
        public decimal nextAp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.EditorExperience, ap);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Pontos de repórter
        /// </summary>
        public decimal rp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.ReporterExperience); }
            set { SetPointNumberForDimensions(Medal.Dimension.ReporterExperience, value); }

        }

        /// <summary>
        /// Pontos de repórter para o próximo nível
        /// </summary>
        public decimal nextRp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.ReporterExperience, rp);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Pontos de originalidade
        /// </summary>
        public decimal op
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Originality); }
            set { SetPointNumberForDimensions(Medal.Dimension.Originality, value); }

        }

        /// <summary>
        /// Pontos de originalidade para o próximo nível
        /// </summary>
        public decimal nextOp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.Originality, op);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Pontos de qualidade
        /// </summary>
        public decimal qp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Quality); }
            set { SetPointNumberForDimensions(Medal.Dimension.Quality, value); }

        }

        /// <summary>
        /// Pontos de qualidade para o próximo nível
        /// </summary>
        public decimal nextQp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.Quality, qp);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Pontos de poliglota
        /// </summary>
        public decimal tp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Polyglot); }
            set { SetPointNumberForDimensions(Medal.Dimension.Polyglot, value); }

        }

        /// <summary>
        /// Pontos de poliglota para o próximo nível
        /// </summary>
        public decimal nextTp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.Polyglot, tp);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Pontos de urgência
        /// </summary>
        public decimal up
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Speed); }
            set { SetPointNumberForDimensions(Medal.Dimension.Speed, value); }

        }

        /// <summary>
        /// Pontos de urgência para o próximo nível
        /// </summary>
        public decimal nextUp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.Speed, up);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Pontos de adequção
        /// </summary>
        public decimal sp
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Standartization); }
            set { SetPointNumberForDimensions(Medal.Dimension.Standartization, value); }

        }

        /// <summary>
        /// Pontos de adequção para o próximo nível
        /// </summary>
        public decimal nextSp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.Standartization, sp);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Pontos de acumulação
        /// </summary>
        public decimal ep
        {

            get { return GetPointNumberForDimensions(Medal.Dimension.Accumulator); }
            set { SetPointNumberForDimensions(Medal.Dimension.Accumulator, value); }

        }

        /// <summary>
        /// Pontos de acumulação para o próximo nível
        /// </summary>
        public decimal nextEp
        {

            get
            {

                Medal m = Medal.NextMedalsForPoints(Medal.Dimension.Accumulator, ep);
                if (m != null)
                    return m.requiredPoints;

                return 0;

            }

        }

        /// <summary>
        /// Papéis
        /// </summary>
        public List<ProfileRole> roles
        {

            get
            {

                if (rolesInt == null)
                    rolesInt = ProfileRole.LoadAllFrom(this);

                return rolesInt;

            }

            set
            {

                rolesInt = value;

            }

        }

        /// <summary>
        /// Tem perfil?
        /// </summary>
        /// <param name="r"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public bool HasProfileRole(ProfileRole.Role r, RacMsg.Language l)
        {

            if (l == RacMsg.Language.Indifferent)
            {

                for (int i = 0; i < roles.Count; i++)
                    if (roles[i].role == r)
                        return true;

            }
            else
            {

                for (int i = 0; i < roles.Count; i++)
                    if (roles[i].role == r && roles[i].language == l)
                        return true;

            }

            return false;

        }

        /// <summary>
        /// Tem perfil?
        /// </summary>
        /// <param name="r"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public void RemoveProfileRole(ProfileRole.Role r, RacMsg.Language l)
        {

            if (l == RacMsg.Language.Indifferent)
            {

                for (int i = 0; i < roles.Count; i++)
                    if (roles[i].role == r)
                    {

                        roles.RemoveAt(i);
                        return;

                    }

            }
            else
            {

                for (int i = 0; i < roles.Count; i++)
                    if (roles[i].role == r && roles[i].language == l)
                    {

                        roles.RemoveAt(i);
                        return;

                    }

            }

        }

        /// <summary>
        /// Tem perfil?
        /// </summary>
        /// <param name="r"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public void AddProfileRole(ProfileRole.Role r, RacMsg.Language l)
        {

            if (l == RacMsg.Language.Indifferent)
            {

                for (int i = 0; i < roles.Count; i++)
                    if (roles[i].role == r)
                        return;

            }
            else
            {

                for (int i = 0; i < roles.Count; i++)
                    if (roles[i].role == r && roles[i].language == l)
                        return;

            }

            ProfileRole p = new ProfileRole(this);
            p.role = r;
            p.language = l;

            roles.Add(p);

        }

        /// <summary>
        /// Tradutor para inglês
        /// </summary>
        public bool translatorEnglish
        {

            get { return HasProfileRole(ProfileRole.Role.Translator, RacMsg.Language.English); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Translator, RacMsg.Language.English);
                else
                    RemoveProfileRole(ProfileRole.Role.Translator, RacMsg.Language.English);

            }

        }

        /// <summary>
        /// Tradutor para portugues
        /// </summary>
        public bool translatorPortuguese
        {

            get { return HasProfileRole(ProfileRole.Role.Translator, RacMsg.Language.Portugues); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Translator, RacMsg.Language.Portugues);
                else
                    RemoveProfileRole(ProfileRole.Role.Translator, RacMsg.Language.Portugues);

            }

        }

        /// <summary>
        /// Tradutor para espanhol
        /// </summary>
        public bool translatorSpanish
        {

            get { return HasProfileRole(ProfileRole.Role.Translator, RacMsg.Language.Espanol); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Translator, RacMsg.Language.Espanol);
                else
                    RemoveProfileRole(ProfileRole.Role.Translator, RacMsg.Language.Espanol);

            }

        }


        /// <summary>
        /// Revisor em inglês
        /// </summary>
        public bool revisorEnglish
        {

            get { return HasProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.English); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.English);
                else
                    RemoveProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.English);

            }

        }

        /// <summary>
        /// Revisor em portugues
        /// </summary>
        public bool revisorPortuguese
        {

            get { return HasProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.Portugues); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.Portugues);
                else
                    RemoveProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.Portugues);

            }

        }

        /// <summary>
        /// Revisor em espanhol
        /// </summary>
        public bool revisorSpanish
        {

            get { return HasProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.Espanol); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.Espanol);
                else
                    RemoveProfileRole(ProfileRole.Role.Revisor, RacMsg.Language.Espanol);

            }

        }

        /// <summary>
        /// Narrador em inglês
        /// </summary>
        public bool narratorEnglish
        {

            get { return HasProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.English); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.English);
                else
                    RemoveProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.English);

            }

        }

        /// <summary>
        /// Narrador em portugues
        /// </summary>
        public bool narratorPortuguese
        {

            get { return HasProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.Portugues); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.Portugues);
                else
                    RemoveProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.Portugues);

            }

        }

        /// <summary>
        /// Narrador em espanhol
        /// </summary>
        public bool narratorSpanish
        {

            get { return HasProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.Espanol); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.Espanol);
                else
                    RemoveProfileRole(ProfileRole.Role.Narrator, RacMsg.Language.Espanol);

            }

        }

        /// <summary>
        /// Produtor em inglês
        /// </summary>
        public bool producerEnglish
        {

            get { return HasProfileRole(ProfileRole.Role.Producer, RacMsg.Language.English); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Producer, RacMsg.Language.English);
                else
                    RemoveProfileRole(ProfileRole.Role.Producer, RacMsg.Language.English);

            }

        }

        /// <summary>
        /// Produtor em portugues
        /// </summary>
        public bool producerPortuguese
        {

            get { return HasProfileRole(ProfileRole.Role.Producer, RacMsg.Language.Portugues); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Producer, RacMsg.Language.Portugues);
                else
                    RemoveProfileRole(ProfileRole.Role.Producer, RacMsg.Language.Portugues);

            }

        }

        /// <summary>
        /// Produtor em espanhol
        /// </summary>
        public bool producerSpanish
        {

            get { return HasProfileRole(ProfileRole.Role.Producer, RacMsg.Language.Espanol); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Producer, RacMsg.Language.Espanol);
                else
                    RemoveProfileRole(ProfileRole.Role.Producer, RacMsg.Language.Espanol);

            }

        }
        
        /// <summary>
        /// Pontos
        /// </summary>
        public List<ProfilePoints> points
        {

            get
            {

                if (pointsInt == null)
                    pointsInt = ProfilePoints.LoadAllFrom(this);

                return pointsInt;

            }

            set
            {

                pointsInt = value;

            }

        }

        /// <summary>
        /// Medalha
        /// </summary>
        public List<ProfileMedals> medals
        {

            get
            {

                if (medalsInt == null)
                    medalsInt = ProfileMedals.LoadAllFrom(this);

                return medalsInt;

            }

            set
            {

                medalsInt = value;

            }

        }

        /// <summary>
        /// Pega medalha
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProfileMedals GetProfileMedal(string id)
        {

            for (int i = 0; i < medals.Count; i++)
                if (medals[i].medalsId == id)
                    return medals[i];

            return null;

        }

        /// <summary>
        /// Pagamentos
        /// </summary>
        public List<Payment> payments
        {

            get
            {

                if (paymentsInt == null)
                    paymentsInt = LoadPayments();

                return paymentsInt;

            }

            set
            {

                paymentsInt = value;

            }

        }

        /// <summary>
        /// Pagamentos
        /// </summary>
        /// <returns></returns>
        List<Payment> LoadPayments()
        {

            List<string> ids = Base.conf.LoadStringList(user.id, "newspayment", "userid", "id", "payed");
            List<Payment> lst = new List<Payment>();

            for (int i = 0; i < ids.Count; i++)
            {

                Payment npc = new Payment(this);
                if (npc.Load(ids[i]))
                    lst.Add(npc);

            }

            return lst;

        }

        /// <summary>
        /// Pega medalha
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Payment GetPayment(string id)
        {

            for (int i = 0; i < payments.Count; i++)
                if (payments[i].id == id)
                    return payments[i];

            return null;

        }

        /// <summary>
        /// Carrega o artigo pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Profile Load(BaseUser usr)
        {

            Profile prf = null;

            SqlCommand sel = new SqlCommand();
            sel.CommandText = "SELECT bitcoin, lastmodified FROM " + Base.conf.prefix + "[newsprofile] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", usr.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (rdr.Read())
            {

                // Pega as informações

                prf = new Profile(usr);

                // Pega as informações

                prf.bitcoinInt = rdr.GetString(0);
                prf.lastmodifiedInt = rdr.GetDateTime(1);

            }

            rdr.Close();
            sel.Connection.Close();

            return prf;

        }

        /// <summary>
        /// Salva a ticket
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            bool res = false;

            SqlCommand sel = new SqlCommand();
            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[newsprofile] WHERE id=@id";
            sel.Parameters.Add(new SqlParameter("@id", user.id));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            if (!rdr.Read())
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand ins = new SqlCommand();
                ins.CommandText = "INSERT INTO " + Base.conf.prefix + "[newsprofile] (id, bitcoin, lastmodified) VALUES (@id, @bitcoin, @lastmodified)";
                ins.Parameters.Add(new SqlParameter("@id", user.id));

                ins.Parameters.Add(new SqlParameter("@bitcoin", bitcoinInt));
                ins.Parameters.Add(new SqlParameter("@lastmodified", lastmodifiedInt));

                ins.Connection = Base.conf.Open();
                ins.ExecuteNonQuery();
                ins.Connection.Close();

                res = true;

            }
            else
            {

                rdr.Close();
                sel.Connection.Close();

                SqlCommand upd = new SqlCommand();
                upd.CommandText = "UPDATE " + Base.conf.prefix + "[newsprofile] SET bitcoin=@bitcoin, lastmodified=@lastmodified WHERE id=@id";
                upd.Parameters.Add(new SqlParameter("@id", user.id));

                upd.Parameters.Add(new SqlParameter("@bitcoin", bitcoinInt));
                upd.Parameters.Add(new SqlParameter("@lastmodified", lastmodifiedInt));

                upd.Connection = Base.conf.Open();
                upd.ExecuteNonQuery();
                upd.Connection.Close();

                res = true;

            }

            if (res)
            {

                if (rolesInt != null)
                    ProfileRole.SaveAll(this, roles);

                if (pointsInt != null)
                    foreach (ProfilePoints pts in points) { pts.Save(); }

                if (medalsInt != null)
                    foreach (ProfileMedals med in medals) { med.Save(); }

                if (paymentsInt != null)
                    foreach (Payment pay in payments) { pay.Save(); }

            }

            return res;

        }

        /// <summary>
        /// Apaga a ticket
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            // Apaga todos os pontos

            SqlCommand del0 = new SqlCommand();
            del0.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsprofilepoints] WHERE userid=@userid";
            del0.Parameters.Add(new SqlParameter("@userid", user.id));
            del0.Connection = Base.conf.Open();
            del0.ExecuteNonQuery();
            del0.Connection.Close();

            // Apaga todas as medalhas

            SqlCommand del1 = new SqlCommand();
            del1.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsprofilemedals] WHERE userid=@userid";
            del1.Parameters.Add(new SqlParameter("@userid", user.id));
            del1.Connection = Base.conf.Open();
            del1.ExecuteNonQuery();
            del1.Connection.Close();

            // Apaga todas as medalhas

            SqlCommand del2 = new SqlCommand();
            del2.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsprofilerole] WHERE id=@userid";
            del2.Parameters.Add(new SqlParameter("@userid", user.id));
            del2.Connection = Base.conf.Open();
            del2.ExecuteNonQuery();
            del2.Connection.Close();

            // Apaga o perfil

            SqlCommand del = new SqlCommand();
            del.CommandText = "DELETE FROM " + Base.conf.prefix + "[newsprofile] WHERE id=@id";
            del.Parameters.Add(new SqlParameter("@id", user.id));
            del.Connection = Base.conf.Open();
            del.ExecuteNonQuery();
            del.Connection.Close();

            return true;

        }

        /// <summary>
        /// Verifica se as medalhas estão ok
        /// </summary>
        public void CheckMedals()
        {

            CheckMedals(Medal.Dimension.TotalExperience);
            CheckMedals(Medal.Dimension.EditorExperience);
            CheckMedals(Medal.Dimension.ReporterExperience);
            CheckMedals(Medal.Dimension.Originality);
            CheckMedals(Medal.Dimension.Quality);
            CheckMedals(Medal.Dimension.Speed);
            CheckMedals(Medal.Dimension.Standartization);
            CheckMedals(Medal.Dimension.Accumulator);

        }

        /// <summary>
        /// Verifica se as medalhas estão ok
        /// </summary>
        public void CheckMedals(Medal.Dimension d)
        {

            List<Medal> lst = Medal.AllMedalsForPoints(d, GetPointNumberForDimensions(d));

            for (int i = 0; i < lst.Count; i++)
            {

                ProfileMedals pm = GetProfileMedal(lst[i].id);
                if (pm == null)
                {

                    pm = new ProfileMedals(this);
                    pm.medalsId = lst[i].id;
                    pm.awarded = DateTime.Now;
                    pm.Save();

                    medals.Add(pm);

                }

            }

        }

        public static Profile LoadProfile(string id)
        {

            RacLib.BaseUser user = RacLib.BaseUserSource.source.LoadUser(id);
            if (user != null)
            {

                LibVisLib.Profile p = LibVisLib.Profile.Load(user);
                if (p == null)
                {

                    byte[] fileBytes = new byte[0];

                    try
                    {

                        string imagePath = Base.conf.applicationPath.TrimEnd('\\').TrimEnd('/') + "/wwwroot/dist/img/default-avatar.png";
                        fileBytes = File.ReadAllBytes(imagePath);

                    }
                    catch
                    {

                        string imagePath = Base.conf.applicationPath.TrimEnd('\\').TrimEnd('/') + "/img/default-avatar.png";
                        fileBytes = File.ReadAllBytes(imagePath);

                    }

                    p = new Profile(user);
                    p.lastModified = DateTime.Now;
                    p.Save();

                    byte[] str = fileBytes;

                    MemoryStream ms = new MemoryStream(str, 0, str.Length);
                    ms.Write(str, 0, str.Length);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);

                    string path = Base.conf.tempImageFilePath + "\\u-" + p.user.id + ".jpg";

                    img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                }

                return p;

            }

            return null;

        }

        public enum ProfileAction { RegisterTarget, ReviseTarget, VoteTarget, TargetUsed, RegisterShortNote, ShortNoteApproved, RegisterNote, NoteApproved, RegisterArticle, ArticleApproved, ReviseArticle, ArticlePublishedInVideo, VoteArticle };

        public void RegisterAction(ProfileAction p)
        {

            switch (p)
            {

                case ProfileAction.RegisterTarget:
                    xp = xp + 100;
                    ap = ap + 100;
                    break;

                case ProfileAction.ReviseTarget:
                    xp = xp + 100;
                    ap = ap + 100;
                    break;

                case ProfileAction.TargetUsed:
                    xp = xp + 300;
                    ap = ap + 300;
                    break;

                case ProfileAction.RegisterShortNote:
                    xp = xp + 100;
                    rp = rp + 100;
                    break;

                case ProfileAction.RegisterNote:
                    xp = xp + 500;
                    rp = rp + 500;
                    break;

                case ProfileAction.RegisterArticle:
                    xp = xp + 1000;
                    rp = rp + 1000;
                    break;

                case ProfileAction.ShortNoteApproved:
                    xp = xp + 100;
                    rp = rp + 100;
                    break;

                case ProfileAction.NoteApproved:
                    xp = xp + 500;
                    rp = rp + 500;
                    break;

                case ProfileAction.ArticleApproved:
                    xp = xp + 1000;
                    rp = rp + 1000;
                    break;

                case ProfileAction.VoteTarget:
                    xp = xp + 10;
                    ap = ap + 10;
                    break;

                case ProfileAction.ArticlePublishedInVideo:
                    xp = xp + 2000;
                    rp = rp + 2000;
                    break;

                case ProfileAction.ReviseArticle:
                    xp = xp + 100;
                    rp = rp + 100;
                    break;

                case ProfileAction.VoteArticle:
                    xp = xp + 20;
                    rp = rp + 20;
                    break;

            }

        }

        /// <summary>
        /// Desconsidera esse prêmio
        /// </summary>
        /// <param name="awardId"></param>
        public void RemoveAward(string awardId)
        {

            Award a = Award.LoadAward(awardId);
            if (a != null)
            {

                xp = xp - a.xp;
                ap = ap - a.ap;
                rp = rp - a.rp;
                op = op - a.op;
                qp = qp - a.qp;
                tp = tp - a.tp;
                up = up - a.up;
                sp = sp - a.sp;
                ep = ep - a.ep;

                Save();

            }

        }

        /// <summary>
        /// Considera esse prêmio
        /// </summary>
        /// <param name="awardId"></param>
        public void AddAward(string awardId)
        {

            Award a = Award.LoadAward(awardId);
            if (a != null)
            {

                xp = xp + a.xp;
                ap = ap + a.ap;
                rp = rp + a.rp;
                op = op + a.op;
                qp = qp + a.qp;
                tp = tp + a.tp;
                up = up + a.up;
                sp = sp + a.sp;
                ep = ep + a.ep;

                Save();

            }

        }

        /// <summary>
        /// Todos os perfis cadastrados
        /// </summary>
        public static List<StringString> listAllUsers
        {

            get
            {

                List<StringString> lst = new List<StringString>();

                SqlCommand sel = new SqlCommand();
                sel.CommandText = "SELECT id, name FROM " + Base.conf.prefix + "[racwebuser] ORDER BY name";

                sel.Connection = Base.conf.Open();
                SqlDataReader rdr = sel.ExecuteReader();

                while (rdr.Read())
                {

                    StringString s = new StringString();
                    s.id = rdr.GetString(0);
                    s.name = rdr.GetString(1);

                    lst.Add(s);

                }

                rdr.Close();
                sel.Connection.Close();

                return lst;

            }

        }

        /// <summary>
        /// Todos os perfis cadastrados
        /// </summary>
        public static List<BaseUser> GetUsersWithRole(ProfileRole.Role r)
        {

            List<BaseUser> lst = new List<BaseUser>();
            List<string> ids = new List<string>();

            SqlCommand sel = new SqlCommand();
            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[racwebuser] WHERE id IN (SELECT id  FROM " + Base.conf.prefix + "[newsprofilerole] WHERE role=@role)";
            sel.Parameters.Add(new SqlParameter("@role", (int)r));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            while (rdr.Read())
            {

                string s = rdr.GetString(0);
                ids.Add(s);

            }

            rdr.Close();
            sel.Connection.Close();

            for(int i = 0; i< ids.Count; i++)
            {

                BaseUser u = BaseUserSource.source.LoadUser(ids[i]);
                if (u != null)
                    lst.Add(u);

            }

            return lst;

        }

        /// <summary>
        /// É revisor
        /// </summary>
        public bool revisor
        {

            get
            {

                return revisorEnglish || revisorPortuguese || revisorSpanish;

            }

        }

        /// <summary>
        /// É narrador
        /// </summary>
        public bool narrator
        {

            get
            {

                return narratorEnglish || narratorPortuguese || narratorSpanish;

            }

        }

        /// <summary>
        /// É produtor
        /// </summary>
        public bool producer
        {

            get
            {

                return producerEnglish || producerPortuguese || producerSpanish;

            }

        }

        /// <summary>
        /// Patrocinador
        /// </summary>
        public bool sponsor
        {

            get { return HasProfileRole(ProfileRole.Role.Sponsor, RacMsg.Language.Indifferent); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Sponsor, RacMsg.Language.Indifferent);
                else
                    RemoveProfileRole(ProfileRole.Role.Sponsor, RacMsg.Language.Indifferent);

            }

        }
        
        /// <summary>
        /// Equipe
        /// </summary>
        public bool staff
        {

            get { return HasProfileRole(ProfileRole.Role.Staff, RacMsg.Language.Indifferent); }

            set
            {

                if (value)
                    AddProfileRole(ProfileRole.Role.Staff, RacMsg.Language.Indifferent);
                else
                    RemoveProfileRole(ProfileRole.Role.Staff, RacMsg.Language.Indifferent);

            }

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Profile> GetUsers(int ini, int max, string search)
        {

            List<string> ids = new List<string>();

            SqlCommand sel = new SqlCommand();

            sel.CommandText = "SELECT id FROM " + Base.conf.prefix + "[racwebuser] WHERE email LIKE @email OR name LIKE @name ORDER BY name";
            sel.Parameters.Add(new SqlParameter("@email", "%" + search + "%"));
            sel.Parameters.Add(new SqlParameter("@name", "%" + search + "%"));

            sel.Connection = Base.conf.Open();
            SqlDataReader rdr = sel.ExecuteReader();

            int count = 0;

            while (rdr.Read())
            {

                if (count >= ini && count < ini + max)
                    ids.Add(rdr.GetString(0));
                else if (count >= ini + max)
                    break;

                count++;

            }

            rdr.Close();
            sel.Connection.Close();

            List<Profile> lst = new List<Profile>();
            for (int i = 0; i < ids.Count; i++)
            {

                lst.Add(LoadProfile(ids[i]));

            }

            return lst;

        }

        /// <summary>
        /// Carrega a página
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetTotalUsers(string search)
        {

            try
            {

                List<string> ids = new List<string>();

                SqlCommand sel = new SqlCommand();

                sel.CommandText = "SELECT COUNT(*) FROM " + Base.conf.prefix + "[racwebuser] WHERE email LIKE @email OR name LIKE @name";
                sel.Parameters.Add(new SqlParameter("@email", "%" + search + "%"));
                sel.Parameters.Add(new SqlParameter("@name", "%" + search + "%"));

                sel.Connection = Base.conf.Open();
                SqlDataReader rdr = sel.ExecuteReader();

                int count = 0;

                if (rdr.Read())
                {

                    count = rdr.GetInt32(0);

                }

                rdr.Close();
                sel.Connection.Close();

                return count;

            }
            catch (Exception ex)
            {

                return 0;

            }

        }

    }

}