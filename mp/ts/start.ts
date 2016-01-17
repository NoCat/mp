/// <reference path="mp.ts" />
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
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/resave';
            modal.ShowImage(url, '转存',() =>
            {
                modal.MessageBox('转存成功', '提示',() => { modal.Close(); });
            });

            return false;
        });

        $(document).on('click', '.image-edit-btn',(e) =>
        {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/edit';
            modal.ShowImage(url, '编辑');

            return false;
        });

        $(document).on('click', '.praise-btn',(e) =>
        {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/praise';
            $.post(url,(result: AjaxResult) =>
            {
                if (result.Success)
                {
                    var count = result.Data.count;
                    var text = btn.find('.text');
                    if (count == 0)
                        text.text('');
                    else
                        text.text(count);

                    btn.removeClass('praise-btn');
                    btn.addClass('cancel-praise-btn');
                }
                else
                {
                    modal.MessageBox(result.Message);
                }
            }, 'json');

            return false;
        });

        $(document).on('click', '.cancel-praise-btn',(e) =>
        {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/cancelpraise';
            $.post(url,(result: AjaxResult) =>
            {
                if (result.Success)
                {
                    var count = result.Data.count;
                    var text = btn.find('.text');
                    if (count == 0)
                        text.text('');
                    else
                        text.text(count);

                    btn.removeClass('cancel-praise-btn');
                    btn.addClass('praise-btn');
                }
                else
                {
                    modal.MessageBox(result.Message);
                }
            }, 'json');

            return false;
        });
    });
}