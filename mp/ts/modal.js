/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../scripts/typings/bootstrap/bootstrap.d.ts" />
var mp;
(function (mp) {
    var modal;
    (function (_modal) {
        function MessageBox(msg, onClosed) {
            $('#message-modal').modal('show');
        }
        function ShowModal(selector, onClosed) {
            var modal = $('#modal');
            modal.find('.modal-dialog').hide();
            $(selector).show();
            modal.modal('show');
        }
        $(function () {
            $('#login-btn').click(function () {
                ShowModal('#login-modal');
            });
            $('#signup-btn').click(function () {
                ShowModal('#signup-modal');
            });
            $('#login-submit').click(function () {
                var email = $('#login-email').val();
                var password = $('#login-password').val();
                var remember = $('#login-remember').prop('checked');
                $.post('/account/login', { email: email, password: password, remember: remember }, function (result) {
                    if (result.Success) {
                        location.reload();
                    }
                    else {
                        $('#login-modal .bg-warning').text(result.Message).slideDown();
                    }
                }, 'json');
            });
        });
    })(modal = mp.modal || (mp.modal = {}));
})(mp || (mp = {}));
//# sourceMappingURL=modal.js.map