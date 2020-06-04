using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using StackOverFlow.DomainModels;
using StackOverFlow.Repositories;
using StackOverFlow.ServiceLayer;
using StackOverFlow.ViewModels;

namespace StackOverFlow.ServiceLayer
{
    public interface IUsersService
    {
        int InsertUser(RegisterViewModel registerViewModel);
        void UpdateUserDetails(EditUserDetailsViewModel userDetailsViewModel);
        void UpdateUserPassword(EditUserPasswordViewModel editUserPasswordViewModel);
        void DeleteUser(int userID);
        List<UserViewModel> GetUsers();
        UserViewModel GetUsersByEmailAndPassword(string Email, string Password);
        UserViewModel GetUsersByEmail(string Email);
        UserViewModel GetUsersByUserID(int UserID);
    }
    public class UsersService : IUsersService
    {
        IUsersRepository usersRepository;

        public UsersService()
        {
            usersRepository = new UsersRepository();
        }
        public void DeleteUser(int userID)
        {
            usersRepository.DeleteUser(userID);
        }

        public List<UserViewModel> GetUsers()
        {
            List<User> users = usersRepository.GetUsers();
            List<UserViewModel> usersViewModelList = null;
            if (users != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                usersViewModelList = mapper.Map<List<User>, List<UserViewModel>>(users);
            }
            return usersViewModelList;
        }

        public UserViewModel GetUsersByEmail(string Email)
        {
            User existUser = usersRepository.GetUsersByEmail(Email);
            UserViewModel usersViewModel = null;
            if (existUser != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                usersViewModel = mapper.Map<User, UserViewModel>(existUser);
            }
            return usersViewModel;
        }

        public UserViewModel GetUsersByEmailAndPassword(string Email, string Password)
        {
            User existUser = usersRepository.GetUsersByEmailAndPassword(Email, SHA256HashGenerator.GenerateHash(Password));
            UserViewModel usersViewModel = null;
            if (existUser != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                usersViewModel = mapper.Map<User, UserViewModel>(existUser);
            }
            return usersViewModel;
        }

        public UserViewModel GetUsersByUserID(int UserID)
        {
            User existUser = usersRepository.GetUsersByUserID(UserID);
            UserViewModel usersViewModel = null;
            if (existUser != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                usersViewModel = mapper.Map<User, UserViewModel>(existUser);
            }
            return usersViewModel;
        }

        public int InsertUser(RegisterViewModel registerViewModel)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<RegisterViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User user = mapper.Map<RegisterViewModel, User>(registerViewModel);
            user.PasswordHash = SHA256HashGenerator.GenerateHash(registerViewModel.Password);
            usersRepository.InsertUser(user);
            int lastestUser = usersRepository.GetLatestUserID();
            return lastestUser;
        }

        public void UpdateUserDetails(EditUserDetailsViewModel userDetailsViewModel)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditUserDetailsViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User user = mapper.Map<EditUserDetailsViewModel, User>(userDetailsViewModel);
            usersRepository.UpdateUserDetails(user);
        }

        public void UpdateUserPassword(EditUserPasswordViewModel editUserPasswordViewModel)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditUserPasswordViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User user = mapper.Map<EditUserPasswordViewModel, User>(editUserPasswordViewModel);
            user.PasswordHash = SHA256HashGenerator.GenerateHash(editUserPasswordViewModel.Password);
            usersRepository.UpdateUserPassword(user);
        }
    }
}
