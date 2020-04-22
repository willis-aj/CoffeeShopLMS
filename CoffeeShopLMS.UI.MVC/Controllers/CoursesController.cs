using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoffeeShopLMS.DATA.EF;

namespace CoffeeShopLMS.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    public class CoursesController : Controller
    {
        private CoffeeShopLMSEntities db = new CoffeeShopLMSEntities();

        // GET: Courses
        public ActionResult Index()
        {
            var activeCourses = db.Courses.Where(x => x.IsActive).ToList();
            return View(activeCourses);
        }

        // GET: Courses
        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            return View(db.Courses.ToList());
        }

        //// GET: Courses/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //Cours cours = db.Courses.Find(id);

        //    var lessonsInCourse = db.Courses.Where(x => x.CourseID == id).ToList();
        //    if (lessonsInCourse == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    //return View(cours);
        //    return View(lessonsInCourse);

        //}
        // GET: Courses/Details/5
        public ActionResult DetailsList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Cours cours = db.Courses.Find(id);

            var lessonsInCourse = db.Lessons.Where(x => x.CourseID == id).ToList();
            Cours courseThisIsDetailsFor = db.Courses.Where(x => x.CourseID == id).FirstOrDefault();
            ViewBag.Title = courseThisIsDetailsFor.CourseName;
            if (lessonsInCourse == null)
            {
                return HttpNotFound();
            }
            //return View(cours);
            return View(lessonsInCourse);

        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CourseID,CourseName,CourseDescription,IsActive")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(cours);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cours);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CourseID,CourseName,CourseDescription,IsActive")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cours).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cours);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cours cours = db.Courses.Find(id);
            db.Courses.Remove(cours);
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
