using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using Search_n_View.Models;

namespace Search_n_View.Controllers
{
    public class BulkUploadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: BulkUpload
        public ActionResult UploadData()
        {
            if (TempData["BulkUploadMsg"] != null)
            {
                ViewBag.BulkUploadMsg = "Bulk Upload has been completed!";
            }
            

            return View();
        }

        [HttpPost]
        public ActionResult UploadData(HttpPostedFileBase fileToUpload)
        {
            try
            {
                var ActivePath = db.ActiveDrive.Where(x => x.IsActive == true).FirstOrDefault();
                string CurrentUserName = User.Identity.Name;
                string Date = DateTime.Now.ToString("ddMMyyyy");

                if (fileToUpload != null && fileToUpload.ContentLength > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    // If you use EPPlus in a noncommercial context
                    // according to the Polyform Noncommercial license:
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(fileToUpload.InputStream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet

                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Skip the header row (if present)
                        {
                            var rowData = new AlAinDocument
                            {
                                DocPath = Date + "\\" + GetFileName(worksheet.Cells[row, 1].Value.ToString()),
                                Extension = GetFileExtension(worksheet.Cells[row, 1].Value.ToString()),
                                PageCount = string.IsNullOrEmpty(worksheet.Cells[row, 2].Value.ToString()) ? 0 : Convert.ToInt32(worksheet.Cells[row, 2].Value),
                                BatchNo = worksheet.Cells[row, 3].Value.ToString(),
                                InvoiceNo = worksheet.Cells[row, 4].Value.ToString(),
                                DtId = GetBranchIDByBranchName(worksheet.Cells[row, 5].Value.ToString()), // Pharmacy Branch Name

                                // Parse the date here
                                InvoiceDate = DateTime.Now,
                            };

                            string dateValue = worksheet.Cells[row, 6].Value.ToString();
                            DateTime invoiceDate;

                            if (DateTime.TryParse(dateValue, out invoiceDate))
                            {
                                rowData.InvoiceDate = invoiceDate;
                            }
                            else
                            {
                                // Handle the case where the date is not valid. You can provide a default value or log an error.
                                rowData.InvoiceDate = DateTime.Now; // Default value (you can change this as needed)
                            }

                            rowData.ItID = GetPharmacyIDByPharmacyName(worksheet.Cells[row, 7].Value.ToString()); // Pharmacy Name
                            rowData.MdId = GetDepartmentIDByPharmacyName(worksheet.Cells[row, 7].Value.ToString()); // Department ID

                            // Below are the db columns that cannot be null. So, we need to give them some dummy values
                            // rowData.ItID = 1038,
                            // rowData.DtId = 1040,
                            rowData.ChannelName = "BO_ManualUploader";
                            rowData.BaseDrivePath = ActivePath.BasePath;
                            rowData.ScannedFilePath = ActivePath.BasePath;
                            rowData.AddedBy = CurrentUserName;
                            rowData.AddedTime = DateTime.Now;
                            rowData.ModifiedTime = DateTime.Now;
                            rowData.IsActive = true;
                            rowData.IsApproved = true;
                            rowData.IsDeleted = false;

                            // Save the rowData to the database using Entity Framework
                            db.AlAinDocument.Add(rowData);
                        }

                        db.SaveChanges(); // Save changes to the database
                    }
                }

            }
            catch (Exception ex)
            {

                //throw ex;
                Helpers.Logs.ErrorLog("BulkUploadController", "UploadData", ex.Message, ex.Source, ex.StackTrace);

            }


            TempData["BulkUploadMsg"] = "Bulk Upload has been completed!";
            TempData.Keep("BulkUploadMsg");

            return RedirectToAction("UploadData", "BulkUpload"); // Redirect to your desired view after processing the Excel file.


        }


        private int GetBranchIDByBranchName(string branchName)
        {
            var extractedValue = db.DocumentType.Where(x => x.Name == branchName).FirstOrDefault();

            if (extractedValue != null)
            {
                return Convert.ToInt32(extractedValue.Id);
            }
            else
            {
                return 00;
            }
        }
        private int GetPharmacyIDByPharmacyName(string pharmacyName)
        {
            var extractedValue = db.ItemType.Where(x => x.Name == pharmacyName).FirstOrDefault();

            if (extractedValue != null)
            {
                return Convert.ToInt32(extractedValue.Id);
            }
            else
            {
                return 00;
            }
        }
        private int GetDepartmentIDByPharmacyName(string pharmacyName)
        {
            var extractedValue = db.ItemType.Where(x => x.Name == pharmacyName).FirstOrDefault();

            if (extractedValue != null)
            {
                return Convert.ToInt32(extractedValue.MdId);
            }
            else
            {
                return 00;
            }
        }
        private string GetFileName(string value)
        {
            try
            {
                string fileName = string.Empty;
                //Path.GetExtension(value); // returns .exe
                fileName = Path.GetFileName(value); // returns File

                return fileName;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        private string GetFileExtension(string value)
        {
            try
            {
                string fileExtension = string.Empty;
                fileExtension = Path.GetExtension(value); // returns .exe

                return fileExtension;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public JsonResult ApproveDocumentByDocumentID(int documentID)
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;

                if (CurrentUserName != null && CurrentUserName != "")
                {
                    var itemToUpdate = db.AlAinDocument.Find(documentID);

                    if (itemToUpdate != null)
                    {
                        itemToUpdate.IsApproved = true;

                        db.SaveChanges(); // Save the changes to the database
                        return Json(new { success = true, message = "Value updated successfully" });
                    }

                    return Json(new { success = false, message = "Item not found" });
                }
                else
                {
                    return Json(new { success = false, message = "Current user not found" });
                }


            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("BulkUploadController", "ApproveDocumentByDocumentID", ex.Message, ex.Source, ex.StackTrace);
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult ApproveAllDocument()
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;

                if (CurrentUserName != null && CurrentUserName != "")
                {
                    var itemsToUpdate = db.AlAinDocument.Where(x => x.IsApproved != true).ToList();

                    if (itemsToUpdate.Count > 0)
                    {
                        foreach (var item in itemsToUpdate)
                        {
                            item.IsApproved = true;
                        }

                        db.SaveChanges(); // Save the changes to the database

                        return Json(new { success = true, message = "All documents approved successfully" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Current user not found" });
                }

                return Json(new { success = false, message = "No items found to update" });


            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("BulkUploadController", "ApproveAllDocument", ex.Message, ex.Source, ex.StackTrace);
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult UpdateDocumentByDocumentID(string invoiceNo, int documentID)
        {
            try
            {
                var SessionUserId = User.Identity.GetUserId();
                string CurrentUserName = User.Identity.Name;

                if (CurrentUserName != null && CurrentUserName != "")
                {
                    var itemToUpdate = db.AlAinDocument.Find(documentID);

                    if (itemToUpdate != null)
                    {
                        itemToUpdate.InvoiceNo = invoiceNo;

                        db.SaveChanges(); // Save the changes to the database
                        return Json(new { success = true, message = "Value updated successfully" });
                    }

                    return Json(new { success = false, message = "Item not found" });
                }
                else
                {
                    return Json(new { success = false, message = "Current user not found" });
                }


            }
            catch (Exception ex)
            {
                Helpers.Logs.ErrorLog("BulkUploadController", "UpdateDocumentByDocumentID", ex.Message, ex.Source, ex.StackTrace);
                return Json(false);
            }
        }

    }
}