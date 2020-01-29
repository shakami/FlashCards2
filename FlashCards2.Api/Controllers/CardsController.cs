using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlashCards.Api.Models;
using FlashCards.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
        public IActionResult GetCardOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,HEAD,DELETE,PUT");
            return Ok();
        }


        [HttpGet(Name = nameof(GetCardsInDeck))]
        [HttpHead]
        public ActionResult<IEnumerable<CardDto>> GetCardsInDeck(int deckId,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                    out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
                    .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            if (!_flashCardRepository.DeckExists(deckId))
            {
                return NotFound();
            }

            var cardsFromRepo = _flashCardRepository.GetCards(deckId);

            var cardsToReturn = _mapper.Map<IEnumerable<CardDto>>(cardsFromRepo);

            if (!includeLinks)
            {
                return Ok(cardsToReturn);
            }

            var cardsWithLinks = cardsToReturn.Select(card =>
            {
                var links = CreateLinksForCard(deckId, card.Id);
                return new CardDtoWithLinks(card, links);
            });

            return Ok(new
            {
                value = cardsWithLinks,
                links = CreateLinksForCards(deckId)
            });
        }

        [HttpGet("{cardId}", Name = nameof(GetCardInDeck))]
        [HttpHead("{cardId}")]
        public ActionResult<CardDto> GetCardInDeck(int deckId, int cardId,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                    out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
                    .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

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

            if (!includeLinks)
            {
                return Ok(cardToReturn);
            }

            var links = CreateLinksForCard(deckId, cardToReturn.Id);
            var cardWithLinks = new CardDtoWithLinks(cardToReturn, links);

            return Ok(cardWithLinks);
        }

        [HttpPost(Name = nameof(CreateCardInDeck))]
        public ActionResult<CardDto> CreateCardInDeck(int deckId, CardForCreationDto card,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                    out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
                    .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            if (!_flashCardRepository.DeckExists(deckId))
            {
                return NotFound();
            }

            var cardEntity = _mapper.Map<Entities.Card>(card);
            _flashCardRepository.AddCard(cardEntity, deckId);
            _flashCardRepository.Save();

            var cardToReturn = _mapper.Map<CardDto>(cardEntity);

            if (!includeLinks)
            {
                return CreatedAtRoute(nameof(GetCardInDeck),
                                    new { deckId, cardId = cardToReturn.Id },
                                    cardToReturn);
            }

            var links = CreateLinksForCard(deckId, cardToReturn.Id);
            var cardWithLinks = new CardDtoWithLinks(cardToReturn, links);

            return CreatedAtRoute(nameof(GetCardInDeck),
                                    new { deckId, cardId = cardToReturn.Id },
                                    cardWithLinks);
        }

        [HttpPut("{cardId}", Name = nameof(UpdateCardInDeck))]
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

        [HttpDelete("{cardId}", Name = nameof(DeleteCardInDeck))]
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

        private IEnumerable<LinkDto> CreateLinksForCard(int deckId, int cardId)
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    href: Url.Link(nameof(GetCardInDeck), new { deckId, cardId }),
                    rel: "self",
                    method: HttpMethods.Get
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(UpdateCardInDeck), new { deckId, cardId }),
                    rel: "update_card",
                    method: HttpMethods.Put
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(DeleteCardInDeck), new { deckId, cardId }),
                    rel: "delete_card",
                    method: HttpMethods.Delete
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(GetCardsInDeck), new { deckId }),
                    rel: "cards_in_deck",
                    method: HttpMethods.Get
                )
            };

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForCards(int deckId)
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    href: Url.Link(nameof(GetCardsInDeck), new { deckId }),
                    rel: "self",
                    method: HttpMethods.Get
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(CreateCardInDeck), new { deckId }),
                    rel: "create_card",
                    method: HttpMethods.Post
                )
            };

            return links;
        }
    }
}