var mp;
(function (mp) {
    var modal;
    (function (_modal) {
        var prev = null;
        function MessageBox(msg, title, callback) {
            if (callback === void 0) { callback = null; }
            var modal = $('#message-modal');
            modal.find('.modal-title').text(title);
            modal.find('.msg').text(msg);
            var ok = modal.find('.ok');
            ok.off();
            ok.click(function () {
                if (callback == null) {
                    if (prev != null)
                        ShowModal(prev);
                    else
                        Close();
                }
                else {
                    callback();
                }
            });
            ShowModal(modal);
        }
        _modal.MessageBox = MessageBox;
        function ShowLogin() {
            ShowModal($('#login-modal'));
        }
        _modal.ShowLogin = ShowLogin;
        function ShowSignup() {
            ShowModal($('#signup-modal'));
        }
        _modal.ShowSignup = ShowSignup;
        function ShowImage(url, title) {
            var modal = $('#image-modal');
            modal.find('.modal-title').text(title);
            var loading = modal.find('.loading');
            var content = modal.find('.content');
            loading.show();
            content.hide();
            content.load(url, function () {
                loading.slideUp();
                content.slideDown();
            });
            ShowModal(modal);
        }
        _modal.ShowImage = ShowImage;
        function ShowModal(target) {
            var modal = $('#modal');
            modal.modal('show');
            var visible = modal.find('.modal-dialog:visible');
            visible.animate({ opacity: '0', marginTop: '0px', marginBottom: '0px', height: '0px' }, function () {
                visible.removeAttr('style');
                modal.append(visible);
                prev = visible;
            });
            target.css({ display: 'block', opacity: '0', }).animate({ opacity: '1' }, function () {
                modal.prepend($(this));
            });
        }
        function Close() {
            $('#modal').modal('hide');
        }
        $('#modal').on('hidden.bs.modal', function () {
            $('#modal .modal-dialog').removeAttr('style');
            prev = null;
        });
        $(function () {
            $(document).on('submit', '#login-modal form', function (e) {
                var form = $(e.target);
                var data = form.serialize();
                $.post('/account/login', data, function (result) {
                    if (result.Success) {
                        location.reload();
                    }
                    else {
                        var warning = $('#login-modal .bg-warning');
                        warning.text(result.Message).slideDown();
                        setTimeout(function () {
                            warning.slideUp();
                        }, 2000);
                    }
                }, 'json');
                return false;
            });
            $(document).on('submit', '#signup-modal form', function (e) {
                var form = $(e.target);
                var data = form.serialize();
                $.post('/account/signup', data, function (result) {
                    if (result.Success) {
                        MessageBox('注册成功', '提示', function () {
                            ShowLogin();
                        });
                    }
                    else {
                        var warning = $('#signup-modal .bg-warning');
                        warning.text(result.Message).slideDown();
                        setTimeout(function () {
                            warning.slideUp();
                        }, 2000);
                    }
                }, 'json');
                return false;
            });
        });
    })(modal = mp.modal || (mp.modal = {}));
})(mp || (mp = {}));
