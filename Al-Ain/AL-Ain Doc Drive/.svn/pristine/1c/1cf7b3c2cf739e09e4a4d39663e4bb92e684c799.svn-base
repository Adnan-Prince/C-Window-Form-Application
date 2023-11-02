using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Search_n_View.Models;
using Search_n_View.ViewModels;

namespace Search_n_View.Controllers
{
    public class DriveController : Controller
    {
        // GET: Drive
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            try
            {
            
                var AD = db.ActiveDrive.OrderByDescending(x => x.Id).ToList();
                ManageDriveViewModel DriveListList = new ManageDriveViewModel();
                List<DriveViewModel> adList = new List<DriveViewModel>();
                if (AD != null)
                {
                    for (int i = 0; i < AD.Count; i++)
                    {
                        var D = new DriveViewModel();
                        D.Id = AD[i].Id;
                        D.BasePath = AD[i].BasePath;
                        D.IsActive = AD[i].IsActive;
                        D.AddedBy = AD[i].AddedBy;
                        D.AddedTime = AD[i].AddedTime;

                        adList.Add(D);
                    }
                    DriveListList.Drive = adList;
                }
                return View(DriveListList);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DriveController", "Index", ex.Message, ex.Source, ex.StackTrace);
                return View(new DriveViewModel());
            }
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "PowerAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DriveViewModel model)
        {
            try
            {
                string CurrentUserName = User.Identity.Name;

                if (ModelState.IsValid)
                {
                    string BasePath = model.BasePath.ToUpper() + ":\\Uploads\\";
                    var Dr = db.ActiveDrive.Where(x => x.BasePath == BasePath).FirstOrDefault();
                    if (Dr == null)
                    {
                        var AD = new ActiveDrive
                        {
                            BasePath = BasePath,
                            IsActive = true,
                            AddedBy = CurrentUserName,
                            AddedTime = DateTime.Now,
                            ModifiedBy = CurrentUserName,
                            ModifiedTime = DateTime.Now
                        };

                        var Drlist = db.ActiveDrive.Where(x => x.BasePath != BasePath).ToList();
                        for (int i = 0; i < Drlist.Count(); i++)
                        {
                            Drlist[i].IsActive = false;
                            db.SaveChanges();
                        }

                        DirectoryInfo BulkDirInfo = new DirectoryInfo(BasePath);
                        if (!BulkDirInfo.Exists)
                            BulkDirInfo.Create();

                        db.ActiveDrive.Add(AD);
                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        ViewData["Message"] = "Drive Already Exist!";
                        return View(model);
                    }
                }
                else
                    return View(model);

                return RedirectToAction("Index", "Drive");

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DriveController", "Create", ex.Message, ex.Source, ex.StackTrace);
                return View(new DriveViewModel());
            }
        }

    }
}