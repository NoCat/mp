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
            $(document).on('change', '#upload', function (e) {
                var files = $(e.currentTarget).prop('files');
                if (files.length == 0) {
                    return false;
                }
                mp.modal.ShowProgress();
                $("#total").text(files.length);
                var progress = $(".progress-bar");
                var up = new mp.uploader.BatchUploader();
                up.url = "/upload";
                for (var i = 0; i < files.length; i++) {
                    up.add(files[i]);
                }
                up.onProgress = function (p, c) {
                    $("#current-index").text(c);
                    p = Math.floor(p * 100);
                    progress.css({ "width": p + "%" });
                    progress.text(p + "%");
                };
                up.onDone = function (datas) {
                    var uploadDatas = [];
                    for (var i = 0; i < datas.length; i++) {
                        uploadDatas.push({ id: datas[i].Data.id, description: datas[i].File.name });
                    }
                    mp.modal.Close();
                    mp.modal.ShowImage("image/Add?id=" + datas[0].Data.id, "添加图片", function () {
                        mp.modal.MessageBox("创建成功", "提示", function () {
                            mp.modal.Close();
                            location.reload();
                        });
                    }, function () {
                        var form = $('#image-modal form');
                        for (var i = 0; i < datas.length; i++) {
                            var fileid = $("<input type='hidden' name='fileid' value='" + datas[i].Data.id + "'/>");
                            var filename = $('<input type="hidden" name="filename" value="' + datas[i].File.name + '"/>');
                            form.append(fileid).append(filename);
                        }
                    });
                };
                up.start();
            });
        });
    })(start = mp.start || (mp.start = {}));
})(mp || (mp = {}));
