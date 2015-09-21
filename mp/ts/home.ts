/// <reference path="clientArea.ts" />
module mp.home
{
    $(function ()
    {
        calculate();
        client.onColumnCountChange = calculate;
    });

    function calculate()
    {
        var waterfalls = $('.display-row');
        waterfalls.find('.waterfall-item-236').each(function (index, elem)
        {
            if (index < client.columnCount)
                $(elem).show();
            else
                $(elem).hide();
        });
    }
}