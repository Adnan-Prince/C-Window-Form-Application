using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Search_n_View.ViewModels
{

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class ManageUserViewModel
    {
        public List<UserViewModel> Users { get; set; }
    }

    public class ManageAccessGroupModel
    {
        public List<ManageAccessGroupViewModel> Groups { get; set; }
    }


    public class ManageDepartmentViewModel
    {
        public List<DepartmentViewModel> Department { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AccessGroupName { get; set; }

        public string Role { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ModifiedDate { get; set; }

    }

    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Department Name.")]
        public string Name { get; set; }

        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class EditDepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Department Name.")]
        public string Name { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class EditDepartmentIdViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

   public class CreateDepartmentViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Role")]
        public string RoleId { get; set; }


        [Display(Name = "Role")]
        public List<System.Web.Mvc.SelectListItem> RolesList { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int[] MdId { get; set; }

        [Display(Name = "Departments")]
        public List<System.Web.Mvc.SelectListItem> Departments { get; set; }


        [Required]
        [Display(Name = "ItemType")]
        public int[] ItId { get; set; }

        [Display(Name = "ItemTypes")]
        public List<System.Web.Mvc.SelectListItem> ItemTypes { get; set; }

        [Required]
        [Display(Name = "Access Right")]
        public int AcId { get; set; }

        [Display(Name = "Access Right")]
        public List<System.Web.Mvc.SelectListItem> AccessGroup { get; set; }



        //[Display(Name = "CNIC")]
        //[NumberHyphen(@"^[0-9]+(?:-[0-9]+)?([0-9]+(?:-[0-9]+)?)*$", ErrorMessage = "Not a valid NIC Format")]
        //[CNICRegex(@"^\(?([0-9]{5})\)?[-. ]?([0-9]{7})[-. ]?([0-9]{1})$", ErrorMessage = "Not a valid NIC Number")]
        //public string CNIC { get; set; }

        //[Display(Name = "Phone Number")]
        //[NumberHyphen(@"^[0-9]+(?:-[0-9]+)?([0-9]+(?:-[0-9]+)?)*$", ErrorMessage = "Not a valid Phone Number Format")]
        //[PhoneRegex(@"^\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone Number")]
        //public string PhoneNumber { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [CompareAttribute("NewPassword", ErrorMessage = "Password Doesn't Match.")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }

    public class EditUserViewModel
    {
        public string UserId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Role")]
        public string RoleId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Departments")]
        public int[] MdId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "ItemTypes")]
        public int[] ItId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Access Right")]
        public int AcId { get; set; }


        //[Display(Name = "CNIC")]
        //[NumberHyphen(@"^[0-9]+(?:-[0-9]+)?([0-9]+(?:-[0-9]+)?)*$", ErrorMessage = "Not a valid NIC Format")]
        //[CNICRegex(@"^\(?([0-9]{5})\)?[-. ]?([0-9]{7})[-. ]?([0-9]{1})$", ErrorMessage = "Not a valid NIC Number")]
        //public string CNIC { get; set; }

        //[Display(Name = "Phone Number")]
        //[NumberHyphen(@"^[0-9]+(?:-[0-9]+)?([0-9]+(?:-[0-9]+)?)*$", ErrorMessage = "Not a valid Phone Number Format")]
        //[PhoneRegex(@"^\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone Number")]
        //public string PhoneNumber { get; set; }
    }

    public class ManageItemTypeViewModel
    {
        public List<ItemTypeViewModel> ItemType { get; set; }
    }

    public class ItemTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter ItemType Name.")]
        public string Name { get; set; }

        public string DepartmentName { get; set; }
        public int MdId { get; set; }

        [Display(Name = "Departments")]
        public List<System.Web.Mvc.SelectListItem> Departments { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class EditItemTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter ItemType Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Department Name.")]
        public int MdId { get; set; }


        [Display(Name = "Departments")]
        public List<System.Web.Mvc.SelectListItem> Departments { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class EditItemTypeIdViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MdId { get; set; }

        [Display(Name = "Departments")]
        public List<System.Web.Mvc.SelectListItem> Departments { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }


    public class ManageDocumentTypeViewModel
    {
        public List<DocumentTypeViewModel> DocumentType { get; set; }
    }

    public class DocumentTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter DocumentType Name.")]
        public string Name { get; set; }

        public int ItId { get; set; }

        public string ItemTypeName { get; set; }


        [Display(Name = "ItemTypes")]
        public List<System.Web.Mvc.SelectListItem> ItemTypes { get; set; }

        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class EditDocumentTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter DocumentType Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter ItemType Name.")]
        public int ItId { get; set; }


        [Display(Name = "ItemTypes")]
        public List<System.Web.Mvc.SelectListItem> ItemTypes { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class EditDocumentTypeIdViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItId { get; set; }

        [Display(Name = "ItemTypes")]
        public List<System.Web.Mvc.SelectListItem> ItemTypes { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    //public class NumberHyphen : RegularExpressionAttribute
    //{
    //    public NumberHyphen(string pattern) : base(pattern) { }
    //}

    //public class CNICRegex : RegularExpressionAttribute
    //{
    //    public CNICRegex(string pattern) : base(pattern) { }
    //}

    //public class PhoneRegex : RegularExpressionAttribute
    //{
    //    public PhoneRegex(string pattern) : base(pattern) { }
    //}


}