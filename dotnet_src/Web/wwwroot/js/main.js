; (function () {

    'use strict';

    var isMobile = {
        Android: function () {
            return navigator.userAgent.match(/Android/i);
        },
        BlackBerry: function () {
            return navigator.userAgent.match(/BlackBerry/i);
        },
        iOS: function () {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i);
        },
        Opera: function () {
            return navigator.userAgent.match(/Opera Mini/i);
        },
        Windows: function () {
            return navigator.userAgent.match(/IEMobile/i);
        },
        any: function () {
            return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
        }
    };


    var fullHeight = function () {

        if (!isMobile.any()) {
            $('.js-fullheight').css('height', $(window).height());
            $(window).resize(function () {
                $('.js-fullheight').css('height', $(window).height());
            });
        }
    };

    // Parallax
    var parallax = function () {
        $(window).stellar();
    };

    var contentWayPoint = function () {
        var i = 0;
        $('.animate-box').waypoint(function (direction) {

            if (direction === 'down' && !$(this.element).hasClass('animated-fast')) {

                i++;

                $(this.element).addClass('item-animate');
                setTimeout(function () {

                    $('body .animate-box.item-animate').each(function (k) {
                        var el = $(this);
                        setTimeout(function () {
                            var effect = el.data('animate-effect');
                            if (effect === 'fadeIn') {
                                el.addClass('fadeIn animated-fast');
                            } else if (effect === 'fadeInLeft') {
                                el.addClass('fadeInLeft animated-fast');
                            } else if (effect === 'fadeInRight') {
                                el.addClass('fadeInRight animated-fast');
                            } else {
                                el.addClass('fadeInUp animated-fast');
                            }

                            el.removeClass('item-animate');
                        }, k * 100, 'easeInOutExpo');
                    });

                }, 50);

            }

        }, { offset: '85%' });
    };



    var goToTop = function () {

        $('.js-gotop').on('click', function (event) {

            event.preventDefault();

            $('html, body').animate({
                scrollTop: $('html').offset().top
            }, 500, 'easeInOutExpo');

            return false;
        });

        $(window).scroll(function () {

            var $win = $(window);
            if ($win.scrollTop() > 200) {
                $('.js-top').addClass('active');
            } else {
                $('.js-top').removeClass('active');
            }

        });

    };

    var pieChart = function () {
        $('.chart').easyPieChart({
            scaleColor: false,
            lineWidth: 4,
            lineCap: 'butt',
            barColor: '#000000',
            trackColor: "#f5f5f5",
            size: 160,
            animate: 1000
        });
    };

    var skillsWayPoint = function () {
        if ($('#fh5co-skills').length > 0) {
            $('#fh5co-skills').waypoint(function (direction) {

                if (direction === 'down' && !$(this.element).hasClass('animated')) {
                    setTimeout(pieChart, 400);
                    $(this.element).addClass('animated');
                }
            }, { offset: '90%' });
        }

    };


    // Loading page
    var loaderPage = function () {
        $(".fh5co-loader").fadeOut("slow");
    };

    var contactForm = function () {
        if ($('#contactForm').length > 0) {
            $("#contactForm").validate({
                rules: {
                    fname: "required",
                    lname: "required",
                    email: {
                        required: true,
                        email: true
                    },
                    message: {
                        required: true,
                        minlength: 5
                    }
                },
                messages: {
                    fname: "Please enter your firstname",
                    lname: "Please enter your lastname",
                    email: "Please enter a valid email address",
                    message: "Please enter a message"
                },
                /* submit via ajax */
                submitHandler: function (form) {
                    var $submit = $('.submitting'),
                        waitText = 'Submitting...';

                    $.ajax({
                        type: "POST",
                        url: "php/sendEmail.php",
                        data: $(form).serialize(),

                        beforeSend: function () {
                            $submit.css('display', 'block').text(waitText);
                        },
                        success: function (msg) {
                            if (msg == 'OK') {
                                $('#form-message-warning').hide();
                                setTimeout(function () {
                                    $('#contactForm').fadeOut();
                                }, 1000);
                                setTimeout(function () {
                                    $('#form-message-success').fadeIn();
                                }, 1400);

                            } else {
                                $('#form-message-warning').html(msg);
                                $('#form-message-warning').fadeIn();
                                $submit.css('display', 'none');
                            }
                        },
                        error: function () {
                            $('#form-message-warning').html("Something went wrong. Please try again.");
                            $('#form-message-warning').fadeIn();
                            $submit.css('display', 'none');
                        }
                    });
                }

            });
        }
    };


    $(function () {
        contentWayPoint();
        goToTop();
        loaderPage();
        fullHeight();
        parallax();
        // pieChart();
        skillsWayPoint();
        contactForm();
    });


}());