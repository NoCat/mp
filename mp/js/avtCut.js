/// <reference path="uploader.js" />
/// <reference path="jquery.js" />


$(document).ready(function () {
    $(document).on('change', '.avt-upload-btn', function (e) {
        if ($('.avt-cut-model') != null) {
            $('.avt-cut-model').remove();
        }
        var file = $(e.currentTarget).prop('files');
        if (file.length == 0) {
            return false;
        }
        var up = new mp.uploader.Uploader(file[0]);
        up.url = '/Setting/AvtUpload';
        var ratio;

        up.onDone = function (data) {
            //头像上传完成
            var filename=data.Data;
            var p = $('.avt-content').parent().parent();
            var acm = $('<div class="my-panel panel avt-cut-model"></div>');
            p.append(acm);
            var content = $('.avt-cut-model');
            content.load('/setting/avtcutmodel', { src: filename,r:Math.random() }, function () {
                var avtDialog = content.find('.avt-cut');
                var btn_submit = content.find(".submit");
                var btn_close = content.find('.avt-close');
                var pct = $('.preview-md');
                pct.css('height', pct.width() + 'px');

                var boundx, boundy, cwidth, x, y;
                document.getElementById('avt-cut-img').onload = function () {
                    ratio = mp.tools.fixImgS(avtDialog.find('.origin'), avtDialog.find('.avt-img'));
                    var img = $('.origin').find('img');
                    img.Jcrop({
                        allowResize: true,
                        aspectRatio: 1,
                        onChange: updatePreview,
                        onSelect: updatePreview
                    }, function () {
                        var bounds = this.getBounds();
                        boundx = bounds[0];
                        boundy = bounds[1];
                    });
                }

                //初始化Jcrop

                function updatePreview(c) {
                    preview($('.preview-md'), c);
                }
                function preview(container, c) {
                    if (parseInt(c.w) > 0) {
                        var width = container.width();
                        cwidth = c.w;
                        x = c.x;
                        y = c.y;
                        var r = width / c.w;
                        var img = container.find('img');
                        img.css({
                            width: Math.round(r * boundx) + 'px',
                            height: Math.round(r * boundy) + 'px',
                            marginLeft: '-' + Math.round(r * c.x) + 'px',
                            marginTop: '-' + Math.round(r * c.y) + 'px'
                        });
                    }
                }
                btn_submit.click(function () {
                    var url = '/setting/avtsetting';
                    $.post(url, { left: x, top: y, ratio: ratio, size: cwidth}, function (data) {
                        if (data.Success) {
                            location.reload();
                        }
                    }, 'json')
                })
                btn_close.click(function () {
                    content.fadeOut();
                    content.remove();
                })
            });
        }

        up.start();
    })
})
