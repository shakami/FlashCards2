using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/vnd.sepehr.hateoas+json")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    href: Url.Link(nameof(GetRoot), new { }),
                    rel: "self",
                    method: HttpMethods.Get
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(DecksController.GetDecks), new { }),
                    rel: "decks",
                    method: HttpMethods.Get
                ),

                new LinkDto
                (
                    href: Url.Link(nameof(DecksController.CreateDeck), new { }),
                    rel: "create_deck",
                    method: HttpMethods.Post
                )
            };

            return Ok(links);
        }
    }
}