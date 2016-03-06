/// <reference path="mp.ts" />
/// <reference path="select.ts" />

module mp.modal {
    var prev: Array<JQuery> = [];

    export function ShowMessage(msg: string, title: string = '提示', OnOK: () => void = null): void {
        var modal = $('#message-modal');
        modal.find('.modal-title').text(title);
        modal.find('.msg').text(msg);

        var ok = modal.find('.ok');
        ok.off();
        ok.click(() => {
            if (OnOK == null) {
                Rollback();
            }
            else {
                OnOK();
            }
        });

        ShowModal(modal);
    }

    export function ShowLogin() {
        ShowModal($('#login-modal'));
    }

    export function ShowSignup() {
        ShowModal($('#signup-modal'));
    }

    export function ShowProgress() {
        ShowModal($('#progress-modal'));
    }

    export function ShowImage(url: string, title: string, onSuccess: () => void = null, onLoaded: () => void = null, onCancel: () => void = null) {
        var modal = $('#image-modal');

        modal.find('.modal-title').text(title);

        var loading = modal.find('.loading');
        var content = modal.find('.content');

        loading.show();
        content.hide();

        content.load(url,() => {
            loading.slideUp();
            content.slideDown();

            var selectValue = content.find('.select input[type="hidden"]').val();
            content.find('.select .dropdown-menu li a[data-value="' + selectValue + '"]').click();

            var form = content.find('form');

            if (onLoaded != null)
                onLoaded();

            form.submit(() => {
                var action = form.attr('action');
                var data = form.serialize();

                $.post(action, data,(result: AjaxResult) => {
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
                        setTimeout(() => { warning.slideUp(); }, 2000);
                    }
                }, 'json');

                return false;
            });

            var createPackageBtn = content.find('.package-create');
            createPackageBtn.click(() => {
                ShowPackage('/package/create', '创建',(result) => {
                    var data: { id: number; title: string } = result.Data;
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
            cancel.click(() => {
                if (onCancel != null)
                    onCancel();
                else {
                    Rollback();
                }
            });
        });

        ShowModal(modal);
    }

    export function ShowPackage(url: string, title: string, onSuccess: (result: AjaxResult) => void = null, onCancel: () => void = null) {
        var modal = $('#package-modal');

        modal.find('.modal-title').text(title);

        var loading = modal.find('.loading');
        var content = modal.find('.content');

        loading.show();
        content.hide();

        content.load(url,() => {
            loading.slideUp();
            content.slideDown();

            var form = content.find('form');
            form.submit(() => {
                var action = form.attr('action');
                var data = form.serialize();

                $.post(action, data,(result: AjaxResult) => {
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
                        setTimeout(() => { warning.slideUp(); }, 2000);
                    }
                }, 'json');

                return false;
            });

            var cancel = modal.find('.cancel');
            cancel.off();
            cancel.click(() => {
                if (onCancel != null)
                    onCancel();
                else {
                    Rollback();
                }
            });
        });

        ShowModal(modal);
    }

    export function ShowConfirm(msg: string, title: string, OnOK: () => void = null, OnCancel: () => void = null): void {
        var modal = $('#confirm-modal');
        modal.find('.modal-title').text(title);
        modal.find('.msg').text(msg);

        var ok = modal.find('.ok');
        ok.off();
        ok.click(() => {
            if (OnOK == null) {
                Rollback();
            }
            else {
                OnOK();
            }
        });

        var cancel = modal.find('.cancel');
        cancel.off();
        cancel.click(() => {
            if (OnCancel == null) {
                Rollback();
            }
            else {
                OnCancel();
            }
        });

        ShowModal(modal);

    }

    function Rollback() {
        if (prev.length == 0)
            Close();
        else
            ShowModal(prev.pop(), false);
    }

    function ShowModal(target: JQuery, isPush: boolean = true): void {
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

    export function Close() {
        $('#modal').modal('hide');
    }

    $('#modal').on('hidden.bs.modal', function () {
        $('#modal .modal-dialog').removeAttr('style');

        prev = [];
    });


    //定义对话框按钮行为
    $(function () {
        //登录对话框--表单提交
        $(document).on('submit', '#login-modal form', function (e) {
            var form = $(e.target);
            var data = form.serialize();

            $.post('/account/login', data,(result: AjaxResult) => {
                if (result.Success) {
                    location.reload();
                }
                else {
                    var warning = $('#login-modal .bg-warning');
                    warning.text(result.Message).slideDown();
                    setTimeout(() => { warning.slideUp(); }, 2000);
                }
            }, 'json');

            return false;
        });
        
        //注册对话框--表单提交
        $(document).on('submit', '#signup-modal form',(e) => {
            var form = $(e.target);
            var data = form.serialize();

            $.post('/account/signup', data,(result: AjaxResult) => {
                if (result.Success) {
                    ShowMessage('注册成功', '提示',() => { ShowLogin(); });
                }
                else {
                    var warning = $('#signup-modal .bg-warning');
                    warning.text(result.Message).slideDown();
                    setTimeout(() => { warning.slideUp(); }, 2000);
                }
            }, 'json');

            return false;
        });

        //图片对话框相关
        $(document).on('click', '#image-modal .select .dropdown-menu li a',(e) => {
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
    })
}