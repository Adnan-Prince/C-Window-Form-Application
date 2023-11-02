﻿using Microsoft.AspNet.Identity;
using MsgReader.Outlook;
using Search_n_View.Models;
using Search_n_View.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Windows.Forms;
using WIA;

namespace Search_n_View.Controllers
{
    public class DocumentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult DocumentSearch()
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();

                if (User.IsInRole("SearchUser"))
                {
                    var ItemTypeList = (from uit in db.UserItemType
                                        join It in db.ItemType
                                        on uit.ItID equals It.Id
                                        where uit.UID == SessionUserId
                                        && It.IsDeleted == false
                                        select It).ToList();
                    ViewBag.ItemTypeList = ItemTypeList;
                    var DepartmentList = (from uit in db.UserDepartment
                                        join It in db.MainDepartment
                                        on uit.MdID equals It.Id
                                        where uit.UID == SessionUserId
                                        && It.IsDeleted == false
                                        select It).ToList();

                    ViewBag.DepartmentList = DepartmentList;
                    var DocTypeIds = (from uit in db.UserItemType
                                      join it in db.ItemType
                                      on uit.ItID equals it.Id
                                      join dt in db.DocumentType
                                      on it.Id equals dt.ItID
                                      where uit.UID == SessionUserId
                                      select dt.Id).ToList();

                    ViewBag.DocTypeIds = DocTypeIds;
                }
                else {
                    var DepartmentList = db.MainDepartment.Where(x=>x.IsDeleted == false).ToList();                  
                    ViewBag.DepartmentList = DepartmentList;
                    ViewBag.ItemTypeList = new List<ItemType>();
                    ViewBag.DocTypeIds = new List<DocumentType>();

                }


