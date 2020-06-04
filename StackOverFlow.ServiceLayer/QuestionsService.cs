using StackOverFlow.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using StackOverFlow.Repositories;
using StackOverFlow.DomainModels;
using StackOverFlow.ServiceLayer;

namespace StackOverFlow.ServiceLayer
{
    public interface IQuestionService
    {
        void InsertQuestion(NewQuestionViewModel q);
        void UpdateQuestionDetails(EditQuestionViewModel q);
        void UpdateQuestionVotesCount(int qid, int value);
        void UpdateQuestionAnswersCount(int qid, int value);
        void UpdateQuestionViewsCount(int qid, int value);
        void DeleteQuestion(int qid);
        List<QuestionViewModel> GetQuestions();
        QuestionViewModel GetQuestionByQuestionID(int QuestionID, int UserID);
    }
    public class QuestionsService : IQuestionService
    {
        IQuestionRepository questionRepository;
        public QuestionsService()
        {
            questionRepository = new QuestionsRepository();
        }

        public void DeleteQuestion(int qid)
        {
            questionRepository.DeleteQuestion(qid);
        }

        public QuestionViewModel GetQuestionByQuestionID(int QuestionID, int UserID = 0)
        {
            Question existQuestion = questionRepository.GetQuestionByQuestionID(QuestionID);
            QuestionViewModel questionViewModel = null;
            if (existQuestion != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Question, QuestionViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                questionViewModel = mapper.Map<Question, QuestionViewModel>(existQuestion);
                foreach (var item in questionViewModel.Answers)
                {
                    item.CurrentUserVoteType = 0;
                    VoteViewModel vote = item.Votes.Where(temp => temp.UserID == UserID).FirstOrDefault();
                    if (vote != null)
                    {
                        item.CurrentUserVoteType = vote.VoteValue;
                    }
                }
            }
            return questionViewModel;
        }

        public List<QuestionViewModel> GetQuestions()
        {
            List<Question> existQuestion = questionRepository.GetQuestions();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Question, QuestionViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List<QuestionViewModel> questionViewModels = mapper.Map<List<Question>, List<QuestionViewModel>>(existQuestion);
            return questionViewModels;
        }

        public void InsertQuestion(NewQuestionViewModel newQuestionViewModel)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<NewQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question question = mapper.Map<NewQuestionViewModel, Question>(newQuestionViewModel);
            questionRepository.InsertQuestion(question);
        }

        public void UpdateQuestionAnswersCount(int questionID, int value)
        {
            questionRepository.UpdateQuestionAnswersCount(questionID, value);
        }

        public void UpdateQuestionDetails(EditQuestionViewModel editQuestionViewModel)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question question = mapper.Map<EditQuestionViewModel, Question>(editQuestionViewModel);
            questionRepository.UpdateQuestionDetails(question);
        }

        public void UpdateQuestionViewsCount(int questionID, int value)
        {
            questionRepository.UpdateQuestionViewsCount(questionID, value);
        }

        public void UpdateQuestionVotesCount(int questionID, int value)
        {
            questionRepository.UpdateQuestionVotesCount(questionID, value);
        }
    }
}
