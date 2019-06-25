
$(function () {


    if ($('.playmp3-player').length < 1) {
        //debugger
        $('body').append('<div class="playmp3-player" style="width:1px;height:1px;overflow:hidden;"></div>');
    }
    $('*[mp3]').attr('title', '鼠标停留发音..').hover(function () {

        var burl = window.location.origin;
        if (burl === "") {
            burl = window.location.protocol + "//" + window.location.host;
        }
        var mp3url = burl + $(this).attr('mp3');

        console.log("mp3:" + mp3url);
        MyObj.playmp3({ s: '.playmp3-player', url: mp3url });
    }, function () {
        $('.playmp3-player').html('');
    });

    if ($(".playmp3-player").length < 1) {
        $('body').append('<div class="playmp3-player" style="width:1px;height:1px;overflow:hidden;"></div>');
    }
    if ($("*[mp3btn]").length > 0) {
        //mp3播放按钮
        $("*[mp3btn]").click(function () {
            if ($(this).attr('class').indexOf('mp3btn_p') < 1) {
                $("*[mp3btn]").removeClass('mp3btn_p');
                $(this).addClass('mp3btn_p');
                var mp3_file = $(this).attr('mp3btn');
                if (mp3_file.indexOf('/') < 0) {
                    mp3_file = '/en-yinbiao/word-mp3/gotoabc/' + mp3_file + '.mp3';
                }
                MyObj.playmp3({ s: '.playmp3-player', 'loop': true, url: imghost + mp3_file });
            } else {
                $(this).removeClass('mp3btn_p');
                $(".playmp3-player").html('');
            }
        });
    }
});



/*音标通用功能js*/

var MyObj = {
    playmp3: function (options) {
        var burl = window.location.origin;
        if (burl === "") {
            burl = window.location.protocol + "//" + window.location.host;
        }
        var mp3swf = burl + "/en-letter/mp3.swf";
        if (window.applicationCache) {
            $(options.s).html('<audio autoplay="autoplay"' +
                (options.loop ? ' loop="true" Repeat="true"' : '') +
                ' controls="controls" src="' +
                options.url +
                '">Your browser does not support the audio tag.</audio>');
        }
        else {
            $(options.s)
                .html(
                    '<embed height="20" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" width="226" src=' + mp3swf + '?r=' +
                    Math.random() +
                    '&amp;mp3=' +
                    options.url +
                    '&amp;autostart=1&amp;bgcolor=999999&amp;.swf" wmode="transparent" invokeurls="false"' +
                    (options.loop ? ' loop="true" Repeat="true"' : '') +
                    ' quality="high" allowScriptAccess="never" allowNetworking="internal">');
        }
    }
};
var txtContent = $('#txtContent');
function doFirstCase() {
    var str = txtContent.val();
    var strSplit = " ";
    if (str.indexOf(" ") > 0) {
        strSplit = " ";
    }
    if (str.indexOf("-") > 0) {
        strSplit = "-";
    }
    if (str.indexOf("/") > 0) {
        strSplit = "/";
    }
    var array = str.toLowerCase().split(strSplit);
    for (var i = 0; i < array.length; i++) {
        array[i] = array[i][0].toUpperCase() + array[i].substring(1, array[i].length);
    }
    var ret = array.join(strSplit);
    txtContent.val(ret);

}

//转大写
function doUpperCase() {
    var str = txtContent.val();
    var ret = str.toUpperCase();
    txtContent.val(ret);
}

//转小写
function doLowerCase() {
    var str = txtContent.val();
    var ret = str.toLowerCase();
    txtContent.val(ret);
}

//清空
function reset() {
    txtContent.val("");
}


var CApp = {
    ScreenSizeType: 'S', // 小屏，如手机
    ClickEname: 'click',
    ScreenSizeCheck: function () {
        var screenWidth = 0;
        if (window.screen && screen.width) {
            screenWidth = screen.width;
            if (screenWidth > 1920) {
                CApp.ScreenSizeType = 'L'; // 超大屏，例如iMac
            } else if (screenWidth > 600) {
                CApp.ScreenSizeType = 'M'; // 一般屏，如手机
            }
        }
        return CApp.ScreenSizeType;
    },
    IsTouchDevice: function () {
        return 'ontouchstart' in window // works on most browsers 
            ||
            'onmsgesturechange' in window; // works on ie10
    },
    WebClientDevice: function () {
        if ((navigator.userAgent.match(
            /(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone)/i)
        )) {
            return 'mobile';
        } else {
            return 'pc';
        }
    },
    GetQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    },
    ChangeParam: function (url, name, value) {
        var newUrl = "";
        var reg = new RegExp("(^|)" + name + "=([^&]*)(|$)");
        var tmp = name + "=" + value;
        if (url.match(reg) != null) {
            newUrl = url.replace(eval(reg), tmp);
        } else {
            if (url.match("[\?]")) {
                newUrl = url + "&" + tmp;
            } else {
                newUrl = url + "?" + tmp;
            }
        }
        return newUrl;
    },
    getPcUrl: function () {
        var ma = $("link[rel='canonical']");
        for (var i = 0; i < ma.length; i++) {
            var url = ma.eq(i).attr('href');
            return url;
            break;
        }
        return '';
    },
    getMobileUrl: function () {
        var ma = $("meta[name='mobile-agent']");
        for (var i = 0; i < ma.length; i++) {
            var h5reg = /format=html5/gi;
            if (h5reg.test(ma.eq(i).attr('content'))) {
                var patt = new RegExp('url=(.*)', "g");
                var r = patt.exec(ma.eq(i).attr('content'));
                return r[1];
                break;
            }
        }
        return '';
    }
};
CApp.ClickEname = CApp.IsTouchDevice() ? 'touchend' : 'click';

if ((CApp.WebClientDevice() != 'pc' || (CApp.IsTouchDevice() && CApp.ScreenSizeCheck() == 'S')) && CApp.GetQueryString('sfordev') != 'pc') { //判断是否pc端
    var mobileUrl = CApp.getMobileUrl();
    if (mobileUrl != '') { location.href = mobileUrl; }
}

