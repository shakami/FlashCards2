using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Api.Models
{
    public class DeckForCreationDto
    {
        [Required]
        public string Name { get; set; }

        public ICollection<CardForCreationDto> Cards { get; set; }
            = new List<CardForCreationDto>();
    }
}
