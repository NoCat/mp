/// <reference path="../scripts/typings/jquery/jquery.d.ts" />

interface JQuery
{
    bsSelect(method?: string, args?: any);
}

interface selectItem
{
    text: string
    value: string
}

$.fn.bsSelect = function (method?: string, args?: any)
{
    var select = $(this);
    var options = select.find('.dropdown-menu');
    var input = select.find('input[type="hidden"]');
    var current = select.find('.current');

    switch (method)
    {
        case 'add':
            {
                var item: selectItem = args;

                var option = $('<li><a></a></li>');
                var a = option.find('a');
                a.attr('data-value', item.value);
                option.find('a').text(item.text);

                options.append(option);
            }
            break;
        case 'select':
            {
                var value: string = args;

                var option = options.find('li a[data-value="' + value + '"]');
                if (option.length != 0)
                {
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
}

$(() =>
{
    $(document).on('click', '.select .dropdown-menu li a',(e) =>
    {
        var option = $(e.target);
        var select = option.parents('.select');

        var value = option.data('value');
        select.bsSelect('select', value);
    });
});