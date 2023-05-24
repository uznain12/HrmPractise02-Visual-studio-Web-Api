

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace HrmPractise02.Controllers
{
    public class ExpereinceController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();
        [HttpGet]
        public HttpResponseMessage ExperienceGet(int uid)
        {
            //select *from user
            try
            {
                var exp = db.Experiences.Where(e => e.Uid == uid).OrderBy(b => b.ExpID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, exp);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage ExperiencePost(Experience u)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {
                //Insert Into User Table
                var educations = db.Experiences.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.Company + " " + "Record Inserted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateExperience(Experience u)
        {
            try
            {

                var original = db.Experiences.Find(u.ExpID);
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
        public HttpResponseMessage DeleteExperience(int ExpID)
        {
            try
            {

                var original = db.Experiences.Find(ExpID);
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
        public HttpResponseMessage SearchExperience(string u)
        {
            try
            {

                var search = db.Experiences.Where(b => b.Company == u).OrderBy(b => b.ExpID).ToList();

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






        [HttpGet]
        public HttpResponseMessage AllExperienceGet()
        {
            try
            {
                var experiences = db.Experiences
                    .OrderBy(e => e.ExpID)
                    .Select(e => new
                    {
                        e.ExpID,
                        e.Uid,
                        e.Company,
                        e.Title,
                        e.Startdate,
                        e.currentwork,
                        e.Enddate,
                        e.otherskill
                    })
                    .ToList();

                if (!experiences.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Experience record not found");

                return Request.CreateResponse(HttpStatusCode.OK, experiences);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }










        [HttpGet]
        public HttpResponseMessage NewExperienceGet(int uid)
        {
            try
            {
                var experiences = db.Experiences
                    .Where(e => e.Uid == uid)
                    .OrderBy(e => e.ExpID)
                    .Select(e => new
                    {
                        e.ExpID,
                        e.Uid,
                        e.Company,
                        e.Title,
                        e.Startdate,
                        e.currentwork,
                        e.Enddate,
                        e.otherskill
                    })
                    .ToList();

                if (!experiences.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Experience record not found");

                return Request.CreateResponse(HttpStatusCode.OK, experiences);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }








    }
}
