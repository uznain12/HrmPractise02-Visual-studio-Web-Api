
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
    public class JobApplicationController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();

        [HttpGet]    // With Id
        public HttpResponseMessage JobApplicationGet(int appid)
        {
            //select *from user
            try
            {
                var edu = db.JobApplications.Where(e => e.Uid == appid).OrderBy(b => b.JobApplicationID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]    // With Id
        public HttpResponseMessage JobApplicationHrSideGet(int applicationid)
        {
            //select *from user
            try
            {
                var edu = db.JobApplications.Where(e => e.JobApplicationID == applicationid).OrderBy(b => b.JobApplicationID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet] // All Without id
        public HttpResponseMessage AllJobApplicationGet()
        {
            //select *from user
            try
            {
                var edu = db.JobApplications.OrderBy(b => b.JobApplicationID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage JobApplicationPost(JobApplication us)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {

                var user = db.JobApplications.Where(s => s.Jid == us.Jid).FirstOrDefault();
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Already Applied");

                JobApplication user1 = db.JobApplications.Add(us);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }






        [HttpPost]
        public HttpResponseMessage JobFileApplicationPost()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                int Jid = int.Parse(form["Jid"]);
                int Uid = int.Parse(form["Uid"]);
                string name = form["name"];
                string status = form["status"];
                string shortlist = form["shortlist"];

                var user = db.JobApplications.Where(s => s.Jid == Jid && s.Uid==Uid).FirstOrDefault();
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

                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateJobapplication(JobApplication u)
        {
            try
            {
                var original = db.JobApplications.Find(u.JobApplicationID);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
                }

                // Exclude DocumentPath from being updated
                u.DocumentPath = original.DocumentPath;
                u.Uid = original.Uid;
                u.Jid = original.Jid;

                db.Entry(original).CurrentValues.SetValues(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Record Updated");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public HttpResponseMessage UpdateJobFileApplication()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                int JobApplicationID = int.Parse(form["JobApplicationID"]);
                int Jid = int.Parse(form["Jid"]);
                int Uid = int.Parse(form["Uid"]);
                string name = form["name"];
                string status = form["status"];
                string shortlist = form["shortlist"];

                var jobApplication = db.JobApplications.FirstOrDefault(s => s.JobApplicationID == JobApplicationID && s.Jid == Jid && s.Uid == Uid);
                if (jobApplication == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Job application not found");

                var files = HttpContext.Current.Request.Files;
                string DocumentPath = jobApplication.DocumentPath;

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

                jobApplication.JobApplicationID = JobApplicationID;
                jobApplication.Uid = Uid;
                jobApplication.Jid = Jid;
                jobApplication.name = name;
                jobApplication.status = status;
                jobApplication.shortlist = shortlist;
                jobApplication.DocumentPath = DocumentPath;

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Job application updated");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public HttpResponseMessage NewUpdateJobFileApplication()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                int JobApplicationID = int.Parse(form["JobApplicationID"]);
                int Jid = int.Parse(form["Jid"]);
                int Uid = int.Parse(form["Uid"]);
                string status = form["status"];

                var jobApplication = db.JobApplications.FirstOrDefault(s => s.JobApplicationID == JobApplicationID && s.Jid == Jid && s.Uid == Uid);
                if (jobApplication == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Job application not found");

                jobApplication.status = status;

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Job application updated");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [Route("api/JobApplication/UpdateUser")]
        [HttpPut]
        public HttpResponseMessage UpdateUser()
        {
            try
            {
                int JobApplicationID = Convert.ToInt32(HttpContext.Current.Request.Form["JobApplicationID"]);
                int Jid = Convert.ToInt32(HttpContext.Current.Request.Form["Jid"]);
                int Uid = Convert.ToInt32(HttpContext.Current.Request.Form["Uid"]);
                string name = HttpContext.Current.Request.Form["name"];
                string status = HttpContext.Current.Request.Form["status"];
                string shortlist = HttpContext.Current.Request.Form["shortlist"];
               
              

                var original = db.JobApplications.Find(JobApplicationID);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
                }

                var files = HttpContext.Current.Request.Files;
                string path = HttpContext.Current.Server.MapPath("~/Documents");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileData = null;
                for (int i = 0; i < files.Count; i++)
                {
                    fileData = files[i].FileName;
                    files[i].SaveAs(path + "/" + fileData);
                }

                JobApplication u = new JobApplication()
                {
                    JobApplicationID = JobApplicationID,
                    Jid = Jid,
                    Uid = Uid,
                    name = name,
                    status = status,
                    shortlist = shortlist,
                  
                };

                if (fileData != null)
                {
                    u.DocumentPath = fileData;
                }
                else
                {
                    u.DocumentPath = original.DocumentPath; // Keep the original image if no file is uploaded
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


















        // For Automatically Show reject if user not meet the requirements 
        //[HttpPost]
        //public HttpResponseMessage JobFileApplicationWithFilterPost()
        //{
        //    try
        //    {
        //        var form = HttpContext.Current.Request.Form;

        //        int Jid = int.Parse(form["Jid"]);
        //        int Uid = int.Parse(form["Uid"]);
        //        string name = form["name"];
        //        string status = form["status"];
        //        string shortlist = form["shortlist"];

        //        // Checking if a JobApplication already exists with the same Jid and Uid
        //        var existingApplication = db.JobApplications
        //                                    .FirstOrDefault(s => s.Jid == Jid && s.Uid == Uid);
        //        if (existingApplication != null)
        //            return Request.CreateResponse(HttpStatusCode.OK, "Already Applied");

        //        var userEducation = db.Educations.Where(e => e.Uid == Uid).OrderByDescending(e => e.EduID).FirstOrDefault();
        //        if (userEducation == null)
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "User education not found");

        //        var job = db.Jobs.Where(j => j.Jid == Jid).FirstOrDefault();
        //        if (job == null)
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "Job not found");

        //        List<string> degreeOrder = new List<string> { "Primary", "Middle", "Matric", "Inter", "Bachelor", "Master", "PhD" };

        //        if (job.Title.ToLower() == "Teacher" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Inter"))
        //        {
        //            status = "Rejected";
        //        }

        //        var files = HttpContext.Current.Request.Files;
        //        string DocumentPath = "";

        //        if (files.Count > 0)
        //        {
        //            string path = HttpContext.Current.Server.MapPath("~/Documents");
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }

        //            var document = files[0];
        //            DocumentPath = Path.Combine(path, document.FileName);
        //            document.SaveAs(DocumentPath);
        //        }

        //        JobApplication us = new JobApplication()
        //        {
        //            Jid = Jid,
        //            Uid = Uid,
        //            name = name,
        //            status = status,
        //            shortlist = shortlist,
        //            DocumentPath = DocumentPath
        //        };

        //        JobApplication user1 = db.JobApplications.Add(us);
        //        db.SaveChanges();

        //        return Request.CreateResponse(HttpStatusCode.OK, "Applied");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        [HttpPost]
        public HttpResponseMessage JobFileApplicationWithFilterPost()
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

                if (job.Title.ToLower() == "assistant professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
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

                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        // This Code is for   many else if coditions and this code also correct 
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

                return Request.CreateResponse(HttpStatusCode.OK, "Applied");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }









        // This Code is for   many else if coditions and this code also correct 
        //[HttpPost]
        //public HttpResponseMessage JobFileApplicationWithFilterPost2()
        //{
        //    try
        //    {
        //        var form = HttpContext.Current.Request.Form;

        //        int Jid = int.Parse(form["Jid"]);
        //        int Uid = int.Parse(form["Uid"]);
        //        string name = form["name"];
        //        string shortlist = form["shortlist"];

        //        var user = db.JobApplications.Where(s => s.Jid == Jid && s.Uid == Uid).FirstOrDefault();
        //        if (user != null)
        //            return Request.CreateResponse(HttpStatusCode.OK, "Already Applied");

        //        var files = HttpContext.Current.Request.Files;
        //        string path = HttpContext.Current.Server.MapPath("~/Documents");

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        string fileData = "";

        //        var userEducation = db.Educations.Where(e => e.Uid == Uid).OrderByDescending(e => e.EduID).FirstOrDefault();
        //        if (userEducation == null)
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "User education not found");

        //        var job = db.Jobs.Where(j => j.Jid == Jid).FirstOrDefault();
        //        if (job == null)
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "Job not found");

        //        List<string> degreeOrder = new List<string> { "Primary", "Middle", "Matric", "Inter", "Bachelor", "Master", "PHD" };

        //        string status = "Pending";

        //        if (job.Title.ToLower() == "professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
        //        {
        //            status = "Rejected";
        //        }
        //        else if (job.Title.ToLower() == "assistant professor" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("PHD"))
        //        {
        //            status = "Rejected";
        //        }
        //        else if (job.Title.ToLower() == "lectruer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
        //        {
        //            status = "Rejected";
        //        }
        //        else if (job.Title.ToLower() == "junior lecturer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Bachelor"))
        //        {
        //            status = "Rejected";
        //        }
        //        else if (job.Title.ToLower() == "lab attendant" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Inter"))
        //        {
        //            status = "Rejected";
        //        }
        //        else if (job.Title.ToLower() == "guard" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Matric"))
        //        {
        //            status = "Rejected";
        //        }
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            fileData = files[i].FileName;
        //            files[i].SaveAs(path + "/" + fileData);

        //            JobApplication p = new JobApplication()
        //            {
        //                Jid = Jid,
        //                Uid = Uid,
        //                name = name,
        //                status = status,
        //                shortlist = shortlist,
        //                DocumentPath = path
        //            };

        //            db.JobApplications.Add(p);
        //            db.SaveChanges();
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, "User Added Successfully");
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}




        [HttpGet]
        public HttpResponseMessage JobApplicationsGet()
        {
            try
            {
                var jobApplications = db.JobApplications
                    .OrderBy(ja => ja.JobApplicationID)
                    .Select(ja => new
                    {
                        ja.JobApplicationID,
                        ja.Jid,
                        ja.Uid,
                        ja.name,
                        ja.status,
                        ja.shortlist,
                        ja.DocumentPath
                    })
                    .ToList();

                if (!jobApplications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Job application records not found");

                return Request.CreateResponse(HttpStatusCode.OK, jobApplications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }






        [HttpGet]
        public HttpResponseMessage JobApplicationwithidGet(int appid)
        {
            try
            {
                var jobApplications = db.JobApplications
                    .Where(ja => ja.Uid == appid)
                    .OrderBy(ja => ja.JobApplicationID)
                    .Select(ja => new
                    {
                        ja.JobApplicationID,
                        ja.Jid,
                        ja.Uid,
                        ja.name,
                        ja.status,
                        ja.shortlist,
                        ja.DocumentPath
                    })
                    .ToList();

                if (!jobApplications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Job application records not found for the specified user.");

                return Request.CreateResponse(HttpStatusCode.OK, jobApplications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public HttpResponseMessage JoinJobApplicationwithidGet(int appid)
        {
            try
            {
                var jobApplications = (from ja in db.JobApplications
                                       join j in db.Jobs on ja.Jid equals j.Jid
                                       where ja.Uid == appid
                                       orderby ja.JobApplicationID
                                       select new
                                       {
                                           ja.JobApplicationID,
                                           ja.Jid,
                                           ja.Uid,
                                           ja.name,
                                           ja.status,
                                           ja.shortlist,
                                           ja.DocumentPath,
                                           JobTitle = j.Title,
                                           JobQualification = j.qualification,
                                           JobSalary = j.Salary,
                                           JobExperience = j.experience,
                                           LastDateOfApply = j.LastDateOfApply,
                                           JobLocation = j.Location,
                                           JobDescription = j.Description,
                                           NoOfVacancies = j.noofvacancie
                                       }).ToList();

                if (!jobApplications.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Job application records not found for the specified user.");

                return Request.CreateResponse(HttpStatusCode.OK, jobApplications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        public HttpResponseMessage NewAllJobApplicationsGet()
        {
            try
            {
                var details = (from user in db.Users
                               join jobApplication in db.JobApplications on user.Uid equals jobApplication.Uid
                               join job in db.Jobs on jobApplication.Jid equals job.Jid
                               where jobApplication.status == "pending"  //phir condition lgai ha
                               select new
                               {
                                   UserUid = user.Uid,
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
                                   jobApplication.JobApplicationID,
                                   JobApplicationJid = jobApplication.Jid,
                                   JobApplicationUid = jobApplication.Uid,
                                   jobApplication.name,
                                   jobApplication.status,
                                   jobApplication.shortlist,
                                   jobApplication.DocumentPath,
                                   JobJid = job.Jid,
                                   job.Title,
                                   job.qualification,
                                   job.Salary,
                                   job.experience,
                                   job.LastDateOfApply,
                                   job.Location,
                                   job.Description,
                                   job.noofvacancie
                               }).ToList();

                if (!details.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No job application records found.");

                return Request.CreateResponse(HttpStatusCode.OK, details);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        //           Pending Job Application Ko count karnay ka liya ha Ya 
        //[HttpGet]
        //public HttpResponseMessage NewAllJobApplicationsGet()
        //{
        //    try
        //    {
        //        int pendingApplicationsCount = db.JobApplications.Count(jobApplication => jobApplication.status == "pending");

        //        var details = (from user in db.Users
        //                       join jobApplication in db.JobApplications on user.Uid equals jobApplication.Uid
        //                       join job in db.Jobs on jobApplication.Jid equals job.Jid
        //                       where jobApplication.status == "pending"
        //                       select new
        //                       {
        //                           UserUid = user.Uid,
        //                           user.Fname,
        //                           user.Lname,
        //                           user.email,
        //                           user.mobile,
        //                           user.cnic,
        //                           user.dob,
        //                           user.gender,
        //                           user.address,
        //                           user.password,
        //                           user.role,
        //                           user.image,
        //                           jobApplication.JobApplicationID,
        //                           JobApplicationJid = jobApplication.Jid,
        //                           JobApplicationUid = jobApplication.Uid,
        //                           jobApplication.name,
        //                           jobApplication.status,
        //                           jobApplication.shortlist,
        //                           jobApplication.DocumentPath,
        //                           JobJid = job.Jid,
        //                           job.Title,
        //                           job.qualification,
        //                           job.Salary,
        //                           job.experience,
        //                           job.LastDateOfApply,
        //                           job.Location,
        //                           job.Description,
        //                           job.noofvacancie
        //                       }).ToList();

        //        if (!details.Any())
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "No job application records found.");

        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            PendingApplicationsCount = pendingApplicationsCount,
        //            JobApplications = details
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        [HttpGet]
        public HttpResponseMessage AssignJobapplicationGet()
        {
            try
            {
                var details = (from user in db.Users
                               join jobApplication in db.JobApplications on user.Uid equals jobApplication.Uid
                               join job in db.Jobs on jobApplication.Jid equals job.Jid
                               where jobApplication.status == "mark"  //phir condition lgai ha
                               select new
                               {
                                   UserUid = user.Uid,
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
                                   jobApplication.JobApplicationID,
                                   JobApplicationJid = jobApplication.Jid,
                                   JobApplicationUid = jobApplication.Uid,
                                   jobApplication.name,
                                   jobApplication.status,
                                   jobApplication.shortlist,
                                   jobApplication.DocumentPath,
                                   JobJid = job.Jid,
                                   job.Title,
                                   job.qualification,
                                   job.Salary,
                                   job.experience,
                                   job.LastDateOfApply,
                                   job.Location,
                                   job.Description,
                                   job.noofvacancie
                               }).ToList();

                if (!details.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No job application records found.");

                return Request.CreateResponse(HttpStatusCode.OK, details);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }






        [HttpGet]
        public HttpResponseMessage New2AllJobApplicationsGet()
        {
            try
            {
                var details = (from user in db.Users
                               join jobApplication in db.JobApplications on user.Uid equals jobApplication.Uid
                               join job in db.Jobs on jobApplication.Jid equals job.Jid
                               join education in db.Educations on user.Uid equals education.Uid
                               where jobApplication.status == "pending" && !(education.Institute == "Bims University" || education.Institute == "Priston University") //phir condition lgai ha
                               select new
                               {
                                   UserUid = user.Uid,
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
                                   jobApplication.JobApplicationID,
                                   JobApplicationJid = jobApplication.Jid,
                                   JobApplicationUid = jobApplication.Uid,
                                   jobApplication.name,
                                   jobApplication.status,
                                   jobApplication.shortlist,
                                   jobApplication.DocumentPath,
                                   JobJid = job.Jid,
                                   job.Title,
                                   job.qualification,
                                   job.Salary,
                                   job.experience,
                                   job.LastDateOfApply,
                                   job.Location,
                                   job.Description,
                                   job.noofvacancie
                               }).Distinct().ToList();

                if (!details.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No job application records found.");

                return Request.CreateResponse(HttpStatusCode.OK, details);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }










































        [HttpGet] // With Id
        public HttpResponseMessage NewJobApplicationHrSideGet(int applicationid)
        {
            try
            {
                var jobApplications = db.JobApplications
                    .Where(e => e.JobApplicationID == applicationid)
                    .OrderBy(b => b.JobApplicationID)
                    .Join(db.Jobs, a => a.Jid, j => j.Jid, (a, j) => new
                    {
                        a.JobApplicationID,
                        a.Jid,
                        a.Uid,
                        a.name,
                        a.status,
                        a.shortlist,
                        a.DocumentPath,
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
                    .Join(db.Users, a => a.Uid, u => u.Uid, (a, u) => new
                    {
                        a.JobApplicationID,
                        a.Jid,
                        a.Uid,
                        a.name,
                        a.status,
                        a.shortlist,
                        a.DocumentPath,
                        a.Title,
                        a.qualification,
                        a.Salary,
                        a.experience,
                        a.LastDateOfApply,
                        a.Location,
                        a.Description,
                        a.noofvacancie,
                        a.jobstatus,
                        u.Fname,
                        u.Lname,
                        u.email,
                        u.mobile,
                        u.cnic,
                        u.dob,
                        u.gender,
                        u.address,
                        u.password,
                        u.role,
                        u.image
                    })
                    .GroupJoin(
                        db.Experiences,
                        a => a.Uid,
                        e => e.Uid,
                        (a, e) => new
                        {
                            a.JobApplicationID,
                            a.Jid,
                            a.Uid,
                            a.name,
                            a.status,
                            a.shortlist,
                            a.DocumentPath,
                            a.Title,
                            a.qualification,
                            a.Salary,
                            a.experience,
                            a.LastDateOfApply,
                            a.Location,
                            a.Description,
                            a.noofvacancie,
                            a.jobstatus,
                            a.Fname,
                            a.Lname,
                            a.email,
                            a.mobile,
                            a.cnic,
                            a.dob,
                            a.gender,
                            a.address,
                            a.password,
                            a.role,
                            a.image,
                            Experiences = e.DefaultIfEmpty()
                        }
                    )
                    .GroupJoin(
                        db.Educations,
                        a => a.Uid,
                        edu => edu.Uid,
                        (a, edu) => new
                        {
                            a.JobApplicationID,
                            a.Jid,
                            a.Uid,
                            a.name,
                            a.status,
                            a.shortlist,
                            a.DocumentPath,
                            a.Title,
                            a.qualification,
                            a.Salary,
                            a.experience,
                            a.LastDateOfApply,
                            a.Location,
                            a.Description,
                            a.noofvacancie,
                            a.jobstatus,
                            a.Fname,
                            a.Lname,
                            a.email,
                            a.mobile,
                            a.cnic,
                            a.dob,
                            a.gender,
                            a.address,
                            a.password,
                            a.role,
                            a.image,
                            a.Experiences,
                            Educations = edu.DefaultIfEmpty()
                        }
                    )
                    
                    .ToList();

                var formattedResponse = jobApplications.Select(a => new
                {
                    a.JobApplicationID,
                    a.Jid,
                    a.Uid,
                    a.name,
                    a.status,
                    a.shortlist,
                    a.DocumentPath,
                    a.Title,
                    a.qualification,
                    a.Salary,
                    a.experience,
                    a.LastDateOfApply,
                    a.Location,
                    a.Description,
                    a.noofvacancie,
                    a.jobstatus,
                    a.Fname,
                    a.Lname,
                    a.email,
                    a.mobile,
                    a.cnic,
                    a.dob,
                    a.gender,
                    a.address,
                    a.password,
                    a.role,
                    a.image,
                    Experiences = a.Experiences.Select(e => new
                    {
                        e.ExpID,
                        e.Company,
                        e.Startdate,
                        e.currentwork,
                        e.Enddate,
                        e.otherskill
                    }),
                    Educations = a.Educations.Select(edu => new
                    {
                        edu.EduID,
                        edu.Degree,
                        edu.major,
                        edu.Institute,
                        edu.Board,
                        EduStartdate = edu.Startdate,
                        EduEnddate = edu.Enddate
                    })
                });

                return Request.CreateResponse(HttpStatusCode.OK, formattedResponse);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




















        [HttpGet] // Get the details of the user who applied for the job
        public HttpResponseMessage NewNewJobApplicationHrSideGet(int applicationid)
        {
            try
            {
                var jobApplications = db.JobApplications
                    .Where(e => e.JobApplicationID == applicationid)
                    .OrderBy(b => b.JobApplicationID)
                    .Join(db.Jobs, a => a.Jid, j => j.Jid, (a, j) => new  //ismay a jo ha wo jobapplications table ka ha or  j job table ka dono ko j id ki base pa join kiya ha
                    {
                        a.JobApplicationID,
                        a.Jid,
                        a.Uid,
                        a.name,
                        a.status,
                        a.shortlist,
                        a.DocumentPath,
                        Job = j,
                        Experiences = db.Experiences.Where(e => e.Uid == a.Uid)
                                                    .Select(e => new
                                                    {
                                                        e.ExpID,
                                                        e.Company,
                                                        e.Startdate,
                                                        e.currentwork,
                                                        e.Enddate,
                                                        e.otherskill
                                                    })
                                                    .ToList(),
                        Educations = db.Educations.Where(edu => edu.Uid == a.Uid)
                                                  .Select(edu => new
                                                  {
                                                      edu.EduID,
                                                      edu.Degree,
                                                      edu.major,
                                                      edu.Institute,
                                                      edu.Board,
                                                      EduStartdate = edu.Startdate,
                                                      EduEnddate = edu.Enddate
                                                  })
                                                  .ToList()
                    })
                    .Join(db.Users, a => a.Uid, u => u.Uid, (a, u) => new
                    {
                        a.JobApplicationID,
                        a.Jid,
                        a.Uid,
                        a.name,
                        a.status,
                        a.shortlist,
                        a.DocumentPath,
                        a.Job,
                        a.Experiences,
                        a.Educations,
                        User = u
                    })
                    .ToList();

                var formattedResponse = jobApplications.Select(a => new
                {
                    a.JobApplicationID,
                    a.Jid,
                    a.Uid,
                    a.name,
                    a.status,
                    a.shortlist,
                    a.DocumentPath,
                    Job = new
                    {
                        a.Job.Title,
                        a.Job.qualification,
                        a.Job.Salary,
                        a.Job.experience,
                        a.Job.LastDateOfApply,
                        a.Job.Location,
                        a.Job.Description,
                        a.Job.noofvacancie,
                        a.Job.jobstatus
                    },
                    User = new
                    {
                        a.User.Fname,
                        a.User.Lname,
                        a.User.email,
                        a.User.mobile,
                        a.User.cnic,
                        a.User.dob,
                        a.User.gender,
                        a.User.address,
                        a.User.password,
                        a.User.role,
                        a.User.image
                    },
                    Experiences = a.Experiences,
                    Educations = a.Educations
                });

                return Request.CreateResponse(HttpStatusCode.OK, formattedResponse);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
