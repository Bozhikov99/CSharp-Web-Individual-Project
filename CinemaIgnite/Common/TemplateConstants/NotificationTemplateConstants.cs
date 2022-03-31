using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TemplateConstants
{
    public class NotificationTemplateConstants
    {
        public const string MovieProjectionTitle = "Очаквайте скоро";
        public const string TicketBoughtTitle = "Закупен билет";
        public const string ProjectionCancelledTitle = "Отменена проекция";

        public const string MovieProjectionMessage = "Очаквайте {0} на {1} от {2} часа в Cinema Ignite";
        public const string TicketBoughtMessage = "Информираме ви, че успешно закупихте билет за филма {0} на {1} в {2} часа. Също така и че ще направим този текст ненужно дълъг с цел тестване на известията. :)";
        public const string ProjectionCancelledMessage = "За съжаление, проекцията на {0} с дата {1} в {2} часа бе отменена. Парите, които сте платили за билети, ще ви бъдат възстановени до няколко работни дни.";
    }
}
