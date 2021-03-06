﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace OnlineLearningSystem.Models
{
    public class Sql_connnector
    {
        static string connectionStr = WebConfigurationManager.ConnectionStrings["connectionStr"].ToString();
        SqlConnection con = new SqlConnection(connectionStr);
        public bool open()
        {
            try
            {
                if (con.State.ToString() == "Open")
                {
                    return true;
                }
                else
                {
                    con.Open();
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public bool close()
        {
            try
            {
                if (con.State.ToString() == "Close")
                {
                    return true;
                }
                else
                {
                    con.Close();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        //encrypt password
        private string Encryptpassword(string pass)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(pass);
            string encryptedpassword = Convert.ToBase64String(bytes);
            return encryptedpassword;

        }
        //decrypt password
        private string DecryptPaassowrd(string pass)
        {
            byte[] bytes = Convert.FromBase64String(pass);
            string decryptedpassword = System.Text.Encoding.Unicode.GetString(bytes);
            return decryptedpassword;
        }
        //reterving information of login user
        public IEnumerable<user_info> getuserdetail(string username, string password, string usertype)
        {
            string decryptedpassword = Encryptpassword(password);
            open();
            string query = "select * from user_info where username ='" + username + "' and password = '" + decryptedpassword + "' and usertype = '" + usertype + "'";
            List<user_info> userdetail = new List<user_info>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    if (usertype == "teacher")
                    {
                        var userdetails = new user_info { id = dr.GetInt32(0), firstName = dr.GetString(2), lastName = dr.GetString(3), username = dr.GetString(1), schoolid = dr.GetInt32(8), courseid = dr.GetInt32(9) };
                        userdetail.Add(userdetails);
                    }
                    else
                    {
                        var userdetails = new user_info { id = dr.GetInt32(0), firstName = dr.GetString(2), lastName = dr.GetString(3), username = dr.GetString(1), schoolid = dr.GetInt32(8)};
                        userdetail.Add(userdetails);
                    }
                }
            }
            return userdetail;
            close();
        }

        //checking if user already exist
        public int getuserdetail(string username)
        {
            open();
            if (username == null)
            {
                return 0;
            }
            string query = "select * from user_info where username ='" + username + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    return 0;
                }
                return 1;
            }
            close();
        }

        //checking if assignment name already exist
        public int getassignmentdetail(string assignmentname)
        {
            open();
            if (assignmentname == null)
            {
                return 0;
            }
            string query = "select * from assignments where assignmentname ='" + assignmentname + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    return 0;
                }
                return 1;
            }
            close();
        }
        //displahing teacher
        public IEnumerable<user_info> displayteacher(int schoolid, bool edit, int id)
        {
            open();
            string query;
            if (edit != true)
            {
                query = "select * from user_info where usertype='teacher' and schoolid=" + schoolid;
            }
            else
            {
                query = "select * from user_info where usertype='teacher' and schoolid=" + schoolid + " and id=" + id;
            }
            List<user_info> lstTeachers = new List<user_info>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var teacher = new user_info { id = dr.GetInt32(0), firstName = dr.GetString(2), lastName = dr.GetString(3), password = dr.GetString(4), username = dr.GetString(1), schoolid = dr.GetInt32(8) };
                    lstTeachers.Add(teacher);
                }
            }
            return lstTeachers;
            close();
        }
        //displaying student
        public IEnumerable<user_info> displaystudent(int schoolid)
        {
            open();
            string query = "select * from user_info where usertype='student' and schoolid='" + schoolid + "'";
            List<user_info> lstStudent = new List<user_info>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var student = new user_info { id = dr.GetInt32(0), firstName = dr.GetString(2), lastName = dr.GetString(3), username = dr.GetString(1), schoolid = dr.GetInt32(8) };
                    lstStudent.Add(student);
                }
            }
            return lstStudent;
            close();
        }
        //adding teacher
        public int Insertuser(user_info teacherdetail, string usertype)
        {
            string password = Encryptpassword(teacherdetail.password);
            open();
            SqlCommand cmd = new SqlCommand("IUD_teacher", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "INSERT");
            cmd.Parameters.AddWithValue("@firstname", teacherdetail.firstName);
            cmd.Parameters.AddWithValue("@lastname", teacherdetail.lastName);
            cmd.Parameters.AddWithValue("@username", teacherdetail.username);
            cmd.Parameters.AddWithValue("@password", password);
            switch (usertype)
            {
                case "teacher":
                    cmd.Parameters.AddWithValue("@usertype", "Teacher");
                    cmd.Parameters.AddWithValue("@course", teacherdetail.courseid);
                    break;
                case "schooladmin":
                    cmd.Parameters.AddWithValue("@usertype", "Schooladmin");
                    cmd.Parameters.AddWithValue("@course",0);
                    break;
                case "student":
                    cmd.Parameters.AddWithValue("@usertype", "Student");
                    cmd.Parameters.AddWithValue("@course", 0);
                    break;
            }
            cmd.Parameters.AddWithValue("@schoolid", teacherdetail.schoolid);
            int res = 0;

            try
            {
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
            return res;
            close();
        }
        //adding student
        public int insertStudent(user_info studentdetail)
        {

            open();
            SqlCommand cmd = new SqlCommand("IUD_teacher", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "INSERT");
            cmd.Parameters.AddWithValue("@firstname", studentdetail.firstName);
            cmd.Parameters.AddWithValue("@lastname", studentdetail.lastName);
            cmd.Parameters.AddWithValue("@username", studentdetail.username);
            cmd.Parameters.AddWithValue("@password", studentdetail.password);
            cmd.Parameters.AddWithValue("@usertype", "Student");
            cmd.Parameters.AddWithValue("@schoolid", studentdetail.schoolid);
            int res = 0;
            res = cmd.ExecuteNonQuery();
            return res;
            close();
        }

        public int insertMessage(message message_info, DateTime sentdate, string sender, string schoolid, string usertype)
        {
            int receiverid;
            if (message_info.usertype != "none")
            {
                receiverid = 0;
            }
            else
            {
                receiverid = message_info.receiver;
            }

            open();
            SqlCommand cmd = new SqlCommand("IDU_Message", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "INSERT");
            cmd.Parameters.AddWithValue("@sender", sender);
            cmd.Parameters.AddWithValue("@receiver", receiverid);
            cmd.Parameters.AddWithValue("@subject", message_info.message_subject);
            cmd.Parameters.AddWithValue("@body", message_info.message_body);
            cmd.Parameters.AddWithValue("@usertype", usertype);
            cmd.Parameters.AddWithValue("@sentdate", sentdate);
            cmd.Parameters.AddWithValue("@schoolid", Int32.Parse(schoolid));
            int res = 0;
            res = cmd.ExecuteNonQuery();
            return res;
            close();


        }

        public IEnumerable<message> displaymessage(int schoolid, int userid, string usertype)
        {
            open();
            string query = null;
            if (usertype == "teacher")
            {
                query = "select msg.*,userinfo.id from message AS msg inner join user_info as userinfo on msg.sender = userinfo.username where msg.receiver=" + userid + "or msg.usertype='" + usertype + "'";
            }
            else
            {
                query = "select msg.*,userinfo.id from message AS msg inner join user_info as userinfo on msg.sender = userinfo.username where msg.receiver=" + userid;
            }
            List<message> messagelist = new List<message>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var message = new message { id = dr.GetInt32(0), sender = dr.GetString(1), message_subject = dr.GetString(5), message_body = dr.GetString(6), sentdate = dr.GetDateTime(4), sender_userid = dr.GetInt32(8) };
                    messagelist.Add(message);
                }
            }
            return messagelist;
            close();
        }

        public IEnumerable<message> displaysendmessage(string username)
        {
            open();
            string query = "select msg.*,userinfo.username from message AS msg inner join user_info as userinfo on msg.receiver = userinfo.id where msg.sender='" + username + "'";
            List<message> messagelist = new List<message>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var message = new message { id = dr.GetInt32(0), sender = dr.GetString(1), message_subject = dr.GetString(5), message_body = dr.GetString(6), sentdate = dr.GetDateTime(4), receiver_name = dr.GetString(8) };
                    messagelist.Add(message);
                }
            }
            return messagelist;
            close();
        }
        public int editTeacher(user_info teacherdetail)
        {
            open();
            SqlCommand cmd = new SqlCommand("IUD_teacher", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Update");
            cmd.Parameters.AddWithValue("@id", teacherdetail.id);
            cmd.Parameters.AddWithValue("@firstname", teacherdetail.firstName);
            cmd.Parameters.AddWithValue("@lastname", teacherdetail.lastName);
            cmd.Parameters.AddWithValue("@schoolid", teacherdetail.schoolid);
            var res = 0;
            try
            {
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                string s = Ex.Message;
                return 0;
            }

            return res;
            close();
        }

        public int deleteTeacher(int id)
        {
            open();
            SqlCommand cmd = new SqlCommand("IUD_teacher", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Delete");
            cmd.Parameters.AddWithValue("@id", id);

            int res = 0;
            res = cmd.ExecuteNonQuery();
            return res;
            close();
        }

        //get assignmnet
        public IEnumerable<Assignments> displayassignment(int schoolid, int id)
        {
            open();
            string query;

            query = "select * from assignments where schoolid=" + schoolid + " and teacherid=" + id;

            List<Assignments> lstassignment = new List<Assignments>();
            SqlCommand cmd = new SqlCommand(query, con);

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var assignment = new Assignments { id = dr.GetInt32(0), name = dr.GetString(1), startdate = dr.GetDateTime(2), enddate = dr.GetDateTime(3), Question1 = dr.GetString(9), Question2 = dr.GetString(10), Question3 = dr.GetString(11), Question4 = dr.GetString(12), Question5 = dr.GetString(13) };
                    lstassignment.Add(assignment);
                }
            }
            return lstassignment;
            close();
        }

        //insert assignment
        public int insertAssignment(Assignments assignment, DateTime startdate, int schoolid, int id)
        {
            open();
            SqlCommand cmd = new SqlCommand("IDU_Assignment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "INSERT");
            cmd.Parameters.AddWithValue("@name", assignment.name);
            cmd.Parameters.AddWithValue("@resources", assignment.resources);
            cmd.Parameters.AddWithValue("@description", assignment.description);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", assignment.enddate);
            cmd.Parameters.AddWithValue("@schoolid", schoolid);
            cmd.Parameters.AddWithValue("@teacherid", id);
            if (assignment.Question1 == null)
            {
                assignment.Question1 = "None";
            }
            if (assignment.Question2 == null)
            {
                assignment.Question2 = "None";
            }
            if (assignment.Question3 == null)
            {
                assignment.Question3 = "None";
            }
            if (assignment.Question4 == null)
            {
                assignment.Question4 = "None";
            }
            if (assignment.Question5 == null)
            {
                assignment.Question5 = "None";
            }
            cmd.Parameters.AddWithValue("@question1", assignment.Question1);
            cmd.Parameters.AddWithValue("@question2", assignment.Question2);
            cmd.Parameters.AddWithValue("@question3", assignment.Question3);
            cmd.Parameters.AddWithValue("@question4", assignment.Question4);
            cmd.Parameters.AddWithValue("@question5", assignment.Question5);
            int res = 0;
            res = cmd.ExecuteNonQuery();
            return res;
            close();


        }
        public int assignAssignment(int userid, int assignmentid)
        {

            open();
            string query = "Insert into assignmentsStudentList(studentid,assignmentid) values(" + userid + "," + assignmentid + ")";
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            close();

            return res;
        }

        //display detail assignment for student 
        public IEnumerable<Assignments> Displayassignmentforstudent(int schoolid, int userid, string name)
        {
            open();
            string querychk = "select assignment.*,response.* from assignments AS assignment inner join assignmentresponse as response on assignment.id = response.assignmentid where assignment.assignmentname='" + name + "'";
            SqlCommand cmd1 = new SqlCommand(querychk, con);
            string query = null;
            List<Assignments> assignments = new List<Assignments>();
            using (SqlDataReader dr1 = cmd1.ExecuteReader())
            {

                SqlCommand cmd = new SqlCommand(query, con);
                if (dr1.HasRows)
                {

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    open();
                    query = "select assignment.*,assignmentStd.*,response.* from assignments AS assignment inner join assignmentsStudentList as assignmentStd on assignment.id = assignmentStd.assignmentid  inner join assignmentresponse as response on assignment.id = response.assignmentid where assignmentStd.studentid=" + userid;
                    cmd.CommandText = query;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var assignment = new Assignments { id = dr.GetInt32(0), name = dr.GetString(1), startdate = dr.GetDateTime(2), enddate = dr.GetDateTime(3), resources = dr.GetString(8), description = dr.GetString(6), Question1 = dr.GetString(9), Question2 = dr.GetString(10), Question3 = dr.GetString(11), Question4 = dr.GetString(12), Question5 = dr.GetString(13), Submitted = dr.GetInt32(24), Answer1 = dr.GetString(19), Answer2 = dr.GetString(20), Answer3 = dr.GetString(21), Answer4 = dr.GetString(22), Answer5 = dr.GetString(23) };
                            assignments.Add(assignment);
                        }
                    }

                }

                else
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    open();
                    query = "select assignment.*,assignmentStd.* from assignments AS assignment inner join assignmentsStudentList as assignmentStd on assignment.id = assignmentStd.assignmentid   where assignmentStd.studentid=" + userid;
                    cmd.CommandText = query;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var assignment = new Assignments { id = dr.GetInt32(0), name = dr.GetString(1), startdate = dr.GetDateTime(2), enddate = dr.GetDateTime(3), resources = dr.GetString(8), description = dr.GetString(6), Question1 = dr.GetString(9), Question2 = dr.GetString(10), Question3 = dr.GetString(11), Question4 = dr.GetString(12), Question5 = dr.GetString(13) };
                            assignments.Add(assignment);
                        }
                    }
                }
            }



            return assignments;
            close();
        }

        // all assign assignment
        public IEnumerable<Assignments> Displayassignment(int schoolid, int userid)
        {
            open();
            string query = "select assignment.*,assignmentStd.* from assignments AS assignment inner join assignmentsStudentList as assignmentStd on assignment.id = assignmentStd.assignmentid where assignmentStd.studentid=" + userid;
            List<Assignments> assignments = new List<Assignments>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var assignment = new Assignments { id = dr.GetInt32(0), name = dr.GetString(1), startdate = dr.GetDateTime(2), enddate = dr.GetDateTime(3), resources = dr.GetString(8), description = dr.GetString(6), Question1 = dr.GetString(9), Question2 = dr.GetString(10), Question3 = dr.GetString(11), Question4 = dr.GetString(12), Question5 = dr.GetString(13) };
                    assignments.Add(assignment);
                }
            }
            return assignments;
            close();
        }
        public int StudentResponse(StudentResponse response)
        {
            if (response.answer1 == null)
            {
                response.answer1 = "None";
            }
            if (response.answer2 == null)
            {
                response.answer2 = "None";
            }
            if (response.answer3 == null)
            {
                response.answer3 = "None";
            }
            if (response.answer4 == null)
            {
                response.answer4 = "None";
            }
            if (response.answer5 == null)
            {
                response.answer5 = "None";
            }
            open();
            string querycheck = "Select * from assignmentresponse where assignmentid =" + response.assignmentid;
            SqlCommand cmdcheck = new SqlCommand(querycheck, con);
            string query = null;
            using (SqlDataReader dr = cmdcheck.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    query = "UPDATE assignmentresponse SET answer1='" + response.answer1 + "',answer2='" + response.answer2 + "',answer3='" + response.answer3 + "',answer4='" + response.answer4 + "',answer5='" + response.answer5 + "' where assignmentid = " + response.assignmentid;
                }
                else
                {
                    query = "Insert into assignmentresponse(assignmentid,answer1,answer2,answer3,answer4,answer5,submited) values('" + response.assignmentid + "','" + response.answer1 + "','" + response.answer2 + "','" + response.answer3 + "','" + response.answer4 + "','" + response.answer5 + "',0)";
                }

            }




            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            close();

            return res;
        }

        public int SubmitAssignment(int id)
        {
            open();
            string query = "UPDATE assignmentresponse SET submited=1 WHERE assignmentid=" + id;
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            close();
            return res;

        }

        //adding school
        public int InsertSchool(School schooldetail)
        {

            open();
            SqlCommand cmd = new SqlCommand("idu_school", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "INSERT");
            cmd.Parameters.AddWithValue("@schoolname", schooldetail.SchoolName);
            cmd.Parameters.AddWithValue("@location", schooldetail.Location);
            cmd.Parameters.AddWithValue("@contact", schooldetail.Contact);
            int res = 0;

            try
            {
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
            return res;
            close();
        }

        public int EditSchool(School schooldetail)
        {

            open();
            SqlCommand cmd = new SqlCommand("idu_school", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Update");
            cmd.Parameters.AddWithValue("@location", schooldetail.Location);
            cmd.Parameters.AddWithValue("@contact", schooldetail.Contact);
            cmd.Parameters.AddWithValue("@id", schooldetail.id);
            int res = 0;

            try
            {
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                res = 0;
            }
            return res;
            close();
        }

        public int DeleteSchool(School schooldetail)
        {

            open();
            SqlCommand cmd = new SqlCommand("idu_school", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Delete");
            cmd.Parameters.AddWithValue("@id", schooldetail.id);
            int res = 0;

            try
            {
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                res = 0;
            }
            return res;
            close();
        }

        //listing school
        public IEnumerable<School> Getallschool()
        {
            open();
            string query = "select * from school";
            List<School> schoollist = new List<School>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var school = new School { id = dr.GetInt32(0), SchoolName = dr.GetString(1), Location = dr.GetString(2), Contact = dr.GetInt64(3) };
                    schoollist.Add(school);

                }

            }
            return schoollist;
            close();
        }

        //backup
        public int Backup()
        {
            open();

            string query = "Backup database OnlineLearningSystem To Disk ='F:\\myTuts\\mvc\\new1.bak'";
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            return res;
            close();
        }

        public int Restore()
        {
            open();
            string query = "restore database OnlineLearningSystem from disk ='F:\\myTuts\\mvc\\new1.bak'";
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            return res;
            close();
        }

        public int Insertreviewteacher(int userid, int teacherid, int stars)
        {

            open();
            //checking if already rated
            string query1 = "select * from review where studentid=" + userid + "and teacherid=" + teacherid;
            SqlCommand cmdcheck = new SqlCommand(query1, con);
            string query = null;
            using (SqlDataReader dr = cmdcheck.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    query = "Update review set stars=" + stars + "where studentid=" + userid + "and teacherid=" + teacherid;
                }
                else
                {
                    query = "Insert into review(studentid,teacherid,stars) values(" + userid + "," + teacherid + "," + stars + ")";
                }

            }
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            close();

            return res;


        }

        //course detail
        public IEnumerable<Coursedetail> Detailcourse()
        {
            open();
            string query = "select course.*,coursedetail.* from subcourse as course inner join subcourse as coursedetail on course.id = coursedetail.courseid";
            List<Coursedetail> coursedetail = new List<Coursedetail>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var coursedetails = new Coursedetail { coursename = dr.GetString(1), title1 = dr.GetString(4), title1_detail = dr.GetString(5), title2 = dr.GetString(6), title2_detail = dr.GetString(7), title3 = dr.GetString(8), title3_detail = dr.GetString(9), title4 = dr.GetString(10), title4_detail = dr.GetString(11), title5 = dr.GetString(12), title5_detail = dr.GetString(13) };
                    coursedetail.Add(coursedetails);
                }
            }
            return coursedetail;
            close();
        }
        //adding courses
        public int Addcourses(subject subject)
        {

            open();
            string query = "Insert into subject(course) values('"+subject.subjectname+"')";
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            close();

            return res;
        }

        public IEnumerable<subject> Listcourses()
        {
            open();
            string query = "select * from course";
            List<subject> courses = new List<subject>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var course = new subject { subjectid =dr.GetInt32(0),subjectname =dr.GetString(1)};
                    courses.Add(course);
                }
            }
            return courses;
            close();
        }
        //add topic
        public int AddTopic(Coursedetail topic,int courseid,int userid)
        {

            open();
            string query = "Insert into subcourse(courseid,title,t1,t1_detail,t2,t2_detail,t3,t3_detail,t4,t4_detail,t5,t5_detail,teacherid) values(" + courseid + ",'" + topic.topictitle + "','" + topic.title1 + "','" + topic.title1_detail + "','" + topic.title2 + "','" + topic.title2_detail + "','" + topic.title3 + "','" + topic.title3_detail + "','" + topic.title4 + "','" + topic.title4_detail + "','" + topic.title5 + "','" + topic.title5_detail + "'," + userid + ")";
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            close();

            return res;
        }

        public int UpdateTopic(Coursedetail topic, int courseid, int userid)
        {

            open();
            string query = "Update subcourse set t1='" + topic.title1 + "',t1_detail='" + topic.title1_detail + "',t2='" + topic.title2 + "',t2_detail='" + topic.title3_detail + "',t4='" + topic.title4 + "',t4_detail='" + topic.title4_detail + "',t5='" + topic.title5 + "',t5_detail='" + topic.title5_detail + "' where teacherid="+userid;
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            close();

            return res;
        }
        //topic detail
        public IEnumerable<Coursedetail> topic(int userid)
        {
            open();
            string query = "select * from subcourse where teacherid="+userid;
            List<Coursedetail> subcourse = new List<Coursedetail>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var subcourses = new Coursedetail { topictitle = dr.GetString(12), title1 = dr.GetString(2), title1_detail = dr.GetString(3), title2 = dr.GetString(4), title2_detail = dr.GetString(5), title3 = dr.GetString(6), title3_detail = dr.GetString(7), title4 = dr.GetString(8), title4_detail = dr.GetString(9), title5 = dr.GetString(10), title5_detail = dr.GetString(11) };
                    subcourse.Add(subcourses);
                }
            }
            return subcourse;
            close();
        }
        public int CheckIfAddedTopic(int userid)
        {
            open();
            if (userid == null)
            {
                return 0;
            }
            string query = "select * from subcourse where teacherid =" + userid ;
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    return 0;
                }
                return 1;
            }
            close();
        }

        //student view sub topic 
        public IEnumerable<Coursedetail> Selectsubtopics(int courseid)
        {
            open();
            string query = "select * from subcourse where courseid=" + courseid;
            List<Coursedetail> subcourse = new List<Coursedetail>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var subcourses = new Coursedetail { id = dr.GetInt32(0),topictitle = dr.GetString(12), title1 = dr.GetString(2), title1_detail = dr.GetString(3), title2 = dr.GetString(4), title2_detail = dr.GetString(5), title3 = dr.GetString(6), title3_detail = dr.GetString(7), title4 = dr.GetString(8), title4_detail = dr.GetString(9), title5 = dr.GetString(10), title5_detail = dr.GetString(11) };
                    subcourse.Add(subcourses);
                }
            }
            return subcourse;
            close();
        }


        //checking if student already took the course
        public int CheckingStudentcourse(int subcourseid,int studentid)
        {
            open();
            string query = "select * from student_course where studentid=" + studentid +"and subjectdetailid="+subcourseid;
            List<Coursedetail> subcourse = new List<Coursedetail>();
            SqlCommand cmd = new SqlCommand(query, con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    return 0;
                }
                else {
                    return 1;
                }
            }
            return 0;
            close();
        }

        //chose courses
        public int SelectCourse(int subcourseid, int studentid)
        {

            open();
            string query = "Insert into student_course(subjectdetailid,studentid) values(" + subcourseid + "," + studentid + ")";
            SqlCommand cmd = new SqlCommand(query, con);
            var res = cmd.ExecuteNonQuery();
            close();

            return res;
        }
    }
}