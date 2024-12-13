namespace FurnitureShop
{
    public interface IRabbitMQService
    {
        void SendMessage(string message);
    }
}
