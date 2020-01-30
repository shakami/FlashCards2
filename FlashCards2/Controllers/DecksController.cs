﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Controllers
{
    [Route("Decks")]
    public class DecksController : Controller
    {
        [HttpGet(Name = "App.GetDecks")]
        public IActionResult GetDecks()
        {
            return View();
            /*var model = _flashCardData.GetDecks().ToList();
            return View(model);
            */
        }

        [HttpGet("CreateDeck")]
        public IActionResult CreateDeck()
        {
            return View();
        }

        [HttpPost("CreateDeck")]
        public IActionResult CreateDeck(Deck model)
        {
            /*
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(CreateDeck), model);
            }
            _flashCardData.AddDeck(model);

            return RedirectToAction(nameof(GetDecks));
            */
            return null;
        }

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