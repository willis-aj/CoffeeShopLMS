using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoffeeShopLMS.DATA.EF;
using Microsoft.AspNet.Identity;
using System.Net.Mail;//added for .NET email (mail message and Smtp Client)
using System.Net;//for network credential

namespace CoffeeShopLMS.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    public class LessonsController : Controller
    {
        private CoffeeShopLMSEntities db = new CoffeeShopLMSEntities();

        // GET: Lessons
        public ActionResult Index()
        {
            var lessons = db.Lessons.Include(l => l.Cours);
            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            LessonView lessonView = new LessonView();
            lessonView.LessonID = (int)id;
            lessonView.UserID = User.Identity.GetUserId();
            DateTime today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            lessonView.DateViewed = today;

            LessonView entryHasBeenEnteredBefore = db.LessonViews.Where(x => x.LessonID == lessonView.LessonID).Where(x => x.UserID == lessonView.UserID).FirstOrDefault();
            if (entryHasBeenEnteredBefore == null)
            {
                db.LessonViews.Add(lessonView);
                db.SaveChanges();
            }


            //if the rest of the course is done
            Lesson lessonThatWasViewed = db.Lessons.Where(x => x.LessonID == lessonView.LessonID).FirstOrDefault();
            int numberOfLessonsInTheCourse = db.Lessons.Where(x => x.CourseID == lessonThatWasViewed.CourseID).Count();
            var lessonsInTheCourse = db.Lessons.Where(x => x.CourseID == lessonThatWasViewed.CourseID);
            List<Lesson> lessonsInLessonViews = new List<Lesson>();

            foreach (var l in db.LessonViews)
            {
                var lessonViewedInThisLessonView = db.Lessons.Where(x => x.LessonID == l.LessonID).FirstOrDefault();
                if (lessonsInTheCourse.Where(x => x.LessonID == l.LessonID).FirstOrDefault() != null)
                {
                    lessonsInLessonViews.Add(lessonViewedInThisLessonView);
                }
            }

            UserDet employeeViewing = db.UserDets.Where(x => x.UserID == lessonView.UserID).FirstOrDefault();
            Cours courseViewed = db.Courses.Where(x => x.CourseID == lessonThatWasViewed.CourseID).FirstOrDefault();
            string confirmationMessage = $"{employeeViewing.FirstName} {employeeViewing.LastName} has finished course {courseViewed.CourseName}";
            if (numberOfLessonsInTheCourse == lessonsInLessonViews.Count)
            {
                //string courseFinishMessage = $"Employee {lessonView.UserID} has finished a course.";
                MailMessage m = new MailMessage(
                "no-reply@abigaylewillis.com",
                "willis.aj@outlook.com",
                "Course Completion",
                confirmationMessage);
                m.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("mail.abigaylewillis.com");
                client.Credentials = new NetworkCredential("no-reply@abigaylewillis.com", "@1001w");
                client.Port = 8889;
                client.Send(m);


                CourseCompletion courseCompletion = new CourseCompletion();
                courseCompletion.CourseID = courseViewed.CourseID;
                courseCompletion.UserID = User.Identity.GetUserId();
                courseCompletion.DateCompleted = today;

                CourseCompletion courseHasBeenCompletedBefore = db.CourseCompletions.Where(x => x.CourseID == courseCompletion.CourseID).Where(x => x.UserID == courseCompletion.UserID).FirstOrDefault();
                if (courseHasBeenCompletedBefore == null)
                {
                    db.CourseCompletions.Add(courseCompletion);
                    db.SaveChanges();
                }

            }

            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }


        // GET: Lessons/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "LessonID,LessonTitle,CourseID,Introduction,VideoURL,PdfFileName,IsActive,QuizRequired,VideoRequired")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Lessons.Add(lesson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", lesson.CourseID);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", lesson.CourseID);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "LessonID,LessonTitle,CourseID,Introduction,VideoURL,PdfFileName,IsActive,QuizRequired,VideoRequired")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", lesson.CourseID);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lessons.Find(id);
            db.Lessons.Remove(lesson);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
