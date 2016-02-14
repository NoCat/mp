﻿/// <reference path="mp.ts" />
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
                modal.ShowMessage('转存成功', '提示',() => { modal.Close(); });
            });

            return false;
        });

        $(document).on('click', '.image-edit-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/edit';

            var onSuccess = () => {
                location.reload();
            };

            modal.ShowImage(url, '编辑', onSuccess);

            return false;
        });

        $(document).on('click', '.image-delete-btn',(e) =>
        {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/delete';

            var onOK = () =>
            {
                $.post(url,(result: AjaxResult) =>
                {
                    if (result.Success)
                    {
                        location.href = result.Data.url;
                    }
                    else
                    {
                        modal.ShowMessage(result.Message, '提示',() => { modal.Close(); });
                    }
                }, 'json');
            };

            modal.ShowConfirm('图片删除后无法恢复,确定要删除吗?', '确定', onOK);

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

                    btn.removeClass('praise-btn').removeClass('btn-default');
                    btn.addClass('cancel-praise-btn').addClass('btn-danger');
                }
                else {
                    modal.ShowMessage(result.Message);
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

                    btn.removeClass('cancel-praise-btn').removeClass('btn-danger');
                    btn.addClass('praise-btn').addClass('btn-default');
                }
                else {
                    modal.ShowMessage(result.Message);
                }
            }, 'json');

            return false;
        });

        //点击上传按钮
        $(document).on('click', '.navbar .tool-upload',(e) => {
            var p = $(e.currentTarget).parents('.nav .dropdown.open');
            p.removeClass("open");
        })

        //上传文件
        $(document).on('change', '.upload-btn',(e) => {           
            var files = $(e.currentTarget).prop('files');
            if (files.length == 0) {
                return false;
            }
            var currentPackageId = $(e.currentTarget).data('id');
            modal.ShowProgress();
            var dialog = $("#progress-modal");
            dialog.find(".total-index").text(files.length);
            var progress = dialog.find(".progress-bar");
            var current = dialog.find(".current-index");

            var up = new uploader.BatchUploader();
            up.url = "/upload";
            for (var i = 0; i < files.length; i++) {
                up.add(files[i]);
            }

            up.onProgress = function (p, c) {
                current.text(c);
                p = Math.floor(p * 100);
                progress.css({ "width": p + "%" });
                progress.text(p + "%");
            }

            up.onDone = function (datas) {
                //alert(JSON.stringify(datas));

                var uploadDatas = [];
                for (var i = 0; i < datas.length; i++) {
                    uploadDatas.push({ id: datas[i].Data.id, description: datas[i].File.name });
                }
                progress.css({ 'width': '0%' });
                progress.text('');

                modal.Close();

                var onSuccess = () => {
                    var packageid = $('#image-modal form').find('input[name="packageid"]').val();
                    var url = '/package?id=' + packageid;
                    modal.ShowMessage("创建成功", "提示",() => { modal.Close(); location.replace(url); });
                };

                var onLoaded = () => {                    
                    var form = $('#image-modal form');
                    form.find('.select .dropdown-menu li a[data-value="' + currentPackageId + '"]').click();
                    for (var i = 0; i < datas.length; i++) {
                        var fileid = $('<input type="hidden" name="fileid" value="' + datas[i].Data.id + '"/>');
                        var filename = $('<input type="hidden" name="filename" value="' + datas[i].File.name + '"/>');
                        form.append(fileid).append(filename);
                    }
                }

                modal.ShowImage("/image/Add?id=" + datas[0].Data.id, "添加图片", onSuccess, onLoaded);
            }
            up.start();
            var close = dialog.find('.close');
            close.click(() => {
                up.stop();
                progress.css({ 'width': '0%' });
                progress.text('');
                modal.Close();
                close.unbind();
            })
        })

        //创建图包
        $(document).on('click', '.navbar .tool-package',(e) => {
            var onSuccess = (d) => {
                var url = '/package?id=' + d.Data.id;
                location.replace(url);
            }
            modal.ShowPackage('/package/create', '创建图包', onSuccess, null);
        })

        //编辑图包
        $(document).on('click', '.package-edit-btn',(e) => {
            var t = $(e.currentTarget);
            var packageid = t.data('id');
            modal.ShowPackage('/package/edit?id=' + packageid, '编辑图包',(result: AjaxResult) => {
                location.reload();
            },null);
        })
    });
}
