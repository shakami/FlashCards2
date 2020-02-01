using AutoMapper;
using FlashCards.Api.Controllers;
using FlashCards.Api.Models;
using FlashCards.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashCards.Api.Tests
{
    [TestClass]
    public class CardsControllerTests
    {
        static IFlashCardRepository _flashCardRepository;
        static IMapper _mapper;
        static CardsController _cards;
        const string jsonMediaType = "application/json";

        [ClassInitialize]
        public static void CardsControllerInitialize(TestContext context)
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

            _cards = new CardsController(_flashCardRepository, _mapper);
        }

        [TestMethod]
        public void GetCards()
        {
            //-- Arrange

            //-- Act
            var response = _cards.GetCardsInDeck(8, jsonMediaType).Result as OkObjectResult;
            var results = response.Value as IEnumerable<CardDto>;

            //-- Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(7, results.Count());
        }

        [TestMethod]
        public void GetCardsNoneExistingDeck()
        {
            //-- Arrange

            //-- Act
            var response = _cards.GetCardsInDeck(1, jsonMediaType).Result;

            //-- Assert
            Assert.IsNotNull(response as NotFoundResult);
        }

        [TestMethod]
        public void GetCard()
        {
            //-- Arrange
            var response = _cards.GetCardInDeck(8, 16, jsonMediaType).Result as OkObjectResult;

            //-- Act
            var result = response.Value as CardDto;

            //-- Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("molarity", result.Title);
        }

        [TestMethod]
        public void GetCardNoneExistingDeck()
        {
            //-- Arrange

            //-- Act
            var response = _cards.GetCardInDeck(1, 16, jsonMediaType).Result;

            //-- Assert
            Assert.IsNotNull(response as NotFoundResult);
        }

        [TestMethod]
        public void GetCardNoneExistingCard()
        {
            //-- Arrange

            //-- Act
            var response = _cards.GetCardInDeck(8, 1, jsonMediaType).Result;

            //-- Assert
            Assert.IsNotNull(response as NotFoundResult);
        }

        [TestMethod]
        public void CreateCardAndDeleteCard()
        {
            //-- Arrange
            var newCard = new CardForCreationDto
            {
                Title = "new card",
                Description = "description"
            };
            var deckId = 5;

            //-- Act
            var response = _cards.CreateCardInDeck(deckId, newCard, jsonMediaType).Result;

            var createdAtRouteResult = response as CreatedAtRouteResult;

            var newCardId = (int)createdAtRouteResult.RouteValues["cardId"];
            var createdCard = _flashCardRepository.GetCard(newCardId);

            //-- Assert
            Assert.IsNotNull(createdAtRouteResult);
            Assert.AreEqual(deckId, createdCard.DeckId);
            Assert.AreEqual("new card", createdCard.Title);
            Assert.AreEqual("description", createdCard.Description);


            /*--------- delete the card ------------*/
            //-- Act
            _cards.DeleteCardInDeck(deckId, newCardId);

            var getResponse = _cards.GetCardInDeck(deckId, newCardId, jsonMediaType).Result;

            //-- Assert
            Assert.IsNotNull(getResponse as NotFoundResult);
        }

        [TestMethod]
        public void CreateCardNoneExistingDeck()
        {
            //-- Arrange
            var newCard = new CardForCreationDto
            {
                Title = "new card",
                Description = "description"
            };
            var noneExistingDeckId = 1;

            //-- Act
            var response = _cards.CreateCardInDeck(noneExistingDeckId, newCard, jsonMediaType).Result;

            //-- Assert
            Assert.IsNotNull(response as NotFoundResult);
        }

        [TestMethod]
        public void UpdateCard()
        {
            //-- Arrange
            var cardId = 16;
            var oldCard = _flashCardRepository.GetCard(cardId);
            var oldTitle = oldCard.Title;
            var oldDescription = oldCard.Description;

            var updatedCard = new CardForUpdateDto
            {
                Title = "updated" + oldCard.Title,
                Description = "updated" + oldCard.Description
            };
            var deckId = oldCard.DeckId;

            //-- Act
            var response = _cards.UpdateCardInDeck(deckId, cardId, updatedCard);

            var cardAfterUpdate = _flashCardRepository.GetCard(cardId);

            //-- Assert
            Assert.IsNotNull(response as NoContentResult);
            Assert.AreEqual(cardAfterUpdate.DeckId, deckId);
            Assert.AreEqual(cardAfterUpdate.Title, updatedCard.Title);
            Assert.AreEqual(cardAfterUpdate.Description, updatedCard.Description);

            // cleanup
            oldCard.Title = oldTitle;
            oldCard.Description = oldDescription;
            _flashCardRepository.UpdateCard(oldCard);
            _flashCardRepository.Save();
        }

        [TestMethod]
        public void UpdateCardNoneExistingDeck()
        {
            //-- Arrange
            var nonExistingDeckId = 1;
            var cardId = 16;

            var updatedCard = new CardForUpdateDto
            {
                Title = "updated",
                Description = "updated"
            };

            //-- Act
            var response = _cards.UpdateCardInDeck(nonExistingDeckId, cardId, updatedCard);

            //-- Assert
            Assert.IsNotNull(response as NotFoundResult);
        }

        [TestMethod]
        public void UpdateCardNoneExistingCard()
        {
            //-- Arrange
            var deckId = 5;
            var nonExistingCardId = 1;

            var updatedCard = new CardForUpdateDto
            {
                Title = "updated",
                Description = "updated"
            };

            //-- Act
            var response = _cards.UpdateCardInDeck(deckId, nonExistingCardId, updatedCard);

            //-- Assert
            Assert.IsNotNull(response as NotFoundResult);
        }
    }
}
