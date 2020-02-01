$(document).ready(function () {
    var deckId = GetDeckIdFromUrl();

    $('#cancel-btn').on('click', function () {
        event.preventDefault();
        GoBackToDeck(deckId);
    });

    $('input[type=submit]').on('click', () => SubmitForm(deckId));

    ApiCall.GetDeck(deckId)
        .done(function (deck) {
            $('#deck-name').attr('value', deck.name);
        });
});

function SubmitForm(deckId) {
    event.preventDefault();
    var deckName = $('#deck-name').val();
    var updatedDeck = { name: deckName };

    ApiCall.UpdateDeck(deckId, JSON.stringify(updatedDeck))
        .done(() => GoBackToDeck(deckId));
}

function GoBackToDeck(deckId) {
    window.location.replace(window.location.origin + '/Decks/' + deckId + '/Cards');
}
