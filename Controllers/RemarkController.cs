using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class RemarkController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();

        [HttpGet]
        public HttpResponseMessage AllMarkedJobGet()
        {
            //select *from user
            try
            {
                var edu = db.jobremarkofmembers.OrderBy(b => b.RemarkID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage MembersRemarksGet(int jobid)
        {
            try
            {
                var result = (
                    from jm in db.jobremarkofmembers
                    join cm in db.CommitteeMembers on jm.CommitteeImemberId equals cm.CommitteeImemberId
                    join u in db.Users on cm.Uid equals u.Uid
                    join ja in db.JobApplications on jm.JobApplicationID equals ja.JobApplicationID
                    join j in db.Jobs on ja.Jid equals j.Jid
                    join app in db.Users on ja.Uid equals app.Uid
                    where jm.JobApplicationID == jobid
                    select new
                    {
                        MemberRemarks = jm.Remarks,
                        CommitteeMemberName = string.Concat(u.Fname, " ", u.Lname),
                        CommitteeMemberGender = u.gender,
                        JobApplicationName = j.Title,
                        ApplicantID = app.Uid,
                        ApplicantName = string.Concat(app.Fname, " ", app.Lname)
                    }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        public HttpResponseMessage RemarkPost(jobremarkofmember u)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {
                //Insert Into User Table
                var user = db.jobremarkofmembers.FirstOrDefault(s => s.JobApplicationID == u.JobApplicationID && s.CommitteeImemberId == u.CommitteeImemberId);
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "You Already Gave Remarks On This Job");

                var educations = db.jobremarkofmembers.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.RemarkID + " " + "Record Inserted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage Remark2Post(jobremarkofmember u)
        {
            try
            {
                // Check if a remark from this user for this job already exists
                var existingRemark = db.jobremarkofmembers
                    .FirstOrDefault(remark => remark.CommitteeImemberId == u.CommitteeImemberId && remark.JobApplicationID == u.JobApplicationID);

                if (existingRemark != null)
                {
                    // If a matching remark already exists, reject the insert
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "User with ID " + u.CommitteeImemberId + " has already posted a remark for job with ID " + u.JobApplicationID);
                }

                // If no matching remark exists, proceed with the insert
                var educations = db.jobremarkofmembers.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.RemarkID + " Record Inserted");
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
