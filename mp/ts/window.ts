/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
module mp
{
    export class window
    {
        static columnCount: number = 0;
    }

    var columnWidth = 248;
    var padding = 100;

    $(function ()
    {
        calculateWidth();
        $(window).resize(calculateWidth);
    });


    function calculateWidth()
    {
        var width = $(window).width();
        var container = $(".mp-container");

        if (width > columnWidth * 6 + padding)
        {
            window.columnCount = 6;
            container.css({ 'width': columnWidth * 6 });
        }
        else if (width > columnWidth * 5 + padding)
        {
            window.columnCount = 5;
            container.css({ 'width': columnWidth * 5 });
        }
        else if (width > columnWidth * 4 + padding)
        {
            window.columnCount = 4;
            container.css({ 'width': columnWidth * 4 });
        }
    }
}