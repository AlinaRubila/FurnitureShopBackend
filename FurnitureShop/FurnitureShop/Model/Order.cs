namespace FurnitureShop.Model
{
    public class Order
    {
        public int ID { get; set; }
        public User User { get; set; }
        public DateTime CreationDate {  get; set; }
        public DateTime DeliveryDate { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
