$(document).ready(function () {
    var deckId = GetDeckIdFromUrl();
    var cardId = GetCardIdFromUrl();

    $('#cancel-btn').on('click', function () {
        event.preventDefault();
        GoBackToDeck(deckId);
    });

    $('input[type=submit]').on('click', () => SubmitForm(deckId, cardId));

    ApiCall.GetDeck(deckId)
        .done(function (deck) {
            $('#deck-name').attr('value', deck.name);
        });

    ApiCall.GetCardInDeck(deckId, cardId)
        .done(function (card) {
            $('#card-title').attr('value', card.title);
            $('#card-description').text(card.description);
        });
});

function SubmitForm(deckId, cardId) {
    event.preventDefault();
    var cardTitle = $('#card-title').val();
    var cardDescription = $('#card-description').val();
    var updatedCard = { title: cardTitle, description: cardDescription };

    ApiCall.UpdateCard(deckId, cardId, JSON.stringify(updatedCard))
        .done(() => GoBackToDeck(deckId));
}

function GoBackToDeck(deckId) {
    window.location.replace(window.location.origin + '/Decks/' + deckId + '/Cards');
}
