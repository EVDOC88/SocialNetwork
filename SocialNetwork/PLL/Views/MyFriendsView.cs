using SocialNetwork.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.PLL.Views
{
    public class MyFriendsView
    {
        public void Show(IEnumerable<User> myfriends)
        {
            Console.WriteLine("У меня в друзьях");

            if (myfriends.Count() == 0)
            {
                Console.WriteLine("У вас нет друзей");
                return;
            }

            myfriends.ToList().ForEach(myfriend =>
            {
                Console.WriteLine("Имя друга: {0}. Фамилия друга: {1} Почтовый адрес друга: {2}.",  myfriend.FirstName, myfriend.LastName, myfriend.Email);
            });

        }

    }
}
