using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibVisWeb.Models;
using LibVisLib;

namespace LibVisWeb.Controllers
{
    
    public class ControllerBase
    {

        /// <summary>
        /// Retorna o usuário
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        static public AccountModel GetUser(string userid, bool own, bool full)
        {

            LibVisLib.Profile p = LibVisLib.Profile.LoadProfile(userid);
            if (p != null)
            {

                AccountModel m = new AccountModel(p, own, full);
                return m;

            }
            else
            {

                return new AccountModel();

            }

        }

        /// <summary>
        /// Retorna as últimas notícias
        /// </summary>
        /// <param name="max"></param>
        /// <param name="numperrow"></param>
        /// <returns></returns>
        static public List<ArticleRowModel> LatestArticlesInRow(int lang, int max, int numperrow)
        {

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Article> nn = Article.GetLastArticles(lang, "", "", 0, max, 0);

            int nl = nn.Count / numperrow;

            if (nn.Count % numperrow != 0)
                nl++;

            int c = 0;

            List<ArticleRowModel> lst = new List<ArticleRowModel>();
            for (int i = 0; i < nl; i++)
            {

                ArticleRowModel nr = new ArticleRowModel();
                lst.Add(nr);

                for(int j = 0; j < numperrow && c < nn.Count; j++)
                    nr.Articles.Add(new ArticleModel(msgs, nn[c++], false, false, false, true, true, false, false));

                if (c >= nn.Count)
                    break;

            }  

            return lst;

        }

        /// <summary>
        /// Retorna as últimas notícias na versão V2
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        static public List<ArticleModel> LatestArticles(int lang, int max)
        {

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Article> nn = Article.GetLastArticles(lang, "", "", 0, max, 0);

            List<ArticleModel> lst = new List<ArticleModel>();
            for (int i = 0; i < nn.Count; i++)
                lst.Add(new ArticleModel(msgs, nn[i], false, false, false, true, true, false, false));

            return lst;

        }

        /// <summary>
        /// Retorna as pautas por categoria
        /// </summary>
        /// <param name="categ"></param>
        /// <param name="onlyValid"></param>
        /// <param name="ini"></param>
        /// <param name="max"></param>
        /// <param name="userid"></param>
        /// <param name="lastdate"></param>
        /// <returns></returns>
        static public List<TargetModel> LatestTargetByCategory(int lang, string categ, bool onlyValid, int ini, int max, string userid, DateTime lastdate, string search = "")
        {
            
            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Target> nn = Target.GetLastTargets(lang, categ, onlyValid, ini, max, lastdate, search);

            List<TargetModel> lst = new List<TargetModel>();
            for (int i = 0; i < nn.Count; i++)
            {

                TargetModel v = new TargetModel(msgs, nn[i], false, true, true, false);

                if (userid != null && userid != "")
                    v.UserVote = (int)nn[i].GetUserVote(userid);

                lst.Add(v);

            }

            return lst;

        }

        /// <summary>
        /// Retorna pautas do usuário
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ini"></param>
        /// <param name="max"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static public List<TargetModel> LatestTargetForUser(string id, int lang, int ini, int max, DateTime n)
        {

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Target> nn = Target.GetLastTargetsForUser(id, ini, max, n);

            List<TargetModel> lst = new List<TargetModel>();
            for (int i = 0; i < nn.Count; i++)
            {

                TargetModel v = new TargetModel(msgs, nn[i], false, true, true, false);
                v.UserVote = (int)nn[i].GetUserVote(id);

                lst.Add(v);

            }

            return lst;

        }

        /// <summary>
        /// Retorna últimas notícias por categoria
        /// </summary>
        /// <param name="categ"></param>
        /// <param name="ini"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static public List<VideoModel> LatestVideosByCategory(int lang, string categ, int ini, int max, string search = "")
        {

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Video> nn = Video.GetLastVideos(lang, categ, ini, max, search);

            List<VideoModel> lst = new List<VideoModel>();
            for (int i = 0; i < nn.Count; i++)
                lst.Add(new VideoModel(msgs, nn[i], false, true, true, false));

            return lst;

        }

