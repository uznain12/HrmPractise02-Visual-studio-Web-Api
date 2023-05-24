using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrmPractise02.Controllers
{
    public class AttendanceController : ApiController
    {
        PracticeHrmDBEntities db = new PracticeHrmDBEntities();

        [HttpGet]  //Based On Uid
        public HttpResponseMessage AttendanceGet(int uid)
        {
            //select *from user
            try
            {
                var edu = db.Attendances.Where(e => e.Uid == uid).OrderBy(b => b.date).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //based on uid and attendance id 
        [HttpGet]  //Based On Uid
        public HttpResponseMessage AttendanceWithuidandAtendidGet(int uid,int attendid)
        {
            //select *from user
            try
            {
                var edu = db.Attendances.Where(e => e.Uid == uid && e.Attendanceid==attendid).OrderBy(b => b.Attendanceid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]  //Based On Uid
        public HttpResponseMessage WithDateAndIDAttendanceGet(int uid,DateTime Date)
        {
            //select *from user
            try
            {
                var edu = db.Attendances.Where(e => e.Uid == uid && e.date==Date).OrderBy(b => b.Attendanceid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]   // get All Attendance
        public HttpResponseMessage AllAttendanceGet()
        {
            //select *from user
            try
            {
                var edu = db.Attendances.OrderBy(b => b.Attendanceid).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, edu);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        //filter by date     [HttpGet]   // get All Attendance
        [HttpGet]   // get All Attendance
        public HttpResponseMessage AlldateAttendanceGet()
        {
            try
            {
                var attendances = db.Attendances.ToList();

                var uniqueDates = attendances
                    .Select(a => a.date)
                    .Distinct()
                    .ToList();

                var attendanceRecords = new List<object>();
                foreach (var date in uniqueDates)
                {
                    var records = attendances
                        .Where(a => a.date.HasValue && a.date.Value.Date == date)
                        .Select(a => new
                        {
                            checkin = a.checkin,
                            checkout = a.checkout,
                            status = a.status
                        })
                        .ToList();

                    var attendanceObject = new
                    {
                        date = date?.ToString("yyyy-MM-dd"),
                        records = records
                    };

                    attendanceRecords.Add(attendanceObject);
                }

                return Request.CreateResponse(HttpStatusCode.OK, attendanceRecords);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // get only selected date
        // Get Attandence by dates with user id 
        [HttpGet]   // get All Attendance
        public HttpResponseMessage ByDateAttendanceGet(DateTime Date)
        {
            try
            {
                var attendances = db.Attendances.Where(e => e.date == Date).OrderBy(b => b.Attendanceid).ToList();


                var uniqueDates = attendances
                    .Select(a => a.date)
                    .Distinct()
                    .ToList();

                var attendanceRecords = new List<object>();
                foreach (var date in uniqueDates)
                {
                    var records = attendances
                        .Where(a => a.date.HasValue && a.date.Value.Date == date)
                         .Join(db.Users, a => a.Uid, u => u.Uid, (a, u) => new
                         {
                             Fname = u.Fname,
                             Uid = a.Uid,
                             checkin = a.checkin,
                             checkout = a.checkout,
                             status = a.status
                         })
                        .ToList();

                    var attendanceObject = new
                    {
                        date = date?.ToString("yyyy-MM-dd"),
                        records = records
                    };

                    attendanceRecords.Add(attendanceObject);
                }

                return Request.CreateResponse(HttpStatusCode.OK, attendanceRecords);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        //
        //

        [HttpGet]
        public HttpResponseMessage NewDateAttendanceGet(DateTime Date)
        {
            try
            {
                var attendances = db.Attendances.Where(e => e.date == Date).OrderBy(b => b.Attendanceid).ToList();

                var uniqueDates = attendances
                    .Select(a => a.date)
                    .Distinct()
                    .ToList();

                var attendanceRecords = new List<object>();
                foreach (var date in uniqueDates)
                {
                    var uniqueStatuses = attendances
                        .Where(a => a.date.HasValue && a.date.Value.Date == date)
                        .Select(a => a.status)
                        .Distinct()
                        .ToList();

                    var recordsByStatus = new List<object>();
                    foreach (var status in uniqueStatuses)
                    {
                        var records = attendances
                            .Where(a => a.date.HasValue && a.date.Value.Date == date && a.status == status)
                            .Join(db.Users, a => a.Uid, u => u.Uid, (a, u) => new
                            {
                                Fname = u.Fname,
                                Uid = a.Uid,
                                checkin = a.checkin,
                                checkout = a.checkout,
                                status = a.status
                            })
                            .ToList();

                        var statusObject = new
                        {
                            status = status,
                            records = records
                        };

                        recordsByStatus.Add(statusObject);
                    }

                    var attendanceObject = new
                    {
                        date = date?.ToString("yyyy-MM-dd"),
                        recordsByStatus = recordsByStatus
                    };

                    attendanceRecords.Add(attendanceObject);
                }

                return Request.CreateResponse(HttpStatusCode.OK, attendanceRecords);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        ///

        [HttpGet]   // get All Attendance
        public HttpResponseMessage ByDatewithuserAttendanceGet(DateTime Date)
        {
            try
            {
                var attendances = db.Attendances.Where(e => e.date == Date).OrderBy(b => b.Attendanceid).ToList();
                var users = db.Users.OrderBy(b => b.Uid).ToList();

                if (!users.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No users found.");
                }

                var usersWithAttendance = new List<object>();

                foreach (var user in users)
                {
                    var records = attendances
                        .Where(a => a.Uid == user.Uid)
                        .Select(a => new
                        {
                            Fname = user.Fname,
                            Uid = a.Uid,
                            checkin = a.checkin,
                            checkout = a.checkout,
                            status = a.status
                        })
                        .ToList();

                    var userWithAttendance = new
                    {
                        user = new
                        {
                            Uid = user.Uid,
                            Fname = user.Fname,
                            Lname = user.Lname,
                            email = user.email,
                            role = user.role,
                            mobile = user.mobile,
                            cnic = user.cnic,
                            dob = user.dob,
                            gender = user.gender,
                            address = user.address,
                            image = user.image,
                        },
                        attendanceRecords = records
                    };

                    usersWithAttendance.Add(userWithAttendance);
                }

                return Request.CreateResponse(HttpStatusCode.OK, usersWithAttendance);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //

        /// <summary>
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>



        // Get Attandence by dates with user id 
        [HttpGet]   // get All Attendance
        public HttpResponseMessage AlldatewithidAttendanceGet(int uid)
        {
            try
            {
                var attendances = db.Attendances.Where(e => e.Uid == uid).OrderBy(b => b.Attendanceid).ToList();
                var user = db.Users.FirstOrDefault(u => u.Uid == uid);

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"User with ID {uid} not found.");
                }
                var uniqueDates = attendances
                    .Select(a => a.date)
                    .Distinct()
                    .ToList();

                var attendanceRecords = new List<object>();
                foreach (var date in uniqueDates)
                {
                    var records = attendances
                        .Where(a => a.date.HasValue && a.date.Value.Date == date)
                        .Join(db.Users, a => a.Uid, u => u.Uid, (a, u) => new
                        {
                            Fname = u.Fname,
                            Uid = a.Uid,
                            checkin = a.checkin,
                            checkout = a.checkout,
                            status = a.status
                        })
                        .ToList();

                    var attendanceObject = new
                    {
                        date = date?.ToString("yyyy-MM-dd"),
                        records = records
                    };

                    attendanceRecords.Add(attendanceObject);
                }
                var result = new
                {
                    user = new
                    {
                        Uid = user.Uid,
                        Fname = user.Fname,
                        Lname = user.Lname,
                        email = user.email,
                        role = user.role,
                        mobile = user.mobile,
                        cnic = user.cnic,
                        dob = user.dob,
                        gender = user.gender,
                        address = user.address,
                        image = user.image,
                    },
                    attendanceRecords = attendanceRecords
                };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        public HttpResponseMessage AttendancePost(Attendance u)   // ya wala function value insert karnay ka liya bnaya or httpresppnsemesseage return type ha
        {
            try
            {
                //Insert Into User Table
                var attend = db.Attendances.Add(u);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, u.date + " " + "Record Inserted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateAttendance(Attendance u)
        {
            try
            {

                var original = db.Attendances.Find(u.Attendanceid);
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







        [HttpGet]
        public HttpResponseMessage NewAttendanceGet(int uid)
        {
            try
            {
                var attendance = db.Attendances
                    .Where(a => a.Uid == uid)
                    .OrderBy(a => a.date)
                    .Select(a => new
                    {
                        a.Attendanceid,
                        a.Uid,
                        a.date,
                        a.checkin,
                        a.status,
                        a.checkout
                    })
                    .ToList();

                if (!attendance.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Attendance records not found for the specified user.");

                return Request.CreateResponse(HttpStatusCode.OK, attendance);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }






        [HttpGet]
        public HttpResponseMessage NewAttendanceWithuidandAtendidGet(int uid, int attendid)
        {
            try
            {
                var attendance = db.Attendances
                    .Where(a => a.Uid == uid && a.Attendanceid == attendid)
                    .OrderBy(a => a.Attendanceid)
                    .Select(a => new
                    {
                        a.Attendanceid,
                        a.Uid,
                        a.date,
                        a.checkin,
                        a.status,
                        a.checkout
                    })
                    .ToList();

                if (!attendance.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Attendance records not found for the specified user and attendance ID.");

                return Request.CreateResponse(HttpStatusCode.OK, attendance);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        [HttpGet]
        public HttpResponseMessage NewWithDateAndIDAttendanceGet(int uid, DateTime Date)
        {
            try
            {
                var attendance = db.Attendances
                    .Where(a => a.Uid == uid && a.date == Date )
                    .OrderBy(a => a.Attendanceid)
                    .Select(a => new
                    {
                        a.Attendanceid,
                        a.Uid,
                        a.date,
                        a.checkin,
                        a.status,
                        a.checkout
                    })
                    .ToList();

                if (!attendance.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Attendance records not found for the specified user and date.");

                return Request.CreateResponse(HttpStatusCode.OK, attendance);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}