using NUnit.Framework;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;

namespace SocialNetwork.Tests
{
    public class UserServiceTest
    {
       
        [Test]

        public void FindByEmailMustReturnUser()
        {
            var userService = new UserService();


            IUserRepository userRepository;

            UserEntity userEntity = new UserEntity()
            {
                id = 1,
                firstname = "Test",
                lastname = "Test",
                password = "Test",
                email = "1@mail.ru"
            };

            userRepository = new UserRepository();
            userRepository.Create(userEntity);
            userRepository.FindByEmail(userEntity.email);
            var result = userService.FindByEmail(userEntity.email);

            Assert.IsNotNull(result);

           
        }

    }
}