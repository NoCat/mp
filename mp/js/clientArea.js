var mp;
(function (mp) {
    var client = (function () {
        function client() {
        }
        client.columnCount = 0;
        return client;
    })();
    mp.client = client;
    var columnWidth = 248;
    var padding = 100;
    $(function () {
        calculateWidth();
        $(window).resize(calculateWidth);
        $('.waterfall-236').masonry({
            columnWidth: 248,
            itemSelector: '.waterfall-item-236'
        });
    });
    function calculateWidth() {
        var width = $(window).width();
        var container = $(".mp-container");
        var count = 0;
        if (width > columnWidth * 6 + padding) {
            count = 6;
            container.css({ 'width': columnWidth * 6 });
        }
        else if (width > columnWidth * 5 + padding) {
            count = 5;
            container.css({ 'width': columnWidth * 5 });
        }
        else {
            count = 4;
            container.css({ 'width': columnWidth * 4 });
        }
        if (count != client.columnCount) {
            client.columnCount = count;
            if (client.onColumnCountChange != null) {
                client.onColumnCountChange();
            }
        }
    }
})(mp || (mp = {}));
