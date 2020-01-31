$(document).ready(function () {
    $('#copyright-year').text(new Date().getFullYear());
});

function GetDeckIdFromUrl() {
    return window.location.pathname.match('(?<=Decks\/)[0-9]+')[0];

}

function GetCardIdFromUrl() {
    return window.location.pathname.match('(?<=Cards\/)[0-9]+')[0];
}

class ErrorHandler {
    static ShowValidationError(jqXHR) {
        ClearErrors();
        var errors = jqXHR.responseJSON.errors;
        $.each(errors, function (key, value) {
            ShowError(value);
        });
    }

    static ShowGenericError() {
        ClearErrors();
        ShowError('An unexpected error happened while ' +
            'communicating with the server. Please try again later');
    }
}

function ClearErrors() {
    $('#validation-error').html('');
}

function ShowError(err) {
    $('#validation-error')
        .append($('<span>').text(err).css('display', 'block'))
        .removeAttr('hidden');
}