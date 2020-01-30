$(document).ready(function () {
    $.ajax({
        accepts: {
            json: 'application/json'
        },
        url: 'https://localhost:44123/api/decks',
        method: 'GET'
    }).done(function (data) {
        $.each(data, function (i, obj) {
            var deck = ParseJsonToDeck(obj);

            var domCard = $(".card-deck > div:first-child").clone();

            var domCardTitle = domCard.find("a");
            domCardTitle.text(deck.Name);
            domCardTitle.attr("href", "Decks/" + deck.Id + "/Cards");

            domCard.appendTo(".card-deck");
        });
        $(".card-deck > div:first-child").remove();
        $(".card-deck > div:first-child").appendTo(".card-deck");
    });
});