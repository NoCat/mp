var mp;
(function (mp) {
    var modal;
    (function (modal_1) {
        var prev = [];
        function ShowMessage(msg, title, OnOK) {
            if (title === void 0) { title = '提示'; }
            if (OnOK === void 0) { OnOK = null; }
            var modal = $('#message-modal');
            modal.find('.modal-title').text(title);
            modal.find('.msg').text(msg);
            var ok = modal.find('.ok');
            ok.off();
            ok.click(function () {
                if (OnOK == null) {
                    Rollback();
                }
                else {
                    OnOK();
                }
            });
            ShowModal(modal);
        }
        modal_1.ShowMessage = ShowMessage;
        function ShowLogin() {
            ShowModal($('#login-modal'));
        }
        modal_1.ShowLogin = ShowLogin;
        function ShowSignup() {
            ShowModal($('#signup-modal'));
        }
        modal_1.ShowSignup = ShowSignup;
        function ShowProgress() {
            ShowModal($('#progress-modal'));
        }
        modal_1.ShowProgress = ShowProgress;
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
                var selectValue = content.find('.select input[type="hidden"]').val();
                content.find('.select .dropdown-menu li a[data-value="' + selectValue + '"]').click();
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
                            setTimeout(function () { warning.slideUp(); }, 2000);
                        }
                    }, 'json');
                    return false;
                });
                var createPackageBtn = content.find('.package-create');
                createPackageBtn.click(function () {
                    ShowPackage('/package/create', '创建', function (result) {
                        var data = result.Data;
                        var options = content.find('.select .dropdown-menu');
                        var option = $('<li><a></a></li>');
                        var a = option.find('a');
                        a.attr('data-value', data.id);
                        option.find('a').text(data.title);
                        options.add(option);
                        option.click();
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
        modal_1.ShowImage = ShowImage;
        function ShowPackage(url, title, onSuccess, onCancel) {
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
                            setTimeout(function () { warning.slideUp(); }, 2000);
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
        modal_1.ShowPackage = ShowPackage;
        function ShowConfirm(msg, title, OnOK, OnCancel) {
            if (OnOK === void 0) { OnOK = null; }
            if (OnCancel === void 0) { OnCancel = null; }
            var modal = $('#confirm-modal');
            modal.find('.modal-title').text(title);
            modal.find('.msg').text(msg);
            var ok = modal.find('.ok');
            ok.off();
            ok.click(function () {
                if (OnOK == null) {
                    Rollback();
                }
                else {
                    OnOK();
                }
            });
            var cancel = modal.find('.cancel');
            cancel.off();
            cancel.click(function () {
                if (OnCancel == null) {
                    Rollback();
                }
                else {
                    OnCancel();
                }
            });
            ShowModal(modal);
        }
        modal_1.ShowConfirm = ShowConfirm;
        function ShowSendEmail() {
            ShowModal($('#send-email-modal'));
        }
        modal_1.ShowSendEmail = ShowSendEmail;
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
        modal_1.Close = Close;
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
                        setTimeout(function () { warning.slideUp(); }, 2000);
                    }
                }, 'json');
                return false;
            });
            $(document).on('submit', '#signup-modal form', function (e) {
                var form = $(e.target);
                var data = form.serialize();
                $.post('/account/signup', data, function (result) {
                    if (result.Success) {
                        ShowMessage('注册成功', '提示', function () { ShowLogin(); });
                    }
                    else {
                        var warning = $('#signup-modal .bg-warning');
                        warning.text(result.Message).slideDown();
                        setTimeout(function () { warning.slideUp(); }, 2000);
                    }
                }, 'json');
                return false;
            });
            $(document).on('click', '#image-modal .select .dropdown-menu li a', function (e) {
                var modal = $('#image-modal');
                var option = $(e.target);
                var select = option.parents('.select');
                var options = select.find('.dropdown-menu');
                var value = option.data('value');
                var input = select.find('input[type="hidden"]');
                var current = select.find('.current');
                var inPackage = option.data('inpackage');
                var warning = modal.find('.bg-warning');
                if (inPackage == true) {
                    warning.text('图片已存在图包中');
                    warning.slideDown();
                }
                else {
                    warning.slideUp();
                }
                options.find('.active').removeClass('active');
                option.parent().addClass('active');
                input.val(value);
                current.text(option.text());
            });
            $(document).on('submit', '#send-email-modal form', function (e) {
                var form = $(e.target);
                var data = form.serialize();
                var ok = form.find('.ok');
                ok.attr('disabled', 'disabled');
                $.post('/account/sendresetmail', data, function (result) {
                    if (result.Success == true) {
                        ShowMessage(result.Message, '邮件发送成功', function () { Close(); });
                    }
                    else {
                        var warning = form.find('.bg-warning');
                        warning.text(result.Message).slideDown();
                        setTimeout(function () { warning.slideUp(); }, 2000);
                    }
                    ok.removeAttr('disabled');
                });
                return false;
            });
            $(document).on('submit', '#reset-password-modal form', function (e) {
                var form = $(e.target);
                var data = form.serialize();
                var ok = form.find('.ok');
                ok.attr('disabled', 'disabled');
                $.post('', data, function (result) {
                    if (result.Success == true) {
                        ShowMessage('密码重置成功', '提示', function () { location.href = '/'; });
                    }
                    else {
                        var warning = form.find('.bg-warning');
                        warning.text(result.Message).slideDown();
                        setTimeout(function () { warning.slideUp(); }, 2000);
                    }
                    ok.removeAttr('disabled');
                });
                return false;
            });
        });
    })(modal = mp.modal || (mp.modal = {}));
})(mp || (mp = {}));
