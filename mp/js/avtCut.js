/// <reference path="uploader.js" />
/// <reference path="jquery.js" />


$(document).ready(function () {
    $(document).on('change', '.avt-upload-btn', function (e) {
        var file = $(e.currentTarget).prop('files');
        if (file.length == 0) {
            return false;
        }
        var up = new mp.uploader.Uploader(file[0]);
        var content = $('.avt-content');
        up.url = '/Setting/AvtUpload';
        var ratio;
        up.onDone = function (data) {
            //头像上传完成
            content.load('/setting/avtcutmodel?src=' + data.Data, function () {
                var loading = content.find('.loading');
                loading.slideUp();
                var avtDialog = content.find('.avt-cut');
                avtDialog.slideDown();
                var btn = content.find(".avt-submit");

                ratio = mp.tools.fixImgS(avtDialog.find('.origin'), avtDialog.find('.avt-img'));
                var pct = $('.preview-md');
                pct.css('height', pct.width()+'px');
                var img = $('.origin').find('img');
                var boundx, boundy,cwidth,x,y;
                //初始化Jcrop
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
                btn.click(function () {
                    var url = '/setting/avtsetting';
                    $.post(url, {left:x,top:y,ratio:ratio,size:cwidth}, function (data) {
                        if (data.Success) {
                            location.reload();
                        }
                    },'json')
                })
            });
        }        

        up.start();
    })
})
