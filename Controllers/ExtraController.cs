using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class ExtraController : ApiController
    {






        PracticeHrmDBEntities db = new PracticeHrmDBEntities();








        //Leave related Important Code 


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
                    TotalCasual = countCasual,
                    TotalEarned = countEarned,
                    TotalAnnual = countAnnual,
                    MaxSickAllow = maxSick,
                    MaxCasualAllow = maxCasual,
                    MaxEarnedAllow = maxEarned,
                    MaxAnnualAllow = maxAnnual,
                    Applications = applications
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
