
//<sopcce.com>
//--------------------------------------------------------------
//<version>1.0.1</verion>
//<createdate>2018-3-14</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-3-14" version="1.0">可以使用，后期在处理bug</log>
//--------------------------------------------------------------
//<sopcce.com>

// 获取当前时间戳(以s为单位)
var timestamp = Date.parse(new Date());
timestamp = timestamp / 1000;
//当前时间戳为：1403149534
//console.log("当前时间戳为：" + timestamp);

// 获取某个时间格式的时间戳
var stringTime = "2014-07-10 10:21:12";
var timestamp2 = Date.parse(new Date(stringTime));
timestamp2 = timestamp2 / 1000;
//2014-07-10 10:21:12的时间戳为：1404958872 
//console.log(stringTime + "的时间戳为：" + timestamp2);

// 将当前时间换成时间格式字符串
var timestamp3 = 1403058804;
var newDate = new Date();
newDate.setTime(timestamp3 * 1000);
// Wed Jun 18 2014 
//console.log(newDate.toDateString());
// Wed, 18 Jun 2014 02:33:24 GMT 
//console.log(newDate.toGMTString());
// 2014-06-18T02:33:24.000Z
//console.log(newDate.toISOString());
// 2014-06-18T02:33:24.000Z 
//console.log(newDate.toJSON());
// 2014年6月18日 
//console.log(newDate.toLocaleDateString());
// 2014年6月18日 上午10:33:24 
//console.log(newDate.toLocaleString());
// 上午10:33:24 
//console.log(newDate.toLocaleTimeString());
// Wed Jun 18 2014 10:33:24 GMT+0800 (中国标准时间)
//console.log(newDate.toString());
// 10:33:24 GMT+0800 (中国标准时间) 
//console.log(newDate.toTimeString());
// Wed, 18 Jun 2014 02:33:24 GMT
//console.log(newDate.toUTCString());

// ReSharper disable once NativeTypePrototypeExtending 
Date.prototype.format = function (format) {
    var date = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "F+":"日一二三四五六".charAt(this.getDay()),
        "f+": this.getMilliseconds()
    };
   
    if (/(y+)/i.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
    }
    for (var k in date) {
        if (date.hasOwnProperty(k)) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1,
                    RegExp.$1.length == 1
                        ? date[k]
                        : ("00" + date[k]).substr(("" + date[k]).length));
            }
        }
    }
    return format;
}
//console.log(newDate.format('yyyy-MM-dd h:m:s

/** 
* y年， m月， d日， h小时， n分钟，s秒  
*/
// ReSharper disable once NativeTypePrototypeExtending
Date.prototype.add = function (part, value) {
    value *= 1;
    if (isNaN(value)) {
        value = 0;
    }
    switch (part) {
        case "y":
            this.setFullYear(this.getFullYear() + value);
            break;
        case "m":
            this.setMonth(this.getMonth() + value);
            break;
        case "d":
            this.setDate(this.getDate() + value);
            break;
        case "h":
            this.setHours(this.getHours() + value);
            break;
        case "n":
            this.setMinutes(this.getMinutes() + value);
            break;
        case "s":
            this.setSeconds(this.getSeconds() + value);
            break;
        default:

    }
}
// ReSharper disable once 
/**
* Returns the number of milliseconds between midnight, January 1, 1970 Universal Coordinated Time (UTC) (or GMT) and the specified date.
* @param year The full year designation is required for cross-century date accuracy. If year is between 0 and 99 is used, then year is assumed to be 1900 + year.
* @param month The month as an number between 0 and 11 (January to December).
* @param date The date as an number between 1 and 31.
* @param hours Must be supplied if minutes is supplied. An number from 0 to 23 (midnight to 11pm) that specifies the hour.
* @param minutes Must be supplied if seconds is supplied. An number from 0 to 59 that specifies the minutes.
* @param seconds Must be supplied if milliseconds is supplied. An number from 0 to 59 that specifies the seconds.
* @param ms An number from 0 to 999 that specifies the milliseconds.
 * //TODO:BUG 没有测试，具体不是很清楚  哈哈哈
*/
var stringToDate = function (dateStr, format, ymdSeparator, hmsSeparator) {

    if (!ymdSeparator) {
        ymdSeparator = "-";
    }
    if (!hmsSeparator) {
        hmsSeparator = ":";
    }

    var dateStrArr = dateStr.split(" ");
    var ymdArr = dateStrArr[0] !== undefined && dateStrArr[0].indexOf(ymdSeparator) > -1
        ? dateStrArr[0].split(ymdSeparator)
        : "1970-1-1".split(ymdSeparator);
    var hmsArr = dateStrArr[1] !== undefined && dateStrArr[1].indexOf(hmsSeparator) > -1
        ? dateStrArr[1].split(hmsSeparator)
        : "00:00:00".split(hmsSeparator);



    var year = parseInt(ymdArr[0]);
    var month;
    //处理月份为04这样的情况
    if (ymdArr[1].indexOf("0") === 0) {
        month = parseInt(ymdArr[1].substring(1));
    } else {
        month = parseInt(ymdArr[1]);
    }

    var date = parseInt(ymdArr[2]);

    var hours = parseInt(hmsArr.length > 0 ? hmsArr[0] : 0);
    var minutes = parseInt(hmsArr.length > 1 ? hmsArr[1] : 0);
    var seconds = parseInt(hmsArr.length > 2 ? hmsArr[2] : 0);

    var ms = parseInt(dateStrArr.length === 3 ? dateStrArr[2] : 0);

    var data = new Date(year, month - 1, date, hours, minutes, seconds, ms).format(format);
    console.log(data);
    return data;

}


