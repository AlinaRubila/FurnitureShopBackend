using FurnitureShop.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FurnitureShop
{
    public class ViewItemsDB
    {
        Model.ApplicationContext _database = new Model.ApplicationContext();
        public List<Contracts.Item> ShowAll()
        {
            var s = _database.Items.Include(i => i.Furniture).ToList();
            var items = s.Select(item => new Contracts.Item
            {
                ID = item.ID,
                Name = item.Name,
                Category = new Contracts.FurnitureCat { ID = item.Furniture.ID, Name = item.Furniture.Name },
                Height = item.Height,
                Length = item.Length,
                Price = item.Price,
                Width = item.Width,
            }).ToList();
     
            return items;
        }
        public List<Contracts.FurnitureCat> ShowCategories()
        {
            var s = _database.Furnitures.ToList();
            List<Contracts.FurnitureCat> categories = s.Select(cat => new Contracts.FurnitureCat
            {
                ID = cat.ID,
                Name = cat.Name,
            }).ToList();
            return categories;
        }
        public List<Contracts.Item>? ShowByCategory(int id)
        {
            var d_items = _database.Items.Include(i => i.Furniture).Where(p => p.Furniture.ID == id).ToList();
            if (d_items.Count == 0) return null;
            List<Contracts.Item> items = d_items.Select(item => new Contracts.Item
            {
                ID = item.ID,
                Name = item.Name,
                Category = new Contracts.FurnitureCat { ID = item.Furniture.ID, Name = item.Furniture.Name },
                Height = item.Height,
                Length = item.Length,
                Price = item.Price,
                Width = item.Width,
            }).ToList();
            return items;
        }
        public List<Contracts.Item> ShowInStock()
        {
            var s = _database.Items.Include(i => i.Furniture).Where(it => it.Count >= 1).ToList();
            List <Contracts.Item> items = s.Select(item => new Contracts.Item
            {
                ID = item.ID,
                Name = item.Name,
                Category = new Contracts.FurnitureCat { ID = item.Furniture.ID, Name = item.Furniture.Name },
                Height = item.Height,
                Length = item.Length,
                Price = item.Price,
                Width = item.Width,
            }).ToList();
            return items;
        }
        public Contracts.Item? ShowOne(int id)
        {
            Item? d_item = _database.Items.Include(i => i.Furniture).Where(it => it.ID == id).FirstOrDefault();
            if (d_item == null) {return null;}
            Contracts.Item item = new Contracts.Item
            {
                ID = d_item.ID,
                Name = d_item.Name,
                Category = new Contracts.FurnitureCat { ID = d_item.Furniture.ID, Name = d_item.Furniture.Name },
                Length = d_item.Length,
                Width = d_item.Width,
                Height = d_item.Height,
                Price = d_item.Price
            };
            return item;
        }
        public bool? FitItem(int id, float square, float height)
        {
            Item? d_item = _database.Items.Where(it => it.ID == id).FirstOrDefault();
            if (d_item == null) { return null; }
            double room_volume = square * height;
            double item_volume = d_item.Width * d_item.Length * d_item.Height;
            double percentage = Math.Round(item_volume / room_volume, 2) * 100;
            bool result = true;
            if (percentage > 60 || d_item.Height > height) { result = false; }
            return result;
        }
        public bool AddNewItem(Contracts.Item item)
        {
            Contracts.FurnitureCat category = item.Category;
            Furniture? d_category = _database.Furnitures.Where(f => f.ID == category.ID).FirstOrDefault();
            if (d_category == null) { return false; }
            List<Item>? items = _database.Items.ToList();
            Item d_item = new Item 
            {
                ID = items.Count() + 1,
                Name = item.Name,
                Price = item.Price, 
                Furniture = d_category, 
                Length = item.Length,
                Width = item.Width,
                Height = item.Height,
                Count = 1,
            };
            _database.Add(d_item);
            _database.SaveChanges();
            return true;
        }
        public bool ChangeCount(int itemid, int newcount)
        {
            Item? item = _database.Items.FirstOrDefault(i => i.ID == itemid);
            if (item == null) return false;
            item.Count = newcount;
            _database.SaveChanges();
            return true;
        }
    }
}
