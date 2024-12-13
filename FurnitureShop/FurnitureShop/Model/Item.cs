namespace FurnitureShop.Model
{
    public class Item
    {
        public int ID { get; set; }
        public Furniture Furniture { get; set; }
        public string Name { get; set; } = "";
        public double Price { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Count { get; set; }
        public ICollection<User> Users {  get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
