using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WHMS.DAL.Data;
using WHMS.DAL.Repositories.Interfaces;
using WHMS.DAL.Repositories;
using DAL.Repositories;

namespace WHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<ApplicationUserController> _logger;
        private readonly GenericRepository<AppUser> genericRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext applicationDbContext;
        public TransactionController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<ApplicationUserController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            this.signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        
        [HttpGet("AllTransaction")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var trans = _unitOfWork.RecieverRepository.GetAll().ToList(); // Assuming you have a repository for users

                if (trans != null)
                {
                    // You can customize the user information returned as needed
                    var userResponse = trans.Select(trans => new
                    {
                        WOID = trans.WOID,
                        BoxNo = trans.BoxNo,
                        ClientName = trans.ClientName,
                        StatusId = trans.StatusId,
                        RoleId = trans.RoleId,
                        TimeIn = trans.TimeIn,
                        TimeOut = trans.TimeOut,
                    });
                    return Ok(new
                    {
                        data = userResponse,
                        status = "All Transactions Retrieved Successfully"
                    });
                }

                return NotFound("No users found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }
        
        [HttpGet("TransactionByBoxno")]
        public IActionResult GetTransactionByBoxno()
        {
            try
            {
                var transactions = _unitOfWork.RecieverRepository.getAdnan()
                    // Assuming you have a repository for users

                if (trans != null)
                {
                    // You can customize the user information returned as needed
                    var userResponse = trans.Select(trans => new
                    {
                        WOID = trans.WOID,
                        BoxNo = trans.BoxNo,
                        ClientName = trans.ClientName,
                        StatusId = trans.StatusId,
                        RoleId = trans.RoleId,
                        TimeIn = trans.TimeIn,
                        TimeOut = trans.TimeOut,
                    });
                    return Ok(new
                    {
                        data = userResponse,
                        status = "All Transactions Retrieved Successfully"
                    });
                }

                return NotFound("No users found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

    }
}
