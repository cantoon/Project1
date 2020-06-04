using StackOverFlow.CustomFilters;
using StackOverFlow.ServiceLayer;
using StackOverFlow.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverFlow.Controllers
{
    public class QuestionsController : Controller
    {
        IQuestionService questionService;
        ICategoriesService categoriesService;
        IAnswersService answersService;

        public QuestionsController(IQuestionService questionService, ICategoriesService categoriesService, IAnswersService answersService)
        {
            this.questionService = questionService;
            this.categoriesService = categoriesService;
            this.answersService = answersService;
        }
        // GET: Questions
        public ActionResult View(int questionId)
        {
            this.questionService.UpdateQuestionViewsCount(questionId, 1);
            int userID = Convert.ToInt32(Session["CurrentUserID"]);
            QuestionViewModel questionViewModel =  this.questionService.GetQuestionByQuestionID(questionId, userID);
            return View(questionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult AddAnswer(NewAnswerViewModel newAnswer)
        {
            newAnswer.UserID = Convert.ToInt32(Session["CurrentUserID"]);
            newAnswer.AnswerDateAndTime = DateTime.Now;
            newAnswer.VotesCount = 0;
            if (ModelState.IsValid)
            {
                this.answersService.InsertAnswer(newAnswer);
                return RedirectToAction("Views", "Questions", new { questionId = newAnswer.QuestionID });
            }
            else
            {
                ModelState.AddModelError("x", "invalid Data");
                QuestionViewModel questionView = this.questionService.GetQuestionByQuestionID(newAnswer.QuestionID, newAnswer.UserID);
                return View("View", questionView);
            }
        }

        [HttpPost]
        public ActionResult EditAnswer(EditAnswerViewModel avm)
        {
            if (ModelState.IsValid)
            {
                avm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.answersService.UpdateAnswer(avm);
                return RedirectToAction("View", new { questionId = avm.QuestionID });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return RedirectToAction("View", new { questionId = avm.QuestionID });
            }
        }

        public ActionResult Create()
        {
            List<CategoryViewModel> categories = this.categoriesService.GetCategories();
            ViewBag.categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult Create(NewQuestionViewModel qvm)
        {
            if (ModelState.IsValid)
            {
                qvm.AnswersCount = 0;
                qvm.ViewsCount = 0;
                qvm.VotesCount = 0;
                qvm.QuestionDateAndTime = DateTime.Now;
                qvm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.questionService.InsertQuestion(qvm);
                return RedirectToAction("Questions", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View();
            }
        }
    }
}