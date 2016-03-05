var mp;
(function (mp) {
    var tools;
    (function (tools) {
        function fixImgS(container) {
            var width = container.width();
            var img = container.find("img");
            var ratio;
            var iwidth = img.prop('width');
            var iheight = img.prop('height');
            if (iwidth > iheight) {
                ratio = width / iwidth;
            }
            else if (iheight > iwidth) {
                ratio = width / iheight;
            }
            iheight = iheight * ratio;
            iwidth = iwidth * ratio;
            img.css({ 'height': iheight + 'px', 'width': iwidth + 'px' });
            img.css({ 'top': (width - iheight) / 2 + 'px', 'left': (width - iwidth) / 2 + 'px' });
            return ratio;
        }
        tools.fixImgS = fixImgS;
    })(tools = mp.tools || (mp.tools = {}));
})(mp || (mp = {}));
