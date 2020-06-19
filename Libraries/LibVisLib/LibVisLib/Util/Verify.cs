using System;
using System.Text;
using System.Text.RegularExpressions;
using RacLib;

namespace LibVisLib
{

    /// <summary>
    /// Validation
    /// </summary>
    public class Verify
    {

        /// <summary>
        /// Não é criado... tudo estático
        /// </summary>
        private Verify()
        {
        }

        /// <summary>
        /// Retira espaços
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NonPritableToSpace(string s)
        {

            string r = "";

            for (int i = 0; i < s.Length; i++)
            {

                if (s[i] == '\n')
                {

                    r += s[i];

                }
                else
                {

                    if (Char.IsWhiteSpace(s[i]) || s[i] == '\t' || ((int)s[i] < 32))
                    {

                        r += " ";

                    }
                    else
                    {

                        r += s[i];

                    }

                }

            }

            return r;

        }

        /// <summary>
        /// VerifyChars apenas estes
        /// </summary>
        /// <param name="a">Campo de entrada</param>
        /// <param name="perm">Caracteres aceitos</param>
        /// <param name="acc_enter">AcceptChars enter</param>
        /// <returns>String após validação</returns>
        static string VerifyChars(string a, string perm)
        {

            if (a == null || a == "")
                return "";

            string b = "";

            for (int i = 0; i < a.Length; i++)
            {

                if (perm.IndexOf(a.Substring(i, 1)) >= 0)
                    b += a.Substring(i, 1);

            }

            return b;

        }

        /// <summary>
        /// AcceptChars essa string
        /// </summary>
        /// <param name="a"></param>
        /// <param name="perm"></param>
        /// <returns></returns>
        static bool AcceptChars(string a, string perm)
        {

            if (a == null)
                return false;

            for (int i = 0; i < a.Length; i++)
            {

                if (perm.IndexOf(a.Substring(i, 1)) < 0)
                    return false;

            }

            return true;

        }

        /// <summary>
        /// AcceptChars essa string
        /// </summary>
        /// <param name="a"></param>
        /// <param name="perm"></param>
        /// <returns></returns>
        static bool AcceptChars(string a, string perm, ref string p)
        {

            bool ret = true;

            for (int i = 0; i < a.Length; i++)
            {

                if (perm.IndexOf(a.Substring(i, 1)) < 0)
                {

                    if (((int)(a.Substring(i, 1)[0])) < 8000)
                    {

                        if (!p.Contains(a.Substring(i, 1)))
                            p += a.Substring(i, 1);

                        ret = false;

                    }

                }

            }

            return ret;

        }

        /// <summary>
        /// Tabela asci até o espaço
        /// </summary>
        static string[] lowcname = { "NUL (\x00)", "SOH (\x01)", "STX (\x02)", "ETX (\x03)", "EOT (\x04)", "ENQ (\x05)", "ACK (\x06)", "BEL (\x07)", "BS (\x08)", "TAB (\x09)", "LF (\x0a)",
                                     "VT (\x0b)", "FF (\x0c)", "CR (\x0d)", "SO (\x0e)", "SI (\x0f)", "DLE (\x10)", "DC1 (\x11)", "DC2 (\x12)", "DC3 (\x13)", "DC4 (\x14)", "NAK (\x15)",
                                     "SYN (\x16)", "ETB (\x17)", "CAN (\x18)", "EM (\x19)", "SUB (\x1a)", "ESC (\x1b)", "FS (\x1c)", "GS (\x1d)", "RS (\x1e)", "US (\x1f)", "Space (\x20)" };

        /// <summary>
        /// Nome do caractere
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string CharName(char c)
        {

            if (c <= 32) return lowcname[(int)c];

            if (c == '\'')
                return "Plic (')";

            return "'" + c + "'";

        }

        /// <summary>
        /// Caraceres inválido
        /// </summary>
        /// <param name="a"></param>
        /// <param name="perm"></param>
        /// <returns></returns>
        static string InvalidChars(string a, string perm)
        {

            string res = "";

            for (int i = 0; i < a.Length; i++)
            {

                if (perm.IndexOf(a.Substring(i, 1)) < 0)
                {

                    byte[] asciiBytes = Encoding.ASCII.GetBytes(a.Substring(i, 1));
                    char c = (char)asciiBytes[0];
                    string cn = CharName(c);

                    if (res.IndexOf(cn) < 0)
                    {

                        if (res != "")
                            res += ", ";

                        res += cn;

                    }

                }

            }

            return res;

        }

