using StackOverFlow.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StackOverFlow.ApiControllers
{
    public class QuestionsController : ApiController
    {
        IQuestionService questionService;
        IAnswersService answersService;

        public QuestionsController(IQuestionService questionService, IAnswersService answersService)
        {
            this.questionService = questionService;
            this.answersService = answersService;
        }

        public void Post(int AnswerID, int UserID, int value)
        {
            this.answersService.UpdateAnswerVotesCount(AnswerID, UserID, value);
        }
    }
}