                ViewData["DocList"] = null;
                return View();
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "DocumentSearch", ex.Message, ex.Source, ex.StackTrace);
                return View();
            }
        }

        [HttpPost]
        public PartialViewResult DocumentFilter(string Channel, string InvoiceNo, string FromDate, string ToDate, int? itemTypeId, int? DepartId, int? docTypeId)
        {
            try
            {
                DateTime? From = null;
                DateTime? To = null;
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FromDate = FromDate + " " + "00:00:00";
                    From = Convert.ToDateTime(FromDate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    ToDate = ToDate + " " + "23:59:59";
                    To = Convert.ToDateTime(ToDate);
                }
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;

                if (CurrentUserName != null && CurrentUserName != "")
                {
                    try
                    {
                        if (User.IsInRole("SearchUserApprove"))
                        {
                            var tblAlAinDocument = db.AlAinDocument.Where(x => x.IsApproved != true).ToList();

                            if (Channel!="0")
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.ChannelName.Contains(Channel)).ToList();
                            }
                            if (!string.IsNullOrEmpty(InvoiceNo))
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.InvoiceNo.Contains(InvoiceNo)).ToList();
                            }
                            if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                            {
                                DateTime fromDateValue;
                                DateTime toDateValue;
                                if (DateTime.TryParse(FromDate, out fromDateValue) && DateTime.TryParse(ToDate, out toDateValue))
                                {
                                    tblAlAinDocument = tblAlAinDocument.Where(x => x.InvoiceDate >= fromDateValue && x.InvoiceDate <= toDateValue).ToList();
                                }
                            }
                            if (itemTypeId != 0)
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.ItID == itemTypeId).ToList();
                            }
                            if (docTypeId != 0)
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.DtId == docTypeId).ToList();
                            }
                            if (DepartId !=0)
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.MdId == DepartId).ToList();
                            }

                            ViewData["DocList"] = tblAlAinDocument; 
                        }
                        else
                        {
                            var tblAlAinDocument = db.AlAinDocument.ToList();
                            if (Channel != "0")
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.ChannelName.Contains(Channel)).ToList();
                            }
                            if (!string.IsNullOrEmpty(InvoiceNo))
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.InvoiceNo.Contains(InvoiceNo)).ToList();
                            }

                            if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                            {
                                DateTime fromDateValue;
                                DateTime toDateValue;
                                if (DateTime.TryParse(FromDate, out fromDateValue) && DateTime.TryParse(ToDate, out toDateValue))
                                {
                                    tblAlAinDocument = tblAlAinDocument.Where(x => x.InvoiceDate >= fromDateValue && x.InvoiceDate <= toDateValue).ToList();
                                }
                            }
                            if (itemTypeId != 0)
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.ItID == itemTypeId).ToList();
                            }
                            if (docTypeId != 0)
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.DtId == docTypeId).ToList();
                            }
                            if (DepartId != 0)
                            {
                                tblAlAinDocument = tblAlAinDocument.Where(x => x.MdId == DepartId).ToList();
                            }
                            ViewData["DocList"] = tblAlAinDocument;

                        }
                        var Access = db.Users.Include(x => x.AccessGroup).Where(x => x.Id == SessionUserId).FirstOrDefault();
                        ViewBag.Access = Access;
                    }
                    catch (Exception ex)
                    {
                        Helpers.Logs.ErrorLog("DocumentController", "DocumentFilter", ex.Message, ex.Source, ex.StackTrace);
                        return PartialView("~/Views/Shared/_DocList.cshtml");
                    }

                }
                else
                {
                    List<Document> Doc = new List<Document>();
                    ViewData["DocList"] = Doc;
                }


                return PartialView("~/Views/Shared/_DocList.cshtml");
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "DocumentFilter", ex.Message, ex.Source, ex.StackTrace);
                return PartialView("~/Views/Shared/_DocList.cshtml");
            }
        }

        [HttpPost]
        public JsonResult SingleDocumentUpload(Document Doc, EmailSender EmailDetail, string ScannedDocPath)
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                var SessionUserName = User.Identity.Name;
                string CurrentUserName = Convert.ToString(SessionUserName);
                if (CurrentUserName != null && CurrentUserName != "")
                {
                    var check = db.Document.Where(x => x.ClaimNo == Doc.ClaimNo && x.PolicyNo == Doc.PolicyNo && x.ItID == Doc.ItID && x.DtId == Doc.DtId).FirstOrDefault();
                    if (check == null)
                    {
                        EmailAttachment E = null;
                        var D = new Document();
                        if (EmailDetail.DocPath != null)
                        {
                            E = new EmailAttachment();

                            E.DocPath = EmailDetail.DocPath;
                            E.AttachmentName = EmailDetail.AttachmentName;
                            E.Header = EmailDetail.Header;
                            E.Recipients = EmailDetail.Recipients;
                            E.Body = EmailDetail.Body;
                            E.AddedTime = DateTime.Now;
                            E.ModifiedBy = CurrentUserName;
                            E.ModifiedTime = DateTime.Now;

                            db.EmailAttachment.Add(E);
                            db.SaveChanges();
                        }
                    
                        D.ClaimNo = Doc.ClaimNo;
                        D.PolicyNo = Doc.PolicyNo;
                        D.Year = Doc.Year;
                        D.ItID = Doc.ItID;
                        D.DtId = Doc.DtId;
                        if (ScannedDocPath != "")
                            D.ScannedFilePath = ScannedDocPath;
                        if (EmailDetail.DocPath != null)
                            D.DEId = E.Id;
                        D.UId = SessionUserId;
                        D.AddedBy = CurrentUserName;
                        D.AddedTime = DateTime.Now;
                        D.ModifiedBy = CurrentUserName;
                        D.ModifiedTime = DateTime.Now;
                        D.IsActive = true;

                        db.Document.Add(D);
                        db.SaveChanges();

                        if (ScannedDocPath != null)
                        {
                            var DocHis = new DocumentHistory();
                            DocHis.Scanned = true;
                            DocHis.AddedBy = CurrentUserName;
                            DocHis.AddedTime = DateTime.Now;
                            DocHis.ModifiedBy = CurrentUserName;
                            DocHis.ModifiedTime = DateTime.Now;
                            DocHis.DId = D.Id;
                            DocHis.UId = SessionUserId;

                            db.DocumentHistory.Add(DocHis);
                            db.SaveChanges();

                        }
                        


                        return Json(new { Exist = false});
                    }
                    else
                        return Json(new { Exist = true });


                }
                else
                    return Json(false);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "SingleDocumentUpload", ex.Message, ex.Source, ex.StackTrace);
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult SingleFileUpload()
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;


                if (CurrentUserName != null && CurrentUserName != "")
                {
                    try
                    {                   
                        string fname, FileName;
                        string[] FN = null;
                        var D = db.Document.OrderByDescending(x => x.Id).FirstOrDefault();
                        if (Request.Files.Count > 0)
                        {
                            HttpPostedFileBase file = Request.Files[0];

                            FileName = file.FileName;

                            FN = FileName.Split('.');
                            FileName = FN[0] + DateTime.Now.ToString("ms") + "." + FN[1];

                            string extension = Path.GetExtension(FileName);

                            var ActivePath = db.ActiveDrive.Where(x => x.IsActive == true).FirstOrDefault();
                            string UploadRoot = ActivePath.BasePath;
                            string Date = DateTime.Now.ToString("ddMMyyyy");
                            string fullPath = UploadRoot + Date;

                            DirectoryInfo BulkDirInfo = new DirectoryInfo(fullPath);
                            if (!BulkDirInfo.Exists)
                            {
                                BulkDirInfo.Create();
                                fname = Path.Combine(fullPath, FileName);
                            }
                            else
                                fname = Path.Combine(fullPath, FileName);

                            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Uploads/Temp/"));
                            EmptyFolder(directory);

                            file.SaveAs(fname);

                            D.DocPath = Date + "\\" + FileName;
                            D.BaseDrivePath = ActivePath.BasePath;
                            D.Extension = extension;

                            db.SaveChanges();

                            var DocHis = new DocumentHistory();
                            DocHis.Uploaded = true;
                            DocHis.AddedBy = CurrentUserName;
                            DocHis.AddedTime = DateTime.Now;
                            DocHis.ModifiedBy = CurrentUserName;
                            DocHis.ModifiedTime = DateTime.Now;
                            DocHis.DId = D.Id;
                            DocHis.UId = SessionUserId;

                            db.DocumentHistory.Add(DocHis);
                            db.SaveChanges();
                        }
                        else if (D.EmailAttachment != null)
                        {

                            byte[] F = System.IO.File.ReadAllBytes(D.EmailAttachment.DocPath);

                            FileName = D.EmailAttachment.DocPath.Substring(D.EmailAttachment.DocPath.LastIndexOf("\\") + 1);
                            FN = FileName.Split('.');
                            FileName = FN[0] + DateTime.Now.ToString("ms") + "." + FN[1];

                            string extension = Path.GetExtension(FileName);

                            var ActivePath = db.ActiveDrive.Where(x => x.IsActive == true).FirstOrDefault();
                            string UploadRoot = ActivePath.BasePath;
                            string Date = DateTime.Now.ToString("ddMMyyyy");
                            string fullPath = UploadRoot + Date;

                            DirectoryInfo BulkDirInfo = new DirectoryInfo(fullPath);
                            if (!BulkDirInfo.Exists)
                            {
                                BulkDirInfo.Create();
                                fullPath = fullPath + "\\" + FileName;
                                System.IO.File.WriteAllBytes(fullPath, F);
                            }
                            else
                            {
                                fullPath = fullPath + "\\" + FileName;
                                System.IO.File.WriteAllBytes(fullPath, F);
                            }

                            D.DocPath = Date + "\\" + FileName;
                            D.BaseDrivePath = ActivePath.BasePath;
                            D.Extension = extension;

                            db.SaveChanges();

                            var DocHis = new DocumentHistory();
                            DocHis.Uploaded = true;
                            DocHis.AddedBy = CurrentUserName;
                            DocHis.AddedTime = DateTime.Now;
                            DocHis.ModifiedBy = CurrentUserName;
                            DocHis.ModifiedTime = DateTime.Now;
                            DocHis.DId = D.Id;
                            DocHis.UId = SessionUserId;

                            db.DocumentHistory.Add(DocHis);
                            db.SaveChanges();
                        }
                        else if (D.ScannedFilePath != null) {
                            byte[] F = System.IO.File.ReadAllBytes(D.ScannedFilePath);

                            FileName = D.ScannedFilePath.Substring(D.ScannedFilePath.LastIndexOf("\\") + 1);

                            string extension = Path.GetExtension(FileName);

                            var ActivePath = db.ActiveDrive.Where(x => x.IsActive == true).FirstOrDefault();
                            string UploadRoot = ActivePath.BasePath;
                            string Date = DateTime.Now.ToString("ddMMyyyy");
                            string fullPath = UploadRoot + Date;

                            DirectoryInfo BulkDirInfo = new DirectoryInfo(fullPath);
                            if (!BulkDirInfo.Exists)
                            {
                                BulkDirInfo.Create();
                                fullPath = fullPath + "\\" + FileName;
                                System.IO.File.WriteAllBytes(fullPath, F);
                            }
                            else
                            {
                                fullPath = fullPath + "\\" + FileName;
                                System.IO.File.WriteAllBytes(fullPath, F);
                            }

                            D.DocPath = Date + "\\" + FileName;
                            D.BaseDrivePath = ActivePath.BasePath;
                            D.Extension = extension;

                            db.SaveChanges();

                            var DocHis = new DocumentHistory();
                            DocHis.Uploaded = true;
                            DocHis.AddedBy = CurrentUserName;
                            DocHis.AddedTime = DateTime.Now;
                            DocHis.ModifiedBy = CurrentUserName;
                            DocHis.ModifiedTime = DateTime.Now;
                            DocHis.DId = D.Id;
                            DocHis.UId = SessionUserId;

                            db.DocumentHistory.Add(DocHis);
                            db.SaveChanges();
                        }            
                        return Json(true);
                    }
                    catch (Exception ex)
                    {
                        Helpers.Logs.ErrorLog("DocumentController", "SingleFileUpload", ex.Message, ex.Source, ex.StackTrace);
                        return Json(false);
                    }
                }
                else
                    return Json(false);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "SingleFileUpload", ex.Message, ex.Source, ex.StackTrace);
                return Json(false);
            }
        }

        public ActionResult SelectedDocumentDetail(int DId)
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;

                if (CurrentUserName != null && CurrentUserName != "")
                {
                    try
                    {

                        var Doc = db.AlAinDocument.Where(x => x.Id == DId).FirstOrDefault();


                        if (Doc != null)
                        {

                            var Access = db.Users.Include(x=>x.AccessGroup).Where(x => x.Id == SessionUserId).FirstOrDefault();


                            var DocHis = new DocumentHistory();
                            DocHis.Viewed = true;
                            DocHis.AddedBy = CurrentUserName;
                            DocHis.AddedTime = DateTime.Now;
                            DocHis.ModifiedBy = CurrentUserName;
                            DocHis.ModifiedTime = DateTime.Now;
                            DocHis.DId = DId;
                            DocHis.UId = SessionUserId;

                            db.DocumentHistory.Add(DocHis);
                            db.SaveChanges();

                            string fileName = "";
                            fileName = Doc.DocPath.Substring(Doc.DocPath.LastIndexOf('/') + 1);


                            ViewBag.Access = Access;
                            ViewBag.fileName = fileName;
                            ViewBag.UserId = SessionUserId;
                            ViewBag.NoteAddedCount = db.Notes.Where(x => x.DId == Doc.Id).Count();

                        }

                        ViewData["DocDetail"] = Doc;

                        ViewBag.BasePath = db.ActiveDrive.Where(x => x.IsActive == true).Select(x => x.BasePath).FirstOrDefault();
                        
                    }
                    catch (Exception ex)
                    {
                        Helpers.Logs.ErrorLog("DocumentController", "SelectedDocumentDetail", ex.Message, ex.Source, ex.StackTrace);
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "SelectedDocumentDetail", ex.Message, ex.Source, ex.StackTrace);
                return View();
            }
        }

        [HttpPost]
        public PartialViewResult FileTempUpload()
        {
            try
            {
                string extension = "";
                string fname = "";
                if (Request.Files.Count > 0)
                {
                        HttpPostedFileBase file = Request.Files[0];

                        string FileName = file.FileName;
                        extension = Path.GetExtension(FileName);
                        fname = Path.Combine(Server.MapPath("~/Uploads/Temp"), FileName);
                        file.SaveAs(fname);

           
                    string UploadRoot = Convert.ToString(WebConfigurationManager.AppSettings["FileUploadTempPath"]);
                    string TempDocPath = "/Uploads/Temp/" + FileName;
                    string Fn = UploadRoot + "\\" + FileName;
                    Fn = Fn.Replace("/", "\\");
                    if (extension != "")
                    {
                      
                        if (extension.Contains("pdf") || extension.Contains("PDF") || extension.Contains("jpg") || extension.Contains("jpeg") || extension.Contains("png") || extension.Contains("PNG") || extension.Contains("txt") || extension.Contains("TXT"))
                        {
                            string embed = "<object data=\"{0}\" type=\"" + MimeType(FileName) + "\" style=\"width:99%;\"></object>";
                            TempData["File"] = string.Format(embed, TempDocPath);
                            ViewBag.ImgFile = FileName;
                            ViewBag.PageCount = 0;
                            ViewBag.Email = null;
                            ViewBag.ScannedFilePath = null;
                        }
                        else  if (extension.Contains("tif") || extension.Contains("TIF"))
                        {
                            TempData["File"] = TempDocPath;
                            ViewBag.ImgFile = FileName;
                            ViewBag.PageCount = GetTotalpages(Fn);
                            ViewBag.Email = null;
                            ViewBag.ScannedFilePath = null;

                        }
                        else
                        {
                            string embed = fname;
                            TempData["File"] = TempDocPath;
                            ViewBag.ImgFile = FileName;
                            ViewBag.PageCount = 0;
                            ViewBag.Email = null;
                            ViewBag.ScannedFilePath = null;

                        }
                    }
                }
                return PartialView("_ViewDocumentBox", TempData["File"]);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "FileTempUpload", ex.Message, ex.Source, ex.StackTrace);
                return PartialView("_ViewDocumentBox", null);
            }
        }

        [HttpPost]
        public  PartialViewResult FileTempScanUpload()
        {
            try
            {
                string fname = "", FileName = "";
                bool HasScanner = false;
                var deviceManager = new DeviceManager();
                DeviceInfo AvailableScanner = null;
                try
                {            
                    for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
                    {
                        if (deviceManager.DeviceInfos[i].Type == WiaDeviceType.ScannerDeviceType)
                        {
                            AvailableScanner = deviceManager.DeviceInfos[i];
                            HasScanner = true;
                            break;
                        }                     
                    }
                }
                catch (COMException ex)
                {
                    Helpers.Logs.ErrorLog("DocumentController", "FileTempScanUpload-> DeviceInfos", ex.Message, ex.Source, ex.StackTrace);
                    return PartialView("_ViewDocumentBox", null);
                }
                if (HasScanner)
                {
                    var device = AvailableScanner.Connect();
                    var ScannerItem = device.Items[1];
                    setItem(ScannerItem, "6146", 4);
                    var file = (ImageFile)ScannerItem.Transfer(FormatID.wiaFormatTIFF);
                    FileName = FileName = "Scan" + DateTime.Now.ToString("ms") + ".TIF";
                    fname = Path.Combine(Server.MapPath("~/Uploads/Temp"), FileName);
                    file.SaveFile(fname);

                    string UploadRoot = Convert.ToString(WebConfigurationManager.AppSettings["FileUploadTempPath"]);
                    string TempDocPath = "/Uploads/Temp/" + FileName;
                    string Fn = UploadRoot + "\\" + FileName;
                    Fn = Fn.Replace("/", "\\");

                        TempData["File"] = TempDocPath;
                        ViewBag.ImgFile = FileName;
                        ViewBag.PageCount = GetTotalpages(Fn);
                        ViewBag.Email = null;
                        ViewBag.ScannedFilePath = fname;


                }
                return PartialView("_ViewDocumentBox", TempData["File"]);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "FileTempScanUpload", ex.Message, ex.Source, ex.StackTrace);
                return PartialView("_ViewDocumentBox", null);
            }
        }

        [HttpPost]
        public PartialViewResult FileTempEmailUpload()
        {
            try
            {
                string extension = "", fname = "", AttachmentName = "", TempDocPath = "", Fn = "", EmailBody= "";
                int AttachementCount = 0;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];

                    string FileName = file.FileName;
                    extension = Path.GetExtension(FileName);
                    var Email = new EmailSender();

                    fname = Path.Combine(Server.MapPath("~/Uploads/Temp"), FileName);
                        file.SaveAs(fname);
               
                        string UploadRoot = Convert.ToString(WebConfigurationManager.AppSettings["FileUploadTempPath"]);
                    
                        using (var stream = System.IO.File.Open(fname, FileMode.Open, FileAccess.Read))
                        {
                            using (var message = new Storage.Message(stream))
                            {                             
                                Email.Recipients = message.Sender.Email;                           
                                Email.Header = message.Subject;
                                Email.Body = message.BodyText; 
                            foreach (Storage.Attachment attachment in message.Attachments.OfType<Storage.Attachment>())
                                {
                                    if (message.Attachments.Count > 1)
                                    {
                                        if (AttachementCount == 1)
                                    { 
                                            AttachmentName = attachment.FileName;
                                            Email.AttachmentName = attachment.FileName;
                                            TempDocPath = Path.Combine(Server.MapPath("~/Uploads/Temp"), attachment.FileName);
                                            Email.DocPath = TempDocPath;
                                            System.IO.File.WriteAllBytes(TempDocPath, attachment.Data);
                                        }                                  
                                    }
                                    else {
                                        AttachmentName = attachment.FileName;
                                        Email.AttachmentName = attachment.FileName;
                                        TempDocPath = Path.Combine(Server.MapPath("~/Uploads/Temp"), attachment.FileName);
                                        Email.DocPath = TempDocPath;
                                        System.IO.File.WriteAllBytes(TempDocPath, attachment.Data);
                                    }
                                AttachementCount++;
                                }
                            }
                        }
                        TempDocPath = "/Uploads/Temp/" + AttachmentName;
                    if (TempDocPath.Contains("pdf") || TempDocPath.Contains("PDF") || TempDocPath.Contains("jpg") || TempDocPath.Contains("jpeg") || TempDocPath.Contains("png") || TempDocPath.Contains("PNG") || TempDocPath.Contains("txt") || TempDocPath.Contains("TXT"))
                        {
                            string embed = "<object data=\"{0}\" type=\"" + MimeType(AttachmentName) + "\" style=\"width:99%;\"></object>";
                            TempData["File"] = string.Format(embed, TempDocPath);
                            ViewBag.Email = Email;
                            ViewBag.ImgFile = AttachmentName;
                    } else if (TempDocPath.Contains("tif") || TempDocPath.Contains("TIF")) {
                            Fn = UploadRoot + "\\" + AttachmentName;
                            Fn = Fn.Replace("/", "\\");
                            ViewBag.Email = Email;
                            ViewBag.ImgFile = AttachmentName;                         
                            ViewBag.PageCount = GetTotalpages(Fn);
                            TempData["File"] = TempDocPath;
                        }
                        else {
                            TempData["File"] = TempDocPath;
                            ViewBag.Email = Email;
                    }
                                 
                }
                return PartialView("_ViewDocumentBox", TempData["File"]);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "FileTempUpload", ex.Message, ex.Source, ex.StackTrace);
                return PartialView("_ViewDocumentBox", null);
            }
        }


        public PartialViewResult DocumentViewSelected(int DocId)
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;

                var Document = db.AlAinDocument.Where(x => x.Id == DocId).FirstOrDefault();
                string PermanentFilePath = "", NewName = "", NewTempPath = "", ForCountPath = "";
                string TempPath = Convert.ToString(WebConfigurationManager.AppSettings["FileUploadTempPath"]);
                string FilePath = Document.BaseDrivePath;
                PermanentFilePath = FilePath + Document.DocPath;
                string FName = PermanentFilePath.Substring(PermanentFilePath.LastIndexOf("\\") + 1);

                if (Document.DocPath != "")
                {
                    if (Document.DocPath.Contains("pdf") || Document.DocPath.Contains("PDF") || Document.DocPath.Contains("jpg") || Document.DocPath.Contains("jpeg") || Document.DocPath.Contains("png") || Document.DocPath.Contains("txt") || Document.DocPath.Contains("TXT"))
                    {
                        NewName = CurrentUserName + "_" + FName;

                        string[] newPath = System.IO.Directory.GetFiles(TempPath, NewName);
                        if (newPath.Length > 0)
                            System.IO.File.Delete(newPath[0]);
                        string p = PermanentFilePath.Substring(0, PermanentFilePath.LastIndexOf("\\"));
                        string[] newPath2 = System.IO.Directory.GetFiles(p, FName);
                        if (newPath2.Length > 0)
                            System.IO.File.Copy(newPath2[0], TempPath + "\\" + NewName);

                        TempPath = Path.Combine(Server.MapPath("~/Uploads/Temp"), NewName);

                        TempPath = TempPath.Substring(TempPath.IndexOf("U"));

                        NewTempPath = "/" + TempPath;
                        NewTempPath = NewTempPath.Replace("\\", "/");

                        string embed = "<object data=\"{0}\" type=\"" + MimeType(NewTempPath) + "\" style=\"width:99%;\"></object>";
                        TempData["File"] = string.Format(embed, NewTempPath);
                        ViewBag.ImgFile = NewTempPath;
                        ViewBag.PageCount = 0;
                    }
                    else if (Document.DocPath.Contains("tif") || Document.DocPath.Contains("TIF"))
                    {
                        NewName = CurrentUserName + "_" + FName;

                        string[] newPath = System.IO.Directory.GetFiles(TempPath, NewName);
                        if (newPath.Length > 0)
                            System.IO.File.Delete(newPath[0]);

                        string[] newPath2 = System.IO.Directory.GetFiles(PermanentFilePath.Substring(0, PermanentFilePath.LastIndexOf("\\")), FName);
                        if (newPath2.Length > 0)
                            System.IO.File.Copy(newPath2[0], TempPath + "\\" + NewName);

                        TempPath = Path.Combine(Server.MapPath("~/Uploads/Temp"), NewName);
                        ForCountPath = TempPath;
                        TempPath = TempPath.Substring(TempPath.IndexOf("U"));

                        NewTempPath = "/" + TempPath;
                        NewTempPath = NewTempPath.Replace("\\", "/");

                        TempData["File"] = NewTempPath;
                        ViewBag.ImgFile = NewTempPath;
                        ViewBag.PageCount = GetTotalpages(ForCountPath);
                    }
                    else
                    {
                        NewName = CurrentUserName + "_" + FName;
                        string[] newPath = System.IO.Directory.GetFiles(TempPath, NewName);
                        if (newPath.Length > 0)
                            System.IO.File.Delete(newPath[0]);

                        string[] newPath2 = System.IO.Directory.GetFiles(PermanentFilePath.Substring(0, PermanentFilePath.LastIndexOf("\\")), FName);
                        if (newPath2.Length > 0)
                            System.IO.File.Copy(newPath2[0], TempPath + "\\" + NewName);

                        TempPath = Path.Combine(Server.MapPath("~/Uploads/Temp"), NewName);

                        TempPath = TempPath.Substring(TempPath.IndexOf("U"));

                        NewTempPath = "/" + TempPath;
                        NewTempPath = NewTempPath.Replace("\\", "/");

                        TempData["File"] = NewTempPath;
                        ViewBag.ImgFile = NewTempPath;
                        ViewBag.PageCount = 0;
                    }
                }

                return PartialView("_ViewDocumentBox", TempData["File"]);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "DocumentViewSelected", ex.Message, ex.Source, ex.StackTrace);
                return PartialView("_ViewDocumentBox", null);
            }
        }

        public FileResult DownloadFile(string DocPath, int DocId)
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;
                string fileName = DocPath.Substring(DocPath.LastIndexOf('\\') + 1);
                string FilePath = "", Name = "";
                if (DocPath.Contains('\\'))
                {
                    FilePath = DocPath.Substring(0, DocPath.LastIndexOf('\\'));
                    Name = fileName.Substring(0, fileName.LastIndexOf('.'));
                }
                else
                {
                    fileName = DocPath;
                    Name = fileName.Substring(0, fileName.LastIndexOf('.'));
                }
                
                var docs = db.AlAinDocument.Where(x => x.Id == DocId).FirstOrDefault();
                FilePath = docs.BaseDrivePath + FilePath;
                if (docs != null)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(FilePath, fileName));
                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(MimeType(fileName));
                    string MediaType = Convert.ToString(ct);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        Response.Clear();
                        Response.ContentType = MediaType;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                        Response.Buffer = true;
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        var utf8 = Encoding.UTF8;
                        Response.Charset = utf8.HeaderName;
                        Response.ContentEncoding = utf8;
                        Response.Flush();
                        Response.BinaryWrite(fileBytes);
                        Response.End();
                        Response.Close();
                        memoryStream.Close();
                    }

                    var DocHis = new DocumentHistory();
                    DocHis.Downloaded = true;
                    DocHis.AddedBy = CurrentUserName;
                    DocHis.AddedTime = DateTime.Now;
                    DocHis.ModifiedBy = CurrentUserName;
                    DocHis.ModifiedTime = DateTime.Now;
                    DocHis.DId = DocId;
                    DocHis.UId = SessionUserId;

                    db.DocumentHistory.Add(DocHis);
                    db.SaveChanges();

                    return File(fileBytes, MediaType, Name);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "DownloadFile", ex.Message, ex.Source, ex.StackTrace);
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> SendEmail(EmailSender Detail)
        {
            try
            {
                var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                var smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                var smtpPort = ConfigurationManager.AppSettings["SmtpPort"];
                var smtpUser = ConfigurationManager.AppSettings["FromEmail"];

                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;
                string fromEmail = db.Users.Where(x => x.Id == SessionUserId).Select(x => x.Email).FirstOrDefault();
                string FromName = "JLI-Health", FromEmail = fromEmail, Message = Detail.Header;
                var message = new MailMessage();

                if (Detail.Recipients != null)
                {
                    if (Detail.Recipients != "")
                    {
                        if (Detail.Recipients.Contains(';'))
                        {
                            string[] To = Detail.Recipients.Split(';');
                            foreach (string ToEmail in To)
                            {
                                if (ToEmail != "")
                                {
                                    message.To.Add(new MailAddress(ToEmail));
                                    var Emails = db.Email.Where(x => x.Recipients == ToEmail).FirstOrDefault();
                                    if (Emails == null)
                                    {
                                        Email E = new Email();
                                        E.Recipients = ToEmail;
                                        E.AddedBy = CurrentUserName;
                                        E.ModifiedBy = CurrentUserName;
                                        E.AddedTime = DateTime.Now;
                                        E.ModifiedTime = DateTime.Now;

                                        db.Email.Add(E);
                                        db.SaveChanges();
                                    }

                                }
                            }
                        }
                        else
                        {
                            string ToEmail = Detail.Recipients;
                            message.To.Add(new MailAddress(ToEmail));
                            var Emails = db.Email.Where(x => x.Recipients == ToEmail).FirstOrDefault();
                            if (Emails == null)
                            {
                                Email E = new Email();
                                E.Recipients = ToEmail;
                                E.AddedBy = CurrentUserName;
                                E.ModifiedBy = CurrentUserName;
                                E.AddedTime = DateTime.Now;
                                E.ModifiedTime = DateTime.Now;

                                db.Email.Add(E);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                if (Detail.BccRecipients != null)
                {
                    if (Detail.BccRecipients != "")
                    {
                        if (Detail.BccRecipients.Contains(';'))
                        {
                            string[] BCC = Detail.BccRecipients.Split(';');
                            foreach (string BCCEmail in BCC)
                            {
                                message.Bcc.Add(new MailAddress(BCCEmail));
                                var Emails = db.Email.Where(x => x.Recipients == BCCEmail).FirstOrDefault();
                                if (Emails == null)
                                {
                                    Email E = new Email();
                                    E.Recipients = BCCEmail;
                                    E.AddedBy = CurrentUserName;
                                    E.ModifiedBy = CurrentUserName;
                                    E.AddedTime = DateTime.Now;
                                    E.ModifiedTime = DateTime.Now;

                                    db.Email.Add(E);
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            string BCCEmail = Detail.BccRecipients;
                            message.Bcc.Add(new MailAddress(BCCEmail));
                            var Emails = db.Email.Where(x => x.Recipients == BCCEmail).FirstOrDefault();
                            if (Emails == null)
                            {
                                Email E = new Email();
                                E.Recipients = BCCEmail;
                                E.AddedBy = CurrentUserName;
                                E.ModifiedBy = CurrentUserName;
                                E.AddedTime = DateTime.Now;
                                E.ModifiedTime = DateTime.Now;

                                db.Email.Add(E);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                message.From = new MailAddress(FromEmail);
                if (Detail.Header != null)
                {
                    try
                    {
                        string UploadRoot = Detail.BasePath;

                        string fileName = Detail.DocPath.Substring(Detail.DocPath.LastIndexOf('\\') + 1);
                        string FilePath = UploadRoot + Detail.DocPath.Substring(0, Detail.DocPath.LastIndexOf('\\'));

                        byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(FilePath, fileName));
                        Stream stream = new MemoryStream(fileBytes);
                        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(MimeType(fileName));
                        message.Attachments.Add(new Attachment(stream, fileName, Convert.ToString(ct)));

                    }
                    catch (Exception ex)
                    {
                        Helpers.Logs.ErrorLog("DocumentController", "SendEmail->Detail.Header", ex.Message, ex.Source, ex.StackTrace);
                        return Json(false);
                    }
                }
                message.Subject = Detail.Header;
                if (Detail.Body == null)
                    Detail.Body = "";
                message.Body = string.Format(Detail.Body, FromName, FromEmail, Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    try
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = smtpUser,
                            Password = smtpPassword
                        };
                        smtp.Credentials = credential;
                        smtp.Host = smtpHost;
                        smtp.Port = Convert.ToInt32(smtpPort);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);

                        var DocHis = new DocumentHistory();
                        DocHis.Emailed = true;
                        DocHis.AddedBy = CurrentUserName;
                        DocHis.AddedTime = DateTime.Now;
                        DocHis.ModifiedBy = CurrentUserName;
                        DocHis.ModifiedTime = DateTime.Now;
                        DocHis.EmailRecipients = Detail.Recipients;
                        if (Detail.BccRecipients != null)
                            DocHis.EmailBccRecipients = Detail.BccRecipients;
                        DocHis.EmailSubject = Detail.Header;
                        DocHis.EmailBody = Detail.Body;
                        DocHis.DId = Detail.DocId;
                        DocHis.UId = SessionUserId;

                        db.DocumentHistory.Add(DocHis);
                        db.SaveChanges();

                        return Json(true);
                    }
                    catch (Exception ex)
                    {
                        Helpers.Logs.ErrorLog("DocumentController", "SendEmail->SmtpClient", ex.Message, ex.Source, ex.StackTrace);
                        return Json(false);
                    }
                }

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "SendEmail", ex.Message, ex.Source, ex.StackTrace);
                return Json(false);
            }

        }


        [HttpPost]
        public JsonResult AddNote(int DocId, string Note)
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;

                if (CurrentUserName != null && CurrentUserName != "")
                {

                    var N = new Notes();
                    N.Note = Note;
                    N.AddedBy = CurrentUserName;
                    N.AddedTime = DateTime.Now;
                    N.ModifiedBy = CurrentUserName;
                    N.ModifiedTime = DateTime.Now;
                    N.DId = DocId;
                    N.UId = SessionUserId;

                    db.Notes.Add(N);
                    db.SaveChanges();

                    var DocHis = new DocumentHistory();
                    DocHis.AddedNote = true;
                    DocHis.UserNote = Note;
                    DocHis.AddedBy = CurrentUserName;
                    DocHis.AddedTime = DateTime.Now;
                    DocHis.ModifiedBy = CurrentUserName;
                    DocHis.ModifiedTime = DateTime.Now;
                    DocHis.DId = DocId;
                    DocHis.UId = SessionUserId;

                    db.DocumentHistory.Add(DocHis);
                    db.SaveChanges();

                    return Json(true);
                }
                else
                    return Json(false);


            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "AddNote", ex.Message, ex.Source, ex.StackTrace);
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult DocumentPreviousNote(int DocId)
        {
            try
            {

                var Doc = (from n in db.Notes
                            join d in db.Document
                            on n.DId equals d.Id
                            join u in db.Users
                            on n.UId equals u.Id
                            where d.Id == DocId
                            select new UserDocument()
                            {
                                NoteId = n.Id,
                                UserNote = n.Note,
                                UserId = u.Id,
                                UserEmail = u.Email,
                                AddedTime = n.AddedTime
                            }).OrderByDescending(x => x.NoteId).ToList();

                return Json(Doc);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "DocumentPreviousNote", ex.Message, ex.Source, ex.StackTrace);
                return Json(false);
            }
        }

 
        public ActionResult AccessHistory()
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();

                ViewData["DocViewers"] = new List<UserDocument>();
                var Users = db.Users.ToList();
                var ItemTypeList = db.ItemType.Where(x=>x.IsDeleted == false).ToList();
                var DocumentTypeList = db.DocumentType.Where(x => x.IsDeleted == false).ToList();
                ViewBag.Users = Users;
                ViewBag.ItemTypeList = ItemTypeList;
                ViewBag.DocumentTypeList = DocumentTypeList;

                return View();

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "AccessHistory", ex.Message, ex.Source, ex.StackTrace);
                return View();
            }
        }


        [HttpPost]
        public RedirectToRouteResult AccessHistoryFiltered(string From, string To, string UId, int ItId, int DtId)
        {
            try
            {
                return RedirectToAction("FilteredAccessHistory", new { FromDate = From, ToDate = To, UserId = UId, ItId = ItId, DtId = DtId });

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "AccessHistoryFiltered", ex.Message, ex.Source, ex.StackTrace);
                return null;
            }
        }
        public PartialViewResult FilteredAccessHistory(string FromDate, string ToDate, string UserId, int ItId, int DtId)
        {
            try
            {
                DateTime? From = null;
                DateTime? To = null;
                if (FromDate != null)
                {
                    FromDate = FromDate + " " + "00:00:00";
                    From = Convert.ToDateTime(FromDate);
                }
                if (ToDate != null)
                {
                    ToDate = ToDate + " " + "23:59:59";
                    To = Convert.ToDateTime(ToDate);
                }

                if (FromDate == null && ToDate == null && UserId == null && ItId == 0 && DtId == 0)
                {
                    //var Doc = (from d in db.Document
                    //           join dh in db.DocumentHistory
                    //           on d.Id equals dh.DId
                    //           join It in db.ItemType
                    //           on d.ItID equals It.Id
                    //           join Dt in db.DocumentType
                    //           on d.ItID equals Dt.Id
                    //           where   dh.Viewed == true
                    //             && Dt.IsDeleted == false
                    //               && It.IsDeleted == false
                    //           select new UserDocument()
                    //           {
                    //               ClaimNo = d.ClaimNo,
                    //               PolicyNo = d.PolicyNo,
                    //               DocHistoryId = dh.Id,
                    //               AddedBy = dh.AddedBy,
                    //               AddedTime = dh.AddedTime,
                    //               DocViewed = dh.Viewed,
                    //               DocDownloaded = dh.Downloaded,
                    //               DocUploaded = dh.Uploaded,
                    //               DocAddedNote = dh.AddedNote,
                    //               DocEmailed = dh.Emailed,
                    //               ItemType = It.Name,
                    //               DocumentType = Dt.Name
                    //           }).Take(100).OrderByDescending(x => x.AddedTime).ToList();

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where dh.Viewed == true
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).Take(100).OrderByDescending(x => x.AddedTime).ToList();


                    ViewData["DocViewers"] = Doc;

                }
                else if (From != null && To != null && UserId != null && ItId != 0 && DtId != 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.UId == UserId
                               && dh.Viewed == true
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                               && It.Id == ItId
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();

                    ViewData["DocViewers"] = Doc;

                }
                else if (From != null && To != null && UserId != null && ItId == 0 && DtId == 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where dh.UId == UserId
                               && dh.Viewed == true
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   PolicyNo = d.InvoiceNo,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    ViewData["DocViewers"] = Doc;

                }
                else if (From != null && To != null && UserId != null && ItId != 0 && DtId == 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where   dh.UId == UserId
                               && dh.Viewed == true
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                               && It.Id == ItId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    ViewData["DocViewers"] = Doc;

                }
                else if (From != null && To != null && UserId != null && ItId == 0 && DtId != 0)
                {


                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.UId == UserId
                               && dh.Viewed == true
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    ViewData["DocViewers"] = Doc;

                }
                else if (From != null && To != null && UserId == null && ItId == 0 && DtId == 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == true
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    ViewData["DocViewers"] = Doc;

                }
                else if (From != null && To != null && UserId == null && ItId != 0 && DtId == 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == true
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                               && It.Id == ItId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();

                    ViewData["DocViewers"] = Doc;

                }
                else if (From != null && To != null && UserId == null && ItId == 0 && DtId != 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == true
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();

                    ViewData["DocViewers"] = Doc;

                }
                else if (From != null && To != null)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where dh.Viewed == true
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();

                    ViewData["DocViewers"] = Doc;

                }
                else if (UserId != null && ItId != 0 && DtId != 0)
                {


                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.UId == UserId
                               && dh.Viewed == true
                               && It.Id == ItId
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();

                    ViewData["DocViewers"] = Doc;

                }
                else if (UserId != null && ItId != 0 && DtId == 0)
                {


                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.UId == UserId
                               && dh.Viewed == true
                               && It.Id == ItId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();



                    ViewData["DocViewers"] = Doc;

                }
                else if (UserId != null && ItId == 0 && DtId != 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.UId == UserId
                               && dh.Viewed == true
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();

                    ViewData["DocViewers"] = Doc;

                }
                else if (UserId != null)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.UId == UserId
                               && dh.Viewed == true
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    ViewData["DocViewers"] = Doc;

                }
                else if (ItId != 0 && DtId != 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == true
                               && It.Id == ItId
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();

                    ViewData["DocViewers"] = Doc;

                }
                else if (ItId != 0)
                {
                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where dh.Viewed == true
                               && It.Id == ItId
                               select new UserDocument()
                               {
                                  //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();

                    ViewData["DocViewers"] = Doc;

                }
                else if (DtId != 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == true
                               && It.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   //ClaimNo = d.ClaimNo,
                                   PolicyNo = d.InvoiceNo,
                                   DocHistoryId = dh.Id,
                                   AddedBy = dh.AddedBy,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    ViewData["DocViewers"] = Doc;

                }


                return PartialView("~/Views/Shared/_DocViewed.cshtml");

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "FilteredAccessHistory", ex.Message, ex.Source, ex.StackTrace);
                return PartialView("~/Views/Shared/_DocViewed.cshtml");
            }
        }

        public ActionResult ActionHistory()
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();

                ViewData["DocPerformedAction"] = new List<UserDocument>();
                var Users = db.Users.ToList();
                var ItemTypeList = db.ItemType.Where(x => x.IsDeleted == false).ToList();
                var DocumentTypeList = db.DocumentType.Where(x => x.IsDeleted == false).ToList();
                ViewBag.ItemTypeList = ItemTypeList;
                ViewBag.DocumentTypeList = DocumentTypeList;
                ViewBag.Users = Users;

                return View();

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "ActionHistory", ex.Message, ex.Source, ex.StackTrace);
                return View();
            }
        }

        [HttpPost]
        public RedirectToRouteResult ActionHistoryFiltered(string From, string To, string UId, int ItId, int DtId, bool Email, bool Upload, bool Download, bool AddNote)
        {
            try
            {
                return RedirectToAction("FilteredActionHistory", new { FromDate = From, ToDate = To, UserId = UId, ItId = ItId, DtId = DtId, Emailed = Email, Uploaded = Upload, Downloaded = Download, AddedNote = AddNote });

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "ActionHistoryFiltered", ex.Message, ex.Source, ex.StackTrace);
                return null;
            }
        }

        public PartialViewResult FilteredActionHistory(string FromDate, string ToDate, string UserId, int ItId, int DtId, bool Emailed, bool Uploaded, bool Downloaded, bool AddedNote)
        {
            try
            {
                DateTime? From = null;
                DateTime? To = null;
                if (FromDate != null)
                {
                    FromDate = FromDate + " " + "00:00:00";
                    From = Convert.ToDateTime(FromDate);
                }
                if (ToDate != null)
                {
                    ToDate = ToDate + " " + "23:59:59";
                    To = Convert.ToDateTime(ToDate);
                }

                if (FromDate == null && ToDate == null && UserId == null && ItId == 0 && DtId == 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == false
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).Take(100).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;


                }
                else if (FromDate != null && ToDate != null && UserId != null && ItId != 0 && DtId != 0)
                {
                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == false
                               && dh.UId == UserId
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                               && It.Id == ItId
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }



                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;

                }
                else if (FromDate != null && ToDate != null && UserId != null && ItId == 0 && DtId == 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == false
                               && dh.UId == UserId
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }


                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;
                }
                else if (FromDate != null && ToDate != null && UserId != null && ItId != 0 && DtId == 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where dh.Viewed == false
                               && dh.UId == UserId
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                               && It.Id == ItId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;
                }
                else if (FromDate != null && ToDate != null && UserId != null && ItId == 0 && DtId != 0)
                {



                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == false
                               && dh.UId == UserId
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }
                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;

                }
                else if (FromDate != null && ToDate != null && UserId == null && ItId == 0 && DtId == 0)
                {


                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == false
                               && (dh.AddedTime >= From && dh.AddedTime <= To)
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;

                }
                else if (UserId != null && ItId == 0 && DtId == 0)
                {


                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == false
                               && dh.UId == UserId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name

                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else
                        ViewData["DocPerformedAction"] = Doc;

                }
                else if (UserId != null && ItId != 0 && DtId != 0)
                {


                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where dh.Viewed == false
                               && dh.UId == UserId
                               && It.Id == ItId
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;

                }
                else if (UserId != null && ItId != 0 && DtId == 0)
                {
                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == false
                               && dh.UId == UserId
                               && It.Id == ItId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;

                }
                else if (UserId != null && ItId == 0 && DtId != 0)
                {


                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where dh.Viewed == false
                               && dh.UId == UserId
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }


                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;
                }
                else if (UserId != null)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where dh.Viewed == false
                               && dh.UId == UserId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;

                }
                else if (ItId != 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                               on d.Id equals dh.DId
                               join It in db.ItemType
                               on d.ItID equals It.Id
                               join Dt in db.DocumentType
                               on d.DtId equals Dt.Id
                               where  dh.Viewed == false
                               && It.Id == ItId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {                            
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name

                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;

                }
                else if (DtId != 0)
                {

                    var Doc = (from d in db.AlAinDocument
                               join dh in db.DocumentHistory
                              on d.Id equals dh.DId
                              join It in db.ItemType
                              on d.ItID equals It.Id
                              join Dt in db.DocumentType
                              on d.DtId equals Dt.Id
                              where  dh.Viewed == false
                               && Dt.Id == DtId
                                 && Dt.IsDeleted == false
                                   && It.IsDeleted == false
                               select new UserDocument()
                               {                             
                                   DocHistoryId = dh.Id,
                                   AddedTime = dh.AddedTime,
                                   DocViewed = dh.Viewed,
                                   DocDownloaded = dh.Downloaded,
                                   DocUploaded = dh.Uploaded,
                                   DocAddedNote = dh.AddedNote,
                                   DocEmailed = dh.Emailed,
                                   ItemType = It.Name,
                                   DocumentType = Dt.Name
                               }).OrderByDescending(x => x.AddedTime).ToList();


                    List<UserDocument> Docu = new List<UserDocument>();


                    if (Doc != null)
                    {
                        if (Emailed)
                            Docu.AddRange(Doc.Where(x => x.DocEmailed == Emailed).ToList());

                        if (Uploaded)
                            Docu.AddRange(Doc.Where(x => x.DocUploaded == Uploaded).ToList());

                        if (Downloaded)
                            Docu.AddRange(Doc.Where(x => x.DocDownloaded == Downloaded).ToList());

                        if (AddedNote)
                            Docu.AddRange(Doc.Where(x => x.DocAddedNote == AddedNote).ToList());
                    }

                    if (Docu.Count() > 0)
                        ViewData["DocPerformedAction"] = Docu;
                    else if (Emailed == false && Uploaded == false && Downloaded == false && AddedNote == false)
                        ViewData["DocPerformedAction"] = Doc;
                    else
                        ViewData["DocPerformedAction"] = null;
                }

                return PartialView("~/Views/Shared/_DocActions.cshtml");

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "FilteredActionHistory", ex.Message, ex.Source, ex.StackTrace);
                return PartialView("~/Views/Shared/_DocActions.cshtml");
            }
        }


        [HttpPost]
        public JsonResult ShowEmailDetail(int DocHId)
        {
            try
            {
               var data = (from d in db.Document
                            join dh in db.DocumentHistory
                            on d.Id equals dh.DId
                            join u in db.Users
                            on dh.UId equals u.Id
                            where  dh.Id == DocHId
                            select new UserDocument()
                            {
                                EmailRecipients = dh.EmailRecipients,
                                EmailBccRecipients = dh.EmailBccRecipients,
                                EmailSubject = dh.EmailBody,
                                EmailBody = dh.AddedBy,
                                UserId = u.Id,
                                UserEmail = u.Email
                            }).FirstOrDefault();


                return Json(data);


            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "ShowEmailDetail", ex.Message, ex.Source, ex.StackTrace);
                return Json(null);
            }
        }



        [HttpPost]
        public JsonResult ShowAttachmentEmail(int DId)
        {
            try
            {
                var Email = db.Document.Where(x => x.Id == DId).FirstOrDefault();
                return Json(Email);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "ShowAttachmentEmail", ex.Message, ex.Source, ex.StackTrace);
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult ShowNoteDetail(int DocHId)
        {
            try
            {


            var data = (from d in db.Document
                            join dh in db.DocumentHistory
                            on d.Id equals dh.DId
                            join u in db.Users
                            on dh.UId equals u.Id
                            where  dh.Id == DocHId
                            && dh.AddedNote == true
                            select new UserDocument()
                            {
                                UserNote = dh.UserNote,
                                UserId = u.Id,
                                UserEmail = u.Email
                            }).FirstOrDefault();


                return Json(data);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "ShowNoteDetail", ex.Message, ex.Source, ex.StackTrace);
                return Json(null);
            }
        }


        [HttpPost]
        public JsonResult SearchEmail(string Val)
        {
            try
            {
                if (Val != "")
                {
                    var data = db.Email.Where(x => x.Recipients.StartsWith(Val)).Select(x => new { x.Id, x.Recipients }).Distinct().ToList();
                    return Json(data);
                }
                else
                    return Json(0);
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "SearchEmail", ex.Message, ex.Source, ex.StackTrace);
                return Json(0);
            }

        }
        public string MimeType(string fileName)
        {
            try
            {
                fileName = fileName.Substring(fileName.LastIndexOf(".") + 1);
                string MediaType = "";
                if (fileName == "pdf" || fileName == "PDF")
                    MediaType = "application/pdf";
                else if (fileName == "tif" || fileName == "TIF" || fileName == "tiff" || fileName == "TIFF")
                   MediaType = "image/tiff";
                else if (fileName == "xls")
                    MediaType = "application/vnd.ms-excel";
                else if (fileName == "xlsx")
                    MediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                else if (fileName == "txt")
                    MediaType = "text/plain";
                else if (fileName == "doc")
                    MediaType = "application/msword";
                else if (fileName == "docx")
                    MediaType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                else if (fileName == "ppt")
                    MediaType = "application/vnd.ms-powerpoint";
                else if (fileName == "pptm")
                    MediaType = "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                else if (fileName == "png")
                    MediaType = "image/png";
                else if (fileName == "jpg" || fileName == "jpeg")
                    MediaType = "image/jpeg";
                else if (fileName == "bmp")
                    MediaType = "image/bmp";

                return MediaType;
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "MimeType", ex.Message, ex.Source, ex.StackTrace);
                return "";
            }

        }

        private void EmptyFolder(DirectoryInfo directory)
        {
            try
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "EmptyFolder", ex.Message, ex.Source, ex.StackTrace);
            }
        }

        private int GetTotalpages(string filePath)
        {
            try
            {
                int pageCount = 0;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (Image temp = Image.FromStream(fs))
                    {
                        pageCount = temp.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                    }
                }
                return pageCount;
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "GetTotalpages", ex.Message, ex.Source, ex.StackTrace);
                return 0;
            }
        }

        public ActionResult DocCreation()
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();

                if (User.IsInRole("DataEntryUser"))
                {
                    var ItemTypeList = (from uit in db.UserItemType
                                        join It in db.ItemType
                                        on uit.ItID equals It.Id
                                        where uit.UID == SessionUserId
                                        && It.IsDeleted == false
                                        select It).ToList();

                    ViewBag.ItemTypeList = ItemTypeList;
                }
                else
                {
                    var ItemTypeList = db.ItemType.Where(x => x.IsDeleted == false).ToList();
                    ViewBag.ItemTypeList = ItemTypeList;
                }


                return View();
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("DocumentController", "DocCreation", ex.Message, ex.Source, ex.StackTrace);
                return View();
            }
        }

        [HttpPost]
        public ActionResult GetUserDocTypes(int ItemTypeId)
        {
            try
            {
                var DocumentTypes = db.DocumentType.Where(x=>x.ItID == ItemTypeId && x.IsDeleted == false).ToList();
                return Json(DocumentTypes);
                
            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("UserController", "GetUserDocTypes", ex.Message, ex.Source, ex.StackTrace);
                return Json(null);
            }
        }
        [HttpPost]
        public ActionResult GetItemtypelist(int DeptId)
        {
            try
            {
                var DocumentTypes = db.ItemType.Where(x => x.MdId == DeptId && x.IsDeleted == false).ToList();
                return Json(DocumentTypes);

            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("UserController", "GetItemtypelist", ex.Message, ex.Source, ex.StackTrace);
                return Json(null);
            }
        }


        private void setItem(IItem item, object property, object value)
        {
            WIA.Property aProperty = item.Properties.get_Item(ref property);
            aProperty.set_Value(ref value);
        }

    }
}