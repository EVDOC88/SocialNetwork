using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Services
{
    public class MessageService
    {
        IMessageRepository messageRepository; // Объявления репозитария
        IUserRepository userRepository;
        public MessageService()
        {
            userRepository = new UserRepository(); // Объявления репозитария в конструктуре
            messageRepository = new MessageRepository();
        }

        public IEnumerable<Message> GetIncomingMessagesByUserId(int recipientId) // Функция по получению ссобщения, на входе ID, на выходе список
        {
            var messages = new List<Message>(); // Объявялем список

            messageRepository.FindByRecipientId(recipientId).ToList().ForEach(m =>   // Обращаемся к репозитария, для выполнения функуции соответсвющего класса, передав ID пользователя и преобразовав сразу вс список
            {
                var senderUserEntity = userRepository.FindById(m.sender_id); // Присваиваем найденное значение по ID
                var recipientUserEntity = userRepository.FindById(m.recipient_id); // Присваиваем найденное значение по ID

                messages.Add(new Message(m.id, m.content, senderUserEntity.email, recipientUserEntity.email)); // Добавляем в список, через создание объекта 
            });

            return messages; // Возвращаем список
        }

        public IEnumerable<Message> GetOutcomingMessagesByUserId(int senderId)  // Аналогично функции выше
        {
            var messages = new List<Message>();

            messageRepository.FindBySenderId(senderId).ToList().ForEach(m =>
            {
                var senderUserEntity = userRepository.FindById(m.sender_id);
                var recipientUserEntity = userRepository.FindById(m.recipient_id);

                messages.Add(new Message(m.id, m.content, senderUserEntity.email, recipientUserEntity.email));
            });

            return messages;
        }

        public void SendMessage(MessageSendingData messageSendingData)  // Отправка сообщения , на входе данные соглассно типу MessageSendingData
        {
            if (String.IsNullOrEmpty(messageSendingData.Content)) //Проверки 
                throw new ArgumentNullException();

            if (messageSendingData.Content.Length > 5000)
                throw new ArgumentOutOfRangeException();

            var findUserEntity = this.userRepository.FindByEmail(messageSendingData.RecipientEmail);// Поиск сообщения для отправки
            if (findUserEntity is null) throw new UserNotFoundException();

            var messageEntity = new MessageEntity() // Когда нашли сообщениие записываем его в объект 
            {
                content = messageSendingData.Content,
                sender_id = messageSendingData.SenderId,
                recipient_id = findUserEntity.id
            };

            if (this.messageRepository.Create(messageEntity) == 0)
                throw new Exception();
        }
    }
}
