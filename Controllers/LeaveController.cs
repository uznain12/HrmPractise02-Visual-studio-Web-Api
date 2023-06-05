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
        //

       

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

                // Update specific properties
                original.leavetype = u.leavetype;
                original.startdate = u.startdate;
                original.enddate = u.enddate;
                original.reason = u.reason;
                original.status = u.status;
                original.applydate = u.applydate;

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
                                    orderby leave.leavetype
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
        public HttpResponseMessage LeaveWithIDGet(int leaveappid)
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where leave.leaveappid == leaveappid
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
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"No leave application records found for leaveappid: {leaveappid}.");

                return Request.CreateResponse(HttpStatusCode.OK, applications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        //[HttpGet]
        //public HttpResponseMessage NewLeaveWithIDGet(int leaveappid)
        //{
        //    try
        //    {
        //        int countSick = 0;
        //        int countCasual = 0;
        //        int countEarned = 0;
        //        int countAnnual = 0;

        //        var applications = (from user in db.Users
        //                            join leave in db.Leave_Application on user.Uid equals leave.Uid
        //                            where leave.Uid == leaveappid &&
        //                                  (leave.leavetype == "Sick" || leave.leavetype == "Casual" || leave.leavetype == "Earned" || leave.leavetype == "Annual")
        //                            orderby leave.leaveappid
        //                            select new
        //                            {
        //                                user.Uid,
        //                                user.Fname,
        //                                user.Lname,
        //                                user.email,
        //                                user.mobile,
        //                                user.cnic,
        //                                user.dob,
        //                                user.gender,
        //                                user.address,
        //                                user.password,
        //                                user.role,
        //                                user.image,
        //                                leave.leaveappid,
        //                                leave.leavetype,
        //                                leave.startdate,
        //                                leave.enddate,
        //                                leave.reason,
        //                                leave.status,
        //                                leave.applydate
        //                            }).ToList();

        //        foreach (var application in applications)
        //        {
        //            if (application.leavetype == "Sick" && application.status == "approved")
        //            {
        //                if (countSick < 10)
        //                    countSick++;
        //                else
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum limit of sick leave applications reached.");
        //            }
        //            else if (application.leavetype == "Casual" && application.status == "approved")
        //            {
        //                if (countCasual < 10)
        //                    countCasual++;
        //                else
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum limit of casual leave applications reached.");
        //            }
        //            else if (application.leavetype == "Earned" && application.status == "approved")
        //            {
        //                if (countEarned < 10)
        //                    countEarned++;
        //                else
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum limit of earned leave applications reached.");
        //            }
        //            else if (application.leavetype == "Annual" && application.status == "approved")
        //            {
        //                if (countAnnual < 10)
        //                    countAnnual++;
        //                else
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum limit of annual leave applications reached.");
        //            }
        //        }

        //        if (!applications.Any())
        //            return Request.CreateResponse(HttpStatusCode.NotFound, $"No leave application records found for leaveappid: {leaveappid}.");

        //        var response = new
        //        {
        //            TotalSick = countSick,
        //            TotalACasual = countCasual,
        //            TotalEarned = countEarned,
        //            TotalAnnual = countAnnual,
        //            Applications = applications
        //        };

        //        return Request.CreateResponse(HttpStatusCode.OK, response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        [HttpGet]
        public HttpResponseMessage TotalNewLeaveWithIDGet(int uid)
        {
            try
            {
                int countSick = 0;
                int countCasual = 0;
                int countEarned = 0;
                int countAnnual = 0;

                int maxSick = 10;
                int maxCasual = 5;
                int maxEarned = 3;
                int maxAnnual = 9;

                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where leave.Uid == uid &&
                                          (leave.leavetype == "Sick" || leave.leavetype == "Casual" || leave.leavetype == "Earned" || leave.leavetype == "Annual")
                                    orderby leave.status
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
                                    }).Distinct().ToList();

                foreach (var application in applications)
                {
                    if (application.leavetype == "Sick" && application.status == "approved")
                    {
                        if (countSick < maxSick)
                            countSick++;
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum limit of sick leave applications reached.");
                    }
                    else if (application.leavetype == "Casual" && application.status == "approved")
                    {
                        if (countCasual < maxCasual)
                            countCasual++;
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum limit of casual leave applications reached.");
                    }
                    else if (application.leavetype == "Earned" && application.status == "approved")
                    {
                        if (countEarned < maxEarned)
                            countEarned++;
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum limit of earned leave applications reached.");
                    }
                    else if (application.leavetype == "Annual" && application.status == "approved")
                    {
                        if (countAnnual < maxAnnual)
                            countAnnual++;
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum limit of annual leave applications reached.");
                    }
                }

                if (!applications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"No leave application records found for leaveappid: {uid}.");

                var response = new
                {
                    TotalSick = countSick,
                    TotalACasual = countCasual,
                    TotalEarned = countEarned,
                    TotalAnnual = countAnnual,
                    Applications = applications
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        public HttpResponseMessage NewLeaveWithIDGet(int leaveappid)
        {
            try
            {
                int countSick = 0;
                int countCasual = 0;
                int countEarned = 0;
                int countAnnual = 0;

                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where leave.Uid == leaveappid &&
                                          (leave.leavetype == "Sick" || leave.leavetype == "Casual" || leave.leavetype == "Earned" || leave.leavetype == "Annual")
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

                foreach (var application in applications)
                {
                    if (application.leavetype == "Sick" && application.status == "approved")
                        countSick++;
                    else if (application.leavetype == "Casual" && application.status == "approved")
                        countCasual++;
                    else if (application.leavetype == "Earned" && application.status == "approved")
                        countEarned++;
                    else if (application.leavetype == "Annual" && application.status == "approved")
                        countAnnual++;
                }

                if (!applications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"No leave application records found for leaveappid: {leaveappid}.");

                var response = new
                {
                    TotalSick = countSick,
                    TotalACasual = countCasual,
                    TotalEarned = countEarned,
                    TotalAnnual = countAnnual,
                    Applications = applications
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        public HttpResponseMessage WithNewLeaveWithIDGet(int leaveappid)
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where leave.Uid == leaveappid &&
                                          (leave.leavetype == "Sick" || leave.leavetype == "Casual" || leave.leavetype == "Earned" || leave.leavetype == "Annual") &&
                                          leave.status == "approved"
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

                int countApproved = applications.Count;

                if (!applications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"No approved leave application records found for leaveappid: {leaveappid}.");

                var response = new
                {
                    TotalApproved = countApproved,
                    Applications = applications
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
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

        int count = applications.Count;

        if (count == 0)
            return Request.CreateResponse(HttpStatusCode.NotFound, "Pending leave application records not found for the specified user.");

        var response = new
        {
            TotalPending = count,
            Applications = applications
        };

        return Request.CreateResponse(HttpStatusCode.OK, response);
    }
    catch (Exception ex)
    {
        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
    }
}




        [HttpGet]
        public HttpResponseMessage AllNewPendingLeaveGet()
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where  leave.status == "Pending"
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
        public HttpResponseMessage AllNewRejectedLeaveGet()
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where   leave.status == "rejected"
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






        [HttpGet]
        public HttpResponseMessage AllNewApprovedLeaveGet()
        {
            try
            {
                var applications = (from user in db.Users
                                    join leave in db.Leave_Application on user.Uid equals leave.Uid
                                    where leave.status == "approved"
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
