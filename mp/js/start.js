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
                    mp.modal.ShowMessage('转存成功', '提示', function () {
                        mp.modal.Close();
                    });
                });
                return false;
            });
            $(document).on('click', '.image-edit-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/edit';
                var onSuccess = function () {
                    location.reload();
                };
                mp.modal.ShowImage(url, '编辑', onSuccess);
                return false;
            });
            $(document).on('click', '.image-delete-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/delete';
                var onOK = function () {
                    $.post(url, function (result) {
                        if (result.Success) {
                            location.href = result.Data.url;
                        }
                        else {
                            mp.modal.ShowMessage(result.Message, '提示', function () {
                                mp.modal.Close();
                            });
                        }
                    }, 'json');
                };
                mp.modal.ShowConfirm('图片删除后无法恢复,确定要删除吗?', '确定', onOK);
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
                        mp.modal.ShowMessage(result.Message);
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
                        mp.modal.ShowMessage(result.Message);
                    }
                }, 'json');
                return false;
            });
            $(document).on('click', '.navbar .tool-upload', function (e) {
                var p = $(e.currentTarget).parents('.nav .dropdown.open');
                p.removeClass("open");
            });
            $(document).on('change', '.navbar .tool-upload', function (e) {
                var files = $(e.currentTarget).prop('files');
                if (files.length == 0) {
                    return false;
                }
                mp.modal.ShowProgress();
                var dialog = $("#progress-modal");
                dialog.find(".total-index").text(files.length);
                var progress = dialog.find(".progress-bar");
                var current = dialog.find(".current-index");
                var up = new mp.uploader.BatchUploader();
                up.url = "/upload";
                for (var i = 0; i < files.length; i++) {
                    up.add(files[i]);
                }
                up.onProgress = function (p, c) {
                    current.text(c);
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
                    var onSuccess = function () {
                        progress.css({ width: '0' });
                        progress.text();
                        var packageid = $('#image-modal form').find('input[name="packageid"]').val();
                        var url = '/package?id=' + packageid;
                        mp.modal.ShowMessage("创建成功", "提示", function () {
                            mp.modal.Close();
                            location.replace(url);
                        });
                    };
                    var onLoaded = function () {
                        var form = $('#image-modal form');
                        for (var i = 0; i < datas.length; i++) {
                            var fileid = $('<input type="hidden" name="fileid" value="' + datas[i].Data.id + '"/>');
                            var filename = $('<input type="hidden" name="filename" value="' + datas[i].File.name + '"/>');
                            form.append(fileid).append(filename);
                        }
                    };
                    mp.modal.ShowImage("/image/Add?id=" + datas[0].Data.id, "添加图片", onSuccess, onLoaded);
                };
                up.start();
                var close = dialog.find('.close');
                close.click(function () {
                    up.stop();
                    progress.css({ width: '0' });
                    progress.text();
                    close.unbind();
                });
            });
            $(document).on('click', '.navbar .tool-package', function (e) {
                $(e.currentTarget).parents('.navbar .dropdown.open').removeClass('open');
                var onSuccess = function (d) {
                    var url = '/package?id=' + d.Data.id;
                    location.replace(url);
                };
                mp.modal.ShowPackage('/package/create', '创建图包', onSuccess, null);
            });
            $(document).on('click', '.package-edit-btn', function (e) {
                var t = $(e.currentTarget);
                var packageid = t.data('id');
                mp.modal.ShowPackage('/package/edit?id=' + packageid, '编辑图包', function (result) {
                    $('.package-title').text(result.Data.Title);
                    $('.package-description').text(result.Data.Description);
                    mp.modal.Close();
                }, function () {
                });
            });
        });
    })(start = mp.start || (mp.start = {}));
})(mp || (mp = {}));
