//scrollers for sidebar
(function ($) {
    $(window).load(function () {

        $("#myModal .modal-body").mCustomScrollbar({
            setHeight: 340,
            theme: "minimal-dark"
        });

        $("#accordion .panel-body").mCustomScrollbar({
            setHeight: 300,
            theme: "dark-3"
        });

        $("#myTab .tab-pane").mCustomScrollbar({
            setHeight: 280,
            theme: "inset-2-dark"
        });
    })
})(jQuery);

//CheckBox events
$(document).on('click', '#showLinks', function () {
    $("#linksToggle").toggle("slow");
    $("#linksRadioOrder").toggle("slow");
    $("div#indexOrderDiv").toggle();

    if ($(this).is(':checked') == false) $("input#showRss").attr("disabled", true);
    if ($(this).is(':checked') == true) $("input#showRss").removeAttr("disabled");
});

$(document).on('click', '#showRss', function (event) {
    $("#rssToggle").toggle("slow");
    if ($(this).is(':checked') == false) $("input#showLinks").attr("disabled", true);
    if ($(this).is(':checked') == true) $("input#showLinks").removeAttr("disabled");
});


function AddPagedRssNewsContent(data) {
    $("#indexRss").html(data);
};

function AddPagedContent(data) {
    $("#pagedContentContainer").html(data);
};

function AddContent(data) {
    $("#bodyContainer").html(data);
};

//  ----- click Events -----

//get partial view of all content
$(document).on('click', '.sidebar-allcontent-btn', function (event) {
    $.get($(event).attr('id'), AddContent);
    });

//get partial view of category editing
$(document).on('click', '.sidebar-editcategories-btn', function () {
    $.get("/InoReader/AllCategories", function (data) {
        AddContent(data);
    });

});

//get partial view of tags
$(document).on('click', '.sidebar-tags-btn', function () {

    $.get("/InoReader/AllTags", function (data) {
        AddContent(data);
    });

});

//get partial view of Rss content
$(document).on('click', '.sidebar-rss-btn', function () {
    $.get("/InoReader/AllContent", function (data) {
        AddContent(data);
    });
});

//get partial view of link adding page
$(document).on('click', '#btn-add-link', function () {
    $.get("/InoReader/AddLink", function (data) {
        AddContent(data);
    });
});

//get partial view of Rss canal adding page
$(document).on('click', '#btn-subscribe', function () {
    $.get("/InoReader/SubscribeRss", function (data) {
        AddContent(data);
    });
});




//PaginationEvent
$(document).on('click', '.paginationButton', function () {
    $.get($(this).attr('id'), AddPagedContent);
});

//PaginationEvent
$(document).on('click', '.paginationRssNewsButton', function () {
    $.get($(this).attr('id'), AddPagedRssNewsContent);
});
