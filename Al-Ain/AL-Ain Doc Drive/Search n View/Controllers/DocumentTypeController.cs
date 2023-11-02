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
    public class DocumentTypeController : Controller
    {
        // GET: DocumentType
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            try
            {
                var DTypes = db.DocumentType.OrderByDescending(x => x.Id).Where(x => x.IsDeleted == false).ToList();

                var DtVM = new List<DocumentTypeViewModel>();
                for (int i = 0; i < DTypes.Count; i++)
                {
                    var Dt = new DocumentTypeViewModel();

                    Dt.Id = DTypes[i].Id;
                    Dt.Name = DTypes[i].Name;
                    Dt.ItemTypeName = DTypes[i].ItemType.Name;
                    Dt.AddedTime = DTypes[i].AddedTime;
                    Dt.ModifiedTime = DTypes[i].ModifiedTime;
                    Dt.CreatedBy = DTypes[i].AddedBy;
                    Dt.ModifiedBy = DTypes[i].ModifiedBy;
                    DtVM.Add(Dt);
                }

                var model = new ManageDocumentTypeViewModel();
                model.DocumentType = DtVM;
                return View(model);
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentTypeController", "Index", ex.Message, ex.Source, ex.StackTrace);
                return View();
            }
        }

        [Authorize(Roles = "PowerAdmin, Admin")]
        public ActionResult Create()
        {
            try
            {
                var model = new DocumentTypeViewModel();
                var ITyps = db.ItemType.ToList();
                model.ItemTypes = ITyps.Select(s => new SelectListItem { Value = Convert.ToString(s.Id), Text = s.Name }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentTypeController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View(new DocumentTypeViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DocumentTypeViewModel IType)
        {
            try
            {
                var model = new DocumentType();

                if (ModelState.IsValid)
                {
                    string CurrentUserName = User.Identity.Name;

                    var doc = db.DocumentType.Where(x => x.Name == IType.Name.Trim()).Where(x => x.IsDeleted == false).FirstOrDefault();
                    if (doc == null)
                    {
                        model.Name = IType.Name;
                        model.ItID = IType.ItId;
                        model.AddedTime = DateTime.Now;
                        model.ModifiedTime = DateTime.Now;
                        model.AddedBy = CurrentUserName;
                        model.ModifiedBy = CurrentUserName;

                        db.DocumentType.Add(model);
                        await db.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var Items = db.ItemType.ToList();
                        IType.ItemTypes = Items.Select(s => new SelectListItem { Value = Convert.ToString(s.Id), Text = s.Name }).ToList();
                        ViewData["Message"] = "Item Type Already Exist!";
                        return View(IType);

                    }


                }
                else
                {
                    var Items = db.ItemType.ToList();
                    IType.ItemTypes = Items.Select(s => new SelectListItem { Value = Convert.ToString(s.Id), Text = s.Name }).ToList();
                    return View(IType);
                }

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentTypeController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View(new DocumentTypeViewModel());
            }
        }


        [Authorize(Roles = "PowerAdmin, Admin")]
        public ActionResult Edit(EditDocumentTypeIdViewModel mod)
        {
            try
            {
                var IType = db.ItemType.ToList();
                var DocType = db.DocumentType.Find(mod.Id);
                var model = new EditDocumentTypeIdViewModel();

                model.Name = DocType.Name;
                model.ItId = Convert.ToInt32(DocType.ItemType.Id);
                ViewBag.ItId = new SelectList(IType, "Id", "Name", model.ItId);
                model.AddedTime = DocType.AddedTime;
                model.ModifiedTime = DocType.ModifiedTime;
                model.CreatedBy = DocType.AddedBy;
                model.ModifiedBy = DocType.ModifiedBy;

                return View(model);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentTypeController", "Edit", ex.Message, ex.Source, ex.StackTrace);
                return View(new EditDocumentTypeIdViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditDocumentTypeViewModel It)
        {
            try
            {
                string CurrentUserName = User.Identity.Name;
                var IType = db.DocumentType.Find(It.Id);
                var model = new EditDocumentTypeIdViewModel();
                model.Name = It.Name;

                if (ModelState.IsValid)
                {
                    IType.Name = It.Name;
                    IType.ItID = It.ItId;
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
                Helpers.Logs.ErrorLog("DocumentTypeController", "Edit", ex.Message, ex.Source, ex.StackTrace);
                return View(new EditDocumentTypeIdViewModel());
            }
        }

    }
}