        // Nome
        public static string name_allow_chars = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890&-ÁÉÍÓÚáéíóúäëïüÜÄËÏÖÜñÑÂÔÊÛâôêûãÃÕõÀàñÑçÇ@#“”/_ ";
        public static string VerifyName(string a) { return VerifyChars(a, name_allow_chars); }
        public static bool AcceptName(string a) { return AcceptChars(a, name_allow_chars); }

        // Código 64
        public static string code64_allow_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890+/=_-.:";
        public static string VerifyCode64(string a) { return VerifyChars(a, code64_allow_chars); }
        public static bool AcceptCode64(string a) { return AcceptChars(a, code64_allow_chars); }

        // Código de confirmação
        public static string confirmationcode_allow_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        public static string VerifyConfirmationCode(string a) { return VerifyChars(a, confirmationcode_allow_chars); }
        public static bool AcceptConfirmationCode(string a) { return a.Length == 8 && AcceptChars(a, confirmationcode_allow_chars); }

        // Código de licença
        public static string code_allow_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-1234567890";
        public static string VerifyCode(string a) { return VerifyChars(a, code_allow_chars); }
        public static bool AcceptCode(string a) { return AcceptChars(a, code_allow_chars); }

        // Letra
        public static string letter_allow_chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string VerifyLetter(string a) { return VerifyChars(a, letter_allow_chars); }
        public static bool AcceptLetter(string a) { return AcceptChars(a, letter_allow_chars); }

        // Alphanum
        public static string alphanum_allow_chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
        public static string VerifyAlphanum(string a) { return VerifyChars(a, alphanum_allow_chars); }
        public static bool AcceptAlphanum(string a) { return AcceptChars(a, alphanum_allow_chars); }
        public static bool AcceptAlphanum(string a, ref string s) { return AcceptChars(a, alphanum_allow_chars, ref s); }

        public static string VerifySessionCode(string a) { return VerifyChars(a, alphanum_allow_chars); }
        public static bool AcceptSessionCode(string a) { return AcceptChars(a, alphanum_allow_chars); }

        // Telefone
        public static string phone_allow_chars = "0123456789 ()-";
        public static string VerifyPhone(string a) { return VerifyChars(a, phone_allow_chars); }
        public static bool AcceptPhone(string a) { return AcceptChars(a, phone_allow_chars); }

        // Email
        public static string email_allow_chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789áéíóúÁÉÍÓÚâêôÂÊÔãõÃÕçÇ@“”_-.,";
        public static string VerifyEmail(string a) { return VerifyChars(a, email_allow_chars); }
        public static bool AcceptEmail(string a) { return AcceptChars(a, email_allow_chars) && EmailSintaxVerify(a); }

        // Nome servidor
        public static string server_name_allow_chars = "abcdefghijklmnopqrstuvwxyz.:0123456789_-";
        public static string VerifyServerName(string a) { return VerifyChars(a, server_name_allow_chars); }
        public static bool AcceptServerName(string a) { return AcceptChars(a, server_name_allow_chars); }

        // Login -- sempre um email
        public static string login_allow_chars = email_allow_chars;
        public static string VerifyLogin(string a) { return VerifyEmail(a); }
        public static bool AcceptLogin(string a) { return AcceptEmail(a); }

        // Senha
        public static string password_allow_chars = " _abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\ .,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑ()°!#$“”*@+=#";
        public static string VerifyPassword(string a) { return VerifyChars(a, password_allow_chars); }
        public static bool AcceptPassword(string a) { return AcceptChars(a, password_allow_chars); }

        // Texto livre
        public static string multiline_free_text_allow_chars = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\n\\\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+={}[]%&“”'\"";
        public static string multiline_free_text_allow_chars_js = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\\\\\\\n\\\\r\\\\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”\\'\\\"";
        public static string VerifyMultilineFreeText(string a) { return VerifyChars(a, multiline_free_text_allow_chars); }
        public static string multiline_free_text_allow_chars_acc = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\\n\r ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”'\"";
        public static bool AcceptMultilineFreeText(string a) { return AcceptChars(a, multiline_free_text_allow_chars_acc); }
        public static bool AcceptMultilineFreeText(string a, ref string p) { return AcceptChars(a, multiline_free_text_allow_chars_acc, ref p); }
        public static string InvalidMultilineFreeText(string a) { return InvalidChars(a, multiline_free_text_allow_chars); }

