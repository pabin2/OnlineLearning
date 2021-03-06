﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLearningSystem.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;
namespace OnlineLearningSystem.Controllers
{
    [Authorization]
    [Authorization_isTeacher]
    public class TeacherController : Controller
    {
        Sql_connnector sql = new Sql_connnector();

        // GET: /Teacher/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StudentView(string search, int? page)
        {
            if (search == null)
            {

                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                //converted list to IpagedLIst in order to pass IpagedList in model for pagination
                List<user_info> studentlist = sql.displaystudent(schoolid).ToList();
                //added blank student so that while adding student , they dont appears there
                IPagedList<user_info> studentlist1 = studentlist.ToPagedList(page ?? 1, 4);
                return View(studentlist1);
            }
            else
            {
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                //converted list to IpagedLIst in order to pass IpagedList in model for pagination
                List<user_info> studentlist = sql.displaystudent(schoolid).Where(t => t.username.StartsWith(search)).ToList();
                //added blank student so that while adding student , they dont appears there
                IPagedList<user_info> studentlist1 = studentlist.ToPagedList(page ?? 1, 4);
                return View(studentlist1);
            }

        }


        [HttpGet]
        public ActionResult AssignmentView(int? value, int? page)
        {
            try
            {
                if (value == 1)
                {
                    ViewBag.message = "Successfully Uploaded";
                }
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());

                int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
                IPagedList<Assignments> assignmentlist = sql.displayassignment(schoolid, userid).ToList().ToPagedList(page ?? 1, 4);

                return View(assignmentlist);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult AssignmentView(Assignments assignment)
        {
            try
            {
                int userid = Int32.Parse(Session["loggedinusernameid"].ToString());

                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                DateTime startdate = DateTime.Today;
                var result = sql.insertAssignment(assignment, startdate, schoolid, userid);
                return RedirectToAction("AssignmentView", "Teacher");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Login");
            }



        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            int msg = 0;
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
                msg = 1;
            }
            return RedirectToAction("AssignmentView", "Teacher", new { value = msg });
        }


        [HttpGet]
        public ActionResult MessageView(string search, string sortBy, int? page)
        {
            List<message> message = new List<message>();
            //triggering asc and desc
            ViewBag.sortByParam = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            string usertype = Session["loggedinusertype"].ToString();
            if (search == null)
            {

                message = sql.displaymessage(schoolid, userid, usertype).ToList();
            }
            else
            {
                message = sql.displaymessage(schoolid, userid, usertype).Where(m => m.sender.StartsWith(search)).ToList();


            }
            IPagedList<message> message1 = message.ToPagedList(page ?? 1, 4);
            switch (sortBy)
            {
                case "name_desc":
                    message = message.OrderByDescending(s => s.sender).ToList();
                    break;
                case "name_asc":
                    message = message.OrderBy(s => s.sender).ToList();
                    break;
                default:
                    message = message.OrderBy(s => s.sender).ToList();
                    break;
            }
            return View(message1);
        }
        public ActionResult SchoolAdmin()
        {
            return View();
        }

        [HttpPost]
        public int MessageView(message message_info)
        {
            int a = 0;
            int i = 0;
            List<string> listofall = new List<string>();
            if (message_info.usertype == "All")
            {
                listofall.Add("teacher");
                listofall.Add("student");
            }
            else
            {
                listofall.Add(message_info.usertype);
            }
            while (i < listofall.Count)
            {
                string sender = Session["loggedinusername"].ToString();
                string schoolid = Session["loggedinuserschoolid"].ToString();
                DateTime date = DateTime.Now;
                a = sql.insertMessage(message_info, date, sender, schoolid, listofall[i]);
                i++;
            }
            return a;

        }
        public ActionResult SentMessage(string search, string sortBy)
        {
            //triggering asc and desc
            ViewBag.sortByParam = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            List<message> message = new List<message>();
            if (search == null)
            {
                string username = Session["loggedinusername"].ToString();
                message = sql.displaysendmessage(username).ToList();

            }
            else
            {
                string username = Session["loggedinusername"].ToString();
                message = sql.displaysendmessage(username).Where(m => m.receiver_name.StartsWith(search)).ToList(); ;

            }
            switch (sortBy)
            {
                case "name_desc":
                    message = message.OrderByDescending(s => s.receiver_name).ToList();
                    break;
                case "name_asc":
                    message = message.OrderBy(s => s.receiver_name).ToList();
                    break;
                default:
                    message = message.OrderBy(s => s.receiver_name).ToList();
                    break;
            }
            return View(message);
        }

