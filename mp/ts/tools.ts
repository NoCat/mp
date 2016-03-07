/// <reference path="../scripts/typings/jquery/jquery.d.ts" />

module mp.tools {
    export function fixImgS(outer: JQuery,container:JQuery) {
        var width = outer.width();
        outer.css('height', width);
        var img = outer.find("img");
        var ratio: number;
        var iwidth = img.prop('width');
        var iheight = img.prop('height');

        //算出比例
        if (iwidth > iheight ) {
            ratio = width/iwidth;
        }
        else if(iheight>iwidth){
            ratio = width / iheight;
        }

        //更改图片大小并定位
        iheight = iheight * ratio;
        iwidth = iwidth * ratio;
        img.css({ 'height': iheight + 'px', 'width': iwidth + 'px' });
        container.css({ 'height': iheight + 'px', 'width': iwidth + 'px' });
        container.css({ 'top': (width - iheight) / 2 + 'px', 'left': (width - iwidth) / 2 + 'px' });
        return ratio;
    }
}