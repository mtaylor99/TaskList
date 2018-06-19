function setContentHeight() {
    var windowHeight = $(window).height();

    $('.TL-Main').height(windowHeight - 75);
}

$(window).resize(function (e) {
    setContentHeight();
});

$(document).ready(function (e) {
    setContentHeight();
});

function OpenDialogWindow(title, content, dialogWidth, dialogHeight) {
    $('#exampleModalLabel').text(title);
    $('.modal-dialog').width(dialogWidth);
    $('.modal-body').height(dialogHeight);

    $('.modal-body').load(content, function () {
        $('#myModal').modal({ show: true });
    });
}

function GetSelectedCheckBoxesIds(checkBoxId) {
    var checkedIds = $('input[id="' + checkBoxId + '"]:checked');

    var selectedIds = "";
    for (i = 0; i < checkedIds.length; i++) {
        selectedIds += checkedIds[i].value + ";";
    }

    return selectedIds;
}
