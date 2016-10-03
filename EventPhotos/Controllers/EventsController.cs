using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventPhotos.Models;

namespace EventPhotos.Controllers
{
    public class EventsController : Controller
    {
        private EventPhotosContext db = new EventPhotosContext();

        private void saveChangesInContext(Action<EventPhotosContext> saveBlock)
        {
            using (EventPhotosContext context = new EventPhotosContext())
            {
                saveBlock(context);
            }
        }

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Album
        public ActionResult Album(Guid? id)
        {
           if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        [HttpPost]
        public ActionResult SaveUploadedFile(Guid? id)
        {
            var eventID = Request.UrlReferrer.Segments.Last<String>();
            var guid = Guid.Parse(eventID);
            if (guid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(guid);
            if (@event == null)
            {
                return HttpNotFound();
            }

            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here

                    if (file != null && file.ContentLength > 0)
                    {
                        this.saveChangesInContext((context) => {

                            Event currentEvent = context.Events.Find(guid);
                            //Creating and saving new Photo
                            Photo photo = context.Photos.Create();
                            photo.CreatedAt = DateTime.Now;
                            photo.Name = file.FileName;
                            photo.Event = currentEvent;

                            file.SaveAs(Server.MapPath(photo.photoPath()));
                            
                            context.Photos.Add(photo);
                            context.SaveChanges();
                        });
                  
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully != false)
                return Json(new { Message = fName });
            else
                return Json(new { Message = "Error in saving file" });
        }


        // GET: Events/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = (new EventPhotosContext()).Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            SelectList SelectingClubs = new SelectList(db.Clubs, "Id", "Name");
            ViewBag.Clubs = SelectingClubs;
            return View();
        }

        // POST: Events/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Name,Date,Place,Club,Type")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.EventID = Guid.NewGuid();
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,Name,Date,Place,Club,Type")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            this.saveChangesInContext((context) =>
            {
                Event currentEvent = context.Events.Find(id);
                List<Photo> photos = currentEvent.Photos.ToList<Photo>();
                photos.ForEach(photo => context.Photos.Remove(photo));
                context.Events.Remove(currentEvent);
                context.SaveChanges();
            });
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
