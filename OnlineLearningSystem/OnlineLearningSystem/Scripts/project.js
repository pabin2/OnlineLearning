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