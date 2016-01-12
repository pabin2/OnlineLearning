$(function () {
    $('.studentview').click(function () {
        $('.studentview').css('background', 'linear-gradient(45deg, #b2d01a 0%,#b2d01a 100%)');
        $('.studentview').css('border-radius', '20px');
        $('.studentview').css('color', 'black');
        $('.studentview').css("box-shadow", "10px 10px 12px #888888");
    });
    $('.messageview').click(function () {
        $('.messageview').css('background', 'linear-gradient(45deg, #b2d01a 0%,#b2d01a 100%)');
        $('.messageview').css('border-radius', '20px');
        $('.messageview').css('color', 'black');
        $('.messageview').css("box-shadow", "10px 10px 12px #888888");
    });
    $('.assignmentview').click(function () {
        $('.assignmentview').css('background', 'linear-gradient(45deg, #b2d01a 0%,#b2d01a 100%)');
        $('.assignmentview').css('border-radius', '20px');
        $('.assignmentview').css('color', 'black');
        $('.assignmentview').css("box-shadow", "10px 10px 12px #888888");
    });

    $('.addDate ').click(function () {
        $('.editable').editable({
            type: 'date',
            pk: 1,
            name: 'enddate',
            title: 'Select End Date'
        });
    })

    //adding teacher
    $('#teacheradd').click(function () {
        $("#username").val("");
        $("#firstName").val("");
        $("#lastName").val("");
        $("#username").val("");
        $("#password").val("");
        $('#addteacher').attr('disabled', 'disabled');
        $('#messageAddTeacher').html("");
        $('.wronguser').hide();
        $('.correctuser').hide();
    })
    $('#addteacher').click(function () {
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
            url: '/School/TeacherView',
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
            else
            {
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
    $('#myModal #myModalEditTeacher #myModalViewTeacher').on('hidden.bs.modal', function () {
        $("#username").val("");
        $("#firstName").val("");
        $("#lastName").val("");
        $("#username").val("");
        $("#password").val("");
        $('#addteacher').attr('disabled', 'disabled');
        $('#messageAddTeacher').html("");
        $('.wronguser').hide();
        $('.correctuser').hide();
    });

    //deleting teacher
    $('.dltImg').click(function (e) {
        var id = $($(e.currentTarget).parents('.row').find('div')[0]).attr('id');
        var name = $($(e.currentTarget).parents('.row').find('div')[0]).text();
        $('#teachername').text(name);
        $('#teacherid').text(id);
    })
    $('#deleteselectedTeacher').click(function (e) {
        debugger;
        var myData = {
            id: $('#teacherid').text()
        }
        $.ajax({
            url: '/School/DeleteTeacher',
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
        var id = $($(e.currentTarget).parents('.row').find('div')[0]).attr('id');
        var password = $($(e.currentTarget).parents('.row').find('div')[1]).attr('id');
        var firstname = $($(e.currentTarget).parents('.row').find('div')[0]).text();
        var lastname = $($(e.currentTarget).parents('.row').find('div')[1]).text();
        var username = $($(e.currentTarget).parents('.row').find('div')[2]).text();
        var schoolid = $($(e.currentTarget).parents('.row').find('div')[4]).text();
        $('#editFn').val(firstname);
        $('#editLn').val(lastname);
        $('#editUn').val(username);
        $('#editId').val(id);
        $('#editSid').val(schoolid);
        $('#editPwd').val(password);
        $('#editUn').attr('disabled', 'disabled');
        $('#editUn').attr('title','Cannot change id');
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
        var password = $($(e.currentTarget).parents('.row').find('div')[1]).attr('id');
        var firstname = $($(e.currentTarget).parents('.row').find('div')[0]).text();
        var lastname = $($(e.currentTarget).parents('.row').find('div')[1]).text();
        var username = $($(e.currentTarget).parents('.row').find('div')[2]).text();
        var schoolid = $($(e.currentTarget).parents('.row').find('div')[4]).text();
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
    $('.msgImg').click(function (e) {
        $('#sendMessage').attr('disabled', 'disabled');
        var id = $($(e.currentTarget).parents('.row').find('div')[0]).attr('id')
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
        if (controller == "Teacher")
        {
            url = '/Teacher/MessageView'
        }
        else if (controller == "School")
        {
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
        if ($('#message_subject').val() == "")
        {
            $('#errorMsg').text("*Subject cannot be empty");
            $('#sendMessage').attr('disabled', 'disabled');
        }
        else
        {
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

    //uploading file 
    $('.addingAssignment').click(function () {
        var myData = {
            name: $('#assignment_name').val(),
            resources: $('#assignment_resources').val(),
            enddate: $('#enddate').val() + "12:00:00 AM",
            description: $("#description").val()
            , description: $("#description").val()
            , question1: $("#question1").val()
            , question2: $("#question2").val()
            , question3: $("#question3").val()
            , question4: $("#question4").val()
            , question5: $("#question5").val()
        }
        $.ajax({
            url: '/Teacher/AssignmentView',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .done(function () {
            $('.closemodal').trigger('click');

        }).fail(function () {
            $('.closemodal').trigger('click');
        });
    });
    //check box
    $('#checkallteacher').click(function () {
        $('input#checkedteacher').prop('checked', true)
    })

    $('input#checkedteacher').click(function () {
        if ($('input#checkedteacher').length == $('input#checkedteacher:checked').length)
        {
            $('input#checkallteacher').prop('checked', true);
        }
        else {
            $('input#checkallteacher').prop('checked', false);
        }
    })

    $('#checkallstudent').click(function () {
        if ($('input#checkallstudent').prop('checked')) {
            $('input#checkstudent').prop('checked', true);
        }
        else {
            $('input#checkstudent').prop('checked', false);
        }

    })

    $('input#checkstudent').click(function () {
        if ($('input#checkstudent').length == $('input#checkstudent:checked').length) {
            $('input#checkallstudent').prop('checked', true);
        }
        else {
            $('input#checkallstudent').prop('checked', false);
        }
    })

    //assign student post
    $('.assignStudent').click(function () {
        var selectedid = []
        var start = 0;
        var id = 0;
        var selectedstudent = $('#checkstudent:checked').length;
        for (start = 0; start < selectedstudent; start++)
        {
            id = $($($('#checkstudent:checked')[start]).parents('.row').find('div')[3]).attr('id');
            selectedid.push(id);
        }
        var assignmentid = getParameterByName('assignmentid');

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        var myData = {
            name: selectedid,
            aid: assignmentid
        }
        $.ajax({
            url: '/Teacher/AssignStudents',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .done(function () {
            debugger;
            console.log("Success: Files sent!");
        }).fail(function () {
            debugger;
            console.log("An error occurred, the files couldn't be sent!");
        });


    })


    
});