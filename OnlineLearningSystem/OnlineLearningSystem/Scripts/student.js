$(function () {

    //student response to assignment
    $('#submitanswer').click(function () {
        var myData = {
            assignmentid : $('.assignmentname').attr('id'),
            answer1: $('#answer1').val(),
            answer2: $('#answer2').val(),
            answer3: $('#answer3').val(),
            answer4: $('#answer4').val(),
            answer5: $('#answer5').val(),
        }
        $.ajax({
            url: '/Student/AssignmentViewDetail',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .success(function (data) {
            if (data == 1) {
                $('.list-group').html("Success");
            }
            else {
                $('#messageDisplay').html("Error sending message");
            }
        })
        .fail(function (data) {
            debugger;
            $('.closemodal').trigger('click');
            location.reload();
        });
    });

    //submit assignment
    $('#confirmsubmitassignment').click(function () {
        var assignmentid = $('.assignmentID').text();
        var myData = {
            assignmentid:assignmentid
        }
        $.ajax({
            url: '/Student/SubmitAssignment',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .success(function (data) {
            $('.closemodal').trigger('click');
        })
        .fail(function (data) {
            debugger;
            $('.closemodal').trigger('click');
            location.reload();
        });
    });

    $('.summitedAssignment').click(function (e) {
        var id = $(e.currentTarget).parents('.row').find('div').attr('id');
        var name = $($($(e.currentTarget).parents('.row').find('div'))[0]).text();
        $('.assignmentName').text(name);
        $('.assignmentID').text(id);
    });
    
    //review teacher
    $('.stars').click(function (e) {
        $('.rating').addClass('hidden')
        var message = $(e.currentTarget).attr('id');
        $('#message').text("You have rated " +  message  + " for this teacher");
    })

    $('#review').click(function (e) {
        $('.rating').removeClass('hidden')
    })
});