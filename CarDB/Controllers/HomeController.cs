using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Data.Entity;
using CarDB.Models;


namespace CarDB.Controllers
{
    /* HTTPS enabled by selecting properties (F4) on CarDB and changing to enable SSL.
    Then right click and select properties and then go to "web" and under project url we enter the
    url that shows up under the "SSL Enabled" text, i.e. "SSL URL" (from F4 properties).
    The [RequireHttps] annotation forces all requests to use SSL.
    Previously: http://localhost:52438/ under web*/
    [RequireHttps]
    public class HomeController : Controller
    {
        // The Car database entities
        private CarDatabaseEntities _db = new CarDatabaseEntities();

        // GET: /Home/Index or /Home
        public ActionResult Index()
        {
            return View(_db.CarTable.ToList());
        }

        // GET: /Home/About - requires the user to be logged in to enter
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // GET: /Home/Details/{id}
        public ActionResult Details(int id)
        {
            var carToSeeDetailsFor = (from c in _db.CarTable where c.Id == id select c).First();

            return View(carToSeeDetailsFor);
        }

        // GET: /Home/Delete/{id}
        public ActionResult Delete(int id)
        {
            var car = (from c in _db.CarTable where c.Id == id select c).First();
            _db.CarTable.Remove(car);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Home/Create 
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude ="id")] CarTable carToCreate)
        {
            if (!ModelState.IsValid)
                return View();

            //_db AddToMovieSet(movieToCreate);
            _db.CarTable.Add(carToCreate);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Home/Edit/5
        public ActionResult Edit(int id)
        {
            var carToEdit = (from c in _db.CarTable where c.Id == id select c).First();

            return View(carToEdit);
        }

        // POST: /Home/Edit/5 
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(CarTable car)
        {
            var originalCar = (from c in _db.CarTable where c.Id == car.Id select c).First();

            if (!ModelState.IsValid)
            {
                return View(originalCar);
            }

            //_db.CarTable.Add(car);
            //_db.CarTable.Attach(car);
            //var entry = _db.Entry(car);
            //entry.State = EntityState.Modified;
            originalCar.Brand = car.Brand;
            originalCar.Year = car.Year;
            originalCar.Description = car.Description;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Home/Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}