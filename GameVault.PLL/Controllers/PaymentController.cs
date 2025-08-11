using Microsoft.AspNetCore.Mvc;

namespace GameVault.PLL.Controllers
{
    public class PaymentController : Controller
    {

        public IActionResult BuyGame(int GameId)
        {
            ViewBag.GameId = GameId;
            return View();
        }
    }

}
