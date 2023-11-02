using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Search_n_View.ViewModels;
using Search_n_View.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Search_n_View.Controllers
{
    public class ItemTypeController : Controller
    {
        // GET: ItemType
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            try
            {
                var ITypes = db.ItemType.OrderByDescending(x => x.Id).Where(x => x.IsDeleted == false).ToList();

                var ItVM = new List<ItemTypeViewModel>();
                for (int i = 0; i < ITypes.Count; i++)
                {
                    var It = new ItemTypeViewModel();

                    It.Id = ITypes[i].Id;
                    It.Name = ITypes[i].Name;
                    It.DepartmentName = ITypes[i].MainDepartment.Name;
                    It.AddedTime = ITypes[i].AddedTime;
                    It.ModifiedTime = ITypes[i].ModifiedTime;
                    It.CreatedBy = ITypes[i].AddedBy;
                    It.ModifiedBy = ITypes[i].ModifiedBy;
                    ItVM.Add(It);
                }

                var model = new ManageItemTypeViewModel();
                model.ItemType = ItVM;
                return View(model);
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("ItemTypeController", "Index", ex.Message, ex.Source, ex.StackTrace);
                return View();
            }
        }

        [Authorize(Roles = "PowerAdmin, Admin")]
        public ActionResult Create()
        {
            try
            {
                var model = new ItemTypeViewModel();
                var depts = db.MainDepartment.ToList();
                model.Departments = depts.Select(s => new SelectListItem { Value = Convert.ToString(s.Id), Text = s.Name }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("ItemTypeController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View(new ItemTypeViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ItemTypeViewModel IType)
        {
            try
            {
                var model = new ItemType();

                if (ModelState.IsValid)
                {
                    string CurrentUserName = User.Identity.Name;

                    var doc = db.ItemType.Where(x => x.Name == IType.Name.Trim()).Where(x => x.IsDeleted == false).FirstOrDefault();
                    if (doc == null)
                    {
                        model.Name = IType.Name;
                        model.MdId = IType.MdId;
                        model.AddedTime = DateTime.Now;
                        model.ModifiedTime = DateTime.Now;
                        model.AddedBy = CurrentUserName;
                        model.ModifiedBy = CurrentUserName;

                        db.ItemType.Add(model);
                        await db.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var depts = db.MainDepartment.ToList();
                        IType.Departments = depts.Select(s => new SelectListItem { Value = Convert.ToString(s.Id), Text = s.Name }).ToList();
                        ViewData["Message"] = "Item Type Already Exist!";
                        return View(IType);

                    }
                }
                else
                {
                    var depts = db.MainDepartment.ToList();
                    IType.Departments = depts.Select(s => new SelectListItem { Value = Convert.ToString(s.Id), Text = s.Name }).ToList();
                    return View(IType);
                }

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("ItemTypeController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View(new ItemTypeViewModel());
            }
        }


        [Authorize(Roles = "PowerAdmin, Admin")]
        public ActionResult Edit(EditItemTypeIdViewModel mod)
        {
            try
            {
                var depts = db.MainDepartment.ToList();
                var IType = db.ItemType.Find(mod.Id);
                var model = new EditItemTypeIdViewModel();

                model.Name = IType.Name;
                model.MdId = IType.MainDepartment.Id;
                ViewBag.MdId = new SelectList(depts, "Id", "Name", model.MdId);
                model.AddedTime = IType.AddedTime;
                model.ModifiedTime = IType.ModifiedTime;
                model.CreatedBy = IType.AddedBy;
                model.ModifiedBy = IType.ModifiedBy;

                return View(model);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("ItemTypeController", "Edit", ex.Message, ex.Source, ex.StackTrace);
                return View(new EditItemTypeIdViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditItemTypeViewModel It)
        {
            try
            {
                string CurrentUserName = User.Identity.Name;
                var IType = db.ItemType.Find(It.Id);
                var model = new EditItemTypeIdViewModel();
                model.Name = It.Name;

                if (ModelState.IsValid)
                {
                    IType.Name = It.Name;
                    IType.MdId = It.MdId;
                    IType.ModifiedTime = DateTime.Now;
                    IType.ModifiedBy = CurrentUserName;

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                    return View(model);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("ItemTypeController", "Edit", ex.Message, ex.Source, ex.StackTrace);
                return View(new EditItemTypeIdViewModel());
            }
        }

    }
}