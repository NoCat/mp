$.fn.bsSelect = function (method, args) {
    var select = $(this);
    var options = select.find('.dropdown-menu');
    var input = select.find('input[type="hidden"]');
    var current = select.find('.current');
    switch (method) {
        case 'add':
            {
                var item = args;
                var option = $('<li><a></a></li>');
                var a = option.find('a');
                a.attr('data-value', item.value);
                option.find('a').text(item.text);
                options.append(option);
            }
            break;
        case 'select':
            {
                var value = args;
                var option = options.find('li a[data-value="' + value + '"]');
                if (option.length != 0) {
                    options.find('.active').removeClass('active');
                    option.parent().addClass('active');
                    input.val(value);
                    current.text(option.text());
                }
            }
            break;
        default:
            {
                select.bsSelect('select', input.val());
            }
            break;
    }
};
$(function () {
    $(document).on('click', '.select .dropdown-menu li a', function (e) {
        var option = $(e.target);
        var select = option.parents('.select');
        var value = option.data('value');
        select.bsSelect('select', value);
    });
});
