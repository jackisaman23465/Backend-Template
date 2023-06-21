using System;
using backend_template.Data;
using backend_template.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_template.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost("{firstName}/{lastName}/{address}")]
    public string GetQuery(string id, string firstName, string lastName, string address)
    {
        return $"{firstName}:{lastName}:{address}";
    }

    [HttpGet]
    public IActionResult Index()
    {
        // 使用路由名称生成 URL
        string? url = Url.RouteUrl("GetWeatherForecast");

        // 使用生成的 URL 进行其他操作，例如重定向或渲染到视图中
        return Redirect(url ?? "");
    }
}

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    [HttpPost]
    public IActionResult Login([FromBody] User user)
    {
        // 根据提供的用户名查找用户
        var existingUser = UserRepository.GetByUsername(user.Username);

        if (existingUser == null || existingUser.Password != user.Password)
        {
            return Unauthorized(); // 返回 401 未授权状态码
        }

        // 登录成功，可以生成 JWT 令牌或其他身份验证标识
        // 在这里你可以根据需要返回其他数据，例如用户信息、访问令牌等
        return Ok(new { Message = "登录成功" });
    }

}

[ApiController]
[Route("api/register")]
public class RegisterController : ControllerBase
{
    private readonly UserDbContext _context;

    public RegisterController(UserDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Register([FromBody] User user)
    {
        // 检查用户名是否已存在
        var existingUser = UserRepository.GetByUsername(user.Username);
        if (existingUser != null)
        {
            return Conflict(new { Message = "用户名已存在" }); // 返回 409 冲突状态码
        }

        Console.WriteLine("Register");

        _context.Add(user);
        _context.SaveChanges();

        // 可以在这里进行密码哈希等其他处理
        UserRepository.AddUser(user);

        return CreatedAtRoute("GetUserById", new { id = user.Id }, user); // 返回 201 创建成功状态码，并包含用户信息
    }

    [HttpGet("{id}", Name = "GetUserById")]
    public IActionResult GetUserById(int id)
    {
        var user = UserRepository.GetByUserId(id);
        if (user == null)
        {
            return NotFound(); // 返回 404 未找到状态码
        }

        return Ok(user); // 返回 200 成功状态码，并包含用户信息
    }
}