        public ActionResult Downloads(string id)
        {
            if (id == null)
            {
                return RedirectToAction("AssignmentView", "Teacher");
            }
            List<Assignments> assignmentdetaillist;
            int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());

            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            assignmentdetaillist = sql.displayassignment(schoolid, userid).Where(m => m.name == id).ToList();
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/uploads/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles(id + "*.*");
            List<string> items = new List<string>();
            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }
            ViewBag.files = items;

            return View(assignmentdetaillist);
        }

        public FileResult Download(string ImageName)
        {

            return File("~/App_Data/uploads/" + ImageName, System.Net.Mime.MediaTypeNames.Application.Octet);
        }

        public ActionResult AssignStudents(string search, int? page)
        {
            if (search == null)
            {
                var sear1ch = Request.QueryString["assignmentid"];
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                //converted list to IpagedLIst in order to pass IpagedList in model for pagination
                List<user_info> studentlist = sql.displaystudent(schoolid).ToList();
                //added blank student so that while adding student , they dont appears there
                studentlist.Add(new user_info());
                IPagedList<user_info> studentlist1 = studentlist.ToPagedList(page ?? 1, 4);
                return View(studentlist1);
            }
            else
            {
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                //converted list to IpagedLIst in order to pass IpagedList in model for pagination
                List<user_info> studentlist = sql.displaystudent(schoolid).Where(t => t.username.StartsWith(search)).ToList();
                //added blank student so that while adding student , they dont appears there
                studentlist.Add(new user_info());
                IPagedList<user_info> studentlist1 = studentlist.ToPagedList(page ?? 1, 4);
                return View(studentlist1);
            }
        }
        //edit student
        //[HttpGet]
        //public ActionResult EditStudent(int id)
        //{
        //    int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
        //    List<user_info> editStd = new List<user_info>();
        //    editStd = sql.displayteacher(schoolid, true, id).ToList();
        //    return View("editTeacher", editStd);
        //}
        [HttpPost]
        public int EditStudent(user_info userinfo)
        {
            int i = sql.editTeacher(userinfo);
            return i;
        }
        [HttpPost]
        public ActionResult AssignStudents(int[] name, int aid)
        {
            foreach (var studentid in name)
            {
                var res = sql.assignAssignment(studentid, aid);
            }
            return RedirectToAction("AssignmentView", "Teacher");
        }

        [HttpPost]
        public int CheckExistingAssignmentName(string assignmentname)
        {
            var result = sql.getassignmentdetail(assignmentname);
            return result;
        }
        [HttpPost]
        public int DeleteStudent(user_info userinfo)
        {
            int i = sql.deleteTeacher(userinfo.id);
            return i;
        }

        //subject
        [HttpGet]
        public ActionResult SubjectView()
        {
            List<Coursedetail> coursedetail = new List<Coursedetail>();
            int courseid = Int32.Parse(Session["loggedinusercourseid"].ToString());
            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            int result = sql.CheckIfAddedTopic(userid);
            if (result == 1)
            {
                return View(coursedetail);
            }
            else
            {
                //make all data visible

                coursedetail = sql.topic(userid).ToList();
                return View(coursedetail);
            }


        }
        [HttpPost]
        public ActionResult SubjectView(Coursedetail topic)
        {
            int courseid = Int32.Parse(Session["loggedinusercourseid"].ToString());
            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            int result = sql.CheckIfAddedTopic(userid);
            int addtopic = 0;
            if (result == 1)
            {
                addtopic = sql.AddTopic(topic, courseid, userid);
            }
            else
            {
                addtopic = sql.UpdateTopic(topic, courseid, userid);
            }

            return RedirectToAction("SubjectView", "Teacher");

        }
    }
}
