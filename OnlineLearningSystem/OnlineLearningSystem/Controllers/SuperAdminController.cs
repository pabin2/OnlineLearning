using CrystalDecisions.CrystalReports.Engine;
using OnlineLearningSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace OnlineLearningSystem.Controllers
{
    [Authorization]
    [Authorization_isSuperAdmin]
    public class SuperAdminController : Controller
    {
        Sql_connnector sql = new Sql_connnector();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult School(int? success,int? page,string search)
        {
            List<School> myschool = new List<School>();
            if (search == null)
            {
                myschool = sql.Getallschool().ToList();
            }
            else
            {
                myschool = sql.Getallschool().Where(a=>a.SchoolName==search).ToList();
            }
            IPagedList<School> schoollist = myschool.ToPagedList(page ?? 1, 4);
            if (success == 1)
            {
                @ViewBag.message = "Successfully Added";
            }
            else if (success == 0)
            {
                @ViewBag.message = "Failed Adding";
            }
            return View(schoollist);
        }

        //insert school
        [HttpPost]
        public int Addschool(School schooldetail)
        {
            var result = sql.InsertSchool(schooldetail);
            return result;

        }

        //edit school
        [HttpGet]
        public ActionResult Editschool(int? success,int id)
        {
            List<School> myschool = new List<School>();
            myschool = sql.Getallschool().ToList();
            if (success == 1)
            {
                @ViewBag.message = "Successfully Updated";
            }
            else if (success == 0)
            {
                @ViewBag.message = "Failed Updated";
            }
            return View(myschool);

        }
        //subject
        [HttpGet]
        public ActionResult Subject()
        {
            List<subject> courses = sql.Listcourses().ToList();
            return View(courses);

        }
        [HttpPost]
        public ActionResult Subject(subject course)
        {
            int result = sql.Addcourses(course);
            return RedirectToAction("subject", "superadmin");

        }
        //delete teacher
        [HttpPost]
        public int DeleteSchool(School schooldetail)
        {
            int i = sql.DeleteSchool(schooldetail);
            return i;
        }
        [HttpPost]
        public ActionResult Editschool(School schooldetail)
        {
            try
            {
                var result = sql.EditSchool(schooldetail);
                if (result == 1)
                {
                    return RedirectToAction("School", "SuperAdmin", new { success = 1 });
                }
                else
                {
                    return RedirectToAction("School", "SuperAdmin", new { success = 2 });
                }
            }
            catch (Exception)
            {

                return RedirectToAction("School", "SuperAdmin", new { success = 2 });
            }


        }
        //addming admin for school
        [HttpGet]
        public ActionResult Addschooladmin(int? schoolid)
        {
            return View();

        }

        [HttpGet]
        public ActionResult Report(int schoolname =0)
        {
            List<user_info> alluser;
            List<School> school = sql.Getallschool().ToList();
            @ViewBag.schoollist = school;
            if (schoolname == default(int))
            {
                alluser = sql.displayteacher(1, false, 1).ToList();
            }
            else
            {
                alluser = sql.displayteacher(1, false, 1).Where(m => m.schoolid == schoolname).ToList();
            }
            return View(alluser);
        }

        public ActionResult GetUsersInSchool(int schoolname)
        {
            List<user_info> alluser = sql.displayteacher(1, false, 1).Where(m => m.schoolid == schoolname).ToList();
            return Content("", "text/html");

        }
        public ActionResult Exportreport(int? schoolname)
        {
            string a = Request.QueryString["schoolname"];
            List<user_info> alluser;
            if (schoolname == null)
            {
                alluser = sql.displayteacher(1, false, 1).ToList();
            }
            else
            {
                alluser = sql.displayteacher(1, false, 1).Where(m => m.schoolid == schoolname).ToList();
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "Myreport.rpt"));
            rd.SetDataSource(alluser);
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "report.pdf");
        }

        [HttpPost]
        public ActionResult Addschooladmin(user_info userinfo)
        {
            var result = sql.Insertuser(userinfo, "schooladmin");
            if (result == 1)
                return RedirectToAction("School", "SuperAdmin", new { success = 1 });
            else
                return RedirectToAction("School", "SuperAdmin", new { success = 0 });

        }

        [HttpPost]
        public int CheckExistingUser(user_info userinfo)
        {
            var result = sql.getuserdetail(userinfo.username);
            return result;
        }

        [HttpGet]
        [ActionName("Backup")]
        public ActionResult Backup(int? success)
        {
            if (success == 1)
            {
                @ViewBag.message = "Successfully backup completed";
            }
            else if (success == 2)
            {
                @ViewBag.message = "Successfully restored completed";
            }

            return View();
        }

        [HttpPost]
        public ActionResult BackupPost()
        {
            var result = sql.Backup();
            return RedirectToAction("Backup", "SuperAdmin", new { success = 1 });
        }
        [HttpPost]
        public ActionResult Restore()
        {
            var result = sql.Restore();
            return RedirectToAction("Backup", "SuperAdmin", new { success = 2 });
        }

        [HttpGet]
        public ActionResult MessageView(string search, string sortBy,int? page)
        {
            List<message> message = new List<message>();
            //triggering asc and desc
            int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            string usertype = Session["loggedinusertype"].ToString();
            ViewBag.sortByParam = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            if (search == null)
            {


                message = sql.displaymessage(schoolid, userid, usertype).ToList();
            }
            else
            {
                message = sql.displaymessage(schoolid, userid, usertype).Where(m => m.sender.StartsWith(search)).ToList();


            }
            IPagedList<message> messagelist = message.ToPagedList(page ?? 1, 4);
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
            return View(messagelist);
        }

        [HttpPost]
        public int MessageView(message message_info)
        {
            int a = 0;

            string sender = Session["loggedinusername"].ToString();
            string schoolid = Session["loggedinuserschoolid"].ToString();
            DateTime date = DateTime.Now;
            a = sql.insertMessage(message_info, date, sender, schoolid, message_info.usertype);
            return a;

        }




    }
}
