$((function () {
    var url;
    var redirectUrl;
    var target;

    $('body').append(`
        <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close-delete"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="myModalLabel">Delete Pet</h4>
                    </div>
                    <div class="modal-body delete-modal-body">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" id="confirm-delete">Yes</button>
                        <button type="button" class="btn btn-default" id="btnClosePopup">No</button>
                    </div>
                </div>
            </div>
        </div>`);

    //Delete Action
    $(".btn-delete").on('click', (e) => {
        e.preventDefault();

        target = e.currentTarget;
        var Id = $(target).data('id');
        var controller = $(target).data('controller');
        var action = $(target).data('action');
        var bodyMessage = $(target).data('body-message');
        redirectUrl = $(target).data('redirect-url');
        console.log(redirectUrl);

        url = "/" + controller + "/" + action + "?Id=" + Id;
        $(".delete-modal-body").text(bodyMessage);

        $("#deleteModal").modal('show');
    });

    $("#confirm-delete").on('click', () => {
        $.get(url)
            .done((result) => {
                if (result.redirectUrl) {
                    window.location.href = result.redirectUrl;
                } else {
                    if (!redirectUrl) {
                        $(target).closest("tr").remove();
                    } else {
                        window.location.href = redirectUrl;
                    }
                    $("#deleteModal").modal('hide'); // Move this statement here
                }
            })
            .fail((error) => {
                if (error.status === 403) {
                    window.location.href = "/Home/NoPermission";
                } else if (redirectUrl) {
                    window.location.href = redirectUrl;
                }
                $("#deleteModal").modal('hide'); // Also move this statement here
            });
    });

    $(function () {
        $("#close-delete").click(function () {
            $("#deleteModal").modal("hide");
        });
    });

    $(function () {
        $("#btnClosePopup").click(function () {
            $("#deleteModal").modal("hide");
        });
    });
})());
