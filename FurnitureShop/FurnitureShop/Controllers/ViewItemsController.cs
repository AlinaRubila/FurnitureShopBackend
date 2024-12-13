using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace FurnitureShop.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ViewItemsController : ControllerBase
    {
        ViewItemsDB _viewItemsDB = new ViewItemsDB();
        ILogger<ViewItemsController> _logger;
        public ViewItemsController(ILogger<ViewItemsController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult ShowAll()
        {
            List<Contracts.Item> items = _viewItemsDB.ShowAll();
            _logger.LogInformation("[Info --> ViewItemsController] Пользователь запросил список товаров из БД");
            return Ok(items.ToArray());
        }
        [HttpGet]
        public IActionResult ShowCategories()
        {
            List<Contracts.FurnitureCat> categories = _viewItemsDB.ShowCategories();
            _logger.LogInformation("[Info --> ViewItemsController] Пользователь запросил список категорий из БД");
            return Ok(categories.ToArray());
        }
        [HttpGet]
        public IActionResult ShowByCategory(int catid)
        {
            List<Contracts.Item>? items = _viewItemsDB.ShowByCategory(catid);
            if (items == null) { return Ok("Такой категории не существует!"); }
            _logger.LogInformation("[Info --> ViewItemsController] Пользователь запросил список товаров по категории из БД");
            return Ok(items.ToArray());
        }
        [HttpGet]
        public IActionResult ShowInStock()
        {
            List<Contracts.Item> items = _viewItemsDB.ShowInStock();
            _logger.LogInformation("[Info --> ViewItemsController] Пользователь запросил список товаров в наличии из БД");
            return Ok(items.ToArray());
        }
        [HttpGet]
        public IActionResult ShowItem(int itemid)
        {
            Contracts.Item? item = _viewItemsDB.ShowOne(itemid);
            if (item == null) { return Ok("Товара с таким id не существует"); }
            _logger.LogInformation("[Info --> ViewItemsController] Пользователь запросил инфо о товаре из БД");
            return Ok(item);
        }
        [HttpPost]
        public IActionResult FitItemIn(int itemid, float roomsquare, float roomheight)
        {
            bool? ifFitted = _viewItemsDB.FitItem(itemid, roomsquare, roomheight);
            if (ifFitted == null) { return Ok("Предмета с таким id не существует"); }
            if (ifFitted == true) { return Ok("Предмет помещается в комнате"); }
            else { return Ok("Предмет не помещается в комнате"); }
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Access", Roles = "Admin")]
        public IActionResult AddNewItem(string name, double price, double length, double width, double height, int catid)
        {
            Contracts.Item item = new Contracts.Item();
            item.ID = 0; item.name = name; item.price = price; item.length = length; item.width = width; item.height = height;
            Contracts.FurnitureCat cat = new Contracts.FurnitureCat();
            cat.ID = catid; cat.name = ""; item.category = cat;
            bool result = _viewItemsDB.AddNewItem(item);
            if (result == false) 
            {
                _logger.LogInformation("[Info --> ViewItemsController] Не удалось добавить новый товар");
                return Ok("При добавлении предмета произошла ошибка!"); 
            }
            _logger.LogInformation("[Info --> ViewItemsController] В БД был добавлен новый товар");
            return Ok("Новый предмет добавлен");
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Access", Roles = "Admin")]
        public IActionResult ChangeCount(int itemid, int newcount)
        {
            if (newcount < 0) { return Ok("Значение количества не должно быть отрицательным!"); }
            bool result = _viewItemsDB.ChangeCount(itemid, newcount);
            if (result == false) { return Ok("Товара с таким id не существует!"); }
            _logger.LogInformation("[Info --> ViewItemsController] Количество товара изменено");
            return Ok("Количество изменено");
        }
    }
}
