/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="masonry.ts" />
module mp
{
    export class client
    {
        static columnCount = 0;
        static onColumnCountChange: () => void;
    }

    var columnWidth = 248;
    var padding = 100;

    $(function ()
    {
        calculateWidth();
        $(window).resize(calculateWidth);
        $('.waterfall-236').masonry({
            columnWidth: 248,
            itemSelector: '.waterfall-item-236'
        });

        $(window).scroll((e) =>
        {
            var waterfall = $('.waterfall-236');
            if (waterfall.length == 0)
                return;

            var more = waterfall.find('.waterfall-item-236.more');
            if (more.length == 0)
                return;

            var w = $(window);
            var bottom = w.scrollTop() + w.height();

            var moreTop = more.offset().top;
            if (moreTop <= bottom)
            {
                more.remove();
                waterfall.masonry('remove', more);
                var url = waterfall.data('url');
                if (url == null)
                    return;

                var max = waterfall.find('.waterfall-item-236:last').data('id');
                url += max;
                $.get(url,(data) =>
                {
                    var div = $('<div></div>');
                    div.append(data);
                    var children = div.children();
                    waterfall.append(children).masonry('appended', children);
                });
            }
        });
    });


    function calculateWidth()
    {
        var width = $(window).width();
        var container = $(".mp-container");
        var count = 0;

        if (width > columnWidth * 6 + padding)
        {
            count = 6;
            container.css({ 'width': columnWidth * 6 });
        }
        else if (width > columnWidth * 5 + padding)
        {
            count = 5;
            container.css({ 'width': columnWidth * 5 });
        }
        else
        {
            count = 4;
            container.css({ 'width': columnWidth * 4 });
        }

        if (count != client.columnCount)
        {
            client.columnCount = count;
            if (client.onColumnCountChange != null)
            {
                client.onColumnCountChange();
            }
        }
    }
}