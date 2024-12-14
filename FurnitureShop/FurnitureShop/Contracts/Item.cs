namespace FurnitureShop.Contracts
{
    public class Item
    {
        public required int ID { get; init; }
        public required string Name { get; init; }
        public required double Price { get; init; }
        public required double Length { get; init; }
        public required double Width { get; init; }
        public required double Height { get; init; }
        public required FurnitureCat Category { get; init; }
    }
}
