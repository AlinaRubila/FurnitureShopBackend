namespace FurnitureShop.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public Role Role { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders {  get; set; }
    }
}
