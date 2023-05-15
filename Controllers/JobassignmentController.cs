using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class JobassignmentController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();

        [HttpGet]
        public HttpResponseMessage AllJobassignmentGet(/*int uid*/)
        {
            //select *from user
            try
            {
                var app = db.JobAssignments/*.Where(e => e.Uid == uid)*/.OrderBy(b => b.assignmentid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, app);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]   // based on uid
        public HttpResponseMessage JobassignmentwithidGet(int uid)
        {
            //select *from user
            try
            {
                var app = db.JobAssignments.Where(e => e.Uid == uid).OrderBy(b => b.assignmentid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, app);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage JobDetailGet(int jid)
        {
            //select *from user
            try
            {
                var app = db.Jobs.Where(e => e.Jid == jid).OrderBy(b => b.Jid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, app);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage JobassignmentPost(JobAssignment u)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {
                //Insert Into User Table
                var users = db.JobAssignments.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.JobApplicationID + " " + "Record Inserted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateJob(Job u)
        {
            try
            {

                var original = db.Jobs.Find(u.Jid);
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
        public HttpResponseMessage DeleteJob(int Jid)
        {
            try
            {

                var original = db.Jobs.Find(Jid);
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

                var search = db.Jobs.Where(b => b.Title == u).OrderBy(b => b.Jid).ToList();

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

