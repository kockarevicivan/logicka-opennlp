let spinnerActive = false;
let $spinner = $('.spinner');
let $logickaResponse = $('.response');
let $trackSpan = $('.input-wrapper > span');

$(document).ready(function(){
    $('input').on('focus', displayBorder);
    $('input').on('focusout', hideBorder);

    $('input').on('keyup', function (e) {
		if(this.value) {
			if(!spinnerActive)
				displaySpinner();
			
			if (e.which == 13) {

                if(this.value.includes('?'))
                    submitQuery(this.value);
                else
                    submitSentence(this.value);

				$(this).val('');
			}
		} else {
			hideSpinner();
		}
	});
});

function displayBorder() {
    $trackSpan.animate({
        left: 0,
        right: 0,
        width: '100%',
        opacity: 1
    }, 200);
}

function hideBorder() {
    $trackSpan.animate({
        left: '50%',
        width: 0,
        opacity: 0
    }, 200);
}

function displaySpinner() {
    $spinner.addClass('active');
	
	spinnerActive = true;
}

function hideSpinner() {
	$spinner.removeClass('active');
	
	spinnerActive = false;
}

function submitSentence(statement) {
    $.ajax({
        url: '/train/submit',
        type: 'POST',
        data: {
            statement: statement
        },
        success: function (result) {
            $logickaResponse.html(result);
            $logickaResponse.fadeIn(200);

            setTimeout(function () {
                hideSpinner();
                $logickaResponse.fadeOut(200);
            }, 2000);
        },
        error: function (jqXHR, textStatus) {
            $logickaResponse.html('Error: ' + textStatus);
            $logickaResponse.fadeIn(200);
            
            setTimeout(function () {
                hideSpinner();
                $logickaResponse.fadeOut(200);
            }, 2000);
        }
    });
}

function submitQuery(query) {
    $.ajax({
        url: '/train/query',
        type: 'POST',
        data: {
            query: query
        },
        success: function (result) {
            $logickaResponse.html(result);
            $logickaResponse.fadeIn(200);

            setTimeout(function () {
                hideSpinner();
                $logickaResponse.fadeOut(200);
            }, 2000);
        },
        error: function (jqXHR, textStatus) {
            $logickaResponse.html('Error: ' + textStatus);
            $logickaResponse.fadeIn(200);

            setTimeout(function () {
                hideSpinner();
                $logickaResponse.fadeOut(200);
            }, 2000);
        }
    });
}