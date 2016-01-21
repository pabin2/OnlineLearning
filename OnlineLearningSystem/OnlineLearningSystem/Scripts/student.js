$(function () {
    $(document).ready(function () {
        $('[data-toggle="popover"]').popover();
    });
    //student response to assignment
    $('#submitanswer').click(function () {
        var myData = {
            assignmentid: $('.assignmentname').attr('id'),
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
            $('.closemodal').trigger('click');
            location.reload();
        });
    });

    //submit assignment
    $('#confirmsubmitassignment').click(function () {
        var assignmentid = $('.assignmentID').text();
        var myData = {
            assignmentid: assignmentid
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
        $('.ratingreview').addClass('hidden')
        var message = $(e.currentTarget).attr('id');
        var teachername = $($(e.currentTarget).parents('.row').parent().find('div')[3]).text();
        var teacherid = $($(e.currentTarget).parents('.row').find('div')[2]).attr('id');
        var myData = {
            teacherid: teacherid,
            stars: message
        }
        $.ajax({
            url: '/Student/Reviewteacher',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .success(function (data) {
            $('#message').text("You have rated " + message + " for " + teachername);
        })

    })

    $('.review').click(function (e) {
        $('.ratingreview').addClass('hidden');
        $(e.currentTarget).parents('.row').parent().find('.ratingreview').removeClass('hidden');
    })

    //refrest
    $('.refreshbutton').click(function (e) {
        location.reload();
    })
    //message
    //sending msg to selected id
    $('.msgstdImg').click(function (e) {
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
            url: '/Student/MessageView',
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
                //hide every 3 seconds
                setTimeout(function () { $('#messageDisplay').hide(); }, 3000);
            }

        })
        .fail(function (data) {
            alert('Failed');
        });
    })
    //view student
    $('.viewstudentImg').click(function (e) {
        var id = $($(e.currentTarget).parents('.row').find('div')[2]).attr('id');
        var firstname = $($(e.currentTarget).parents('.row').find('div')[0]).text();
        var lastname = $($(e.currentTarget).parents('.row').find('div')[1]).text();
        var username = $($(e.currentTarget).parents('.row').find('div')[2]).text();
        var schoolid = $($(e.currentTarget).parents('.row').find('div')[4]).text();
        $('#viewFn').val(firstname);
        $('#viewLn').val(lastname);
        $('#viewUn').val(username);
        $('#viewId').val(id);
        $('#viewSid').val(schoolid);
        $('#viewUn').attr('disabled', 'disabled');
        $('#viewFn').attr('disabled', 'disabled');
        $('#viewLn').attr('disabled', 'disabled');
        $('#viewUn').attr('disabled', 'disabled');
        $('#viewId').attr('disabled', 'disabled');
        $('#viewSid').attr('disabled', 'disabled');
        $('#viewUn').attr('disabled', 'disabled');

    });

    $('.subcourse').click(function (e) {
        $('.learningmodal').trigger('click');
        var titleid = $(e.currentTarget).attr('id');
        var title = $(e.currentTarget).text();
        var title1 = $(e.currentTarget).attr('data-mytitle1');
        var title1_detail = $(e.currentTarget).attr('data-mytitle1_detail');
        var title2 = $(e.currentTarget).attr('data-mytitle2');
        var title2_detail = $(e.currentTarget).attr('data-mytitle2_detail');
        var title3 = $(e.currentTarget).attr('data-mytitle3');
        var title3_detail = $(e.currentTarget).attr('data-mytitle3_detail');
        var title4 = $(e.currentTarget).attr('data-mytitle4');
        var title4_detail = $(e.currentTarget).attr('data-mytitle4_detail');
        var title5 = $(e.currentTarget).attr('data-mytitle5');
        var title5_detail = $(e.currentTarget).attr('data-mytitle5_detail');
        $('.titleid').text(titleid);
        $('.titleName').text(title);
        $('.title1').text(title1);
        $('.title1_detail').text(title1);
        $('.title2').text(title2);
        $('.title2_detail').text(title2_detail);
        $('.title3').text(title3);
        $('.title3_detail').text(title3_detail);
        $('.title4').text(title4);
        $('.title4_detail').text(title4_detail);
        $('.title5').text(title5);
        $('.title5_detail').text(title5_detail);


    })

    //take course
    $('#takecourse').click(function (e) {
        var subcourseid = $($(e.currentTarget).parent().parent().find('ul').find('li').find('div')[1]).text();
        var myData = {
            id: subcourseid
        }
        $.ajax({
            url: '/Student/SelectedCourse',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(myData)
        })
        .success(function (data) {
            $('.closemodal').trigger('click');
            if (data == 1) {
                $('.confirmBox').trigger('click');
            }
            else {
                $('.confirmBox').trigger('click');
            }

        })
        .fail(function (data) {
            debugger;
            $('.closemodal').trigger('click');
            location.reload();
        });
    });
});