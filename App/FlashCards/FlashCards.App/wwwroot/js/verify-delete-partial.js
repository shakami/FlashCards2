var div = $('<div>')
    .attr({
        id: 'verify-delete',
        class: 'alert alert-danger',
        role: 'alert'
    })
    .append([
        $('<p>')
            .attr('class', 'font-weight-bold')
            .text('Are you sure you want to delete this?'),
        $('<button>')
            .attr({
                id: 'delete-yes',
                class: 'btn btn-danger',
            })
            .text('Yes')
            .on('click', PerformDelete),
        $('<button>')
            .attr({
                id: 'delete-no',
                class: 'btn btn-success',
                type: 'button'
            })
            .css('margin', '5px')
            .text('No')
            .on('click', ToggleDiv)
    ]).hide();


$(document).ready(function () {
    $('#toolbar').append(div);
    $('#delete-btn').on('click', ToggleDiv);
})

function ToggleDiv() {
    div.slideToggle('slow');
}

function PerformDelete() {
    var currentUrl = window.location.pathname;
    var i = currentUrl.lastIndexOf('/');
    var path = currentUrl.substring(0, i);
    var deleteUrl = 'https://localhost:44789/api' + path;

    i = path.lastIndexOf('/');
    path = path.substring(0, i);
    var redirectUrl = window.location.origin + path;

    ApiCall.Delete(deleteUrl)
        .done(() => window.location.replace(redirectUrl));
}