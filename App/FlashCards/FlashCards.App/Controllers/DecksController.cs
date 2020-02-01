using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Controllers
{
    [Route("Decks")]
    public class DecksController : Controller
    {
        [HttpGet(Name = nameof(GetDecks))]
        public IActionResult GetDecks()
        {
            return View();
        }

        [HttpGet("New")]
        public IActionResult CreateDeck()
        {
            return View();
        }

        [HttpGet("{deckId}/Edit")]
        public IActionResult EditDeck()
        {
            return View();
        }
    }
}