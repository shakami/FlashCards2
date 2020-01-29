using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlashCards.Models;
using FlashCards.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers
{
    [ApiController]
    [Route("api/decks")]
    public class DecksController : ControllerBase
    {
        private readonly IFlashCardRepository _flashCardRepository;
        private readonly IMapper _mapper;

        public DecksController(IFlashCardRepository flashCardRepository,
                                IMapper mapper)
        {
            _flashCardRepository = flashCardRepository ??
                throw new ArgumentNullException(nameof(flashCardRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpOptions]
        public IActionResult GetDecksOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpOptions("{deckId}")]
        public IActionResult GetDeckOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,DELETE");
            return Ok();
        }

        [HttpGet]
        public IActionResult GetDecks()
        {
            var decksFromRepo = _flashCardRepository.GetDecks();
            var decksToReturn = _mapper.Map<IEnumerable<DeckDto>>(decksFromRepo);
            return Ok(decksToReturn);
        }

        [HttpGet("{deckId}", Name = "GetDeck")]
        public ActionResult<DeckDto> GetDeck(int deckId)
        {
            var deckFromRepo = _flashCardRepository.GetDeck(deckId);
            if (deckFromRepo == null)
            {
                return NotFound();
            }

            var deckToReturn = _mapper.Map<DeckDto>(deckFromRepo);
            return Ok(deckToReturn);
        }

        [HttpPost]
        public IActionResult CreateDeck(DeckForCreationDto deck)
        {
            var deckEntity = _mapper.Map<Entities.Deck>(deck);

            _flashCardRepository.AddDeck(deckEntity);
            _flashCardRepository.Save();

            var deckToReturn = _mapper.Map<DeckDto>(deckEntity);

            return CreatedAtRoute(nameof(GetDeck), new { deckId = deckToReturn.Id }, deckToReturn);
        }

        [HttpDelete("{deckId}")]
        public IActionResult DeleteDeck(int deckId)
        {
            var deckFromRepo = _flashCardRepository.GetDeck(deckId);

            if (deckFromRepo == null)
            {
                return NotFound();
            }

            _flashCardRepository.DeleteDeck(deckFromRepo);
            _flashCardRepository.Save();

            return NoContent();
        }
    }
}