        /// <summary>
        /// Retorna últimos vídeos para o usuário
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ini"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static public List<VideoModel> LatestVideosForUser(string id, int lang, int ini, int max)
        {

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Video> nv = Video.GetLastVideosForUser(id, ini, max);

            List<VideoModel> lst = new List<VideoModel>();
            for (int i = 0; i < nv.Count; i++)
            {
                lst.Add(new VideoModel(msgs, nv[i], false, true, true, false));
            }

            return lst;

        }

        /// <summary>
        /// Retorna últimos artigos por categoria
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="category"></param>
        /// <param name="ini"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static public List<ArticleModel> LatestArticlesByCategory(int lang, string userid, string category, int ini, int max, int sts, string search = "")
        {

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Article> nn = Article.GetLastArticles(lang, userid, category, ini, max, sts, search);
            
            List<ArticleModel> lst = new List<ArticleModel>();
            for (int i = 0; i < nn.Count; i++)
            {

                ArticleModel v = new ArticleModel(msgs, nn[i], false, false, false, true, true, false, false);  

                if (userid != null && userid != "")
                    v.UserVote = (int)nn[i].GetUserVote(userid);

                lst.Add(v);

            }

            return lst;

        }


        /// <summary>
        /// Retorna últimos artigos por categoria
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="category"></param>
        /// <param name="ini"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static public List<AccountModel> GetUsers(int lang, int ini, int max, string search = "")
        {

            lang = LibVisLib.Verify.ValidLanguage(lang);
            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Profile> nn = Profile.GetUsers(ini, max, search);

            List<AccountModel> lst = new List<AccountModel>();
            for (int i = 0; i < nn.Count; i++)
            {

                AccountModel v = new AccountModel(nn[i], false, true);
                lst.Add(v);

            }

            return lst;

        }

        /// <summary>
        /// Retorna últimos artigos por categoria
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="category"></param>
        /// <param name="ini"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static public List<ArticleModel> LatestArticlesForTranslation(int lang, string userid, int ini, int max)
        {

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Article> nn = Article.GetLastArticlesForTranslation(lang, ini, max);

            List<ArticleModel> lst = new List<ArticleModel>();
            for (int i = 0; i < nn.Count; i++)
            {

                ArticleModel v = new ArticleModel(msgs, nn[i], false, false, false, true, true, false, false);

                if (userid != null && userid != "")
                    v.UserVote = (int)nn[i].GetUserVote(userid);

                lst.Add(v);

            }

            return lst;

        }

        /// <summary>
        /// Retorna os artigos para o usuário
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ini"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static public List<ArticleModel> LatestArticlesForUser(string id, int lang, int ini, int max)
        {

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Article> nn = Article.GetLastArticlesForUser(id, ini, max);

            List<ArticleModel> lst = new List<ArticleModel>();
            for (int i = 0; i < nn.Count; i++)
                lst.Add(new ArticleModel(msgs, nn[i], false, false, false, true, true, false, false));

            return lst;

        }

        /// <summary>
        /// Retorna as estatísticas
        /// </summary>
        /// <returns></returns>
        static public List<NewsStatisticModel> GetStatistics(int lang)
        {

            if (lang < 3 || lang > 4)
                lang = 2;

            RacLib.RacMsg msgs = RacLib.RacMsg.cache.GetMessage((RacLib.RacMsg.Language)lang);

            List<Statistics> nv = Statistics.GetStatistics(lang);

            List<NewsStatisticModel> lst = new List<NewsStatisticModel>();
            for (int i = 0; i < nv.Count && i < 6; i++)
                lst.Add(new NewsStatisticModel(msgs, nv[i]));

            return lst;

        }

    }

}
