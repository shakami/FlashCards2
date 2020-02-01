const API_URL = 'https://localhost:44789/api/decks';

$.ajaxSetup({
    url: API_URL,
    headers: {
        Accept: "application/json",
        'Cache-Control': 'no-cache'
    }
});

class ApiCall {
    static GetAllDecks() {
        return $.ajax({
            method: 'GET'
        }).fail(ErrorHandler.ShowGenericError);
    }

    static CreateDeck(deck) {
        return $.ajax({
            headers: {
                "Content-Type": "application/json"
            },
            method: 'POST',
            data: deck,
            statusCode: {
                422: jqXHR => ErrorHandler.ShowValidationError(jqXHR)
            }
        }).fail(ErrorHandler.ShowGenericError);
    }

    static GetDeck(deckId) {
        return $.ajax({
            url: API_URL + '/' + deckId,
            method: 'GET'
        }).fail(ErrorHandler.ShowGenericError);
    }

    static UpdateDeck(deckId, data) {
        return $.ajax({
            headers: {
                "Content-Type": "application/json"
            },
            url: API_URL + '/' + deckId,
            method: 'PUT',
            data: data,
            statusCode: {
                422: jqXHR => ErrorHandler.ShowValidationError(jqXHR)
            }
        }).fail(ErrorHandler.ShowGenericError);
    }

    static GetCardsInDeck(deckId) {
        return $.ajax({
            url: API_URL + '/' + deckId + '/cards',
            method: 'GET'
        }).fail(ErrorHandler.ShowGenericError);
    }

    static GetCardInDeck(deckId, cardId) {
        return $.ajax({
            url: API_URL + '/' + deckId + '/cards/' + cardId,
            method: 'GET'
        }).fail(ErrorHandler.ShowGenericError);
    }

    static UpdateCard(deckId, cardId, data) {
        return $.ajax({
            headers: {
                "Content-Type": "application/json"
            },
            url: API_URL + '/' + deckId + '/cards/' + cardId,
            method: 'PUT',
            data: data,
            statusCode: {
                422: jqXHR => ErrorHandler.ShowValidationError(jqXHR)
            }
        }).fail(ErrorHandler.ShowGenericError);
    }

    static CreateCard(deckId, data) {
        return $.ajax({
            headers: {
                "Content-Type": "application/json"
            },
            url: API_URL + '/' + deckId + '/cards',
            method: 'POST',
            data: data,
            statusCode: {
                422: jqXHR => ErrorHandler.ShowValidationError(jqXHR)
            }
        }).fail(ErrorHandler.ShowGenericError);
    }

    static Delete(url) {
        return $.ajax({
            url: url,
            method: 'DELETE',
        }).fail(ErrorHandler.ShowGenericError);
    }
}
