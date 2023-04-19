using System;
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

        [HttpPost]
        public HttpResponseMessage Signup()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;
                string Fname = form["Fname"];
                string Lname = form["Lname"];
                string email = form["email"];
                
                string mobile = form["mobile"];
                string cnic = form["cnic"];
                //int cnic = int.Parse(form["cnic"]);
                DateTime dob = DateTime.Parse(form["dob"]);
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
                // DateTime dt = DateTime.Now;
                string path = HttpContext.Current.Server.MapPath("/Content/Images");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                byte[] imageData = null;
                for (int i = 0; i < files.Count; i++)
                {
                    string fileName = files[i].FileName;
                    files[i].SaveAs(path + "/" + fileName);

                    using (var binaryReader = new BinaryReader(files[i].InputStream))
                    {
                        imageData = binaryReader.ReadBytes(files[i].ContentLength);
                    }
                    User p = new User() { Fname = Fname, Lname = Lname, email = email, mobile = mobile, cnic = cnic, dob = dob, gender = gender, address = address, password = password, role = role, image = imageData };
                    db.Users.Add(p);
                    db.SaveChanges();


                }
                return Request.CreateResponse(HttpStatusCode.OK, "Product Added Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //[HttpPost]
        //public HttpResponseMessage Signup(User newuser)
        //{
        //    try
        //    {
        //        var user = db.Users.Where(s => s.email == newuser.email).FirstOrDefault();
        //        if (user != null)
        //            return Request.CreateResponse(HttpStatusCode.OK, "Exsist");

        //        User user1 = db.Users.Add(newuser);
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, "Created");

        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);
        //        var fullErrorMessage = string.Join("; ", errorMessages);
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, exceptionMessage);
        //    }

        //}

        [HttpPut]
        public HttpResponseMessage UpdateUser(User u)
        {
            try
            {

                var original = db.Users.Find(u.Uid);
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





        [HttpGet]
        public HttpResponseMessage SearchUser(string u)
        {
            try
            {

                var search = db.Users.Where(b => b.Fname == u).OrderBy(b => b.Uid).ToList();

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


