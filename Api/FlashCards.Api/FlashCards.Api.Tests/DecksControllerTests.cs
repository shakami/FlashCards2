using AutoMapper;
using FlashCards.Api.Controllers;
using FlashCards.Api.Models;
using FlashCards.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FlashCards.Api.Tests
{
    [TestClass]
    public class DecksControllerTests
    {
        static IFlashCardRepository _flashCardRepository;
        static IMapper _mapper;
        static DecksController _decks;
        const string jsonMediaType = "application/json";

        [ClassInitialize]
        public static void DecksControllerInitialize(TestContext context)
        {
            _flashCardRepository = new FlashCardRepository("./Data/testData.json");

            var decksProfile = new Profiles.DecksProfile();
            var cardsProfile = new Profiles.CardsProfile();
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile(decksProfile);
                config.AddProfile(cardsProfile);
            });
            _mapper = new Mapper(configuration);

            _decks = new DecksController(_flashCardRepository, _mapper);
        }

        [TestMethod]
        public void GetDecks()
        {
            //-- Arrange

            //-- Act
            var response = _decks.GetDecks(jsonMediaType).Result as OkObjectResult;
            var results = response.Value as IEnumerable<DeckDto>;

            //-- Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(3, results.Count());
        }

        [TestMethod]
        public void GetDeck()
        {
            //-- Arrange

            //-- Act
            var response = _decks.GetDeck(5, jsonMediaType).Result as OkObjectResult;
            var results = response.Value as DeckDto;

            //-- Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("math", results.Name);
        }

        [TestMethod]
        public void GetDeckNoneExisting()
        {
            //-- Arrange

            //-- Act
            var response = _decks.GetDeck(1, jsonMediaType).Result as NotFoundResult;

            //-- Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CreateDeckAndDeleteDeck()
        {
            //-- Arrange
            var newDeck = new DeckForCreationDto
            {
                Name = "test deck"
            };

            //-- Act
            var response = _decks.CreateDeck(newDeck, jsonMediaType).Result as CreatedAtRouteResult;

            var newDeckId = (int)response.RouteValues["deckId"];
            var createdDeck = _flashCardRepository.GetDeck(newDeckId);

            //-- Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("test deck", createdDeck.Name);

            
            /*--------- delete the deck ------------*/
            //-- Act
            _decks.DeleteDeck(newDeckId);

            var getResponse = _decks.GetDecks(jsonMediaType).Result as OkObjectResult;
            var getResult = getResponse.Value as IEnumerable<DeckDto>;

            //-- Assert
            Assert.AreEqual(3, getResult.Count());
        }

        [TestMethod]
        public void CreateDeckWithCards()
        {
            //-- Arrange
            var newDeck = new DeckForCreationDto
            {
                Name = "test deck",
                Cards = new List<CardForCreationDto>()
                {
                    new CardForCreationDto
                    {
                        Title = "card1",
                        Description = "card1-description"
                    },
                    new CardForCreationDto
                    {
                        Title = "card2",
                        Description = "card2-description"
                    },
                    new CardForCreationDto
                    {
                        Title = "card3",
                        Description = "card3-description"
                    }
                }
            };

            //-- Act
            var response = _decks.CreateDeck(newDeck, jsonMediaType).Result as CreatedAtRouteResult;

            //-- Assert
            Assert.IsNotNull(response);

            // check if the right deck was created
            //-- Arrange
            var newDeckId = (int)response.RouteValues["deckId"];
            var createdDeck = _flashCardRepository.GetDeck(newDeckId);

            Assert.AreEqual("test deck", createdDeck.Name);
            Assert.AreEqual(3, createdDeck.Cards.Count());

            // delete the deck
            //-- Act
            _decks.DeleteDeck(newDeckId);
        }
    }
}
