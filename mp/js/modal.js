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
            $(document).on('click', '.login', function () {
                ShowModal('#login-modal');
                return false;
            });
            $(document).on('click', '.resave', function () {
                ShowModal('#resave-modal');
                return false;
            });
        });
    })(modal = mp.modal || (mp.modal = {}));
})(mp || (mp = {}));
