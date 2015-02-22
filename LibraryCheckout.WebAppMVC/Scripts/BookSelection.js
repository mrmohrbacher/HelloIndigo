// -------------------------------------------------------------------------------
// BookSelection.js
//
// -------------------------------------------------------------------------------
var validator;

var states = [
{ label: "Alaska", value: "AK" },
{ label: "Alabama", value: "AL" },
{ label: "Arkansas", value: "AR" },
{ label: "Arizona", value: "AZ" },
{ label: "California", value: "CA" },
{ label: "Colorado", value: "CO" },
{ label: "Connecticut", value: "CT" },
{ label: "Delaware", value: "DE" },
{ label: "D.C.", value: "DC" },
{ label: "Florida", value: "FL" },
{ label: "Georgia", value: "GA" },
{ label: "Hawaii", value: "HI" },
{ label: "Idaho", value: "ID" },
{ label: "Illinois", value: "IL" },
{ label: "Indiana", value: "IN" },
{ label: "Iowa", value: "IA" },
{ label: "Kansas", value: "KS" },
{ label: "Kentucky", value: "KY" },
{ label: "Louisiana", value: "LA" },
{ label: "Maine", value: "ME" },
{ label: "Maryland", value: "MD" },
{ label: "Massachusetts", value: "MA" },
{ label: "MI : Michigan", value: "MI" },
{ label: "Minnestoa", value: "MN" },
{ label: "Mississippi", value: "MS" },
{ label: "Missouri", value: "MO" },
{ label: "Montana", value: "MT" },
{ label: "New England", value: "NE" },
{ label: "Nevada", value: "NV" },
{ label: "New Hampshire", value: "NH" },
{ label: "New Jersey", value: "NJ" },
{ label: "New Mexico", value: "NM" },
{ label: "New York", value: "NY" },
{ label: "North Carolina", value: "NC" },
{ label: "North Dakota", value: "ND" },
{ label: "Ohio", value: "OH" },
{ label: "Oklahoma", value: "OK" },
{ label: "Oregon", value: "OR" },
{ label: "Pennsylvania", value: "PA" },
{ label: "Rhode Island", value: "RI" },
{ label: "South Carolina", value: "SC" },
{ label: "South Dakota", value: "SD" },
{ label: "Tennessee", value: "TN" },
{ label: "Texas", value: "TX" },
{ label: "Utah", value: "UT" },
{ label: "Vermont", value: "VT" },
{ label: "Virgina", value: "VA" },
{ label: "Washington", value: "WA" },
{ label: "West Virginia", value: "WV" },
{ label: "Wisconsin", value: "WI" },
{ label: "Wyoming", value: "WY" }
];

function showPanel(panel) {
   var selector = '#order-entry-menu > li > a[href=' + panel + ']';
   $(selector).click();
}

function checkoutFail(response, url) {
    $('#email-review').show()
    $('#email-confirmation').text('Checkout Failed! ('
                                    + response.status
                                    + ') ' + response.statusText
                                    + ' ' + url);
}

function checkoutResults(data, status, response) {
    // Results returned an HTML page; replace existing body with new results
    if (data.search(/<html/i) != -1) {
        $('#main').html($(data).find('#main').html());
    } else if (response.status == 200) {
        $('#email-review').show()
        $('#email-confirmation').text(data);
    }
}