        // Texto livre uma linha
        public static string free_text_allow_chars = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”'\"%";
        public static string free_text_allow_chars_js = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\\\\\\\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”\\'\\\"%";
        public static string VerifyFreeText(string a) { return VerifyChars(a, free_text_allow_chars); }
        public static bool AcceptFreeText(string a, ref string p) { return AcceptChars(a, free_text_allow_chars, ref p); }
        public static bool AcceptFreeText(string a) { return AcceptChars(a, free_text_allow_chars); }

        // Texto livre
        public static string multiline_free_text_script_allow_chars = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\n\\\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”'\"<>";
        public static string multiline_free_text_script_allow_chars_js = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\\\n\\r\\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”\\'\\\"<>";
        public static string VerifyMultilineFreeScriptText(string a) { return VerifyChars(a, multiline_free_text_script_allow_chars); }
        public static string multiline_free_text_script_allow_chars_acc = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\\n\r\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”'\"<>";
        public static bool AcceptMultilineFreeScriptText(string a) { return AcceptChars(a, multiline_free_text_script_allow_chars); }

        // Texto livre uma linha
        public static string free_text_allow_script_chars = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”'\"<>";
        public static string free_text_allow_script_chars_js = " _–abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\\t ºª.,;/:-?ÁÉÍÓÚáéíóúüÜÂÔÊâôêãÃÕõÀàçÇñÑłëøНиктаЧжовšșăŠ()°!#$*@+=#{}[]%&“”\\'\\\"<>";
        public static string VerifyFreeScriptText(string a) { return VerifyChars(a, free_text_allow_script_chars); }
        public static bool AcceptFreeScriptText(string a) { return AcceptChars(a, free_text_allow_script_chars); }

        // Latitude (e longitude)
        public static string georreference_allow_chars = "0123456789,-+";
        public static string VerificaLatitude(string a) { return VerifyChars(a, georreference_allow_chars); }
        public static bool AceitaLatitude(string a) { return AcceptChars(a, georreference_allow_chars); }

        // Número int
        public static string number_allow_chars = "0123456789.,-+";
        public static string VerifyNumber(string a) { return VerifyChars(a, number_allow_chars); }
        public static bool AcceptNumber(string a) { return AcceptChars(a, number_allow_chars); }

        // Data
        public static string date_allow_chars = "0123456789/ND";
        public static string date_allow_chars_fn = "0123456789/";
        public static string VerifyDate(string a) { return VerifyChars(a, date_allow_chars); }
        public static bool AcceptDate(string a) { return (a == "N/D") || AcceptChars(a, date_allow_chars_fn); }

        // Hora
        public static string time_allow_chars = "0123456789:/ND";
        public static string time_allow_chars_fn = "0123456789:";
        public static string VerifyHour(string a) { return VerifyChars(a, time_allow_chars); }
        public static bool AcceptHour(string a) { return (a == "N/D") || AcceptChars(a, time_allow_chars_fn); }

        // Data e hora
        public static string datetime_allow_chars = "0123456789/ :ND";
        public static string datetime_allow_chars_fn = "0123456789/ :";
        public static string VerifyDateTime(string a) { return VerifyChars(a, datetime_allow_chars); }
        public static bool AcceptDateTime(string a) { return (a == "N/D") || AcceptChars(a, datetime_allow_chars_fn); }

        // Número inteiro
        public static string integer_allow_chars = "0123456789";
        public static string VerifyInteger(string a) { return VerifyChars(a, integer_allow_chars); }
        public static bool AcceptInteger(string a) { return AcceptChars(a, integer_allow_chars); }

        // Arquivo
        public static string file_allow_chars = "abcdefghijklmnopqrstuvwxyzáàãâéêóõôíîïúüûçABCDEFGHIJKLMNOPQRSTUVWXYZÁÀÃÂÉÊÓÕÔÍÏÎÚÜÛÇñÑ0123456789_-.* ()";
        public static string VerifyFile(string a) { return VerifyChars(a, file_allow_chars); }
        public static bool AcceptFile(string a) { return AcceptChars(a, file_allow_chars); }

