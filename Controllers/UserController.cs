
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;


using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

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


        //User Get by roles and Id

        //[HttpGet]
        //public HttpResponseMessage UserroleGet(int id)
        //{
        //    try
        //    {
        //        // Filter records by role (employee and applicant)
        //        var user = db.Users
        //            .Where(e => e.Uid == id && (e.role == "employee" || e.role == "applicant"))
        //            .OrderBy(b => b.Uid)
        //            .ToList();
        //        return Request.CreateResponse(HttpStatusCode.OK, user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}




        //user get  by only roles
        [HttpGet]
        public HttpResponseMessage UserroleGet()
        {
            try
            {
                // Filter records by role (employee and applicant)
                var users = db.Users
                    .Where(e => e.role == "employee" /*|| e.role == "applicant"*/)
                    .OrderBy(b => b.Uid)
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








        //FypDBEntities1 db = new FypDBEntities1();
        //[Route("api/Product/Addproduct")]
        //[HttpPost]
        //public HttpResponseMessage Adduser()
        //{
        //    try
        //    {
        //        var form = HttpContext.Current.Request.Form;
        //        string firstname = form["Fname"];
        //        string lasttname = form["Lname"];
        //        string email = form["email"];
        //        string mobile = form["mobile"];
        //        string cnic = form["cnic"];
        //        string dob = form["dob"];
        //        string gender = form["gender"];
        //        string address = form["address"];
        //        string password = form["password"];
        //        string role = form["role"];
        //        int qty = int.Parse(form["Qty"]);
        //        string size = form["Size"];
        //        int price = int.Parse(form["Price"]);
        //        int Vid = int.Parse(form["VendorID"]);

        //        var check = db.Accounts.Where(s => s.Status == false && s.UserType == "Vendor" && s.AcountId == Vid).FirstOrDefault();
        //        if (check != null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Your Account is on Pending");
        //        }

        //        var pro = db.Users.Where(s => s.email == email).FirstOrDefault();
        //        if (pro != null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Email Exist");
        //        }



        //        var files = HttpContext.Current.Request.Files;
        //        DateTime dt = DateTime.Now;
        //        string path = HttpContext.Current.Server.MapPath("~/Image");

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        string fileData = "";
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            fileData = files[i].FileName;
        //            files[i].SaveAs(path + "/" + fileData);
        //            User p = new User() { Fname = firstname, Lname = lasttname, email = email, mobile = mobile, cnic = cnic, dob = dob, gender = gender, address = address, password = password, role = role, image = fileData };
        //            db.Users.Add(p);
        //            db.SaveChanges();


        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, "Product Added Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
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




        //[HttpPut]
        //public HttpResponseMessage UpdateUser(User u)
        //{
        //    try
        //    {

        //        var original = db.Users.Find(u.Uid);
        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
        //        }
        //        db.Entry(original).CurrentValues.SetValues(u);
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, "Record Updated");

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        //[HttpPut]
        //public HttpResponseMessage UpdateUser()
        //{
        //    try
        //    {
        //        var form = HttpContext.Current.Request.Form;
        //        int uid = Convert.ToInt32(form["Uid"]);
        //        string Fname = form["Fname"];
        //        string Lname = form["Lname"];
        //        string email = form["email"];
        //        string mobile = form["mobile"];
        //        string cnic = form["cnic"];
        //        string dob = form["dob"];
        //        string gender = form["gender"];
        //        string address = form["address"];
        //        string password = form["password"];
        //        string role = form["role"];

        //        var original = db.Users.Find(uid);
        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
        //        }

        //        var files = HttpContext.Current.Request.Files;
        //        byte[] imageData = null;
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            var file = files[i];
        //            string fileName = file.FileName;
        //            using (var binaryReader = new BinaryReader(file.InputStream))
        //            {
        //                imageData = binaryReader.ReadBytes(file.ContentLength);
        //            }
        //        }

        //        User u = new User()
        //        {
        //            Uid = uid,
        //            Fname = Fname,
        //            Lname = Lname,
        //            email = email,
        //            mobile = mobile,
        //            cnic = cnic,
        //            dob = dob,
        //            gender = gender,
        //            address = address,
        //            password = password,
        //            role = role
        //        };

        //        if (imageData != null)
        //        {
        //            u.image = imageData;
        //        }

        //        db.Entry(original).CurrentValues.SetValues(u);
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, "Record Updated");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPut]
        //public HttpResponseMessage UpdateUser([FromBody] User user)
        //{
        //    try
        //    {
        //        var form = HttpContext.Current.Request.Form;
        //        int Uid = Convert.ToInt32(form["Uid"]);
        //        string Fname = form["Fname"];
        //        string Lname = form["Lname"];
        //        string email = form["email"];
        //        string mobile = form["mobile"];
        //        string cnic = form["cnic"];
        //        string dob = form["dob"];
        //        string gender = form["gender"];
        //        string address = form["address"];
        //        string password = form["password"];
        //        string role = form["role"];

        //        var original = db.Users.Find(Uid);
        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
        //        }

        //        var files = HttpContext.Current.Request.Files;
        //        string path = HttpContext.Current.Server.MapPath("~/image");

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        string fileData = "";
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            fileData = files[i].FileName;
        //            files[i].SaveAs(path + "/" + fileData);
        //        }

        //        User u = new User()
        //        {
        //            Uid = Uid,
        //            Fname = Fname,
        //            Lname = Lname,
        //            email = email,
        //            mobile = mobile,
        //            cnic = cnic,
        //            dob = dob,
        //            gender = gender,
        //            address = address,
        //            password = password,
        //            role = role
        //        };

        //        if (fileData != null)
        //        {
        //            u.image = fileData;
        //        }

        //        db.Entry(original).CurrentValues.SetValues(u);
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, "Record Updated");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //[Route("api/User/UpdateUser")]

        //[HttpPut]
        //public HttpResponseMessage UpdateUser(int Uid, string Fname, string Lname, string email, string mobile, string cnic, string dob, string gender, string address, string password, string role)
        //{
        //    try
        //    {
        //        var original = db.Users.Find(Uid);
        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "No record updated");
        //        }

        //        var files = HttpContext.Current.Request.Files;
        //        string path = HttpContext.Current.Server.MapPath("~/image");

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        string fileData = "";
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            fileData = files[i].FileName;
        //            files[i].SaveAs(path + "/" + fileData);
        //        }

        //        User u = new User()
        //        {
        //            Uid = Uid,
        //            Fname = Fname,
        //            Lname = Lname,
        //            email = email,
        //            mobile = mobile,
        //            cnic = cnic,
        //            dob = dob,
        //            gender = gender,
        //            address = address,
        //            password = password,
        //            role = role
        //        };

        //        if (fileData != null)
        //        {
        //            u.image = fileData;
        //        }

        //        db.Entry(original).CurrentValues.SetValues(u);
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, "Record Updated");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

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
    }
}


