var mp;
(function (mp) {
    var tools;
    (function (tools) {
        function fixImgS(outer, container) {
            var width = outer.width();
            var img = outer.find("img");
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
            container.css({ 'height': iheight + 'px', 'width': iwidth + 'px' });
            container.css({ 'top': (width - iheight) / 2 + 'px', 'left': (width - iwidth) / 2 + 'px' });
            return ratio;
        }
        tools.fixImgS = fixImgS;
    })(tools = mp.tools || (mp.tools = {}));
})(mp || (mp = {}));