        // Diretório e Arquivo
        public static string dir_file_allow_chars = "abcdefghijklmnopqrstuvwxyzáàãâéêóõôíîïúüûçABCDEFGHIJKLMNOPQRSTUVWXYZÁÀÃÂÉÊÓÕÔÍÏÎÚÜÛÇñÑ0123456789 ()_-.:\\/+";
        public static string VerifyDirFile(string a) { return VerifyChars(a, dir_file_allow_chars); }
        public static bool AcceptDirFile(string a) { return AcceptChars(a, dir_file_allow_chars); }

        // Arquivo com mascara
        public static string file_wildcards_allow_chars = "abcdefghijklmnopqrstuvwxyzáàãâéêóõôíîïúüûçABCDEFGHIJKLMNOPQRSTUVWXYZÁÀÃÂÉÊÓÕÔÍÏÎÚÜÛÇñÑ0123456789_-.() *?";
        public static string VerifyFileWildCards(string a) { return VerifyChars(a, file_wildcards_allow_chars); }
        public static bool AcceptFileWildCards(string a) { return AcceptChars(a, file_wildcards_allow_chars); }

        // Url
        public static string url_allow_chars = "abcdefghijklmnopqrstuvwxyzáàãâéêóõôíîïúüûçABCDEFGHIJKLMNOPQRSTUVWXYZÁÀÃÂÉÊÓÕÔÍÏÎÚÜÛÇñÑłëøНиктаЧжовšșăŠ[]{}0123456789_-.,:()\\/#$%&?=";
        public static string VerifyUrl(string a) { return VerifyChars(a, url_allow_chars); }
        public static bool AcceptUrl(string a) { return AcceptChars(a, url_allow_chars); }
        public static bool AcceptUrl(string a, ref string s) { return AcceptChars(a, url_allow_chars, ref s); }

        // Arquivo com caminho completo
        public static string fullpath_allow_chars = "abcdefghijklmnopqrstuvwxyzáàãâéêóõôíîïúüûçABCDEFGHIJKLMNOPQRSTUVWXYZÁÀÃÂÉÊÓÕÔÍÏÎÚÜÛÇñÑ0123456789 :=?\\/_()-.";
        public static string VerifyFullPath(string a) { return VerifyChars(a, fullpath_allow_chars); }
        public static bool AcceptFullPath(string a) { return AcceptChars(a, fullpath_allow_chars); }

        // GUID
        public static string guid_allow_chars = "abcdefABCDEF0123456789-";
        public static string VerifyGuid(string a) { if (a == null) return null; else return GuidSintaxVerify(VerifyChars(a, guid_allow_chars)); }
        public static bool AcceptGuid(string a) { return AcceptChars(a, guid_allow_chars) && (GuidSintaxVerify(a) != null); }

        // GUID OR INTEGER
        public static string guidorinteger_allow_chars = "abcdefABCDEF0123456789-";
        public static string VerifyGuidOrInteger(string a) { return VerifyChars(a, guidorinteger_allow_chars); }
        public static bool AcceptGuidOrInteger(string a) { return AcceptChars(a, guidorinteger_allow_chars); }

        // GUID OR NULL
        public static string guidornull_allow_chars = "abcdefABCDEF0123456789-";
        public static string VerifyGuidOrNull(string a) { return VerifyChars(a, guidorinteger_allow_chars); }
        public static bool AcceptGuidOrNull(string a) { return (a == null) || (a == "") || (a == "0") || AcceptChars(a, guidorinteger_allow_chars); }

        /// <summary>
        /// VerifyChars a sintaxe do email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GuidSintaxVerify(string guid)
        {

            if (guid.Length != 36)
                return null;

            if (guid[0] == '-') return null;
            if (guid[1] == '-') return null;
            if (guid[2] == '-') return null;
            if (guid[3] == '-') return null;
            if (guid[4] == '-') return null;
            if (guid[5] == '-') return null;
            if (guid[6] == '-') return null;
            if (guid[7] == '-') return null;

            if (guid[8] != '-') return null;

            if (guid[9] == '-') return null;
            if (guid[10] == '-') return null;
            if (guid[11] == '-') return null;
            if (guid[12] == '-') return null;

            if (guid[13] != '-') return null;

            if (guid[14] == '-') return null;
            if (guid[15] == '-') return null;
            if (guid[16] == '-') return null;
            if (guid[17] == '-') return null;

            if (guid[18] != '-') return null;

            if (guid[19] == '-') return null;
            if (guid[20] == '-') return null;
            if (guid[21] == '-') return null;
            if (guid[22] == '-') return null;

            if (guid[23] != '-') return null;

            if (guid[24] == '-') return null;
            if (guid[25] == '-') return null;
            if (guid[26] == '-') return null;
            if (guid[27] == '-') return null;
            if (guid[28] == '-') return null;
            if (guid[29] == '-') return null;
            if (guid[30] == '-') return null;
            if (guid[31] == '-') return null;
            if (guid[32] == '-') return null;
            if (guid[33] == '-') return null;
            if (guid[34] == '-') return null;
            if (guid[35] == '-') return null;

            return guid;

        }

