using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Search_n_View.ViewModels;
using Search_n_View.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace Search_n_View.Controllers
{
    public class AccessGroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            try
            {
                var AcGp = db.AccessGroup.OrderByDescending(x=>x.Id).ToList();
                ManageAccessGroupModel AccessGroupList = new ManageAccessGroupModel();
                List<ManageAccessGroupViewModel> groupList = new List<ManageAccessGroupViewModel>();

                    for (int i= 0;i<AcGp.Count;i++)
                    {
                        var AccessGroup = new ManageAccessGroupViewModel();
                        AccessGroup.Name = AcGp[i].Name;
                        AccessGroup.AgShortName = AcGp[i].AgShortName;
                        AccessGroup.CanAddNote = AcGp[i].CanAddNote;
                        AccessGroup.CanDownload = AcGp[i].CanDownload;
                        AccessGroup.CanEmail = AcGp[i].CanEmail;
                        AccessGroup.CanUpload = AcGp[i].CanUpload;
                        AccessGroup.CanPrint = AcGp[i].CanPrint;
                        AccessGroup.CanView = AcGp[i].CanView;
                        AccessGroup.AddedBy = AcGp[i].AddedBy;
                        AccessGroup.AddedTime = AcGp[i].AddedTime;

                        groupList.Add(AccessGroup);
                    }
                    AccessGroupList.Groups = groupList;
          
                return View(AccessGroupList);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("AccessGroupController", "Index", ex.Message, ex.Source, ex.StackTrace);
                return View(new ManageAccessGroupModel());
            }
        }

        [Authorize(Roles = "PowerAdmin, Admin")]
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("AccessGroupController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ManageAccessGroupViewModel model)
        {
            try
            {

                string CurrentUserName = User.Identity.Name;
                model.CanPrint = true;
            if (ModelState.IsValid)
            {
                    var Groups = db.AccessGroup.Where(x => x.AgShortName == model.AgShortName).FirstOrDefault();
                    if (Groups == null)
                    {
                        var AcGp = new AccessGroup
                        {
                            Name = model.Name,
                            AgShortName = model.AgShortName.Trim().ToUpper(),
                            CanAddNote = model.CanAddNote,
                            CanDownload = model.CanDownload,
                            CanEmail = model.CanEmail,
                            CanUpload = model.CanUpload,
                            CanPrint = model.CanPrint,
                            CanView = model.CanView,
                            AddedBy = CurrentUserName,
                            AddedTime = DateTime.Now,
                            ModifiedBy = CurrentUserName,
                            ModifiedTime = DateTime.Now
                        };

                        db.AccessGroup.Add(AcGp);
                        await db.SaveChangesAsync();
                    }
                    else {
                        ViewData["Message"] = "Group Code Already Exist!";
                        return View(model);
                    }
            }else
                return View(model);

                return RedirectToAction("Index", "AccessGroup");

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("AccessGroupController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View(new ManageAccessGroupViewModel());
            }
        }

        [Authorize(Roles = "PowerAdmin, Admin")]
        public ActionResult Edit(string GroupCode)
        {
            try
            {         
                var AcGp = db.AccessGroup.Where(x => x.AgShortName == GroupCode).FirstOrDefault();
                var model = new EditAccessGroupViewModel();
                if (AcGp != null){
                    model.Id = AcGp.Id;
                    model.Name = AcGp.Name;
                    model.AgShortName = AcGp.AgShortName;
                    model.CanView = AcGp.CanView;
                    model.CanDownload = AcGp.CanDownload;
                    model.CanEmail = AcGp.CanEmail;
                    model.CanUpload = AcGp.CanUpload;
                    model.CanPrint = AcGp.CanPrint;
                    model.CanAddNote = AcGp.CanAddNote;
                
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("AccessGroupController", "Edit", ex.Message, ex.Source, ex.StackTrace);
                return View(new EditAccessGroupViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditAccessGroupViewModel model)
        {
            try
            {

                string CurrentUserName = User.Identity.Name;
                model.CanPrint = true;
                var AcGp = db.AccessGroup.Where(x => x.Id == model.Id).FirstOrDefault();
                model.AgShortName = AcGp.AgShortName;
                if (model.Name != null)
                {
                    AcGp.Name = model.Name;
                    AcGp.CanAddNote = model.CanAddNote;
                    AcGp.CanDownload = model.CanDownload;
                    AcGp.CanUpload = model.CanUpload;
                    AcGp.CanPrint = model.CanPrint;
                    AcGp.CanView = model.CanView;
                    AcGp.CanEmail = model.CanEmail;
                    AcGp.CanAddNote = model.CanAddNote;
                    AcGp.ModifiedBy = CurrentUserName;
                    AcGp.ModifiedTime = DateTime.Now;

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index", "AccessGroup");
                }
                else {
                    return View(model);
                }                                        
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("AccessGroupController", "Edit", ex.Message, ex.Source, ex.StackTrace);
                return View(new EditAccessGroupViewModel());
            }
        }

    }
}