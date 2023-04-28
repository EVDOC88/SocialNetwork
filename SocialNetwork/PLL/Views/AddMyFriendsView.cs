using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.PLL.Views
{
    public class AddMyFriendsView
    {
        UserService userService;

        public AddMyFriendsView(UserService userService)
        {
            this.userService = userService;
        }

        public void Show(User user)
        {
            try
            {
                var addMyFriendData = new AddMyFriendData();

                Console.WriteLine("Укажите адрес пользователя которого хотите добавить в друзья: ");

                addMyFriendData.FriendEmail = Console.ReadLine();
                addMyFriendData.UserId = user.Id;

                this.userService.AddFriend(addMyFriendData);

                SuccessMessage.Show("Пользователь добавлен в друзья!");
            }

            catch (UserNotFoundException)
            {
                AlertMessage.Show("Пользователя с указанным адресом  не существует!");
            }

            catch (Exception)
            {
                AlertMessage.Show("Ошибка добавления пользователя в друзья!");
            }

        }
    }
}
