using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.BLL.Services
{
    public class UserService // Функциональная часть 
    {
        MessageService messageService; // Объявление переменных для репозитариев, чтобы к ним можно было обращаться
        IUserRepository userRepository;
        IFriendRepository friendRepository;
        public UserService() //Конструктор в которых создаются объекты репозитариев, для их использования
        {
            userRepository = new UserRepository();
            messageService = new MessageService();
            friendRepository = new FriendRepository();
        }

        public void Register(UserRegistrationData userRegistrationData) // Функция регитсрации с моделью данных  UserRegistrationData
        {
            if (String.IsNullOrEmpty(userRegistrationData.FirstName)) // Проверки
                throw new ArgumentNullException("Имя не может быть пустым");
            

            if (String.IsNullOrEmpty(userRegistrationData.LastName))
                throw new ArgumentNullException("Фамилия не может быть пустым");

            if (String.IsNullOrEmpty(userRegistrationData.Password))
                throw new ArgumentNullException("Пароль не может быть пустым");

            if (String.IsNullOrEmpty(userRegistrationData.Email))
                throw new ArgumentNullException("Почта не может быть путстым");

            if (userRegistrationData.Password.Length < 8)
                throw new ArgumentNullException("Длина пароля не должна быть менее 8 символов");

            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new ArgumentNullException();

            if (userRepository.FindByEmail(userRegistrationData.Email) != null)
                throw new ArgumentNullException();

            var userEntity = new UserEntity()  //Создание объекта сущности пользователя
            {
                firstname = userRegistrationData.FirstName, // Присвоение введенных данных через слой PLL для текущего объекта пользователя
                lastname = userRegistrationData.LastName,
                password = userRegistrationData.Password,
                email = userRegistrationData.Email
            };

            if (this.userRepository.Create(userEntity) == 0)
                throw new Exception();

        }

        public User Authenticate(UserAuthenticationData userAuthenticationData) // Функция проверик пароля и логина
        {
            var findUserEntity = userRepository.FindByEmail(userAuthenticationData.Email); // Обращения к хранилищу ( репоизатрию) 
            if (findUserEntity is null) throw new UserNotFoundException();

            if (findUserEntity.password != userAuthenticationData.Password)
                throw new WrongPasswordException();

            return ConstructUserModel(findUserEntity);
        }

        public User FindByEmail(string email) // Функция поиска по почте
        {
            var findUserEntity = userRepository.FindByEmail(email); // Обращения к хранилищу ( репоизатрию) 
            if (findUserEntity is null) throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity); // Вызов функции по данным пользователя, в качестве результата поиска
        }

        public User FindById(int id) // Функция поиска по ID пользователя
        {
            var findUserEntity = userRepository.FindById(id); // Обращения к хранилищу ( репоизатрию) 
            if (findUserEntity is null) throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);// Вызов функции по данным пользователя, в качестве результата поиска
        }

        public void Update(User user) // Функция обновления пользователя 
        {
            var updatableUserEntity = new UserEntity()
            {
                id = user.Id,
                firstname = user.FirstName,
                lastname = user.LastName,
                password = user.Password,
                email = user.Email,
                photo = user.Photo,
                favorite_movie = user.FavoriteMovie,
                favorite_book = user.FavoriteBook
            };

            if (this.userRepository.Update(updatableUserEntity) == 0)
                throw new Exception();
        }

        public void AddFriend(AddMyFriendData addMyFriendData) // Функция добавления друга, на входе модель данных по добавлению друга
        {
            var findFriend = userRepository.FindByEmail(addMyFriendData.FriendEmail); // Обращения к хранилищу ( репоизатрию) 
            if (findFriend is null)
                throw new UserNotFoundException();

            var friendEntity = new FriendEntity() // Если нашли, то создаем запись и присваиваем данные по ID
            {
                user_id = addMyFriendData.UserId,
                friend_id = findFriend.id
            };

            if (this.friendRepository.Create(friendEntity) == 0)
                throw new Exception();
        }
        public IEnumerable<User> GetFriendsByUserId(int userId) // Функция получения друзей , на входе ID текущего пользователя, на выхое список
        {
            return friendRepository.FindAllByUserId(userId).Select(f => FindById(f.friend_id)); // Обращения к хранилищу ( репоизатрию) 
        }

 

        private User ConstructUserModel(UserEntity userEntity) // Получение информации по пользователю
        {
            var incomingMessages = messageService.GetIncomingMessagesByUserId(userEntity.id);

            var outgoingMessages = messageService.GetOutcomingMessagesByUserId(userEntity.id);
            var friends = GetFriendsByUserId(userEntity.id);

            return new User(userEntity.id,
                          userEntity.firstname,
                          userEntity.lastname,
                          userEntity.password,
                          userEntity.email,
                          userEntity.photo,
                          userEntity.favorite_movie,
                          userEntity.favorite_book,
                          incomingMessages,
                          outgoingMessages,
                          friends
                          );
        }
    }
}
