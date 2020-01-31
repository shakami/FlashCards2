$(document).ready(function () {
    var submitButton = $('input[type=submit]');
    submitButton.on('click', SubmitForm);
});

function SubmitForm() {
    event.preventDefault();
    var deckName = $('#deckName').val();
    var deckToCreate = { name: deckName };

    ApiCall.CreateDeck(JSON.stringify(deckToCreate))
        .done(RedirectToHomePage);
}

function RedirectToHomePage() {
    window.location.replace(window.location.origin);
}
