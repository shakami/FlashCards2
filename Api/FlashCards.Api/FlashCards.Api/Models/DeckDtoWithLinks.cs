using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Api.Models
{
    public class DeckDtoWithLinks : DeckDto
    {
        public ICollection<LinkDto> Links { get; set; }

        public DeckDtoWithLinks(DeckDto deck, IEnumerable<LinkDto> links)
        {
            Id = deck.Id;
            Name = deck.Name;
            Links = links.ToList();
        }
    }
}
