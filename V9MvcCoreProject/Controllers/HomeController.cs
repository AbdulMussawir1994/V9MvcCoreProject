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
