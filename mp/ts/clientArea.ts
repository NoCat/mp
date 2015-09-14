/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="masonry.ts" />
module mp
{
    export class clientArea
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
    });


    function calculateWidth()
    {
        var width = $(clientArea).width();
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

        if (count != clientArea.columnCount)
        {
            clientArea.columnCount = count;
            if (clientArea.onColumnCountChange != null)
            {
                clientArea.onColumnCountChange();
            }
        }
    }
}