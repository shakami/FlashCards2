$(document).ready(function () {
    var submitButton = $('input[type=submit]');
    submitButton.on('click', SubmitForm);
});

function SubmitForm() {
    event.preventDefault();
    var deckName = $('#deckName').val();

    $.ajax({
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
        },
        url: 'https://localhost:44789/api/decks',
        method: 'POST',
        data: "{'name': '" + deckName + "'}",
        statusCode: {
            422: jqXHR => ShowValidationError(jqXHR)
        }
    })
        .fail(ShowGenericError)
        .done(RedirectToHomePage);
}

function RedirectToHomePage() {
    window.location.replace(window.location.origin);
}
