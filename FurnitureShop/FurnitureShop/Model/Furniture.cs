namespace FurnitureShop.Model
{
    public class Furniture
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public ICollection<Item> Items { get; set; }
    }
}
