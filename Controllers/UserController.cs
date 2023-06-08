
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;

namespace HrmPractise02.Controllers
{
    public class UserController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();
        [HttpGet]
        public HttpResponseMessage Login(string email, string password)
        {


            try
            {

                var user = db.Users.Where(b => b.email == email && b.password == password).FirstOrDefault();

                //if (login == null)
                //{
                //    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found");

                //}
                return Request.CreateResponse(HttpStatusCode.OK, user);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        //
        //
        //
        //

        [HttpPost]
        public HttpResponseMessage Signup(User newuser)
        {
            try
            {
                var user = db.Users.Where(s => s.email == newuser.email).FirstOrDefault();
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Email Exsist");

                User user1 = db.Users.Add(newuser);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Created");

            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, exceptionMessage);
            }

        }




        //
        //
        //
        //
        //


        [HttpGet]
        public HttpResponseMessage UserGet(int id)
        {
            //select *from user
            try
            {
                var user = db.Users.Where(e => e.Uid == id).OrderBy(b => b.Uid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, user);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public class UserDTO
        {
            public int Uid { get; set; }
            public string Fname { get; set; }
            public string Lname { get; set; }
            // Include other properties you want to return

            public ICollection<Education> Educations { get; set; }
            public ICollection<Experience> Experiences { get; set; }
        }

        [HttpGet]
        public HttpResponseMessage User2Get(int id)
        {
            try
            {
                var user = db.Users
                             .Include(u => u.Educations)
                             .Include(u => u.Experiences)
                             .SingleOrDefault(e => e.Uid == id);

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
                }

                var userDto = new UserDTO
                {
                    Uid = user.Uid,
                    Fname = user.Fname,
                    Lname = user.Lname,
                    // Map other properties

                    Educations = user.Educations,
                    Experiences = user.Experiences,
                };

                return Request.CreateResponse(HttpStatusCode.OK, userDto);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        [HttpGet]
public HttpResponseMessage UserroleGet()
{
    try
    {
        var users = db.Users
            .Where(u => u.role == "employee")
            .OrderBy(u => u.Uid)
            .Select(u => new
            {
                u.Uid,
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
            .ToList();

        return Request.CreateResponse(HttpStatusCode.OK, users);
    }
    catch (Exception ex)
    {
        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
    }
}




        [HttpGet]
        public HttpResponseMessage HodroleGet()
        {
            try
            {
                var users = db.Users
                    .Where(u => u.role == "hod")
                    .OrderBy(u => u.Uid)
                    .Select(u => new
                    {
                        u.Uid,
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
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // ALl User gets by roles

        [HttpGet]
        public HttpResponseMessage UserbyroleGet()
        {
            try
            {
                // Filter records by role (employee and applicant)
                var users = db.Users
                    .Where(e => e.role == "employee" || e.role == "applicant")
                    .OrderBy(b => b.Uid)
                    .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Committe members get

        [HttpGet]
        public HttpResponseMessage CommittemembersGet()
        {
            try
            {
                // Filter records by role (employee and applicant)
                var users = db.Users
                    .Where(e => e.role == "comemployee" /*|| e.role == "applicant"*/)
                    .OrderBy(b => b.Uid)
                    .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        public HttpResponseMessage AlluserGet()
        {
            //select *from user
            try
            {
                var user = db.Users.OrderBy(b => b.Uid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, user);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("api/User/Adduser")]
        [HttpPost]
        public HttpResponseMessage Adduser()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;
                string Fname = form["Fname"];
                string Lname = form["Lname"];
                string email = form["email"];
                string mobile = form["mobile"];
                string cnic = form["cnic"];
                string dob = form["dob"];
                string gender = form["gender"];
                string address = form["address"];
                string password = form["password"];
                string role = form["role"];

                var pro = db.Users.Where(s => s.email == email).FirstOrDefault();
                if (pro != null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Email Exist");
                }

                var files = HttpContext.Current.Request.Files;
                string path = HttpContext.Current.Server.MapPath("~/image");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileData = "";
                for (int i = 0; i < files.Count; i++)
                {
                    fileData = files[i].FileName;
                    files[i].SaveAs(path + "/" + fileData);

                    User p = new User()
                    {
                        Fname = Fname,
                        Lname = Lname,
                        email = email,
                        mobile = mobile,
                        cnic = cnic,
                        dob = dob,
                        gender = gender,
                        address = address,
                        password = password,
                        role = role,
                        image = fileData
                    };

                    db.Users.Add(p);
                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "User Added Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }





        [Route("api/User/UpdateUser")]
        [HttpPut]
        public HttpResponseMessage UpdateUser()
        {
            try
            {
                int Uid = Convert.ToInt32(HttpContext.Current.Request.Form["Uid"]);
                string Fname = HttpContext.Current.Request.Form["Fname"];
                string Lname = HttpContext.Current.Request.Form["Lname"];
                string email = HttpContext.Current.Request.Form["email"];
                string mobile = HttpContext.Current.Request.Form["mobile"];
                string cnic = HttpContext.Current.Request.Form["cnic"];
                string dob = HttpContext.Current.Request.Form["dob"];
                string gender = HttpContext.Current.Request.Form["gender"];
                string address = HttpContext.Current.Request.Form["address"];
                string password = HttpContext.Current.Request.Form["password"];
                string role = HttpContext.Current.Request.Form["role"];

                var original = db.Users.Find(Uid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
                }

                var files = HttpContext.Current.Request.Files;
                string path = HttpContext.Current.Server.MapPath("~/image");

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

                User u = new User()
                {
                    Uid = Uid,
                    Fname = Fname,
                    Lname = Lname,
                    email = email,
                    mobile = mobile,
                    cnic = cnic,
                    dob = dob,
                    gender = gender,
                    address = address,
                    password = password,
                    role = role
                };

                if (fileData != null)
                {
                    u.image = fileData;
                }
                else
                {
                    u.image = original.image; // Keep the original image if no file is uploaded
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




        [HttpPut]
        public HttpResponseMessage UpdateUsserAndAttributeReaminssame(User u)
        {
            try
            {
                var original = db.Users.Find(u.Uid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
                }

                // Exclude DocumentPath from being updated
                u.Fname = original.Fname;
                u.Lname = original.Lname;
                u.email = original.email;
                u.mobile = original.mobile;
                u.cnic = original.cnic;
                u.dob = original.dob;
                u.gender = original.gender;
                u.address = original.address;
                u.password = original.password;
                u.image = original.image;

                db.Entry(original).CurrentValues.SetValues(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Record Updated");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public HttpResponseMessage SearchUser(string u)
        {
            try
            {

                var search = db.Users.Where(b => b.Fname == u || b.Lname==u).OrderBy(b => b.Uid).ToList();

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






























        // Api new Functions  Start From here




        [HttpGet]
        public HttpResponseMessage NewLogin(string email, string password)
        {
            try
            {
                var user = db.Users
                    .Where(b => b.email == email && b.password == password)
                    .Select(b => new
                    {
                        b.Uid,
                        b.Fname,
                        b.Lname,
                        b.email,
                        b.mobile,
                        b.cnic,
                        b.dob,
                        b.gender,
                        b.address,
                        b.role,
                        b.image
                    })
                    .FirstOrDefault();

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

















        [HttpGet]
        public HttpResponseMessage NewUserGet(int id)
        {
            try
            {
                var user = db.Users
                    .Where(u => u.Uid == id)
                    .Select(u => new
                    {
                        u.Uid,
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
                    .SingleOrDefault();

                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }











        [HttpGet]
        public HttpResponseMessage GetAllUserDetails(int uid)
        {
            try
            {
                var details = (from user in db.Users
                               join jobApplication in db.JobApplications on user.Uid equals jobApplication.Uid
                               join job in db.Jobs on jobApplication.Jid equals job.Jid
                               join education in db.Educations on user.Uid equals education.Uid
                               join experience in db.Experiences on user.Uid equals experience.Uid
                               where user.Uid == uid
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
                                   job.noofvacancie,
                                   EducationEduID = education.EduID,
                                   education.Degree,
                                   education.major,
                                   education.Institute,
                                   education.Board,
                                   Education_Startdate = education.Startdate,
                                   Education_Enddate = education.Enddate,
                                   ExperienceExpID = experience.ExpID,
                                   experience.Company,
                                   Experience_Title = experience.Title,
                                   Experience_Startdate = experience.Startdate,
                                   experience.currentwork,
                                   Experience_Enddate = experience.Enddate,
                                   experience.otherskill
                               }).ToList();

                if (!details.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No records found for the specified user ID.");

                return Request.CreateResponse(HttpStatusCode.OK, details);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }







        [HttpGet]
        public HttpResponseMessage NewGetAllUserDetails(int uid)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false; // Turn off lazy loading for this query

                var details = (from user in db.Users
                               join jobApplication in db.JobApplications on user.Uid equals jobApplication.Uid
                               join job in db.Jobs on jobApplication.Jid equals job.Jid
                               join education in db.Educations on user.Uid equals education.Uid
                               join experience in db.Experiences on user.Uid equals experience.Uid
                               where user.Uid == uid
                               select new
                               {
                                   User = user,
                                   JobApplication = jobApplication,
                                   Job = job,
                                   Education = education,
                                   Experience = experience
                               }).ToList();

                db.Configuration.ProxyCreationEnabled = true; // Turn on lazy loading again for other queries

                if (!details.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No records found for the specified user ID.");

                return Request.CreateResponse(HttpStatusCode.OK, details);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }













    }
}


