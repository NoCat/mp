var mp;
(function (mp) {
    var modal;
    (function (_modal) {
        var prev = [];
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
        function ShowImage(url, title, onSuccess, onLoaded, onCancel) {
            if (onSuccess === void 0) { onSuccess = null; }
            if (onLoaded === void 0) { onLoaded = null; }
            if (onCancel === void 0) { onCancel = null; }
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
                if (onLoaded != null)
                    onLoaded();
                form.submit(function () {
                    var action = form.attr('action');
                    var data = form.serialize();
                    $.post(action, data, function (result) {
                        if (result.Success) {
                            if (onSuccess != null)
                                onSuccess();
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
                var cancel = modal.find('.cancel');
                cancel.off();
                cancel.click(function () {
                    if (onCancel != null)
                        onCancel();
                    else {
                        Rollback();
                    }
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
                var cancel = modal.find('.cancel');
                cancel.off();
                cancel.click(function () {
                    if (onCancel != null)
                        onCancel();
                    else {
                        Rollback();
                    }
                });
            });
            ShowModal(modal);
        }
        _modal.PackageModal = PackageModal;
        function Rollback() {
            if (prev.length == 0)
                Close();
            else
                ShowModal(prev.pop(), false);
        }
        function ShowModal(target, isPush) {
            if (isPush === void 0) { isPush = true; }
            var modal = $('#modal');
            modal.modal('show');
            var visible = modal.find('.modal-dialog:visible');
            visible.animate({ opacity: '0', marginTop: '0px', marginBottom: '0px', height: '0px' }, function () {
                visible.removeAttr('style');
                modal.append(visible);
                if (isPush == true) {
                    prev.push(visible);
                }
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
            prev = [];
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
