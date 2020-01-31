class Card {
    constructor(id, deckId, title, description) {
        this.Id = id;
        this.DeckId = deckId;
        this.Title = title;
        this.Description = description;
    }
}

class Deck {
    constructor(id, name) {
        this.Id = id;
        this.Name = name;
    }
}

function ParseJsonToCard(data) {
    let id = 0, deckId = 0, title = "", description = "";

    $.each(data, function (key, val) {
        switch (key) {
            case 'id':
                id = val;
                break;
            case 'deckId':
                deckId = val;
                break;
            case 'title':
                title = val;
                break;
            case 'description':
                description = val;
                break;
            default:
                break;
        }
    });
    var card = new Card(id, deckId, title, description);
    return card;
}

function ParseJsonToDeck(data) {
    let id = 0;
    let name = "";

    $.each(data, function (key, val) {
        if (key === "id") {
            id = val;
        }
        else if (key === "name") {
            name = val;
        }
    });

    var deck = new Deck(id, name);
    return deck;
}