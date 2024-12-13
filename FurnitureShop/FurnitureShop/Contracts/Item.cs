namespace FurnitureShop.Contracts
{
    public class Item
    {
        public int ID;
        public string name = "";
        public double price;
        public double length;
        public double width;
        public double height;
        public FurnitureCat category = new FurnitureCat();
    }
}
