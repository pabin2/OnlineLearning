$(function () {
    $('.correctuser').hide();
    $('.wronguser').hide();
    $('.addingAssignment').hide();
    //validation assignment name
    $('#assignment_name').focusout(function () {
        var myData = {
            assignmentname: $('#assignment_name').val()

        }
        $.ajax({
            url: '/Teacher/CheckExistingAssignmentName',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .success(function (data) {
            if (data == 1) {

                $('#errormessage').hide();
                $('.correctuser').show();
                $('.wronguser').hide();
                $('.addingAssignment').show();
            }
            else {
                $('#errormessage').html("Username already exist/is a null user");
                $('#successmessage').hide()
                $('#username').addClass('validationfail');
                $('.wronguser').show();
                $('.correctuser').hide();
                $('.addingAssignment').hide();
            }

        })
        .fail(function (data) {
            alert("failed");
        });

    });
    $("#assignment_name").keypress(function (e) {
        //allowing only digits
        if (!(e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57))) {
            $("#errmsg").html("Letters only").show().fadeOut("slow");
            return false;
        }
    });
});