$(document).ready(function () {

    window.console && console.log('+ document.ready +');

    // Hide all panels
    $('.oe-panel').hide();

    $(document).unload(function () {
        window.console && console.log('document.unload');
        $('#current-tab').val('');
    }
   );

    //--------------------------------------------------------
    // Define functors to react to 'click' events
    //--------------------------------------------------------
    $('#order-entry-menu > li > a').click(function () {
        // Stash computed selector to invoking element.
        var $this = $(this);

        //    Stash the current UI state into a hidden vbl.
        var panel = $this.attr('href');
        window.console && console.log("+ Panel '%s' +", panel);

        //------------------------------------------------
        // Force the User to review all of the panels
        if (panel == '#confirmation') {

            //------------------------------------------------
            // #book-selection
            if ($('#isbn').val() == null || $('#isbn').val() == "notpicked") {
                $('#choice-book-none').click();
                $('#choice-book-none').attr('checked', 'checked');
                showPanel('#book-selection');
                return;
            }

            //------------------------------------------------
            // 0) Validate form entries 
            //
            // *NOTE*   By returning without doing the rest of the code,
            //          we keep the user nailed to the current tab.
            // #information
            if (validator && !validator.form()) {
                showPanel('#information');
                return;
            }

            if ($("#name").val() == "") {
                showPanel('#information');
                return;
            }
        }

        $('#current-tab').val(panel);

        // 1) Hide all panels
        $('.oe-panel').hide();
        $('#order-entry-menu > li > a.active').removeClass('active');
        $('#order-entry-menu > li.active').removeClass('active');

        // Hide Email Confirmation black
        $('#email-review').hide();

        // 2) Activate the current selection
        $this.parent().addClass('active');
        $this.addClass('active').blur();

        $(panel).fadeIn(250);

        window.console && console.log("- Panel '%s' -", panel)

        // 3) Suppress following the <a> link
        return false;
    }
   ); // End of '#order-entry-menu > li > a' click


    // -------------------------------------------------------
    // Entering 'Confirm Order' panel; build from data content 
    // of previous panels.
    // -------------------------------------------------------
    $('#order-entry-menu > li > a[href=#confirmation]').click(function () {
        // Stash computed selector to invoking element.
        window.console && console.log('+ confirmation +');

        var $this = $(this);

        // 'Book Selection' Review
        var reviewDiv = $('#book-selection-review');
        var bookISBN = $('#isbn').val();
        var bookTitle = $('#title').val();
        reviewDiv.html("<label class='oe-label'>Book selected:</label>&nbsp;" +
               "<label>" +
               bookISBN +
               " - " + bookTitle +
               "</label>");

        var shippingReview = $('#shipping-review');
        shippingReview.html(("<label class='oe-label'>Shipping Information:</label>"
               + "<label class='oe-label'>" + "Name : " + $('#name').val() + "</label>"
               + "<label class='oe-label'>" + "Address : " + $('#address').val() + "</label>"
               + "<label class='oe-label'>" + "City : " + $('#city').val() + " State : " + $('#state').val() + "</label>"
               + "<label class='oe-label'>" + "PostalCode : " + $('#postalcode').val() + "</label>"
               + "<label class='oe-label'>" + "Email : " + $('#email').val() + "</label>"
               ));

        window.console && console.log('- confirmation -');
    });

    // -------------------------------------------------------
    // Stash the Book ISBN choice into a hidden field.
    // -------------------------------------------------------
    $('#book-selection :radio[name=ChoiceBook]').click(function () {
        var $this = $(this);

        var choice = $this.attr('id').slice('choice-book-'.length);
        window.console && console.log('Book: %s', choice);

        $('#isbn').val((choice == 'none') ? '' : choice);

        $('#title').val($('#title-' + choice).html().trim());
    });

    $('#information .oe-text').blur(function () {

        if ($('#name').val() == 'Mike Mohrbacher') {
            $('#address').val('115 N Archimedes Lane');
            $('#city').val('Arlington Heights');
            $('#state').val('IL');
            $('#postalcode').val('60004');
            $('#email').val('mike@blackriverinc.com');
        }
    }); // End of '#billing-information .oe-text' blur


    $('.oe-panel .submit').click(function () {
        var formData = $('#order-entry').serialize();
        var postUrl = 'Library/Checkout';
        //var postUrl = 'CheckoutBook.ashx';
        $.post(postUrl, formData, checkoutResults)
            .error(function () { checkoutFail(arguments[0], postUrl); });
    });

    $('.oe-panel .export').click(function () {
        $.get('/Library/Export');
    });

    // Program the State Entry AutoComplete
    $("#state").autocomplete({ source: states });

    // Attach jquery.validator to order-entry form
    validator = $('#order-entry').validate();

    //--------------------------------------------------------
    // Activate the 1st navigation tab.
    if ($('#current-tab').val() == null
     || $('#current-tab').val().length == 0)
        showPanel('#welcome');
    else
        showPanel($('#current-tab').val());


    window.console && console.log('- document.ready -');

});                                                                                                 // End of document.ready
   
