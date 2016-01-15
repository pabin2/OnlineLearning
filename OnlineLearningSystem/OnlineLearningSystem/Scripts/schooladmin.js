$(function () {

    //validation username
    $('#username').focusout(function () {
        var myData = {
            username: $('#username').val()

        }
        $.ajax({
            url: '/School/CheckExistingUser',
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

    //deleting teacher
    $('.dltImg').click(function (e) {
        var id = $($(e.currentTarget).parents('.row').find('div')[3]).attr('id');
        $('#teachername').text(name);
        $('#teacherid').text(id);
    })
    $('#deleteselectedTeacher').click(function (e) {
        var myData = {
            id: $('#teacherid').text()
        }
        $.ajax({
            url: '/School/DeleteStudent',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .done(function (data) {
            $('.closemodal').trigger('click');
            $('.modal').hide()
            if (data == 1) {
                $('.confirmBox').trigger('click');
            }
            else {
                $('#messageDisplay').html("Teacher cannot be deleted");
            }
            //hide every 3 seconds
            setTimeout(function () { $('#messageDisplay').hide(); }, 3000);
        });
    })

    //edit teacher
    $('.editImg').click(function (e) {
        debugger;
        var id = $($(e.currentTarget).parents('.row').find('div')[0]).attr('id');
        var password = "";
        var firstname = $($(e.currentTarget).parents('.row').find('div')[1]).text();
        var lastname = $($(e.currentTarget).parents('.row').find('div')[2]).text();
        var username = $($(e.currentTarget).parents('.row').find('div')[3]).text();
        var schoolid = $($(e.currentTarget).parents('.row').find('div')[5]).text();
        $('#editFn').val(firstname);
        $('#editLn').val(lastname);
        $('#editUn').val(username);
        $('#editId').val(id);
        $('#editSid').val(schoolid);
        $('#editPwd').val(password);
        $('#editUn').attr('disabled', 'disabled');
        $('#editUn').attr('title', 'Cannot change id');
    });
    $('#editteacher').click(function (e) {
        var myData = {
            firstname: $('#editFn').val(),
            lastname: $('#editLn').val(),
            username: $('#editUn').val(),
            id: $('#editId').val(),
            schoolid: $('#editSid').val(),
            password: $('#editPwd').val()
        }
        $.ajax({
            url: '/School/EditTeacher',
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
                $('#messageDisplay').html("Teacher cannot be updated");
            }
            //hide every 3 seconds
            setTimeout(function () { $('#messageDisplay').hide(); }, 3000);
        });
    });

    //viewteacher
    $('.viewImg').click(function (e) {
        var id = $($(e.currentTarget).parents('.row').find('div')[0]).attr('id');
        var password = "";
        var firstname = $($(e.currentTarget).parents('.row').find('div')[1]).text();
        var lastname = $($(e.currentTarget).parents('.row').find('div')[2]).text();
        var username = $($(e.currentTarget).parents('.row').find('div')[3]).text();
        var schoolid = $($(e.currentTarget).parents('.row').find('div')[5]).text();
        $('#viewFn').val(firstname);
        $('#viewLn').val(lastname);
        $('#viewUn').val(username);
        $('#viewId').val(id);
        $('#viewSid').val(schoolid);
        $('#viewPwd').val(password);
        $('#viewFn').attr('disabled', 'disabled');
        $('#viewUn').attr('disabled', 'disabled');
        $('#viewLn').attr('disabled', 'disabled');
        $('#viewId').attr('disabled', 'disabled');
        $('#viewSid').attr('disabled', 'disabled');
        $('#viewUn').attr('title', 'Cannot change id');
    });

    //sending msg
    $('.messageIcon').click(function (e) {
        var sentdate = $(e.currentTarget).parents('.row').find('.msgDate').text();
        var subject = $(e.currentTarget).parents('.row').find('.msgBody').text();
        var sender = $(e.currentTarget).parents('.row').find('.msgSender').text();
        var msg = $(e.currentTarget).parents('.row').find('.msgBody').attr('title');
        var sender_id = $(e.currentTarget).parents('.row').find('.msgSender').attr('id');
        $('#messageBody').text(msg);
        $('#messageSender').text(sender);
        $('#messageSender').attr('data-senderid', sender_id);
        $('#messageSubject').text(subject);
        $('#msgDate').text(sentdate);
    });


    //sending msg to selected id
    $('.msgstudentImg').click(function (e) {
        $('#sendMessage').attr('disabled', 'disabled');
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
            url: '/School/MessageView',
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

    //disabling send button when msg is empty
    $('#message_subject').focusout(function () {
        if ($('#message_subject').val() == "") {
            $('#errorMsg').text("*Subject cannot be empty");
            $('#sendMessage').attr('disabled', 'disabled');
        }
        else {
            $('#errorMsg').text("");
            $('#sendMessage').attr('disabled', false);
        }
    })
    $('#message_body').focusout(function () {
        if ($('#message_body').val() == "") {
            $('#errorMsg').text("*Message Body cannot be empty");
            $('#sendMessage').attr('disabled', 'disabled');
        }
        else {
            $('#errorMsg').text("");
            $('#sendMessage').attr('disabled', false);
        }
    })
    //refresh click dialog box
    $('.refreshbutton').click(function (e) {
        location.reload();
    })


    //check box
    $('#checkallteacher').click(function () {
        if ($('input#checkallteacher').prop('checked')) {
            $('input#checkedteacher').prop('checked', true)
        }
        else {
            $('input#checkedteacher').prop('checked', false)
        }

    })

    $('input#checkedteacher').click(function () {
        debugger;
        if ($('input#checkedteacher').length == $('input#checkedteacher:checked').length) {
            $('input#checkallteacher').prop('checked', true);
        }
        else {
            $('input#checkallteacher').prop('checked', false);
        }
    })

    $('#clearmodalbox').click(function () {
        $('.firstname').val("");
        $('.lastname').val("");
        $('.username').val("");
        $('.password').val("");
        $('.schoolid').val("");




    })

    //adding student
    $('#clearmodalbox').click(function () {
        $('.firstname').val("");
        $('.lastname').val("");
        $('.username').val("");
        $('.password').val("");
        $('.schoolid').val("");
    })
    $('#addStudent').click(function () {
        $('.loadingimg').show();
        $('.closemodal').trigger('click');
        var myData = {
            username: $("#username").val(),
            firstname: $("#firstName").val(),
            lastname: $("#lastName").val(),
            password: $("#password").val(),
            schoolid: $("#schoolid").val()
        }
        $.ajax({
            url: '/School/StudentView',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .done(function (data) {
            $('.loadingimg').hide();
            $('.closemodal').trigger('click');
            if (data == 1) {
                $('.confirmBox').trigger('click');
            }
            else {
                $('#messageDisplay').html("Teacher cannot be added");
            }
            //hide every 3 seconds
            setTimeout(function () { $('#messageDisplay').hide(); }, 3000);
        })
        .fail(function (data) {
            $('.loadingimg').hide();
            alert('Failed');
        });
    });



    //clering modal pop up 
    $('body').on('hidden.bs.modal', '.modal', function () {
        $(this).removeData('bs.modal');
    });


});