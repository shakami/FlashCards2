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
            Response.Headers.Add("Allow", "GET,OPTIONS,HEAD,POST");
            return Ok();
        }

        [HttpOptions("{deckId}")]
        public IActionResult GetDeckOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,HEAD,PUT,DELETE");
            return Ok();
        }

        [HttpGet(Name = nameof(GetDecks))]
        [HttpHead]
        public ActionResult<IEnumerable<DeckDto>> GetDecks(
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                    out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
                    .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            var decksFromRepo = _flashCardRepository.GetDecks();
            var decksToReturn = _mapper.Map<IEnumerable<DeckDto>>(decksFromRepo);

            if (!includeLinks)
            {
                return Ok(decksToReturn);
            }

            var decksWithLinks = decksToReturn.Select(deck =>
            {
                var links = CreateLinksForDeck(deck.Id);
                return new DeckDtoWithLinks(deck, links);
            });

            return Ok(new
            {
                value = decksWithLinks,
                links = CreateLinksForDecks()
            });
        }

        [HttpGet("{deckId}", Name = nameof(GetDeck))]
        [HttpHead("{deckId}")]
        public ActionResult<DeckDto> GetDeck(int deckId,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                    out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
                    .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            var deckFromRepo = _flashCardRepository.GetDeck(deckId);
            if (deckFromRepo == null)
            {
                return NotFound();
            }

            var deckToReturn = _mapper.Map<DeckDto>(deckFromRepo);

            if (!includeLinks)
            {
                return Ok(deckToReturn);
            }

            var links = CreateLinksForDeck(deckId);
            var deckWithLinks = new DeckDtoWithLinks(deckToReturn, links);

            return Ok(deckWithLinks);
        }

        [HttpPost(Name = nameof(CreateDeck))]
        public ActionResult<DeckDto> CreateDeck(DeckForCreationDto deck,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                    out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
                    .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            var deckEntity = _mapper.Map<Entities.Deck>(deck);

            _flashCardRepository.AddDeck(deckEntity);
            _flashCardRepository.Save();

            var deckToReturn = _mapper.Map<DeckDto>(deckEntity);

            if (!includeLinks)
            {
                return CreatedAtRoute(nameof(GetDeck),
                                        new { deckId = deckToReturn.Id },
                                        deckToReturn);
            }

            var links = CreateLinksForDeck(deckToReturn.Id);
            var deckWithLinks = new DeckDtoWithLinks(deckToReturn, links);

            return CreatedAtRoute(nameof(GetDeck),
                                    new { deckId = deckToReturn.Id },
                                    deckWithLinks);
        }

        [HttpPut("{deckId}", Name = nameof(UpdateDeck))]
        public ActionResult UpdateDeck(int deckId, DeckForUpdateDto deck)
        {
            var deckFromRepo = _flashCardRepository.GetDeck(deckId);

            if (deckFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(deck, deckFromRepo);

            _flashCardRepository.UpdateDeck(deckFromRepo);
            _flashCardRepository.Save();

            return NoContent();
        }

        [HttpDelete("{deckId}", Name = nameof(DeleteDeck))]
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

        private IEnumerable<LinkDto> CreateLinksForDeck(int deckId)
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    href: Url.Link(nameof(GetDeck), new { deckId }),
                    rel: "self",
                    method: HttpMethods.Get
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(UpdateDeck), new { deckId }),
                    rel: "update_deck",
                    method: HttpMethods.Put
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(DeleteDeck), new { deckId }),
                    rel: "delete_deck",
                    method: HttpMethods.Delete
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(CardsController.CreateCardInDeck), new { deckId }),
                    rel: "create_card_for_deck",
                    method: HttpMethods.Post
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(CardsController.GetCardsInDeck), new { deckId }),
                    rel: "cards",
                    method: HttpMethods.Get
                )
            };

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForDecks()
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    href: Url.Link(nameof(GetDecks), new { }),
                    rel: "self",
                    method: HttpMethods.Get
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(CreateDeck), new { }),
                    rel: "create_deck",
                    method: HttpMethods.Post
                )
            };

            return links;
        }
    }
}