function ShowValidationError(jqXHR) {
    ClearErrors();
    var errors = jqXHR.responseJSON.errors;
    $.each(errors, function (key, value) {
        ShowError(value);
    });
}

function ShowGenericError() {
    ClearErrors();
    ShowError('An unexpected error happened while ' +
        'communicating with the server. Please try again later');
}

function ShowError(err) {
    $('#validation-error')
        .append($('<span>').text(err).css('display', 'block'))
        .removeAttr('hidden');
}

function ClearErrors() {
    $('#validation-error').html('');
}