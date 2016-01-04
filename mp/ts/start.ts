/// <reference path="modal.ts" />

module mp.start
{
    $(() =>
    {
        $(document).on('click', '.login-btn',() =>
        {
            modal.ShowLogin();
            return false;
        });

        $(document).on('click', '.signup-btn',() =>
        {
            modal.ShowSignup();
            return false;
        });

        $(document).on('click', '.resave-btn',(e) =>
        {
            var btn = $(e.target);
            var id = btn.data('id');

            var url = '/image/' + id + '/resave';
            modal.ShowImage(url, '转存');

            return false;
        });

        $(document).on('click', '.image-edit-btn',(e) =>
        {
            var btn = $(e.target);
            var id = btn.data('id');

            var url = '/image/' + id + '/edit';
            modal.ShowImage(url, '编辑');

            return false;
        });
    });
}