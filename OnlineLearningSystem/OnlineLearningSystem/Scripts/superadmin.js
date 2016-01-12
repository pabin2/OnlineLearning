$(function () {



    //add school
    $('#addschool').click(function () {
        var schoolname = $('#schoolname').val();
        var location = $('#location').val();
        var contact = $('#contact').val();
        var myData = {
            schoolname: schoolname,
            location:location,
            contact:contact
        }
        $.ajax({
            url: '/SuperAdmin/Addschool',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .done(function (data) {
            $('.closemodal').trigger('click');
            if (data == 1) {
                $('.confirmBox').trigger('click');
            }
            else {
                $('#messageDisplay').html("Error sending message");
            }
            
        })
        .fail(function (data) {
            alert('Failed');
        });
    });

    //validation username
    $('#username').focusout(function () {
        debugger;
        var myData = {
            username: $('#username').val()

        }
        $.ajax({
            url: '/SuperAdmin/CheckExistingUser',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .success(function (data) {
            if (data == 1) {
                $('#messageAddTeacher').html("Valid Username");
                $('.correctuser').show();
                $('.wronguser').hide();
                $('#addteacher').attr('disabled', false);
                $('#username').removeClass('validationfail');
            }
            else {
                $('#messageAddTeacher').html(" Username already exist/is a null user");
                $('#username').addClass('validationfail');
                $('.wronguser').show();
                $('.correctuser').hide();
                $('#addteacher').attr('disabled', 'disabled');
            }

        })
        .fail(function (data) {
            alert("failed");
        });

    });

    //show backup
    $('.showbackupdatabase').click(function () {
        $('.backupdatabase').css('visibility', 'visible');
        $('.backupdatabase').fadeIn("slow");
    })
});