        /// <summary>
        /// VerifyChars a sintaxe do email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool EmailSintaxVerify(string email)
        {

            email = email.Trim();
            Regex regex = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Converte para formato monetário
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string FazMonetario(double d)
        {


            string vs = d.ToString("0.00");
            string vf = "";

            int len = vs.Length;
            int cnt = 0;
            int dec = 1;

            for (int i = len - 1; i >= 0; i--)
            {

                if (vs.Substring(i, 1) == "." || vs.Substring(i, 1) == ",")
                {
                    dec = 0;
                    cnt = -1;
                    vf = "," + vf;
                }
                else
                {

                    if (dec == 1)
                    {

                        vf = vs.Substring(i, 1) + vf;

                    }
                    else
                    {

                        cnt++;
                        if (cnt == 3)
                        {
                            cnt = 0;
                            vf = "." + vf;
                        }

                        vf = vs.Substring(i, 1) + vf;

                    }

                }

            }

            return vf;

        }

        static string TextToHtml(string t)
        {

            string res = "";

            char[] c = t.ToCharArray();

            for (int i = 0; i < c.Length; i++)
            {

                if (c[i] == '\n')
                    res += "<br />";
                else if (c[i] == '<')
                    res += "&lt;";
                else if (c[i] == '>')
                    res += "&gt;";
                else if (c[i] == '&')
                    res += "&amp;";
                else if (!char.IsControl(c[i]))
                    res += c[i];

            }

            return res;

        }

        /// <summary>
        /// É um cpf valido
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool IsCPF(string cpf)
        {

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);

        }

        /// <summary>
        /// VerifyChars se é CNPJ válido
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static bool IsCNPJ(string cnpj)
        {

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);

            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();

            tempCnpj = tempCnpj + digito;

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);

        }

        /// <summary>
        /// Verificar se é número de série válido
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsNUmeroSerieRiskan(string num)
        {

            if (num.Length != 20)
                return false;

            return true;

        }

        /// <summary>
        /// Converte a data e hora
        /// </summary>
        /// <param name="str_data"></param>
        /// <param name="str_hora"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(string str_data)
        {

            char[] sp = new char[1];
            sp[0] = '/';

            string[] e = str_data.Split(sp);
            if (e.Length != 3)
                return Base.NullDateTime;

            int d = 1, m = 1, a = 1999;

            try
            {

                d = int.Parse(e[0]);

            }
            catch (Exception)
            {

                return Base.NullDateTime;

            }

            try
            {

                m = int.Parse(e[1]);

            }
            catch (Exception)
            {

                return Base.NullDateTime;

            }

            try
            {

                a = int.Parse(e[2]);

            }
            catch (Exception)
            {

                return Base.NullDateTime;

            }

            try
            {

                return new DateTime(a, m, d);

            }
            catch (Exception)
            {

                return Base.NullDateTime;

            }

        }

        /// <summary>
        /// Monta a string
        /// </summary>
        public static string FormatDurationInHours(RacMsg msgs, double duration)
        {

            string str = "";

            int h = (int)duration;
            int m = (int)(60.0 * (duration - (double)h));

            if (h > 1)
                str = h.ToString() + " " + msgs.Get(RacMsg.Id.hours);

            if (h == 1)
                str = "1 " + msgs.Get(RacMsg.Id.hour);

            if (m > 0)
            {

                if (str != "")
                    str += " " + msgs.Get(RacMsg.Id.and) + " ";

                if (m == 1)
                    str += "1 " + msgs.Get(RacMsg.Id.minute);
                else
                    str += m.ToString() + " " + msgs.Get(RacMsg.Id.minutes);

            }

            return str;

        }

        /// <summary>
        /// Valida lingua
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static int ValidLanguage(int lang)
        {

            if (lang < 2 || lang > 4)       // Limites de validade
                lang = 3;                   // Linguagem default, se tem erro

            return lang;

        }


    }
    
}