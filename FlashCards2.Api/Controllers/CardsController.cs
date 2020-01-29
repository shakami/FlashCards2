using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlashCards.Api.Models;
using FlashCards.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers
{
    [Route("api/decks/{deckId}/cards")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IFlashCardRepository _flashCardRepository;
        private readonly IMapper _mapper;

        public CardsController(IFlashCardRepository flashCardRepository,
                                IMapper mapper)
        {
            _flashCardRepository = flashCardRepository ??
                throw new ArgumentNullException(nameof(flashCardRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpOptions]
        public IActionResult GetCardsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,HEAD,POST");
            return Ok();
        }

        [HttpOptions("{cardId}")]
        public IActionResult GetDeckOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,HEAD,DELETE,PUT");
            return Ok();
        }


        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<CardDto>> GetCardsInDeck(int deckId)
        {
            if (!_flashCardRepository.DeckExists(deckId))
            {
                return NotFound();
            }

            var cardsFromRepo = _flashCardRepository.GetCards(deckId);

            var cardsToReturn = _mapper.Map<IEnumerable<CardDto>>(cardsFromRepo);
            return Ok(cardsToReturn);
        }

        [HttpGet("{cardId}", Name = nameof(GetCardInDeck))]
        [HttpHead("{cardId}")]
        public ActionResult<CardDto> GetCardInDeck(int deckId, int cardId)
        {
            if (!_flashCardRepository.DeckExists(deckId))
            {
                return NotFound();
            }

            var cardFromRepo = _flashCardRepository.GetCard(cardId);

            if (cardFromRepo == null)
            {
                return NotFound();
            }

            var cardToReturn = _mapper.Map<CardDto>(cardFromRepo);
            return Ok(cardToReturn);
        }

        [HttpPost]
        public ActionResult<CardDto> CreateCardInDeck(int deckId, CardForCreationDto card)
        {
            if (!_flashCardRepository.DeckExists(deckId))
            {
                return NotFound();
            }

            var cardEntity = _mapper.Map<Entities.Card>(card);
            _flashCardRepository.AddCard(cardEntity, deckId);
            _flashCardRepository.Save();

            var cardToReturn = _mapper.Map<CardDto>(cardEntity);
            return CreatedAtRoute(nameof(GetCardInDeck),
                                    new { deckId, cardId = cardToReturn.Id },
                                    cardToReturn);
        }

        [HttpPut("{cardId}")]
        public ActionResult UpdateCardInDeck(int deckId, int cardId, CardForUpdateDto card)
        {
            if (!_flashCardRepository.DeckExists(deckId))
            {
                return NotFound();
            }

            var cardFromRepo = _flashCardRepository.GetCard(cardId);

            if (cardFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(card, cardFromRepo);

            _flashCardRepository.UpdateCard(cardFromRepo);
            _flashCardRepository.Save();

            return NoContent();
        }

        [HttpDelete("{cardId}")]
        public ActionResult DeleteCardInDeck(int deckId, int cardId)
        {
            if (!_flashCardRepository.DeckExists(deckId))
            {
                return NotFound();
            }

            var cardFromRepo = _flashCardRepository.GetCard(cardId);

            if (cardFromRepo == null)
            {
                return NotFound();
            }

            _flashCardRepository.DeleteCard(cardFromRepo);
            _flashCardRepository.Save();

            return NoContent();
        }
    }
}