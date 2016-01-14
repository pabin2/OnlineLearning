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

    //sending msg to selected id
    $('.msgsuperImg').click(function (e) {
        var name = $($(e.currentTarget).parents('.row').find('div')[2]).text();
        var id = $($(e.currentTarget).parents('.row').find('div')[2]).attr('id');
        $('#receiver').val(id);
        $('#receiver').attr('disabled', 'disabled');
        $('#receiver').attr('title', 'Cannot change id');
    })
    $('#sendMessage').click(function (e) {

        var myData = {
            message_body: $('#message_body').val(),
            message_subject: $('#message_subject').val(),
            receiver: $('#receiver').val(),
            usertype: $('#usertype').val()
        }
        $.ajax({
            url: '/SuperAdmin/MessageView',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .done(function (data) {
            debugger;
            $('.closemodal').trigger('click');
            if (data == 1) {
                $('.confirmBox').trigger('click');
            }
            else {
                $('#messageDisplay').html("Error sending message");
                //hide every 3 seconds
                setTimeout(function () { $('#messageDisplay').hide(); }, 3000);
            }

        })
        .fail(function (data) {
            alert('Failed');
        });
    })
    $('.loadingimg').hide();


    //sending msg from msg page
    $('#sendMessage1').click(function (e) {
        var myData = {
            message_body: $('#message_body2').val(),
            message_subject: $('#message_subject2').val(),
            receiver: $('#receiver2').val(),
            usertype: $('#usertype2').val()
        }
        $.ajax({
            url: '/School/MessageView',
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
            //hide every 3 seconds
            setTimeout(function () { $('#messageDisplay').hide(); }, 3000);
        })
        .fail(function (data) {
            alert('Failed');
        });
    })

    //replying msg
    $('.replyMessage').click(function (e) {
        var receiver = $('#messageSender').attr('data-senderid');
        $('#receiver1').val(receiver);
        var subject = $('#messageSubject').text();
        $('#message_subject1').val('Re :' + subject);
        subject = $('#message_subject1').val();
        var url = window.location.pathname.split("/");
        var controller = url[1];
        if (controller == "Teacher") {
            url = '/Teacher/MessageView'
        }
        else if (controller == "School") {
            url = '/School/MessageView'
        }
        var myData = {
            message_body: $('#message_body1').val(),
            message_subject: subject,
            receiver: receiver,
            usertype: $('#usertype1').val()
        }
        $.ajax({
            url: url,
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
            //hide every 3 seconds
            setTimeout(function () { $('#messageDisplay').hide(); }, 3000);
        })
        .fail(function (data) {
            alert('Failed');
        });
    })

    $('.refreshbutton').click(function (e) {
        location.reload();
    })
});