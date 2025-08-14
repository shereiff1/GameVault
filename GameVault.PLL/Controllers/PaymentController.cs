using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameVault.PLL.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {

        public IActionResult BuyGame(int GameId)
        {
            ViewBag.GameId = GameId;
            return View();
        }
    }

}
