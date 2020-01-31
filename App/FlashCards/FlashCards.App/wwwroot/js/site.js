$(document).ready(function () {
    $('#copyright-year').text(new Date().getFullYear());
});

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