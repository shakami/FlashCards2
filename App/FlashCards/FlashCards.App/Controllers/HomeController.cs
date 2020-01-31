using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlashCards.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToRoute(nameof(DecksController.GetDecks));
        }
    }
}
