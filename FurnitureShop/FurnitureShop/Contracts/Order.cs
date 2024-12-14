namespace FurnitureShop.Contracts
{
    public class Order
    {
        public required int ID { get; init; }
        public required int UserID { get; init; }
        public required  List<int> Items { get; init; } = new List<int>();
        public required DateTime CreationDate { get; init; }
        public required DateTime DeliveryDate { get; init; }
    }
}
