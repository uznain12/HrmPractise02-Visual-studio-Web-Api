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


        // leave work


        [HttpGet]
        public HttpResponseMessage RemainingTotalNewLeaveWithIDGet(int uid)
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

                int remainingSick = maxSick - countSick;
                int remainingCasual = maxCasual - countCasual;
                int remainingEarned = maxEarned - countEarned;
                int remainingAnnual = maxAnnual - countAnnual;

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
                    RemainingSick = remainingSick,
                    RemainingCasual = remainingCasual,
                    RemainingEarned = remainingEarned,
                    RemainingAnnual = remainingAnnual,
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
        public HttpResponseMessage AgainnewRemainingTotalNewLeaveWithIDGet(int uid)
        {
            int countSick = 0;
            int countCasual = 0;
            int countEarned = 0;
            int countAnnual = 0;

            int maxSick = 10;
            int maxCasual = 5;
            int maxEarned = 3;
            int maxAnnual = 9;

            try
            {
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

                int remainingSick = maxSick - countSick;
                int remainingCasual = maxCasual - countCasual;
                int remainingEarned = maxEarned - countEarned;
                int remainingAnnual = maxAnnual - countAnnual;

                if (applications.Count == 0)
                {
                    remainingSick = maxSick;
                    remainingCasual = maxCasual;
                    remainingEarned = maxEarned;
                    remainingAnnual = maxAnnual;
                }

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
                    RemainingSick = remainingSick,
                    RemainingCasual = remainingCasual,
                    RemainingEarned = remainingEarned,
                    RemainingAnnual = remainingAnnual,
                    Applications = applications
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage JobFileApplicationWithFilterPost2()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                int Jid = int.Parse(form["Jid"]);
                int Uid = int.Parse(form["Uid"]);
                string name = form["name"];
                string shortlist = form["shortlist"];

                var user = db.JobApplications.Where(s => s.Jid == Jid && s.Uid == Uid).FirstOrDefault();
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

                var userEducation = db.Educations.Where(e => e.Uid == Uid).OrderByDescending(e => e.EduID).FirstOrDefault();
                if (userEducation == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User education not found");

                var job = db.Jobs.Where(j => j.Jid == Jid).FirstOrDefault();
                if (job == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Job not found");

                List<string> degreeOrder = new List<string> { "Primary", "Middle", "Matric", "Inter", "Bachelor", "Master", "PHD" };

                string status = "Pending";

                if (job.Title.ToLower() == "professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "assistant professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "lectruer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "junior lecturer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "lab attendant" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Inter"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "guard" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Matric"))
                {
                    status = "Rejected";
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

                // Match the job title with the committee title
                var committee = db.Committees.FirstOrDefault(c => c.CommitteeTitle.ToLower() == job.Title.ToLower());
                if (committee != null)
                {
                    JobApplicationCommittee jobApplicationCommittee = new JobApplicationCommittee()
                    {
                        JobApplicationID = user1.JobApplicationID,
                        CommitteeId = committee.CommitteeId
                    };

                    db.JobApplicationCommittees.Add(jobApplicationCommittee);
                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }





        [HttpPost]
        public HttpResponseMessage NewJobFileApplicationWithFilterPost2()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                int Jid = int.Parse(form["Jid"]);
                int Uid = int.Parse(form["Uid"]);
                string name = form["name"];
                string shortlist = form["shortlist"];

                var user = db.JobApplications.Where(s => s.Jid == Jid && s.Uid == Uid).FirstOrDefault();
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

                var userEducation = db.Educations.Where(e => e.Uid == Uid).OrderByDescending(e => e.EduID).FirstOrDefault();
                if (userEducation == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User education not found");

                var job = db.Jobs.Where(j => j.Jid == Jid).FirstOrDefault();
                if (job == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Job not found");

                List<string> degreeOrder = new List<string> { "Primary", "Middle", "Matric", "Inter", "Bachelor", "Master", "PHD" };

                string status = "Pending";

                if (job.Title.ToLower() == "professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "assistant professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "lecturer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "junior lecturer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "lab attendant" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Inter"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "guard" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Matric"))
                {
                    status = "Rejected";
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

                if (status == "Rejected")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Automatically Rejected");
                }

                // Match the job title with the committee title
                var committee = db.Committees.FirstOrDefault(c => c.CommitteeTitle.ToLower() == job.Title.ToLower());
                if (committee != null)
                {
                    JobApplicationCommittee jobApplicationCommittee = new JobApplicationCommittee()
                    {
                        JobApplicationID = user1.JobApplicationID,
                        CommitteeId = committee.CommitteeId
                    };

                    db.JobApplicationCommittees.Add(jobApplicationCommittee);
                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        public HttpResponseMessage NewNewJobFileApplicationWithFilterPost2()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                int Jid = int.Parse(form["Jid"]);
                int Uid = int.Parse(form["Uid"]);
                string name = form["name"];
                string shortlist = form["shortlist"];

                var user = db.JobApplications.Where(s => s.Jid == Jid && s.Uid == Uid).FirstOrDefault();
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

                var userEducation = db.Educations.Where(e => e.Uid == Uid).OrderByDescending(e => e.EduID).FirstOrDefault();
                if (userEducation == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User education not found");

                var job = db.Jobs.Where(j => j.Jid == Jid).FirstOrDefault();
                if (job == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Job not found");

                List<string> degreeOrder = new List<string> { "Primary", "Middle", "Matric", "Inter", "Bachelor", "Master", "PHD" };

                string status = "Pending";

                if (job.Title.ToLower() == "professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "assistant professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "lecturer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "junior lecturer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "lab attendant" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Inter"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "guard" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Matric"))
                {
                    status = "Rejected";
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

                if (status == "Rejected")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Automatically Rejected");
                }
                else if (status == "Pending")
                {
                    // Match the job title with the committee title
                    var committee = db.Committees.FirstOrDefault(c => c.CommitteeTitle.ToLower() == job.Title.ToLower());
                    if (committee != null)
                    {
                        JobApplicationCommittee jobApplicationCommittee = new JobApplicationCommittee()
                        {
                            JobApplicationID = user1.JobApplicationID,
                            CommitteeId = committee.CommitteeId
                        };

                        db.JobApplicationCommittees.Add(jobApplicationCommittee);
                        db.SaveChanges();
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        public HttpResponseMessage WithUniversityNewNewJobFileApplicationWithFilterPost2()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                int Jid = int.Parse(form["Jid"]);
                int Uid = int.Parse(form["Uid"]);
                string name = form["name"];
                string shortlist = form["shortlist"];

                var user = db.JobApplications.Where(s => s.Jid == Jid && s.Uid == Uid).FirstOrDefault();
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

                var userEducation = db.Educations.Where(e => e.Uid == Uid).OrderByDescending(e => e.EduID).FirstOrDefault();
                if (userEducation == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User education not found");

                var job = db.Jobs.Where(j => j.Jid == Jid).FirstOrDefault();
                if (job == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Job not found");

                List<string> degreeOrder = new List<string> { "Primary", "Middle", "Matric", "Inter", "Bachelor", "Master", "PHD" };

                string status = "Pending";

                if (job.Title.ToLower() == "professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "assistant professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "lecturer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "junior lecturer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "lab attendant" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Inter"))
                {
                    status = "Rejected";
                }
                else if (job.Title.ToLower() == "guard" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Matric"))
                {
                    status = "Rejected";
                }
                else if (userEducation.Institute.ToLower() == "bims university" ||
                         userEducation.Institute.ToLower() == "priston university" ||
                         userEducation.Institute.ToLower() == "iqra university")
                {
                    status = "Rejected";
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

                if (status == "Rejected")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Automatically Rejected");
                }
                else if (status == "Pending")
                {
                    // Match the job title with the committee title
                    var committee = db.Committees.FirstOrDefault(c => c.CommitteeTitle.ToLower() == job.Title.ToLower());
                    if (committee != null)
                    {
                        JobApplicationCommittee jobApplicationCommittee = new JobApplicationCommittee()
                        {
                            JobApplicationID = user1.JobApplicationID,
                            CommitteeId = committee.CommitteeId
                        };

                        db.JobApplicationCommittees.Add(jobApplicationCommittee);
                        db.SaveChanges();
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }























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

                // Update job statuses to "expire" if the last date of application has passed
               // UpdateExpiredJobStatus();  ya use hoga time ko update karnay ka liya

                // Get the jobs
                var jobs = db.Jobs.Where(j => j.jobstatus == "active")
                    .Join(db.Educations.Where(e => e.Uid == uid && !(e.Institute.Equals("BIMS") || e.Institute.Equals("Iqra University"))),
                        j => j.qualification,
                        e => e.Degree,
                        (j, e) => new
                        {
                            j.Jid,
                            j.Title,
                            j.qualification,
                            j.Salary,
                            j.experience,
                            j.LastDateOfApply,
                            j.Location,
                            j.Description,
                            j.noofvacancie,
                            j.jobstatus
                        })
                    .OrderBy(j => j.Title)
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, jobs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



    }
}
