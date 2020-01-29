using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Api.Profiles
{
    public class CardsProfile : Profile
    {
        public CardsProfile()
        {
            CreateMap<Entities.Card, Models.CardDto>();
            CreateMap<Models.CardForCreationDto, Entities.Card>();
            CreateMap<Models.CardForUpdateDto, Entities.Card>();
        }
    }
}
