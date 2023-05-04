using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DbHandler.Data;
using DbHandler.Model;
using DbHandler.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using static FinancePortal.DTO.DTO;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using FinancePortal.Responses;
using FinancePortal.Models;
using FinancePortal.DTO;
using FinancePortal.Helper;
using System.Transactions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static System.Reflection.Metadata.BlobBuilder;

namespace FinancePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class Auth : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IStudentRepository _studentRep;
        private readonly ICourseDueRepository _courserep;
        private readonly ILibraryDueRepository _libg;
        private readonly IAdminRepository _admin;
        private readonly IConfiguration _config;
        private readonly APIHelper _helper;
        private readonly UserManager<ApplicationUser> _userManager;
        public Auth(ApplicationDbContext ctx, IStudentRepository studentRep, ICourseDueRepository courserep, ILibraryDueRepository libg, IAdminRepository admin, IConfiguration config, APIHelper helper,UserManager<ApplicationUser> usermanager)
        {
            _ctx = ctx;
            _studentRep = studentRep;
            _courserep = courserep;
            _libg = libg;
            _admin = admin;
            _config = config;
            _userManager = usermanager;
            _helper = new APIHelper(studentRep, usermanager, config);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO DTO)
        {
            DateTime _startTime = DateTime.Now;
            try
            {
                var user = _admin.GetByNameandPassword(DTO.name, DTO.password);
                if (user == null || user.password != DTO.password)
                {
                    return await _helper.Response("err-001", Level.Error, "Invalid email or password", ActiveErrorCode.Failed, _startTime,  HttpContext, _config, DTO.BaseClass, DTO, "", ReturnResponse.Unauthorized, null, false);
                }
                return await _helper.Response("suc-001", Level.Success, user.name, ActiveErrorCode.Success, _startTime, HttpContext, _config, DTO.BaseClass, DTO, user.name, ReturnResponse.Success, null, true);

            }
            catch (Exception ex)
            {
                return await _helper.Response("err-001", Level.Error, ex.Message, ActiveErrorCode.Failed, _startTime, HttpContext, _config, DTO.BaseClass, DTO, "", ReturnResponse.BadRequest, ex, false);
            }





        }
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(ActiveResponse<RegistraiontObject>), 200)]

        public async Task<IActionResult> AddStudent([FromBody] StudentDTO DTO)
        {
            DateTime _startTime = DateTime.Now;
            var name = "";
            bool exist = false;
            var id = "";
            try 
            {
                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<StudentDTO>(jso);
                if (!TryValidateModel(DTO))
                {
                    //if (!ModelState.IsValid)
                    //{
                    return await _helper.Response("err-Model", Level.Success, _helper.GetErrors(ModelState), ActiveErrorCode.Failed, _startTime,  HttpContext, _config, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, null, false);    //}

                }
                var addStudent = new StudentDetails
                {
                    Id=DTO.Id,
                    stId=DTO.stId,
                    cstID=DTO.cstID,
                    CreatedOn=DTO.CreatedOn,
                    IsActive=DTO.IsActive,
                    Name=DTO.Name,
                    LastName=DTO.LastName,
                    Email=DTO.Email,
                    Password=DTO.Password,
                    MobileNo=DTO.MobileNo,
                   // IsGraduated="No"
                };
                _studentRep.AddStudentDets(addStudent);
                _studentRep.Save();
                return await _helper.Response("suc-001", Level.Success, addStudent, ActiveErrorCode.Success, _startTime,  HttpContext, _config, DTO.BaseClass, forLog, id, ReturnResponse.Success, null, true);

            }
            catch (Exception ex)
            {

                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<StudentDTO>(jso);

                return await _helper.Response("ex-0001", Level.Error, null, ActiveErrorCode.Failed, _startTime,  HttpContext, null, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, ex, false);

            }



        }

    }
}
