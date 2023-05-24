using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class CommitteeController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();
        [HttpGet]
        public HttpResponseMessage AllCommitteeGet()
        {
            //select *from user
            try
            {
                var edu = db.Committees.OrderBy(b => b.CommitteeId).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage CommitteeGet(int comid)
        {
            //select *from user
            try
            {
                var edu = db.Committees.Where(e => e.CommitteeId == comid).OrderBy(b => b.CommitteeId).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage CommittwithEmployeeeeGet(int uid)
        {
            try
            {
                string sqlQuery = @"
            
  SELECT c.[CommitteeId], c.[CommitteeTitle], c.[CommitteeHead]
FROM [PracticeHrmDB].[dbo].[Committee] c
INNER JOIN [PracticeHrmDB].[dbo].[CommitteeMembers] cm ON c.[CommitteeId] = cm.[CommitteeId]
WHERE cm.[Uid] 
 = @uid
            ORDER BY c.[CommitteeId]";

                var committees = db.Database.SqlQuery<Committee>(sqlQuery, new SqlParameter("@uid", uid)).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, committees);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage Createcommitte(Committee u)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {
                //Insert Into User Table
                var educations = db.Committees.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.CommitteeTitle + " " + "Record Inserted");
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
        public HttpResponseMessage DeleteCommitte(int CommitteeId)
        {
            try
            {

                var original = db.Committees.Find(CommitteeId);
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
