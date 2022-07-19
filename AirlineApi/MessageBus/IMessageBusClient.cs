using AirlineApi.Dtos;

namespace AirlineApi.MessageBus
{
    public interface IMessageBusClient
    {
        void SendMessage(NotificationMessageDto notificationDto);
    }
}