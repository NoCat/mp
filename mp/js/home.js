var mp;
(function (mp) {
    var home;
    (function (home) {
        $(function () {
            calculate();
            mp.clientArea.onColumnCountChange = calculate;
        });
        function calculate() {
            var waterfalls = $('.display-row');
            waterfalls.find('.waterfall-item-236').each(function (index, elem) {
                if (index < mp.clientArea.columnCount)
                    $(elem).show();
                else
                    $(elem).hide();
            });
        }
    })(home = mp.home || (mp.home = {}));
})(mp || (mp = {}));
