/*
File name: 显示、隐藏用户输入密码。
Author:guojiaqiu
Date:2015-12-18
url:http://www.sopcce.com
Description: // 主要用于显示、隐藏用户输入密码。
改变文本属性，依赖于JQ
*/
(function ($) {
    $.fn.hidePassword = function (options) {
        var s = $.extend($.fn.hidePassword.defaults, options),
        input = $(this);

        $(s.el).bind(s.ev, function () {
            "password" == $(input).attr("type") ?
                $(input).attr("type", "text") :
                $(input).attr("type", "password");
        });
    };
    $.fn.hidePassword.defaults = {
        ev: "click"
    };
}(jQuery));