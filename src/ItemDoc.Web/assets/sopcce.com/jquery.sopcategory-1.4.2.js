/*
File name: 栏目联动查询选择
Author:guojiaqiu
Date:2017-4-18
url:http://www.sopcce.com
Description: // 
*/
;
(function (factory) {
    if (typeof define === "function" && (define.amd || define.cmd) && !jQuery) {
        // AMD或CMD
        define(["jquery"], factory);
    } else if (typeof module === 'object' && module.exports) {
        // Node/CommonJS
        module.exports = function (root, jQuery) {
            if (jQuery === undefined) {
                if (typeof window !== 'undefined') {
                    jQuery = require('jquery');
                } else {
                    jQuery = require('jquery')(root);
                }
            }
            factory(jQuery);
            return jQuery;
        };
    } else {
        //Browser globals
        factory(jQuery);
    }
}(function ($) {

    //配置参数
    /// <summary>
    /// s the specified $.
    /// </summary>
    /// <param name="$">The $.</param>
    /// <returns></returns>
    var defaults = {
        parenturl: '/ConsoleBlogsManage/GetRootCategories', //默认一级父类json
        childurl: '/ConsoleBlogsManage/GetChildrenDictionary', //子类获取json
        sop_changecode: 'CategoryId1',
        sop_changename: "CategoryName1",

        showText: 'name', //显示文字code 还是name
        level: 1, //初始化显示1级
        sop_default_value: '请选择',
        isDebug: false,
        selecthtml: "<select class='form-control' " +
        "style='width:100px'" +
        " sop-data='sop-soplevel' > </select>",
        optionhtml: "<option value='sopvalue' " +
        "sop-data='sop-soplevel - ' " +
        "sop-code='sopcode' " +
        "sop-name='sopname' >soptext</option>",
        callback: function () { }	//回调
    };

    var Sopcategory = function (element, options) {
        /**
         * 全局变量
         */
        var opts = options,//配置
            $document = $(document),
            $obj = $(element);//容
        /**
         * 初始化配置
         * @param {} opts 配置数组
         * @returns {} 无法值
         */
        this.filling = function (opts) {
            var html = opts.selecthtml.replace(/soplevel/gi, opts.level);
            if (opts.isDebug) {
                console.log("初始化显示html:" + html);
            }
            $obj.empty().html(html);
            loadDefault(opts.parenturl, opts.level, "");
        };
        /**
         * 加载数据
         * @param {any} url 获取json 的远程URL
         * @param {any} level 当前层次等级
         * @param {any} categoryid 栏目ID
         */
        function loadDefault(url, level, categoryid) {
            $.ajax({
                type: "GET",
                url: url,
                data: { categoryid: categoryid },
                dataType: "json",
                success: function (data) {
                    var defaultoptionhtml = opts.optionhtml
                        .replace(/soplevel/gi, level)
                        .replace(/sopcode/gi, "").replace(/sopname/gi, "")
                        .replace(/sopvalue/gi, "").replace(/soptext/gi, opts.sop_default_value);
                    if (opts.isDebug) {
                        console.log("层数:" + level + "初始化加载数据：" + defaultoptionhtml);
                    }
                    $("select[sop-data='sop-" + level + "']").append(defaultoptionhtml);
                    if (data == "") {
                        if (opts.isDebug) {
                            console.log("为空层数:" + level);
                        }
                        return false;
                    }
                    for (var i = 0; i < data.length; i++) {
                        var value = (opts.showText === "code" ? data[i].code : data[i].name);
                        var appendoptionhtml = opts.optionhtml
                            .replace(/soplevel/gi, level + "-" + data[i].code)
                            .replace(/sopcode/gi, data[i].code).replace(/sopname/gi, data[i].name)
                            .replace(/sopvalue/gi, value).replace(/soptext/gi, data[i].name);
                        if (opts.isDebug) {
                            console.log("层数:" + level + "加载数据第" + i + "个子集：" + appendoptionhtml);
                        }
                        $("select[sop-data='sop-" + level + "']").append(appendoptionhtml);
                    }
                    loadChange(level);
                    level = eval(parseInt(level) + 1);
                }

            });
        }
        /**
         * 加载change事件
         * @param {any} level 
         */
        function loadChange(level) {
            var theid = "select[sop-data='sop-" + level + "']";
            $(theid).change(function () {
                var $currentDropDownList = $(this);
                var sopvalue = $currentDropDownList.attr('sop-data');
                if (sopvalue.indexOf('-') == -1) {
                    sopvalue += "-";
                }
                /**
                * 清除无效层数
                */
                level = sopvalue.split('-')[1];
                level = level.length > 0 ? level : 0;
                for (var i = 0; i < parseInt(level); i++) {
                    $("#" + $obj.attr('id') + " select[sop-data^='sop-']").each(function () {
                        var sopvalue = $(this).attr('sop-data');
                        if (parseInt(sopvalue.split('-')[1]) > parseInt(eval(parseInt(level) + i))) {
                            $("select[sop-data='" + sopvalue + "']").remove();
                        }
                    });

                }
                var codeValue = $(theid + " option:selected").attr('sop-code');
                var nameValue = $(theid + " option:selected").attr('sop-name');
                if (codeValue && codeValue.length > 0) {
                    var childselecthtml = opts.selecthtml.replace(/soplevel/gi, eval(parseInt(level) + 1));
                    $currentDropDownList.after(childselecthtml);
                    level = eval(parseInt(level) + 1);
                    loadDefault(opts.childurl, level, codeValue);
                }
                if (opts.isDebug) {
                    console.log("输出选择的code:" + codeValue + "选择的name：" + nameValue);
                }
                if (typeof (opts.sop_changecode) != "undefined") {
                    if (opts.sop_changecode.indexOf('#') === -1) {
                        opts.sop_changecode = "#" + opts.sop_changecode;
                    }
                    $(opts.sop_changecode).val(codeValue);
                }
                if (typeof (opts.sop_changename) != "undefined") {
                    if (opts.sop_changename.indexOf('#') === -1) {
                        opts.sop_changename = "#" + opts.sop_changename;
                    }
                    $(opts.sop_changename).val(nameValue);
                }
            });
        }
        /**
         * 初始化
         * @returns {} 
         */
        this.init = function () {
            this.filling(opts);

        };
        this.init();
    };

    /// <summary>
    /// s the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    $.fn.sopcategory = function (parameter, callback) { 
        if (typeof parameter == 'function') {//重载
            callback = parameter;
            parameter = {};
        } else {
            parameter = parameter || {};
            callback = callback || function () { };
        }
        var options = $.extend({}, defaults, parameter);
        return this.each(function () {
            var pagination = new Sopcategory(this, options);
            callback(pagination);
        });
    };

}));