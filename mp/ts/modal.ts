 /// <reference path="../scripts/typings/jquery/jquery.d.ts" />

module mp.modal
{    
    function message(msg:string,back:string): void
    {

    }

    $(function ()
    {
        $('#login-submit').click(function ()
        {
            var email: string = $('#login-email').val();
            var password: string = $('#login-password').val();

            if (email.length == 0)
            { }
        });
    })
}