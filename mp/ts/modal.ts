/// <reference path="mp.ts" />
/// <reference path="select.ts" />

module mp.modal
{
    var prev: JQuery = null;

    export function MessageBox(msg: string, title: string, callback: () => void = null): void
    {
        var modal = $('#message-modal');
        modal.find('.modal-title').text(title);
        modal.find('.msg').text(msg);

        var ok = modal.find('.ok');
        ok.off();
        ok.click(() =>
        {
            if (callback == null)
            {
                if (prev != null)
                    ShowModal(prev);
                else
                    Close();
            }
            else
            {
                callback();
            }
        });

        ShowModal(modal);
    }

    export function ShowLogin()
    {
        ShowModal($('#login-modal'));
    }

    export function ShowSignup()
    {
        ShowModal($('#signup-modal'));
    }

    export function ShowImage(url: string, title: string,callback?:()=>void)
    {
        var modal = $('#image-modal');

        modal.find('.modal-title').text(title);

        var loading = modal.find('.loading');
        var content = modal.find('.content');

        loading.show();
        content.hide();

        content.load(url,() =>
        {
            loading.slideUp();
            content.slideDown();

            var select = content.find('.select');
            select.bsSelect();

            var form = content.find('form');
            form.off();
            form.submit(() =>
            {
                var action = form.attr('action');
                var data = form.serialize();

                $.post(action, data,(result:AjaxResult) =>
                {
                    if (result.Success)
                    {
                        if (callback != null)
                            callback();
                        else
                            Close();
                    }
                    else
                    {
                        var warning = content.find('.bg-warning');
                        warning.text(result.Message);

                        warning.slideDown();
                        setTimeout(() => { warning.slideUp(); }, 2000);
                    }
                }, 'json');

                return false;
            });
        });

        ShowModal(modal);
    }

    function ShowModal(target: JQuery): void
    {
        var modal = $('#modal');
        modal.modal('show');
        var visible = modal.find('.modal-dialog:visible');
        visible.animate({ opacity: '0', marginTop: '0px', marginBottom: '0px', height: '0px' }, function ()
        {
            visible.removeAttr('style');
            modal.append(visible);

            prev = visible;
        });

        target.css({ display: 'block', opacity: '0', }).animate({ opacity: '1' }, function ()
        {
            modal.prepend($(this));
        });
    }

    export function Close()
    {
        $('#modal').modal('hide');
    }

    $('#modal').on('hidden.bs.modal', function ()
    {
        $('#modal .modal-dialog').removeAttr('style');

        prev = null;
    });


    //定义对话框按钮行为
    $(function ()
    {
        //登录对话框--表单提交
        $(document).on('submit', '#login-modal form', function (e)
        {
            var form = $(e.target);
            var data = form.serialize();

            $.post('/account/login', data,(result: AjaxResult) =>
            {
                if (result.Success)
                {
                    location.reload();
                }
                else
                {
                    var warning = $('#login-modal .bg-warning');
                    warning.text(result.Message).slideDown();
                    setTimeout(() => { warning.slideUp(); }, 2000);
                }
            }, 'json');

            return false;
        });
        
        //这侧对话框--表单提交
        $(document).on('submit', '#signup-modal form',(e) =>
        {
            var form = $(e.target);
            var data = form.serialize();

            $.post('/account/signup', data,(result: AjaxResult) =>
            {
                if (result.Success)
                {
                    MessageBox('注册成功', '提示',() => { ShowLogin(); });
                }
                else
                {
                    var warning = $('#signup-modal .bg-warning');
                    warning.text(result.Message).slideDown();
                    setTimeout(() => { warning.slideUp(); }, 2000);
                }
            }, 'json');

            return false;
        });
    })
}