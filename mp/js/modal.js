var mp;
(function (mp) {
    var modal;
    (function (_modal) {
        var prev = null;
        function MessageBox(msg, title, callback) {
            if (title === void 0) { title = '提示'; }
            if (callback === void 0) { callback = null; }
            var modal = $('#message-modal');
            modal.find('.modal-title').text(title);
            modal.find('.msg').text(msg);
            var ok = modal.find('.ok');
            ok.off();
            ok.click(function () {
                if (callback == null) {
                    Rollback();
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
        function ShowProgress() {
            ShowModal($('#progress-modal'));
        }
        _modal.ShowProgress = ShowProgress;
        function ShowImage(url, title, callback) {
            var modal = $('#image-modal');
            modal.find('.modal-title').text(title);
            var loading = modal.find('.loading');
            var content = modal.find('.content');
            loading.show();
            content.hide();
            content.load(url, function () {
                loading.slideUp();
                content.slideDown();
                var select = content.find('.select');
                select.bsSelect();
                var form = content.find('form');
                form.submit(function () {
                    var action = form.attr('action');
                    var data = form.serialize();
                    $.post(action, data, function (result) {
                        if (result.Success) {
                            if (callback != null)
                                callback();
                            else
                                Close();
                        }
                        else {
                            var warning = content.find('.bg-warning');
                            warning.text(result.Message);
                            warning.slideDown();
                            setTimeout(function () {
                                warning.slideUp();
                            }, 2000);
                        }
                    }, 'json');
                    return false;
                });
                var createPackageBtn = content.find('.package-create');
                createPackageBtn.click(function () {
                    PackageModal('/package/create', '创建', function (result) {
                        var data = result.Data;
                        select.bsSelect('add', { value: data.id, text: data.title });
                        select.bsSelect('select', data.id);
                        ShowModal(modal);
                    });
                });
            });
            ShowModal(modal);
        }
        _modal.ShowImage = ShowImage;
        function PackageModal(url, title, onSuccess, onCancel) {
            if (onSuccess === void 0) { onSuccess = null; }
            if (onCancel === void 0) { onCancel = null; }
            var modal = $('#package-modal');
            modal.find('.modal-title').text(title);
            var loading = modal.find('.loading');
            var content = modal.find('.content');
            loading.show();
            content.hide();
            content.load(url, function () {
                loading.slideUp();
                content.slideDown();
                var form = content.find('form');
                form.submit(function () {
                    var action = form.attr('action');
                    var data = form.serialize();
                    $.post(action, data, function (result) {
                        if (result.Success) {
                            if (onSuccess != null)
                                onSuccess(result);
                            else
                                Close();
                        }
                        else {
                            var warning = content.find('.bg-warning');
                            warning.text(result.Message);
                            warning.slideDown();
                            setTimeout(function () {
                                warning.slideUp();
                            }, 2000);
                        }
                    }, 'json');
                    return false;
                });
                var cancel = modal.find('.cancel').click(function () {
                    if (onCancel != null)
                        onCancel();
                    else {
                        Rollback();
                    }
                });
            });
            ShowModal(modal);
        }
        function Rollback() {
            if (prev == null)
                Close();
            else
                ShowModal(prev);
        }
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
        _modal.Close = Close;
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
