using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Models
{
    public class AddMyFriendData  // Уникальные свойства и параметры для определенной модели данных
    { 
        public int UserId { get; set; }
        public string FriendEmail { get; set; }
    }

}
