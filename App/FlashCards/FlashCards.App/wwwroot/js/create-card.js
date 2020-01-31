$(document).ready(function () {
    $('#cancel-btn').on('click', function () {
        event.preventDefault();
        GoBackToDeck(deckId);
    });

    var deckId = GetDeckIdFromUrl();

    $('input[type=submit]').on('click', () => SubmitForm(deckId));

    ApiCall.GetDeck(deckId).done(function (deck) {
        $('#deck-name').attr('value', deck.name);
    });
});

function SubmitForm(deckId) {
    event.preventDefault();
    var cardTitle = $('#card-title').val();
    var cardDescription = $('#card-description').val();
    var cardToCreate = { title: cardTitle, description: cardDescription };

    ApiCall.CreateCard(deckId, JSON.stringify(cardToCreate))
        .done(() => GoBackToDeck(deckId));
}

function GoBackToDeck(deckId) {
    window.location.replace(window.location.origin + '/Decks/' + deckId + '/Cards');
}
