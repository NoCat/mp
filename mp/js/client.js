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
        $(window).scroll(function (e) {
            var waterfall = $('.waterfall-236');
            if (waterfall.length == 0)
                return;
            var more = waterfall.find('.waterfall-item-236.more');
            if (more.length == 0)
                return;
            var w = $(window);
            var top = w.scrollTop();
            var bottom = top + w.height();
            var moreTop = more.offset().top;
            if (top <= moreTop && moreTop <= bottom) {
                waterfall.masonry('remove', more);
                var url = waterfall.data('url');
                var max = waterfall.find('.waterfall-item-236:last').data('id');
                url += max;
                $.get(url, function (data) {
                    waterfall.masonry('appended', data);
                });
            }
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
