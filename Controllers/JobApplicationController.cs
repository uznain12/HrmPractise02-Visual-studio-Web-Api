
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
                else if (job.Title.ToLower() == "lectruer" && degreeOrder.IndexOf(userEducation.Degree) < degreeOrder.IndexOf("Master"))
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




    }
}
