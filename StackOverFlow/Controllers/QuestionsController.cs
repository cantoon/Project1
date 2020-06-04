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
    }
}