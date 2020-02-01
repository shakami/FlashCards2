$(document).ready(function () {
    ApiCall.GetAllDecks()
        .done(data => PopulateDecksInDOM(data));
});

function PopulateDecksInDOM(data) {
    $.each(data, function (i, deck) {
        var domCard = $(".card-deck > div:first-child").clone();

        var domCardTitle = domCard.find("a");
        domCardTitle.text(deck.name);
        domCardTitle.attr("href", "Decks/" + deck.id + "/Cards");

        domCard.appendTo(".card-deck");
    });
    $(".card-deck > div:first-child").remove();
    $(".card-deck > div:first-child").appendTo(".card-deck");
    $('.card-deck').removeAttr('hidden');
}