using Microsoft.EntityFrameworkCore;

namespace FurnitureShop
{
    public class CartWorkDB
    {
        Model.ApplicationContext _database = new Model.ApplicationContext();
        bool InStock(int itemid)
        {
            Model.Item? item = _database.Items.Where(i => i.ID == itemid).FirstOrDefault();
            if (item == null || item.Count == 0) { return false; }
            else { return true; }
        }
        public bool AddToCart(int userid, int itemid, int count)
        {
            bool result = true;
            Model.Item? item = _database.Items.Where(i => i.ID == itemid).FirstOrDefault();
            if (item == null) { return false; }
            else
            {
                for (int i = 0; i <= count; i++)
                {
                    //Model.Cart cart_item = new Model.Cart(); cart_item.ItemID = itemid; cart_item.UserID = userid;
                    Model.User? user = _database.Users.Include(u => u.Items).Where(u => u.ID == userid).FirstOrDefault() ?? new Model.User();
                    //cart_item.Item = item; cart_item.User = user;
                    user.Items.Add(item);
                    _database.SaveChanges();
                }
            }
            return result;
        }
        public bool DeleteItem(int userid, int itemid)
        {
            bool result = false;
            Model.User? user = _database.Users.Include(u => u.Items).Where(u => u.ID == userid).FirstOrDefault() ?? new Model.User();
            Model.Item cart_to_delete = new Model.Item();
            foreach (var item in user.Items)
            {
                if (item.ID == itemid) 
                {
                    result = true;
                    cart_to_delete = item;
                    break; 
                }
            }
            if (result)
            {
                user.Items.Remove(cart_to_delete);
                _database.SaveChanges();
            }
            return result;
        }
        public List<int> ViewCart(int userid)
        {
            List<int> items = new List<int>();
            Model.User? user = _database.Users.Include(u => u.Items).Where(u => u.ID == userid).FirstOrDefault() ?? new Model.User();
            foreach (var item in user.Items)
            {
                items.Add(item.ID);
            }
            return items;
        }
        public bool OrderItem(int userid, int itemid)
        {
            bool result = InStock(itemid);
            if (result == false || ViewCart(userid).Contains(itemid) == false) { return false; }
            Model.Item? d_item = _database.Items.Where(i => i.ID == itemid).FirstOrDefault() ?? new Model.Item();
            Model.User? user = _database.Users.Include(u => u.Items).Where(u => u.ID == userid).FirstOrDefault() ?? new Model.User();
            user.Items.Remove(d_item);
            List<Model.Item> items = [d_item];
            //_database.SaveChanges();
            Model.Order order = new Model.Order(); order.User = user; order.Items = items;
            order.CreationDate = DateTime.UtcNow; order.DeliveryDate = TimeZoneInfo.ConvertTime(order.CreationDate.AddDays(3), TimeZoneInfo.Utc);
            d_item.Count -= 1;
            _database.Orders.Add(order);
            _database.SaveChanges();
            return result;
        }
        public bool OrderAll(int userid)
        {
            List<int> items_id = ViewCart(userid);
            List<Model.Item> items = new List<Model.Item>();
            bool result = true;
            foreach (int item in items_id)
            {
                result = InStock(item);
                if (result == false || ViewCart(userid).Contains(item) == false) 
                {
                    result = false;
                    break; 
                }
                Model.Item? d_item = _database.Items.Where(i => i.ID == item).FirstOrDefault() ?? new Model.Item();
                Model.User? user = _database.Users.Include(u => u.Items).Where(u => u.ID == userid).FirstOrDefault() ?? new Model.User();
                user.Items.Remove(d_item);
                //_database.SaveChanges();
                d_item.Count -= 1;
                //_database.SaveChanges();
                items.Add(d_item);
            }
            if (result == true)
            {
                Model.User? user = _database.Users.Include(u => u.Items).Where(u => u.ID == userid).FirstOrDefault() ?? new Model.User();
                Model.Order order = new Model.Order(); order.User = user; order.Items = items;
                order.CreationDate = DateTime.UtcNow; order.DeliveryDate = TimeZoneInfo.ConvertTime(order.CreationDate.AddDays(3), TimeZoneInfo.Utc);
                _database.Orders.Add(order);
                _database.SaveChanges();
            }
            return result;
        }
    }
}
