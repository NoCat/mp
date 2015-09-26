/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../scripts/typings/bootstrap/bootstrap.d.ts" />

module mp
{

}

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
        $('#login-btn').click(() =>
        {
            ShowModal('#login-modal');
        });

        $('#signup-btn').click(() =>
        {
            ShowModal('#signup-modal');
        });

        $('#login-submit').click(function ()
        {
            var email: string = $('#login-email').val();
            var password: string = $('#login-password').val();
            var remember: boolean = $('#login-remember').prop('checked');

            $.post('/account/login', { email: email, password: password,remember:remember },(result: AjaxResult) =>
            {
                if (result.Success)
                {
                    location.reload();
                }
                else
                {
                    $('#login-modal .bg-warning').text(result.Message).show();
                }
            }, 'json');
        });
    })
}