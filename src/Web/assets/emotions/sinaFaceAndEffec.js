/**
 * @author 
 * @time 2017-06-14
 */

/**
 * 自定义hashtable
 * @returns {} 
 */
function Hashtable() {
    this._hash = new Object();
    this.put = function (key, value) {
        if (typeof (key) != "undefined") {
            if (this.containsKey(key) == false) {
                this._hash[key] = typeof (value) == "undefined" ? null : value;
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
    this.remove = function (key) { delete this._hash[key]; }
    this.size = function () { var i = 0; for (var k in this._hash) { i++; } return i; }
    this.get = function (key) { return this._hash[key]; }
    this.containsKey = function (key) { return typeof (this._hash[key]) != "undefined"; }
    this.clear = function () { for (var k in this._hash) { delete this._hash[k]; } }
}


var emotions = new Array();
var categorys = new Array();// 分组
var uSinaEmotionsHt = new Hashtable();

// 初始化缓存，页面仅仅加载一次就可以了
$(function () {
    var app_id = '1362404091';
    $.ajax({
        dataType: 'jsonp',
        url: 'https://api.weibo.com/2/emotions.json?source=' + app_id,
        success: function (response) {
            var data = response.data;
            for (var i in data) {
                if (data[i].category == '') {
                    data[i].category = '默认';
                }
                if (emotions[data[i].category] == undefined) {
                    emotions[data[i].category] = new Array();
                    categorys.push(data[i].category);
                }
                emotions[data[i].category].push({
                    name: data[i].phrase,
                    icon: data[i].icon
                });
                uSinaEmotionsHt.put(data[i].phrase, data[i].icon);
            }
        }
    });
});

/**
 * 替换
 * @param {any} s
 */
function AnalyticEmotion(s) {
    if (typeof (s) != "undefined") {
        var sArr = s.match(/\[.*?\]/g);
        if (null != sArr && '' != sArr) {
            for (var i = 0; i < sArr.length; i++) {
                if (uSinaEmotionsHt.containsKey(sArr[i])) {
                    var reStr = "<img src=\"" + uSinaEmotionsHt.get(sArr[i]) + "\" height=\"22\" width=\"22\" />";
                    s = s.replace(sArr[i], reStr);
                }
            }
        }

    }
    return s;
}

(function ($) {
    $.fn.SopEmotion = function (target) {

        var cat_current;
        var cat_page;
        $(this).click(function (event) {
            event.stopPropagation();
            //暂时不想做了  就这样了
            var eTop = target.offset().top - 70;

            var eLeft = target.offset().left + 30;

            //var eTop1 = target.position().top;
            //var eLeft1 = target.position().left - 1;



            if ($('.sop-emotions .sop-emotions-categorys')[0]) {
                $('.sop-emotions').css({ top: eTop, left: eLeft, "z-index": 2099, "position": "absolute" });
                $('.sop-emotions').toggle();
                return;
            }
            //eTop = 1024;
            //eLeft = 80;
            $('body').append('<div class="sop-emotions"></div>');
            $('.sop-emotions').css({ top: eTop, left: eLeft, "z-index": 2099, "position": "absolute" });
            $('.sop-emotions').html('<div>正在加载，请稍候...</div>');
            $('.sop-emotions').click(function (event) {
                event.stopPropagation();
            });
            var htmltext =
                '<div class="sop-emotions-header" style="float:right">' +
                '<a href="javascript:void(0);" id="prev">&laquo;</a>' +
                '<a href="javascript:void(0);" id="next">&raquo;</a>' +
                '</div>' +
                '<div class="sop-emotions-categorys"></div>' +
                '<div class="sop-emotions-container"></div>';
            $('.sop-emotions').html(htmltext);
            $('.sop-emotions #prev').click(function () {
                showCategorys(cat_page - 1);
            });
            $('.sop-emotions #next').click(function () {
                showCategorys(cat_page + 1);
            });
            showCategorys();
            showEmotions();

        });
        $('body').click(function () {
            $('.sop-emotions').remove();
        });
		/**
		 * 显示栏目
		 */
        function showCategorys() {
            var page = arguments[0] ? arguments[0] : 0;
            if (page < 0 || page >= categorys.length / 5) {
                return;
            }
            $('.sop-emotions .sop-emotions-categorys').html('');
            cat_page = page;
            for (var i = page * 5; i < (page + 1) * 5 && i < categorys.length; ++i) {
                $('.sop-emotions .sop-emotions-categorys').append($('<a href="javascript:void();">' + categorys[i] + '</a>'));
            }
            $('.sop-emotions .sop-emotions-categorys a').click(function () {
                showEmotions($(this).text());
            });
            $('.sop-emotions .sop-emotions-categorys a').each(function () {
                if ($(this).text() == cat_current) {
                    $(this).addClass('current');
                }
            });
        }
        /**
         * 显示表情
         */
        function showEmotions() {
            var category = arguments[0] ? arguments[0] : '默认';
            var page = arguments[1] ? arguments[1] - 1 : 0;
            $('.sop-emotions .sop-emotions-container').html('');
            cat_current = category;
            for (var i = 0; i < emotions[category].length; ++i) {
                $('.sop-emotions .sop-emotions-container').append($('<a href="javascript:void(0);" title="' + emotions[category][i].name + '"><img src="' + emotions[category][i].icon + '" alt="' + emotions[category][i].name + '" width="22" height="22" /></a>'));
            }
            $('.sop-emotions .sop-emotions-container a').click(function () {
                target.insertText($(this).attr('title'));
                $('.sop-emotions').remove();
            });

            $('.sop-emotions .sop-emotions-categorys a.current').removeClass('current');
            $('.sop-emotions .sop-emotions-categorys a').each(function () {
                if ($(this).text() == category) {
                    $(this).addClass('current');
                }
            });
        }
        $.fn.insertText = function (text) {
            this.each(function () {
                if (this.tagName !== 'INPUT' && this.tagName !== 'TEXTAREA') { return; }
                if (document.selection) {
                    this.focus();
                    var cr = document.selection.createRange();
                    cr.text = text;
                    cr.collapse();
                    cr.select();
                } else if (this.selectionStart || this.selectionStart == '0') {
                    var
                        start = this.selectionStart,
                        end = this.selectionEnd;
                    this.value = this.value.substring(0, start) + text + this.value.substring(end, this.value.length);
                    this.selectionStart = this.selectionEnd = start + text.length;
                } else {
                    this.value += text;
                }
            });
            return this;
        }
    }

})(jQuery);
