﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Models;

namespace TellStoryTogether.Controllers
{
    public class ReadController : Controller
    {
        //
        // GET: /Read/
        readonly UsersContext _userContext = new UsersContext();

        public ActionResult Index(string identifier)
        {
            string[] articleId_identifier = Identifier(identifier);
            int firstArticleId = Int32.Parse(articleId_identifier[0]);
            string restArticle = articleId_identifier[1];
            
            Article firstArticle = _userContext.Articles.First(p => p.ArticleId == firstArticleId);
            ViewBag.Identifier = restArticle;
            return View(firstArticle);
        }

        public ActionResult ArticleAttach(string identifierOrArticleId)
        {
            string identifier = Identifier(identifierOrArticleId)[1];
            List<List<Article>> output = new List<List<Article>>();
            List<int> parallelList = identifier.Split('-').Select(Int32.Parse).ToList();
            int sourceId = parallelList[0];
            parallelList.RemoveAt(0);
            foreach (int i in parallelList)
            {
                List<Article> tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == sourceId).OrderByDescending(p=>p.Parallel==i).ThenByDescending(p=>p.Point).ToList();
                sourceId = tempArticles[0].ArticleId;
                output.Add(tempArticles);
            }
            return Json(output);
        }

        private string[] Identifier(string articleIdOrIdentifier)
        {
            string articleId;
            string identifier = "";
            if (articleIdOrIdentifier.Contains("-"))
            {
                List<int> tempList = articleIdOrIdentifier.Split('-').Select(Int32.Parse).ToList();
                int tempId = tempList[0];
                articleId = tempId.ToString();
                tempList.RemoveAt(0);
                foreach (int parallel in tempList)
                {
                    tempId = _userContext.Articles.First(p => p.ArticleInitId == tempId && p.Parallel==parallel).ArticleId;
                }
                identifier = articleIdOrIdentifier;
                var tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                while (tempArticles.Count != 0)
                {
                    Article tempArticle = tempArticles[0];
                    tempId = tempArticle.ArticleId;
                    identifier = identifier + "-" + tempArticle.Parallel;
                    tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                }
            }
            else
            {
                articleId = articleIdOrIdentifier;
                int tempId = Int32.Parse(articleId);
                var tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                while (tempArticles.Count != 0)
                {
                    Article tempArticle = tempArticles[0];
                    tempId = tempArticle.ArticleId;
                    identifier = identifier + "-" + tempArticle.Parallel;
                    tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                }
                identifier = articleId + identifier;
            }
            return new[] { articleId, identifier };
        }

    }
}