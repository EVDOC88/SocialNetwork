using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Models
{
    public class Message // Уникальные свойства и параметры для определенной модели данных
    {
        public int Id { get; }
        public string Content { get; }
        public string SenderEmail { get; }
        public string RecipientEmail { get; }

        public Message(int id, string content, string senderEmail, string recipientEmail) // Конструкторт для обмена сообщениями
        {
            this.Id = id;
            this.Content = content;
            this.SenderEmail = senderEmail;
            this.RecipientEmail = recipientEmail;
        }

    }
}
