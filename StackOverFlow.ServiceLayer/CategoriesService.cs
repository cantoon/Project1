using AutoMapper;
using AutoMapper.Configuration;
using StackOverFlow.DomainModels;
using StackOverFlow.Repositories;
using StackOverFlow.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlow.ServiceLayer
{
    public interface ICategoriesService
    {
        void InsertCategory(CategoryViewModel categoryViewModel);
        void UpdateCategory(CategoryViewModel categoryViewModel);
        void DeleteCategory(int categoryId);
        List<CategoryViewModel> GetCategories();
        CategoryViewModel GetCategoryByCategoryID(int CategoryID);
    }
    public class CategoriesService : ICategoriesService
    {
        ICategoriesRepository categoriesRepository;
        public CategoriesService()
        {
            categoriesRepository = new CategoriesRepository();
        }
        public void DeleteCategory(int categoryId)
        {
            categoriesRepository.DeleteCategory(categoryId);
        }

        public List<CategoryViewModel> GetCategories()
        {
            List<Category> categories = categoriesRepository.GetCategories();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List<CategoryViewModel> categoryViewModels = mapper.Map<List<Category>, List<CategoryViewModel>>(categories);
            return categoryViewModels;
        }

        public CategoryViewModel GetCategoryByCategoryID(int CategoryID)
        {
            Category existCategories = categoriesRepository.GetCategoryByCategoryID(CategoryID);
            CategoryViewModel categoryViewModel = null;
            if (existCategories != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                categoryViewModel = mapper.Map<Category, CategoryViewModel>(existCategories);
            }
            return categoryViewModel;
        }

        public void InsertCategory(CategoryViewModel categoryViewModel)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Category category = mapper.Map<CategoryViewModel, Category>(categoryViewModel);
            categoriesRepository.InsertCategory(category);
        }

        public void UpdateCategory(CategoryViewModel categoryViewModel)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Category category = mapper.Map<CategoryViewModel, Category>(categoryViewModel);
            categoriesRepository.UpdateCategory(category);
        }
    }
}
