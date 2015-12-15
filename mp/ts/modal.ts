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

    function MessageBox(msg: string, onClosed?: () => void): void
    {
        $('#message-modal').modal('show');
    }

    function ShowModal(selector: string, onClosed?: () => void): void
    {
        var modal = $('#modal');
        modal.find('.modal-dialog').hide();
        $(selector).show();
        modal.modal('show');
    }

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