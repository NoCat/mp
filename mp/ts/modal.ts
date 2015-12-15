/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../scripts/typings/bootstrap/bootstrap.d.ts" />

module mp.modal
{
    interface AjaxResult
    {
        Success: boolean;
        Message: string;
        Data: any;
    }

    var prev: JQuery = null;
    var loginModal = $('#login-modal');
    var signupModal = $('#signup - modal');
    var resaveModal = $('#resave - modal');

    export function MessageBox(msg: string): void
    {
        $('#message-modal').modal('show');
    }

    export function ShowLogin()
    {
        ShowModal(signupModal);
    }

    export function ShowSignup()
    {
        ShowModal(signupModal);
    }

    export function ShowResave()
    {
        ShowModal(resaveModal);
    }

    function ShowModal(target:JQuery): void
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

    function Rollback()
    {
        if (prev != null)
            ShowModal(prev);
        else
            Close();
    }

    function Close()
    {
        $('#modal').modal('hide');
    }

    $('#modal').on('hidden.bs.modal', function ()
    {
        $('#modal .modal-dialog').removeAttr('style');

        prev = null;
    });

    $(function ()
    {
        $('#signup-btn').click(() =>
        {
            ShowModal('#signup-modal');
        });

        $('#login-submit').click(function ()
        {
            var email: string = $('#login-email').val();
            var password: string = $('#login-password').val();
            var remember: boolean = $('#login-remember').prop('checked');

            $.post('/account/login', { email: email, password: password, remember: remember },(result: AjaxResult) =>
            {
                if (result.Success)
                {
                    location.reload();
                }
                else
                {
                    $('#login-modal .bg-warning').text(result.Message).slideDown();
                }
            }, 'json');
        });

        $(document).on('click', '.login',() =>
        {
            ShowModal('#login-modal');
            return false;
        });

        $(document).on('click', '.resave',(e) =>
        {
            var t = $(e.target);
            var url = t.data('url');
            var modal = $('#resave-modal');

            var loading = modal.find('.loading');
            var form = modal.find('.form');
            
            //显示loading
            loading.show();
            form.hide();

            //远程加载url
            form.load(url,() =>
            {
                loading.hide();
                form.show();
            });
            ShowModal('#resave-modal');
            return false;
        });

        $('#resave-modal .ok').click(() =>
        {
            var form = $('#resave-modal form');
            form.submit();
        });
    })
}