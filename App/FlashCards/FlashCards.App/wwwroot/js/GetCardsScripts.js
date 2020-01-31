$(document).ready(function () {

    $('.new-btn').attr('href', 'Cards/New');

    var deckId = window.location.pathname.match('[0-9]+')[0];

    $.ajax({
        headers: {
            Accept: "application/json",
            'Cache-Control': 'no-cache'
        },
        url: 'https://localhost:44789/api/decks/' + deckId,
        method: 'GET'
    }).done(function (data) {
        var deck = ParseJsonToDeck(data);
        $('h1').text(deck.Name);
    });


    $.ajax({
        headers: {
            Accept: "application/json",
            'Cache-Control': 'no-cache'
        },
        url: 'https://localhost:44789/api/decks/' + deckId + '/cards',
        method: 'GET'
    }).done(function (data) {
        $.each(data, function (i, obj) {
            var card = ParseJsonToCard(obj);

            var domCard = $(".card-deck > div:first-child").clone();

            var domCardTitle = domCard.find('.card-title');
            domCardTitle.text(card.Title);

            var domCardDescription = domCard.find('h5');
            domCardDescription.text(card.Description);
            domCardDescription.attr('id', 'description' + card.Id);
            domCardDescription.hide();

            var editBtn = domCard.find('.edit-btn');
            editBtn.attr('href', 'Cards/' + card.Id + '/Edit');

            var showButton = domCard.find('.show-btn');
            showButton.on('change', function () {
                domCardDescription.slideDown('fast');
            });

            var hideButton = domCard.find('.hide-btn');
            hideButton.on('change', function () {
                domCardDescription.slideUp('slow');
            });

            domCard.appendTo(".card-deck");
        });
        $(".card-deck > div:first-child").remove();
        $(".card-deck > div:first-child").appendTo(".card-deck");
    });
});