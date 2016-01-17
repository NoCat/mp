var mp;
(function (mp) {
    var start;
    (function (start) {
        $(function () {
            $(document).on('click', '.login-btn', function () {
                mp.modal.ShowLogin();
                return false;
            });
            $(document).on('click', '.signup-btn', function () {
                mp.modal.ShowSignup();
                return false;
            });
            $(document).on('click', '.resave-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/resave';
                mp.modal.ShowImage(url, '转存', function () {
                    mp.modal.MessageBox('转存成功', '提示', function () {
                        mp.modal.Close();
                    });
                });
                return false;
            });
            $(document).on('click', '.image-edit-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/edit';
                mp.modal.ShowImage(url, '编辑');
                return false;
            });
            $(document).on('click', '.praise-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/praise';
                $.post(url, function (result) {
                    if (result.Success) {
                        var count = result.Data.count;
                        var text = btn.find('.text');
                        if (count == 0)
                            text.text('');
                        else
                            text.text(count);
                        btn.removeClass('praise-btn');
                        btn.addClass('cancel-praise-btn');
                    }
                    else {
                        mp.modal.MessageBox(result.Message);
                    }
                }, 'json');
                return false;
            });
            $(document).on('click', '.cancel-praise-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/cancelpraise';
                $.post(url, function (result) {
                    if (result.Success) {
                        var count = result.Data.count;
                        var text = btn.find('.text');
                        if (count == 0)
                            text.text('');
                        else
                            text.text(count);
                        btn.removeClass('cancel-praise-btn');
                        btn.addClass('praise-btn');
                    }
                    else {
                        mp.modal.MessageBox(result.Message);
                    }
                }, 'json');
                return false;
            });
        });
    })(start = mp.start || (mp.start = {}));
})(mp || (mp = {}));
