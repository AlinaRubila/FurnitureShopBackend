namespace FurnitureShop.Model
{
    public class Cart
    {
        public int Id { get; set; }
        public int UsersID { get; set; }
        public User? User { get; set; }
        public int ItemsID { get; set; }
        public Item? Item { get; set; }
    }
}
