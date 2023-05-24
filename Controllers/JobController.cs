

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class JobController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();

        [HttpGet]
        public HttpResponseMessage JobGet(/*int uid*/)
        {
            //select *from user
            try
            {
                var app = db.Jobs/*.Where(e => e.Uid == uid)*/.OrderBy(b => b.Title).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, app);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage JobidGet(int Jid)
        {
            //select *from user
            try
            {
                var app = db.Jobs.Where(e => e.Jid == Jid).OrderBy(b => b.Jid).ToList();
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
        public HttpResponseMessage JobPost(Job u)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {
                //Insert Into User Table
                var users = db.Jobs.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.Title + " " + "Record Inserted");
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








        //This is the filter that is used to chehck user complete their education and expereince or not if not then show please complete education and expereince if yes than show the jobs



        [HttpGet]
        public HttpResponseMessage WithCheckfilterJobGet(int uid)
        {
            try
            {
                // Get the user
                var user = db.Users.Find(uid);

                // Check if the user exists
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");
                }

                // Check if the user has completed the education and experience sections
                if (!user.Educations.Any() || !user.Experiences.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Please complete the Education and Experience sections first.");
                }

                // Get the jobs
                var jobs = db.Jobs
                    .OrderBy(j => j.Title)
                    .Select(j => new
                    {
                        j.Jid,
                        j.Title,
                        j.qualification,
                        j.Salary,
                        j.experience,
                        j.LastDateOfApply,
                        j.Location,
                        j.Description,
                        j.noofvacancie
                    })
                    .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, jobs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }





        [HttpGet]
        public HttpResponseMessage NewJobGet()
        {
            try
            {
                var jobs = db.Jobs
                    .OrderBy(j => j.Jid)
                    .Select(j => new
                    {
                        j.Jid,
                        j.Title,
                        j.qualification,
                        j.Salary,
                        j.experience,
                        j.LastDateOfApply,
                        j.Location,
                        j.Description,
                        j.noofvacancie
                    })
                    .ToList();

                if (!jobs.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Job record not found");

                return Request.CreateResponse(HttpStatusCode.OK, jobs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }








        [HttpGet]
        public HttpResponseMessage NewJobdetailGet(int jid)
        {
            try
            {
                var job = db.Jobs
                    .Where(j => j.Jid == jid)
                    .Select(j => new
                    {
                        j.Jid,
                        j.Title,
                        j.qualification,
                        j.Salary,
                        j.experience,
                        j.LastDateOfApply,
                        j.Location,
                        j.Description,
                        j.noofvacancie
                    })
                    .FirstOrDefault();

                if (job == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Job record not found");

                return Request.CreateResponse(HttpStatusCode.OK, job);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }





    }
}
