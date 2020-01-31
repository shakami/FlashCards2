using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Controllers
{
    [Route("Decks/{deckId}/Cards")]
    public class CardsController : Controller
    {
        [HttpGet]
        public IActionResult GetCards()
        {
            return View();
        }

        [Route("New")]
        public IActionResult CreateCard()
        {
            return View();
        }

        [Route("{flashCardId}/Edit")]
        public IActionResult EditCard()
        {
            return View();
        }
    }
}