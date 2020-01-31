$(document).ready(function () {
    $('#cancel-btn').on('click', function () {
        event.preventDefault();
        GoBackToDeck(deckId);
    });

    var deckId = window.location.pathname.match('(?<=Decks\/)[0-9]+')[0];
    var cardId = window.location.pathname.match('(?<=Cards\/)[0-9]+')[0];

    $('input[type=submit]').on('click', () => SubmitForm(deckId, cardId));

    $.ajax({
        headers: {
            Accept: "application/json",
            'Cache-Control': 'no-cache'
        },
        url: 'https://localhost:44789/api/decks/' + deckId,
        method: 'GET'
    }).done(function (data) {
        var deck = ParseJsonToDeck(data);
        $('#deck-name').attr({
            value: deck.Name,
            disabled: true
        });
    });

    $.ajax({
        headers: {
            Accept: "application/json",
            'Cache-Control': 'no-cache'
        },
        url: 'https://localhost:44789/api/decks/' + deckId + '/cards/' + cardId,
        method: 'GET'
    }).done(function (data) {
        var card = ParseJsonToCard(data);
        $('#card-title').attr('value', card.Title);
        $('#card-description').text(card.Description);
    });
});

function SubmitForm(deckId, cardId) {
    event.preventDefault();
    var cardTitle = $('#card-title').val();
    var cardDescription = $('#card-description').val();

    $.ajax({
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
        },
        url: 'https://localhost:44789/api/decks/' + deckId + '/cards/' + cardId,
        method: 'PUT',
        data: "{'title': '" + cardTitle + "', 'description': '" + cardDescription + "'}",
        statusCode: {
            422: jqXHR => ShowValidationError(jqXHR)
        }
    })
        .fail(ShowGenericError)
        .done(() => GoBackToDeck(deckId));
}

function GoBackToDeck(deckId) {
    window.location.replace(window.location.origin + '/Decks/' + deckId + '/Cards');
}
