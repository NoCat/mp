/// <reference path="modal.ts" />

module mp.start
{
    $(() =>
    {
        $(document).on('.click', '.login',() =>
        {
            modal.ShowLogin();
        });

        $(document).on('.click', '.signup',() =>
        {
            modal.ShowSignup();
        });
    });
}