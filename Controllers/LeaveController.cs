using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class LeaveController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();

        [HttpGet]
        public HttpResponseMessage AllLeaveGet()
        {
            //select *from user
            try
            {
                var edu = db.Leave_Application.OrderBy(b => b.leaveappid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage LeaveGet(int uid)
        {
            //select *from user
            try
            {
                var edu = db.Leave_Application.Where(e => e.Uid == uid).OrderBy(b => b.leaveappid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // This functions is for show pending leaves of user

        [HttpGet]
        public HttpResponseMessage PendingLeaveGet(int uid)
        {
            //select *from user
            try
            {
                var edu = db.Leave_Application.Where(e => e.Uid == uid && e.status == "Pending").OrderBy(b => b.leaveappid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        // This functions is for show rejected leaves of user

        [HttpGet]
        public HttpResponseMessage RejectedLeaveGet(int uid)
        {
            //select *from user
            try
            {
                var edu = db.Leave_Application.Where(e => e.Uid == uid && e.status == "rejected").OrderBy(b => b.leaveappid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // This functions is for show Approved leaves of user

        [HttpGet]
        public HttpResponseMessage ApprovedLeaveGet(int uid)
        {
            //select *from user
            try
            {
                var edu = db.Leave_Application.Where(e => e.Uid == uid && e.status == "approved").OrderBy(b => b.leaveappid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        public HttpResponseMessage LeavePost(Leave_Application u)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {
                //Insert Into User Table
                var educations = db.Leave_Application.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.leavetype + " " + "Record Inserted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateLeave(Leave_Application u)
        {
            try
            {

                var original = db.Leave_Application.Find(u.leaveappid);
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
        public HttpResponseMessage DeleteLeave(int leaveappid)
        {
            try
            {

                var original = db.Leave_Application.Find(leaveappid);
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

                var search = db.Leave_Application.Where(b => b.leavetype == u).OrderBy(b => b.leaveappid).ToList();

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
        public HttpResponseMessage NewAllLeaveapplicationGet()
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    orderby leave.leaveappid
                                    select new
                                    {
                                        user.Uid,
                                        user.Fname,
                                        user.Lname,
                                        user.email,
                                        user.mobile,
                                        user.cnic,
                                        user.dob,
                                        user.gender,
                                        user.address,
                                        user.password,
                                        user.role,
                                        user.image,
                                        leave.leaveappid,
                                        leave.leavetype,
                                        leave.startdate,
                                        leave.enddate,
                                        leave.reason,
                                        leave.status,
                                        leave.applydate
                                    }).ToList();

                if (!applications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No leave application records found.");

                return Request.CreateResponse(HttpStatusCode.OK, applications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }













        [HttpGet]
        public HttpResponseMessage NewLeaveGet(int uid)
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where user.Uid == uid
                                    orderby leave.leaveappid
                                    select new
                                    {
                                        user.Uid,
                                        user.Fname,
                                        user.Lname,
                                        user.email,
                                        user.mobile,
                                        user.cnic,
                                        user.dob,
                                        user.gender,
                                        user.address,
                                        user.password,
                                        user.role,
                                        user.image,
                                        leave.leaveappid,
                                        leave.leavetype,
                                        leave.startdate,
                                        leave.enddate,
                                        leave.reason,
                                        leave.status,
                                        leave.applydate
                                    }).ToList();

                if (!applications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Leave application records not found for the specified user.");

                return Request.CreateResponse(HttpStatusCode.OK, applications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }














        [HttpGet]
        public HttpResponseMessage NewPendingLeaveGet(int uid)
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where user.Uid == uid && leave.status == "Pending"
                                    orderby leave.leaveappid
                                    select new
                                    {
                                        user.Uid,
                                        user.Fname,
                                        user.Lname,
                                        user.email,
                                        user.mobile,
                                        user.cnic,
                                        user.dob,
                                        user.gender,
                                        user.address,
                                        user.password,
                                        user.role,
                                        user.image,
                                        leave.leaveappid,
                                        leave.leavetype,
                                        leave.startdate,
                                        leave.enddate,
                                        leave.reason,
                                        leave.status,
                                        leave.applydate
                                    }).ToList();

                if (!applications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Pending leave application records not found for the specified user.");

                return Request.CreateResponse(HttpStatusCode.OK, applications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        [HttpGet]
        public HttpResponseMessage NewRejectedLeaveGet(int uid)
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where user.Uid == uid && leave.status == "rejected"
                                    orderby leave.leaveappid
                                    select new
                                    {
                                        user.Uid,
                                        user.Fname,
                                        user.Lname,
                                        user.email,
                                        user.mobile,
                                        user.cnic,
                                        user.dob,
                                        user.gender,
                                        user.address,
                                        user.password,
                                        user.role,
                                        user.image,
                                        leave.leaveappid,
                                        leave.leavetype,
                                        leave.startdate,
                                        leave.enddate,
                                        leave.reason,
                                        leave.status,
                                        leave.applydate
                                    }).ToList();

                if (!applications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Rejected leave application records not found for the specified user.");

                return Request.CreateResponse(HttpStatusCode.OK, applications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        [HttpGet]
        public HttpResponseMessage NewApprovedLeaveGet(int uid)
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where user.Uid == uid && leave.status == "approved"
                                    orderby leave.leaveappid
                                    select new
                                    {
                                        user.Uid,
                                        user.Fname,
                                        user.Lname,
                                        user.email,
                                        user.mobile,
                                        user.cnic,
                                        user.dob,
                                        user.gender,
                                        user.address,
                                        user.password,
                                        user.role,
                                        user.image,
                                        leave.leaveappid,
                                        leave.leavetype,
                                        leave.startdate,
                                        leave.enddate,
                                        leave.reason,
                                        leave.status,
                                        leave.applydate
                                    }).ToList();

                if (!applications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Approved leave application records not found for the specified user.");

                return Request.CreateResponse(HttpStatusCode.OK, applications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}
