using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Search_n_View.ViewModels;
using Search_n_View.Models;
using System.Threading.Tasks;

namespace Search_n_View.Controllers
{
    public class DepartmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            try
            {
                var depts = db.MainDepartment.OrderByDescending(x => x.Id).Where(x=>x.IsDeleted == false).ToList();

                var dep1 = new List<DepartmentViewModel>();
                for (int i = 0; i < depts.Count; i++)
                {
                    var dep = new DepartmentViewModel();

                    dep.Id = depts[i].Id;
                    dep.Name = depts[i].Name;
                    dep.AddedTime = depts[i].AddedTime;
                    dep.ModifiedTime = depts[i].ModifiedTime;
                    dep.CreatedBy = depts[i].AddedBy;
                    dep.ModifiedBy = depts[i].ModifiedBy;
                    dep1.Add(dep);
                }

                var model = new ManageDepartmentViewModel();
                model.Department = dep1;
                return View(model);
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DepartmentController", "Index", ex.Message, ex.Source, ex.StackTrace);
                return View(new ManageDepartmentViewModel());
            }
        }

        [Authorize(Roles = "PowerAdmin, Admin")]
        public ActionResult Create()
        {
            try
            {
                var model = new DepartmentViewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DepartmentController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View(new DepartmentViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DepartmentViewModel dept)
        {
            try
            {
                var model = new MainDepartment();

                if (ModelState.IsValid)
                {
                    string CurrentUserName = User.Identity.Name;

                    var doc = db.MainDepartment.Where(x => x.Id == dept.Id).Where(x => x.IsDeleted == false).FirstOrDefault();
                    if (doc == null)
                    {
                        model.Name = dept.Name;
                        model.AddedTime = DateTime.Now;
                        model.ModifiedTime = DateTime.Now;
                        model.AddedBy = CurrentUserName;
                        model.ModifiedBy = CurrentUserName;

                        db.MainDepartment.Add(model);
                        await db.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Message"] = "Department Already Exist!";
                        return View(dept);

                    }
                }
                else
                    return View(dept);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DepartmentController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View(new DepartmentViewModel());
            }
        }


        [Authorize(Roles = "PowerAdmin, Admin")]
        public ActionResult Edit(EditDepartmentIdViewModel mod)
        {
            try
            {
                var dept = db.MainDepartment.Find(mod.Id);
                var model = new EditDepartmentIdViewModel();

                model.Name = dept.Name;
                model.AddedTime = dept.AddedTime;
                model.ModifiedTime = dept.ModifiedTime;
                model.CreatedBy = dept.AddedBy;
                model.ModifiedBy = dept.ModifiedBy;

                return View(model);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DepartmentController", "Edit", ex.Message, ex.Source, ex.StackTrace);
                return View(new EditDepartmentIdViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditDepartmentViewModel dep)
        {
            try
            {
                string CurrentUserName = User.Identity.Name;
                var dept = db.MainDepartment.Find(dep.Id);
                var model = new EditDepartmentIdViewModel();
                model.Name = dep.Name;

                if (ModelState.IsValid)
                {
                    dept.Name = dep.Name;
                    dept.ModifiedTime = DateTime.Now;
                    dept.ModifiedBy = CurrentUserName;

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                    return View(model);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DepartmentController", "Edit", ex.Message, ex.Source, ex.StackTrace);
                return View(new EditDepartmentViewModel());
            }
        }

      
    }
}