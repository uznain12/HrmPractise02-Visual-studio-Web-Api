﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class AssignjobapplicationController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();


        [HttpGet]
        public HttpResponseMessage AllAssignmentGet()
        {
            //select *from user
            try
            {
                var edu = db.JobApplicationCommittees.OrderBy(b => b.JobApplicationCommitteeeID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage AssignmentGet(int comid)
        {
            //select *from user
            try
            {
                var edu = db.JobApplicationCommittees.Where(e => e.CommitteeId == comid).OrderBy(b => b.CommitteeId).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage AssinJobApplicationToCommittee(JobApplicationCommittee u)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {
                //Insert Into User Table
                var educations = db.JobApplicationCommittees.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.CommitteeId + " " + "Record Inserted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage AssinJobApplicationToCommittee2(JobApplicationCommittee u)
        {
            try
            {
                // Check if the JobApplication has already been assigned to the Committee
                var existingAssignment = db.JobApplicationCommittees
                                           .FirstOrDefault(j => j.JobApplicationID == u.JobApplicationID );

                if (existingAssignment != null)
                {
                    // If the assignment already exists, return a response indicating this
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "This Job Application has already been assigned to the Committee.");
                }
                else
                {
                    // If the assignment does not exist, add it to the JobApplicationCommittees table
                    var newAssignment = db.JobApplicationCommittees.Add(u);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, u.CommitteeId + " " + "Record Inserted");
                }
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