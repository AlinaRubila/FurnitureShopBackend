namespace FurnitureShop.Contracts
{
    public class Order
    {
        public int Id;
        public int userid;
        public List<int> items = new List<int>();
        public DateTime creationDate;
        public DateTime deliveryDate;
    }
}
