;
(function ($) {

    // 在我们插件容器内，创造一个公共变量来构建一个私有方法
    var privateFunction = function () {
        // code here
    }

    // 通过字面量创造一个对象，存储我们需要的共有方法
    var methods = {
        // 在字面量对象中定义每个单独的方法
        init: function () {

            // 为了更好的灵活性，对来自主函数，并进入每个方法中的选择器其中的每个单独的元素都执行代码
            return this.each(function () {
                // 为每个独立的元素创建一个jQuery对象
                var $this = $(this);

                // 执行代码
                // 例如： privateFunction();
            });
        },
        destroy: function () {
            // 对选择器每个元素都执行方法
            return this.each(function () {
                // 执行代码
            });
        }
    };

    $.fn.pluginName = function () {
        // 获取我们的方法，遗憾的是，如果我们用function(method){}来实现，这样会毁掉一切的
        var method = arguments[0];

        // 检验方法是否存在
        if (methods[method]) {

            // 如果方法存在，存储起来以便使用
            // 注意：我这样做是为了等下更方便地使用each（）
            method = methods[method];

            // 如果方法不存在，检验对象是否为一个对象（JSON对象）或者method方法没有被传入
        } else if (typeof (method) == 'object' || !method) {

            // 如果我们传入的是一个对象参数，或者根本没有参数，init方法会被调用
            method = methods.init;
        } else {

            // 如果方法不存在或者参数没传入，则报出错误。需要调用的方法没有被正确调用
            $.error('Method ' + method + ' does not exist on jQuery.pluginName');
            return this;
        }

        // 调用我们选中的方法
        // 再一次注意我们是如何将each（）从这里转移到每个单独的方法上的
        return method.call(this);

    }

})(jQuery);





; (function ($, window, document, undefined) {
    //定义Beautifier的构造函数
    var Beautifier = function (ele, opt) {
        this.$element = ele,
            this.defaults = {
                'color': 'red',
                'fontSize': '12px',
                'textDecoration': 'none'
            },
            this.options = $.extend({}, this.defaults, opt)
    }
    //定义Beautifier的方法
    Beautifier.prototype = {
        beautify: function () {
            return this.$element.css({
                'color': this.options.color,
                'fontSize': this.options.fontSize,
                'textDecoration': this.options.textDecoration
            });
        }
    }
    //在插件中使用Beautifier对象
    $.fn.myPlugin = function (options) {
        //创建Beautifier的实体
        var beautifier = new Beautifier(this, options);
        //调用其方法
        return beautifier.beautify();
    }
})(jQuery, window, document);
 
/*
 注意这些例子可以在目前的插件代码中正确运行，并不是所有的插件都使用同样的代码结构 
*/
// 为每个类名为 ".className" 的元素执行init方法
//$('.className').pluginName();
//$('.className').pluginName('init');
//$('.className').pluginName('init', {}); // 向init方法传入“{}”对象作为函数参数
//$('.className').pluginName({}); // 向init方法传入“{}”对象作为函数参数

//// 为每个类名为 “.className” 的元素执行destroy方法
//$('.className').pluginName('destroy');
//$('.className').pluginName('destroy', {}); // 向destroy方法传入“{}”对象作为函数参数

//// 所有代码都可以正常运行
//$('.className').pluginName('init', 'argument1', 'argument2'); // 把 "argument 1" 和 "argument 2" 传入 "init"

//// 不正确的使用
//$('.className').pluginName('nonexistantMethod');
//$('.className').pluginName('nonexistantMethod', {});
//$('.className').pluginName('argument 1'); // 会尝试调用 "argument 1" 方法
//$('.className').pluginName('argument 1', 'argument 2'); // 会尝试调用 "argument 1" ，“argument 2”方法
//$('.className').pluginName('privateFunction'); // 'privateFunction' 不是一个方法