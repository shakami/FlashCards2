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
            /*var model = _flashCardData.GetDecks().ToList();
            return View(model);
            */
        }

        [HttpGet("New")]
        public IActionResult CreateDeck()
        {
            return View();
        }

        //[HttpPost("New")]
        //public IActionResult CreateDeck(Deck model)
        //{
        //    /*
        //    if (!ModelState.IsValid)
        //    {
        //        return RedirectToAction(nameof(CreateDeck), model);
        //    }
        //    _flashCardData.AddDeck(model);

        //    return RedirectToAction(nameof(GetDecks));
        //    */
        //    return null;
        //}

        [HttpGet("{deckId}/Delete")]
        public IActionResult DeleteDeck(int deckId)
        {
            /*
            var deckToRemove = _flashCardData.GetDeck(deckId);
            _flashCardData.DeleteDeck(deckToRemove);
            return RedirectToAction(nameof(GetDecks));
            */
            return null;
        }
    }
}