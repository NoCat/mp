/// <reference path="mp.ts" />
/// <reference path="modal.ts" />
/// <reference path="uploader.ts" />

module mp.start {
    $(() => {
        $(document).on('click', '.login-btn',() => {
            modal.ShowLogin();
            return false;
        });

        $(document).on('click', '.signup-btn',() => {
            modal.ShowSignup();
            return false;
        });

        $(document).on('click', '.resave-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/resave';
            modal.ShowImage(url, '转存',() => {
                modal.MessageBox('转存成功', '提示',() => { modal.Close(); });
            });

            return false;
        });

        $(document).on('click', '.image-edit-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/edit';
            modal.ShowImage(url, '编辑');

            return false;
        });

        $(document).on('click', '.praise-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/praise';
            $.post(url,(result: AjaxResult) => {
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
                    modal.MessageBox(result.Message);
                }
            }, 'json');

            return false;
        });

        $(document).on('click', '.cancel-praise-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/cancelpraise';
            $.post(url,(result: AjaxResult) => {
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
                    modal.MessageBox(result.Message);
                }
            }, 'json');

            return false;
        });

        $(document).on('change', '#upload',(e) => {
            var files = $(e.currentTarget).prop('files');
            if (files.length == 0) {
                return false;
            }
            modal.ShowProgress();
            $("#total").text(files.length);
            var progress = $(".progress-bar");
            var up = new uploader.BatchUploader();
            up.url = "/upload";
            for (var i = 0; i < files.length; i++) {
                up.add(files[i]);
            }

            up.onProgress = function (p, c) {
                $("#current-index").text(c);
                p = Math.floor(p * 100);
                progress.css({ "width": p + "%" });
            }

            up.onDone = function (datas) {
                //alert(JSON.stringify(datas));

                var uploadDatas = [];
                for (var i = 0; i < datas.length; i++) {
                    uploadDatas.push({ id: datas[i].Data.id, description: datas[i].File.name });
                }

                $("#image-modal").modal({ backdrop: 'static' });

                $("#image-modal.modal-body").load("\image\Add\?id="+datas[0].Data.id);
            }
            up.start();
        })
    });
}
