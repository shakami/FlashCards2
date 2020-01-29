using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Api.Models
{
    public class CardDtoWithLinks : CardDto
    {
        public ICollection<LinkDto> Links { get; set; }

        public CardDtoWithLinks(CardDto card, IEnumerable<LinkDto> links)
        {
            Id = card.Id;
            DeckId = card.DeckId;
            Title = card.Title;
            Description = card.Description;
            Links = links.ToList();
        }
    }
}
