using Microsoft.EntityFrameworkCore;

namespace FurnitureShop
{
    public class OrderProcessingDB
    {
        Model.ApplicationContext _database = new Model.ApplicationContext();
        public List<Contracts.Order> OrdersList(int userid)
        {
            var d_orders = _database.Orders.Include(o => o.User).Include(o => o.Items).Where(o => o.User.ID == userid).ToList();
            List<Contracts.Order> orders = new List<Contracts.Order>();
            foreach (var d_order in d_orders)
            {
                List<int> items = new List<int>();
                foreach (var item in d_order.Items) { items.Add(item.ID); }
                Contracts.Order order = new Contracts.Order();
                order.Id = d_order.ID; order.userid = d_order.User.ID; order.items = items;
                order.creationDate = d_order.CreationDate; order.deliveryDate = d_order.DeliveryDate;
                orders.Add(order);
            }
            return orders;
        }
        public bool CancelItem(int orderid, int itemid)
        {
            Model.Order? order = _database.Orders.Include(o => o.User).Include(o => o.Items).Where(o => o.ID == orderid).FirstOrDefault() ?? new Model.Order();
            if (order == null) { return false; }
            bool find_item = false;
            foreach (var item in order.Items)
            {
                if (item.ID == itemid) 
                {
                    order.Items.Remove(item);
                    Model.Item? i = _database.Items.Where(it => it.ID == item.ID).FirstOrDefault();
                    if (i != null) { i.Count += 1; }
                    find_item = true;
                    _database.SaveChanges();
                    break;
                }
            }
            if (find_item == false) { return false; }
            return true;
        }
        public bool CancelOrder(int orderid)
        {
            Model.Order? order = _database.Orders.Include(o => o.User).Include(o => o.Items).Where(o => o.ID == orderid).FirstOrDefault() ?? new Model.Order();
            if (order == null) { return false; }
            foreach (var item in order.Items)
            {
                Model.Item? i = _database.Items.Where(it => it.ID == item.ID).FirstOrDefault();
                if (i != null) { i.Count += 1; }
                order.Items.Remove(item);
                _database.SaveChanges();
                    break;
            }
            _database.Orders.Remove(order);
            _database.SaveChanges();
            return true;
        }
    }
}
