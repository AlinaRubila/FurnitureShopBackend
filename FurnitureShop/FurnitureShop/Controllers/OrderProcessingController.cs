using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderProcessingController : ControllerBase
    {
        OrderProcessingDB _orderProcessingDB = new OrderProcessingDB();
        ILogger<OrderProcessingController> _logger;
        public OrderProcessingController(ILogger<OrderProcessingController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Access")]
        public IActionResult ViewOrders()
        {
            var s = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userid = 0;
            if (s != null) { userid = int.Parse(s); }
            List<Contracts.Order> orders = _orderProcessingDB.OrdersList(userid);
            return Ok(orders.ToArray());
        }
        [HttpPost]

        public IActionResult CancelOne(int orderid, int itemid)
        {
            bool result = _orderProcessingDB.CancelItem(orderid, itemid);
            if (result == false) { return Ok("Неверный номер заказа или товара!"); }
            _logger.LogInformation($"[Info --> OrderProcessingController] Пользователь отменил один из товаров в заказе  №{orderid}");
            return Ok("Вы отменили один из товаров заказа");
        }
        [HttpPost]
        public IActionResult CancelAll(int orderid)
        {
            bool result = _orderProcessingDB.CancelOrder(orderid);
            if (result == false) { return Ok("Неверный номер заказа!"); }
            _logger.LogInformation($"[Info --> OrderProcessingController] Пользователь отменил весь заказ №{orderid}");
            return Ok("Вы отменили весь заказ");
        }
    }
}
