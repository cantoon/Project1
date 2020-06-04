using StackOverFlow.ServiceLayer;
using StackOverFlow.ViewModels;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StackOverFlow.Controllers
{
    public class HomeController : Controller
    {
        IQuestionService questionService;
        ICategoriesService categoriesService;
        public HomeController(IQuestionService questionService, ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
            this.questionService = questionService;

        }
        // GET: Home
        public ActionResult Index()
        {
            List<QuestionViewModel> questionList = this.questionService.GetQuestions();
            return View(questionList);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Categories()
        {
            List<CategoryViewModel> categoryViews = this.categoriesService.GetCategories();
            return View(categoryViews);
        }

        [Route("allquestions")]
        public ActionResult Questions()
        {
            List<QuestionViewModel> questionList = this.questionService.GetQuestions();
            return View(questionList);
        }

        public ActionResult Search(string str)
        {
            List<QuestionViewModel> questions = this.questionService.GetQuestions().Where(temp => temp.QuestionName.ToLower().Contains(str.ToLower()) || temp.Category.CategoryName.ToLower().Contains(str.ToLower())).ToList();
            ViewBag.str = str;
            return View(questions);
        }
    }
}