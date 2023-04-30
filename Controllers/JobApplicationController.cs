
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class JobApplicationController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();

        [HttpGet]    // With Id
        public HttpResponseMessage JobApplicationGet(int appid)
        {
            //select *from user
            try
            {
                var edu = db.JobApplications.Where(e => e.Uid == appid).OrderBy(b => b.JobApplicationID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet] // All Without id
        public HttpResponseMessage AllJobApplicationGet()
        {
            //select *from user
            try
            {
                var edu = db.JobApplications.OrderBy(b => b.JobApplicationID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage JobApplicationPost(JobApplication us)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {

                var user = db.JobApplications.Where(s => s.Jid == us.Jid).FirstOrDefault();
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Already Applied");

                JobApplication user1 = db.JobApplications.Add(us);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }






        [HttpPost]
        public HttpResponseMessage JobFileApplicationPost()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                int Jid = int.Parse(form["Jid"]);
                int Uid = int.Parse(form["Uid"]);
                string name = form["name"];
                string status = form["status"];
                string shortlist = form["shortlist"];

                var user = db.JobApplications.Where(s => s.Jid == Jid).FirstOrDefault();
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Already Applied");

                var files = HttpContext.Current.Request.Files;
                string DocumentPath = "";

                if (files.Count > 0)
                {
                    string path = HttpContext.Current.Server.MapPath("~/Documents");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var document = files[0];
                    DocumentPath = Path.Combine(path, document.FileName);
                    document.SaveAs(DocumentPath);
                }

                JobApplication us = new JobApplication()
                {
                    Jid = Jid,
                    Uid = Uid,
                    name = name,
                    status = status,
                    shortlist = shortlist,
                    DocumentPath = DocumentPath
                };

                JobApplication user1 = db.JobApplications.Add(us);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public HttpResponseMessage UpdateEducation(Education u)
        {
            try
            {

                var original = db.Educations.Find(u.EduID);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
                }
                db.Entry(original).CurrentValues.SetValues(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Record Updated");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteEducation(int EduID)
        {
            try
            {

                var original = db.Educations.Find(EduID);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record Found");
                }
                db.Entry(original).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Record Deleted");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public HttpResponseMessage SearchJob(string u)
        {
            try
            {

                var search = db.Educations.Where(b => b.Degree == u).OrderBy(b => b.EduID).ToList();

                if (search == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found");

                }
                return Request.CreateResponse(HttpStatusCode.OK, search);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
