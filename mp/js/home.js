var mp;
(function (mp) {
    var home;
    (function (home) {
        $(function () {
            calculate();
            mp.client.onColumnCountChange = calculate;
        });
        function calculate() {
            var waterfalls = $('.display-row');
            waterfalls.find('.waterfall-item').each(function (index, elem) {
                if (index < mp.client.columnCount)
                    $(elem).show();
                else
                    $(elem).hide();
            });
        }
    })(home = mp.home || (mp.home = {}));
})(mp || (mp = {}));
