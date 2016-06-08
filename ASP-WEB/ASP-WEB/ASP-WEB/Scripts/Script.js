
var toTranslate, targetLanguage,translated, userLanguage;
function GetSelectedLanguage() {
    return google.translate.TranslateElement().f;
}
function TranslateForSearch(toTranslate, targetLanguage, userLanguage) {
    jQuery(document).ready(function () {
        jQuery.ajax({
            url: "https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20140720T191145Z.05605441c6ee16dc.eaaf6c6c8690cb5fb094cea2bfec4f787af6170c&lang=" + userLanguage + "-" + targetLanguage + "&text= " + toTranslate + "&#8221",
            dataType: 'jsonp',
            success: function (data) {
                translated = data.text;
                alert("translated: " + translated);
            }
        });
    });
}
$(document).ready(function () {
    $("#btnSearch").click(function () {
        toTranslate = $('#textSearch').val().toString();
        targetLanguage = 'nl';
        userLanguage = GetSelectedLanguage();
        TranslateForSearch(toTranslate, targetLanguage, userLanguage);
    });
    $('#nav > li > a').click(function () {
        if ($(this).attr('class') != 'active') {
            $('#nav li ul').slideUp();
            $(this).next().slideToggle();
            $('#nav li a').removeClass('active');
            $(this).addClass('active');
        }
        else {
            $('#nav li a').removeClass('active');
            $('#nav li ul').slideUp();
        }
    });
    $(".dropdown").on("show.bs.dropdown", function (event) {
        var x = $(event.relatedTarget).text(); // Get the button text
    });
});
function googleTranslateElementInit() {
    new google.translate.TranslateElement({ pageLanguage: 'nl', layout: google.translate.TranslateElement.InlineLayout.VERTICAL }, 'google_translate_element');
}
var $el, $ps, $up, totalHeight;

$(".sidebar-box .button").click(function () {

    totalHeight = 0

    $el = $(this);
    $p = $el.parent();
    $up = $p.parent();
    $ps = $up.find("p:not('.read-more')");

    // measure how tall inside should be by adding together heights of all inside paragraphs (except read-more paragraph)
    $ps.each(function () {
        totalHeight += $(this).outerHeight();
    });

    $up
      .css({
          // Set height to prevent instant jumpdown when max height is removed
          "height": $up.height(),
          "max-height": 9999
      })
      .animate({
          "height": totalHeight
      });

    // fade out read-more
    $p.fadeOut();

    // prevent jump-down
    return false;

});