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
            List<Contracts.Item> items = new List<Contracts.Item>();
            foreach (var d_item in s) 
            {
                Contracts.Item item = new Contracts.Item();
                Contracts.FurnitureCat cat = new Contracts.FurnitureCat();
                cat.ID = d_item.Furniture.ID; cat.name = d_item.Furniture.Name;
                item.ID = d_item.ID; item.name = d_item.Name; item.price = d_item.Price;
                item.length = d_item.Length; item.width = d_item.Width; item.height = d_item.Height; item.category = cat;
                items.Add(item);
            }
            return items;
        }
        public List<Contracts.FurnitureCat> ShowCategories()
        {
            var s = _database.Furnitures.ToList();
            List<Contracts.FurnitureCat> categories = new List<Contracts.FurnitureCat>();
            foreach ( var cat in s)
            {
                Contracts.FurnitureCat category = new Contracts.FurnitureCat();
                category.ID = cat.ID; category.name = cat.Name;
                categories.Add(category);
            }
            return categories;
        }
        public List<Contracts.Item>? ShowByCategory(int id)
        {
            var d_items = _database.Items.Include(i => i.Furniture).Where(p => p.Furniture.ID == id).ToList();
            if (d_items.Count == 0) return null;
            List<Contracts.Item> items = new List<Contracts.Item>();
            foreach ( var d_item in d_items)
            {
                Contracts.Item item = new Contracts.Item();
                Model.Furniture d_cat = d_item.Furniture;
                Contracts.FurnitureCat cat = new Contracts.FurnitureCat();
                cat.ID = d_cat.ID; cat.name = d_cat.Name;
                item.ID = d_item.ID; item.name = d_item.Name; item.price = d_item.Price;
                item.length = d_item.Length; item.width = d_item.Width; item.height = d_item.Height; item.category = cat;
                items.Add(item);
            }
            return items;
        }
        public List<Contracts.Item> ShowInStock()
        {
            var s = _database.Items.Include(i => i.Furniture).Where(it => it.Count >= 1).ToList();
            List <Contracts.Item> items = new List<Contracts.Item> ();
            foreach ( var d_item in s)
            {
                    Contracts.Item item = new Contracts.Item();
                    Contracts.FurnitureCat cat = new Contracts.FurnitureCat();
                    cat.ID = d_item.Furniture.ID; cat.name = d_item.Furniture.Name;
                    item.ID = d_item.ID; item.name = d_item.Name; item.price = d_item.Price;
                    item.length = d_item.Length; item.width = d_item.Width; item.height = d_item.Height; item.category = cat;
                    items.Add(item);
            }
            return items;
        }
        public Contracts.Item? ShowOne(int id)
        {
            Model.Item? d_item = _database.Items.Include(i => i.Furniture).Where(it => it.ID == id).FirstOrDefault();
            if (d_item == null) {return null;}
            Contracts.Item item = new Contracts.Item();
            Contracts.FurnitureCat cat = new Contracts.FurnitureCat();
            cat.ID = d_item.Furniture.ID; cat.name = d_item.Furniture.Name;
            item.ID = d_item.ID; item.name = d_item.Name; item.price = d_item.Price;
            item.length = d_item.Length; item.width = d_item.Width; item.height = d_item.Height; item.category = cat;
            return item;
        }
        public bool? FitItem(int id, float square, float height)
        {
            Model.Item? d_item = _database.Items.Where(it => it.ID == id).FirstOrDefault();
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
            Contracts.FurnitureCat category = item.category;
            Furniture? d_category = _database.Furnitures.Where(f => f.ID == category.ID).FirstOrDefault();
            if (d_category == null) { return false; }
            List<Model.Item>? items = _database.Items.ToList();
            Item d_item = new Item();
            d_item.ID = items.Count() + 1;
            d_item.Name = item.name; d_item.Price = item.price; d_item.Furniture = d_category;
            d_item.Length = item.length; d_item.Width = item.width; d_item.Height = item.height; d_item.Count = 1;
            _database.Add(d_item);
            _database.SaveChanges();
            return true;
        }
        public bool ChangeCount(int itemid, int newcount)
        {
            Model.Item? item = _database.Items.FirstOrDefault(i => i.ID == itemid);
            if (item == null) return false;
            item.Count = newcount;
            _database.SaveChanges();
            return true;
        }
    }
}
