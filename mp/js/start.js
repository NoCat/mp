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
                var btn = $(e.target);
                var id = btn.data('id');
                var url = '/image/' + id + '/resave';
                mp.modal.ShowImage(url, '转存');
                return false;
            });
            $(document).on('click', '.image-edit-btn', function (e) {
                var btn = $(e.target);
                var id = btn.data('id');
                var url = '/image/' + id + '/edit';
                mp.modal.ShowImage(url, '编辑');
                return false;
            });
        });
    })(start = mp.start || (mp.start = {}));
})(mp || (mp = {}));
