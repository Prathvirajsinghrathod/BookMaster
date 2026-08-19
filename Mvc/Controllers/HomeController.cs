using Microsoft.AspNetCore.Mvc;

namespace BookMaster.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Error() => View();
}
