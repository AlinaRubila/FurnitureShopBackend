using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;

namespace FurnitureShop.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CartWorkController : ControllerBase
    {
        CartWorkDB _cartWorkDB = new CartWorkDB();
        ILogger<CartWorkController> _logger;
        public CartWorkController(ILogger<CartWorkController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Access")]
        public ActionResult<int[]> ViewCart()
        {
            var s = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userid = 0;
            if (s != null) { userid = int.Parse(s); }
            List<int> items = _cartWorkDB.ViewCart(userid);
            return Ok(items.ToArray());
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Access")]
        public IActionResult AddToCart(int itemid, int count)
        {
            var s = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userid = 0;
            if (s != null) { userid = int.Parse(s); }
            bool result = _cartWorkDB.AddToCart(userid, itemid, count);
            if (result == false) { return Ok("Данного товара сейчас нет в наличии в данном количестве. Возьмите меньше или повторите попытку позже"); }
            return Ok("Добавлен новый товар в корзину");
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Access")]
        public IActionResult DeleteItem(int itemid)
        {
            var s = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userid = 0;
            if (s != null) { userid = int.Parse(s); }
            bool result = _cartWorkDB.DeleteItem(userid, itemid);
            if (result == false) { return Ok("Неверный идентификатор товара"); }
            return Ok("Товар удалён из корзины");
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Access")]
        public IActionResult OrderItem(int itemid)
        {
            var s = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userid = 0;
            if (s != null) { userid = int.Parse(s); }
            bool inStock = _cartWorkDB.OrderItem(userid, itemid);
            if (inStock) 
            {
                _logger.LogInformation("[Info --> CartWorkController] Совершён заказ");
                return Ok("Вы заказали товар"); 
            }
            else { return Ok("Товар недоступен для заказа"); }
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Access")]
        public IActionResult OrderAll()
        {
            var s = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userid = 0;
            if (s != null) { userid = int.Parse(s); }
            bool inStock = _cartWorkDB.OrderAll(userid);
            if (inStock) 
            {
                _logger.LogInformation("[Info --> CartWorkController] Совершён заказ");
                return Ok("Вы заказали товары"); 
            }
            else { return Ok("Один или несколько товаров недоступны для заказа. Пожалуйста, выберите другой товар"); }
        }
    }
}
