﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BuildingProject.DataAccess;
using BuildingProject.Model;

namespace BuildingProject.Controllers
{
    public class ApartmentsController : Controller
    {
        private BuildingContext db = new BuildingContext();

        // GET: Apartments
        public ActionResult Index(int id)
        {
            try
            {
                if (DataUtil.Validation())
                {
                    var apartment = db.Apartment.Include(a => a.section).Include(a => a.section.building);
                    var result = apartment.ToList().Where(x => x.sectionID == id).ToList();
                    string sectionName = "";
                    string buildingName = "";
                    if (result.Count > 0)
                    {
                        sectionName = result[0].section.name;
                        buildingName = result[0].section.building.name;
                    }
                    ViewBag.BuildingName = buildingName;
                    ViewBag.SectionName = sectionName;
                    ViewBag.SectionId = id;
                    return View(result);
                }
                else
                    return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                Error objError = new Error();
                objError.page = "Apartments";
                objError.option = "Index";
                objError.date = DateTime.Now;
                objError.description = ex.Message;
                BaseDataAccess<Error> baseDataAccess = new BaseDataAccess<Error>();
                baseDataAccess.Insert(objError);
                return RedirectToAction("Error", "Home");
            }
        }

       // GET: Apartments/Create
        public ActionResult Create(int id)
        {
            try { 
            if (DataUtil.Validation())
            {
                Apartment apartment = new Apartment();
                apartment.sectionID = id;
                return PartialView(apartment);
            }
            else
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                Error objError = new Error();
                objError.page = "Apartments";
                objError.option = "Create-1";
                objError.date = DateTime.Now;
                objError.description = ex.Message;
                BaseDataAccess<Error> baseDataAccess = new BaseDataAccess<Error>();
                baseDataAccess.Insert(objError);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Apartments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "apartmentID,sectionID,name,active,createDate,createUser,updateDate,updateUser")] Apartment apartment)
        {
            try { 
            if (DataUtil.Validation())
            {
                if (ModelState.IsValid)
                {
                    apartment.createDate = DateTime.Now;
                    apartment.createUser = Helper.GetCurrentUser().userID;
                    db.Apartment.Add(apartment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.sectionID = new SelectList(db.Section, "sectionID", "name", apartment.sectionID);
                return View(apartment);
            }
            else
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                Error objError = new Error();
                objError.page = "Apartments";
                objError.option = "Create-2";
                objError.date = DateTime.Now;
                objError.description = ex.Message;
                BaseDataAccess<Error> baseDataAccess = new BaseDataAccess<Error>();
                baseDataAccess.Insert(objError);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Apartments/Edit/5
        public ActionResult Edit(int? id)
        {
            try { 
            if (DataUtil.Validation())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Apartment apartment = db.Apartment.Include(a=>a.section).FirstOrDefault(a=>a.apartmentID==id);
                
                if (apartment == null)
                {
                    return HttpNotFound();
                }
                //ViewBag.sectionID = new SelectList(db.Section, "sectionID", "name", apartment.sectionID);

                ViewBag.SectionName = apartment.section.name;
                return PartialView(apartment);
            }
            else
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                Error objError = new Error();
                objError.page = "Apartments";
                objError.option = "Edit-1";
                objError.date = DateTime.Now;
                objError.description = ex.Message;
                BaseDataAccess<Error> baseDataAccess = new BaseDataAccess<Error>();
                baseDataAccess.Insert(objError);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Apartments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "apartmentID,sectionID,name,active,createDate,createUser,updateDate,updateUser")] Apartment apartment)
        {
            try
            {
                if (DataUtil.Validation())
                {
                    if (ModelState.IsValid)
                    {
                        apartment.updateDate = DateTime.Now;
                        apartment.updateUser = Helper.GetCurrentUser().userID;
                        db.Entry(apartment).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", new { id = apartment.sectionID });
                    }
                    //ViewBag.sectionID = new SelectList(db.Section, "sectionID", "name", apartment.sectionID);
                    ViewBag.BuildingName = apartment.section.name;
                    return View(apartment);
                }
                else
                    return RedirectToAction("Login", "Home");            
            }
            catch (Exception ex)
            {
                Error objError = new Error();
                objError.page = "Apartments";
                objError.option = "Edit-2";
                objError.date = DateTime.Now;
                objError.description = ex.Message;
                BaseDataAccess<Error> baseDataAccess = new BaseDataAccess<Error>();
                baseDataAccess.Insert(objError);
                return RedirectToAction("Error", "Home");
    }
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
