/// <reference path="client.ts" />
module mp.home {
    $(function () {
        calculate();
        client.onColumnCountChange = calculate;
    });

    function calculate() {
        var waterfalls = $('.display-row');
        waterfalls.find('.waterfall-item').each(function (index, elem) {
            if (index < client.columnCount)
                $(elem).show();
            else
                $(elem).hide();
        });
    }
}