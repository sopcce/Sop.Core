//<sopcce.com>
//--------------------------------------------------------------
//<version>1.5.1</verion>
//<createdate>2018-3-14</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-3-14" version="1.5.0">修改样式</log>
//--------------------------------------------------------------
//<sopcce.com>
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
    //totalData  totalPages
    //配置参数
    var defaults = {
        totalCount: 0,			//数据总条数
        totalPages: 2,			//总页数
        pageSize: 4,			//每页显示的条数
        pageIndex: 1,				//当前第几页

        isDebug: false,
        prevCls: 'prev',		//上一页class
        nextCls: 'next',		//下一页class
        prevContent: '上一页',		//上一页内容
        nextContent: '下一页',		//下一页内容
        activeCls: 'active',	//当前页选中状态
        coping: false,			//首页和尾页
        isHide: true,			//当前页数为0页或者1页时不显示分页
        homePage: '',			//首页节点内容
        endPage: '',			//尾页节点内容
        keepShowPN: false,		//是否一直显示上一页下一页
        count: 2,				//当前页前后分页个数
        jump: false,			//跳转到指定页数
        jumpIptCls: 'jump-ipt',	//文本框内容
        jumpBtnCls: 'jump-btn',	//跳转按钮
        jumpBtn: '跳转',		//跳转按钮文本
        callback: function () { }	//回调
    };

    var Pagination = function (element, options) {
        //全局变量
        var opts = options,//配置
            current,//当前页
            $document = $(document),
            $obj = $(element);//容器

        /**
		 * 每页显示的条数
		 * @param int page 页码
		 * @return opts.pageCount 总页数配置
		 */
        this.pageSize = function () {
            return opts.pageSize;
        };
        /**
       * 获取当前页
       * @return int current 当前第几页
       */
        this.getPageIndex = function () {
            return current;
        };

        /**
		 * 获取总页数
		 * 如果配置了总条数和每页显示条数，将会自动计算总页数并略过总页数配置，反之
		 * @return int p 总页数
		 */
        this.getTotalPages = function () {
            var totalstr = opts.totalCount || opts.pageSize ? Math.ceil(parseInt(opts.totalCount) / opts.pageSize) : opts.totalPages;
            if (opts.isDebug) console.log("sop:getTotalPages:" + totalstr);
            return totalstr;
        };


        /**
		 * 填充html数据
		 * @param int index 页码
		 */
        this.filling = function (index) {
            var html = "<ul class='pagination'>";
            current = index || opts.pageIndex;//当前页码
            if (opts.isDebug) console.log("sop:当前页码:" + current);
            var totalPages = this.getTotalPages();//获取的总页数
            if (opts.isDebug) console.log("sop:获取的总页数:" + totalPages);
            ////上一页
            if (opts.keepShowPN || current > 1) {
                html += '<li><a href="javascript:;" class="' + opts.prevCls + '">' + opts.prevContent + '</a></li>';
            }
            else {
                if (opts.keepShowPN == false) {
                    $obj.find('.' + opts.prevCls) && $obj.find('.' + opts.prevCls).remove();
                }
            }
            if (current >= opts.count + 2 && current != 1 && totalPages != opts.count) {
                var home = opts.coping && opts.homePage ? opts.homePage : '1';
                html += opts.coping ? '<li><a href="javascript:;" data-page="1">' + home + '</a> </li> <li>...</li>' : '';
            }
            var end = current + opts.count;
            var start = '';
            //修复到最后一页时比第一页少显示两页
            start = current === totalPages ? current - opts.count - 2 : current - opts.count;
            ((start > 1 && current < opts.count) || current == 1) && end++;
            (current > totalPages - opts.count && current >= totalPages) && start++;
            for (; start <= end; start++) {
                if (start <= totalPages && start >= 1) {
                    if (start != current) {
                        html += '<li><a href="javascript:;" data-page="' + start + '">' + start + '</a> </li>';
                    } else {
                        html += '<li class="' + opts.activeCls + '"><a>' + start + '</a></li>';
                    }
                }
            }
            if (current + opts.count < totalPages && current >= 1 && totalPages > opts.count) {
                var end = opts.coping && opts.endPage ? opts.endPage : totalPages;
                html += opts.coping ? '<li>...</li><li><a href="javascript:;" data-page="' + totalPages + '">' + end + '</a></li>' : '';
            }
            if (opts.keepShowPN || current < totalPages) {//下一页
                html += '<li><a href="javascript:;" class="' + opts.nextCls + '">' + opts.nextContent + '</a></li>'
            } else {
                if (opts.keepShowPN == false) {
                    $obj.find('.' + opts.nextCls) && $obj.find('.' + opts.nextCls).remove();
                }
            }
            html += opts.jump ? '<input type="text" class="' + opts.jumpIptCls + '"><a href="javascript:;" class="' + opts.jumpBtnCls + '">' + opts.jumpBtn + '</a>' : '';

            html += "</ul>";
            $obj.empty().html(html);
        };

        //绑定事件
        this.eventBind = function () {
            var that = this;
            var pageCount = that.getTotalPages();//总页数
            var index = 1;
            $obj.off().on('click', 'a', function () {
                if ($(this).hasClass(opts.nextCls)) {
                    if ($obj.find('.' + opts.activeCls).text() >= pageCount) {
                        $(this).addClass('disabled');
                        return false;
                    } else {
                        index = parseInt($obj.find('.' + opts.activeCls).text()) + 1;
                    }
                } else if ($(this).hasClass(opts.prevCls)) {
                    if ($obj.find('.' + opts.activeCls).text() <= 1) {
                        $(this).addClass('disabled');
                        return false;
                    } else {
                        index = parseInt($obj.find('.' + opts.activeCls).text()) - 1;
                    }
                } else if ($(this).hasClass(opts.jumpBtnCls)) {
                    if ($obj.find('.' + opts.jumpIptCls).val() !== '') {
                        index = parseInt($obj.find('.' + opts.jumpIptCls).val());
                    } else {
                        return false;
                    }
                } else {
                    index = parseInt($(this).data('page'));
                }
                that.filling(index);
                typeof opts.callback === 'function' && opts.callback(that);
            });
            //输入跳转的页码
            $obj.on('input propertychange', '.' + opts.jumpIptCls, function () {
                var $this = $(this);
                var val = $this.val();
                var reg = /[^\d]/g;
                if (reg.test(val)) {
                    $this.val(val.replace(reg, ''));
                }
                (parseInt(val) > pageCount) && $this.val(pageCount);
                if (parseInt(val) === 0) {//最小值为1
                    $this.val(1);
                }
            });
            //回车跳转指定页码
            $document.keydown(function (e) {
                if (e.keyCode == 13 && $obj.find('.' + opts.jumpIptCls).val()) {
                    var index = parseInt($obj.find('.' + opts.jumpIptCls).val());
                    that.filling(index);
                    typeof opts.callback === 'function' && opts.callback(that);
                }
            });
        };

        //初始化
        this.init = function () {
            this.filling(opts.pageIndex);
            this.eventBind();
            if (opts.isHide && this.getTotalPages() == '1' || this.getTotalPages() == '0')
                $obj.hide();
        };
        this.init();
    };

    $.fn.soppagination = function (parameter, callback) {
        if (typeof parameter == 'function') {//重载
            callback = parameter;
            parameter = {};
        } else {
            parameter = parameter || {};
            callback = callback || function () { };
        }
        var options = $.extend({}, defaults, parameter);
        return this.each(function () {
            var pagination = new Pagination(this, options);
            callback(pagination);
        });
    };

}));