namespace FurnitureShop.Contracts
{
    public class UserInfo
    {
        public required string Name { get; init; } = "";
        public required UserRole Role { get; init; } = new UserRole();
    }
}
