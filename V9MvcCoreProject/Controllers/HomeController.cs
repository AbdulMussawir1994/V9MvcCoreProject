using Microsoft.AspNetCore.Mvc;
using V9MvcCoreProject.Middleware.PermissionAttribute;

namespace V9MvcCoreProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [ServiceFilter(typeof(CheckUserPermission))]
    // [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    // [AllowAnonymous]
    [CheckUserSession]
    public JsonResult GetSelectList()
    {
        List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 4, 6, 3, 6, 7 };
        var result = numbers.Distinct().Select(x => new { Id = x, Name = "Number " + x }).ToList();
        return Json(result);
    }

    [ServiceFilter(typeof(CheckUserPermission))]
    public IActionResult Privacy()
    {
        return View();
    }


    [ServiceFilter(typeof(CheckUserPermission))]
    public IActionResult Test()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Unauthorize()
    {
        return View();
    }

}
