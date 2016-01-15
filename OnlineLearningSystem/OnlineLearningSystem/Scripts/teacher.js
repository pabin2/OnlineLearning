$(function () {

    $('#dropdown-menu').slideUp('fast');
    $('#dropdownMenu1').click(function () {
        $('.dropdown-menu').slideToggle(!this.checked);
    });

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

    //sending msg to selected id
    $('.msgstudentImg').click(function (e) {
        var name = $($(e.currentTarget).parents('.row').find('div')[2]).text();
        var id = $($(e.currentTarget).parents('.row').find('div')[2]).attr('id');
        $('#receiver').val(id);
        $('#receiver').attr('disabled', 'disabled');
        $('#receiver').attr('title', 'Cannot change id');
    })
    $('#sendMessage').click(function (e) {
        debugger;
        var myData = {
            message_body: $('#message_body').val(),
            message_subject: $('#message_subject').val(),
            receiver: $('#receiver').val(),
            usertype: $('#usertype').val()
        }
        $.ajax({
            url: '/Teacher/MessageView',
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
    //refresh click dialog box
    $('.refreshbutton').click(function (e) {
        location.reload();
    })

    //edit student
    $('.editstdImg').click(function (e) {
        debugger;
        var id = $($(e.currentTarget).parents('.row').find('div')[2]).attr('id');
        var firstname = $($(e.currentTarget).parents('.row').find('div')[0]).text();
        var lastname = $($(e.currentTarget).parents('.row').find('div')[1]).text();
        var username = $($(e.currentTarget).parents('.row').find('div')[2]).text();
        var schoolid = $($(e.currentTarget).parents('.row').find('div')[4]).text();
        $('#editFn').val(firstname);
        $('#editLn').val(lastname);
        $('#editUn').val(username);
        $('#editId').val(id);
        $('#editSid').val(schoolid);
        $('#editUn').attr('disabled', 'disabled');
        $('#editUn').attr('title', 'Cannot change id');
    });
    $('#editstd').click(function (e) {
        debugger;
        var myData = {
            firstname: $('#editFn').val(),
            lastname: $('#editLn').val(),
            username: $('#editUn').val(),
            id: $('#editId').val(),
            schoolid: $('#editSid').val(),
        }
        $.ajax({
            url: '/Teacher/EditStudent',
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

    //deleting teacher
    $('.dltImg').click(function (e) {
        debugger;

        var id = $($(e.currentTarget).parents('.row').find('div')[2]).attr('id');
        $('#teachername').text(name);
        $('#teacherid').text(id);
    })
    $('#deleteselectedTeacher').click(function (e) {
        var myData = {
            id: $('#teacherid').text()
        }
        $.ajax({
            url: '/Teacher/DeleteStudent',
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
});