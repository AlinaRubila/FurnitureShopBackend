using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FurnitureShop.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        UserDB _userDB = new UserDB();
        ILogger<UserController> _logger;
        IRabbitMQService _rabbitMQService;
        public UserController(ILogger<UserController> logger, IRabbitMQService rabbitMQ)
        {
            _logger = logger;
            _rabbitMQService = rabbitMQ;
        }
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            if (login.Trim().Length < 5 || password.Trim().Length < 8) { return Ok("Заполните поля!"); }
            Contracts.Token? token = _userDB.Login(login, password);
            if (token == null) { return Ok("Неверный логин или пароль"); }
            else 
            {
                _logger.LogInformation($"[Info --> UserController] В систему вошёл пользователь {login}");
                _rabbitMQService.SendMessage($"{DateTime.Now} User with e-mail {login} has signed in");
                return Ok(token.token); 
            }
        }
        [HttpPost]
        public IActionResult Register(string name, string login, string password)
        {
            if (name.Trim().Length < 2 || login.Trim().Length < 5 || password.Trim().Length < 8) { return Ok("Имя должно быть от 2 символов, логин - от 5 символов, пароль - от 8 символов"); }
            Contracts.Token? token = _userDB.Register(name, login, password);
            if (token == null) { return Ok("Пользователь с таким логином уже существует"); }
            _logger.LogInformation($"[Info --> UserController] В системе был зарегистрирован пользователь {login}");
            _rabbitMQService.SendMessage($"{DateTime.Now} User with e-mail {login} has been registered");
            return Ok(token.token);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Access")]
        public IActionResult GetInfo()
        {
            var s = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userid = 0;
            if (s != null) { userid = int.Parse(s); }
            Contracts.UserInfo userInfo = _userDB.GetUserInfo(userid);
            return Ok(userInfo);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Access", Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            List<Contracts.UserInfo> users = _userDB.GetAllUsers();
            _logger.LogInformation("[Info --> UserController] Была получена информация о пользователях в системе");
            return Ok(users.ToArray());
        }
    }
}
