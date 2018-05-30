
$(function () {

    GoTop("body");

    $(".Dontfriendly").click(function () {
        alert("点我干什么");
    });
    $(".btn-submit").on("click", function () {
        alert("提交之后");
        this.attr("disabled='disabled'");
    });


});
/*
滑动到顶部代码
@param calss 的值
*/
function GoTop(className) {
    var slideToTop = $("<div />");
    slideToTop.html('<i class="fa fa-chevron-up"></i>');
    slideToTop.css({
        position: 'fixed',
        bottom: '20px',
        right: '25px',
        width: '40px',
        height: '40px',
        color: '#eee',
        'font-size': '',
        'line-height': '40px',
        'text-align': 'center',
        'background-color': '#222d32',
        cursor: 'pointer',
        'border-radius': '5px',
        'z-index': '99999',
        opacity: '.7',
        'display': 'none'
    });
    slideToTop.on('mouseenter', function () {
        $(this).css('opacity', '1');
    });
    slideToTop.on('mouseout', function () {
        $(this).css('opacity', '.7');
    });
    $(className).append(slideToTop);
    $(window).scroll(function () {
        if ($(window).scrollTop() >= 150) {
            if (!$(slideToTop).is(':visible')) {
                $(slideToTop).fadeIn(500);
            }
        } else {
            $(slideToTop).fadeOut(500);
        }
    });
    slideToTop.on('click', function () {
       
       
        $('html, body').animate({
            scrollTop: false
        }, 500, "linear");
    });
     
}
 