/// <reference path="clientArea.ts" />
module mp.home
{
    $(function ()
    {
        calculate();
        clientArea.onColumnCountChange = calculate;
    });

    function calculate()
    {
        var waterfalls = $('.display-row');
        waterfalls.find('.waterfall-item-236').each(function (index, elem)
        {
            if (index < clientArea.columnCount)
                $(elem).show();
            else
                $(elem).hide();
        });
    }
}