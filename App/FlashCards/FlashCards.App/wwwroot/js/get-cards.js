$(document).ready(function () {

    $('.new-btn').attr('href', 'Cards/New');
    $('#edit-deck-btn').attr('href', 'Edit');

    var deckId = GetDeckIdFromUrl();

    ApiCall.GetDeck(deckId)
        .done(function (deck) {
            $('h1').text(deck.name);
        });

    ApiCall.GetCardsInDeck(deckId)
        .done(data => PopulateCardsInDOM(data));
});

function PopulateCardsInDOM(data) {
    $.each(data, function (i, card) {
        var domCard = $(".card-deck > div:first-child").clone();

        var domCardTitle = domCard.find('.card-title');
        domCardTitle.text(card.title);

        var domCardDescription = domCard.find('h5');
        domCardDescription.text(card.description);
        domCardDescription.attr('id', 'description' + card.id);
        domCardDescription.hide();

        var editBtn = domCard.find('.edit-btn');
        editBtn.attr('href', 'Cards/' + card.id + '/Edit');

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
}