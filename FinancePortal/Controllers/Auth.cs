﻿using System;
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
        private readonly IAddStudentRepository _addStudentRep;
        private readonly APIHelper _helper;
        private readonly UserManager<ApplicationUser> _userManager;
        public Auth(ApplicationDbContext ctx, IStudentRepository studentRep, ICourseDueRepository courserep, ILibraryDueRepository libg, IAdminRepository admin, IConfiguration config, APIHelper helper,UserManager<ApplicationUser> usermanager, IAddStudentRepository addStudentRep)
        {
            _ctx = ctx;
            _studentRep = studentRep;
            _courserep = courserep;
            _libg = libg;
            _admin = admin;
            _config = config;
            _userManager = usermanager;
            _helper = new APIHelper(studentRep, usermanager, config);
            _addStudentRep = addStudentRep;
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
                    IsGraduated="No"
                };
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7236/"); 
                var requestUri = "api/AuthController/Register";
                var requestBody = new StringContent(JsonConvert.SerializeObject(addStudent), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(requestUri, requestBody);

                // Check response status
                var content = await response.Content.ReadAsStringAsync();
                var financeResponse = JsonConvert.DeserializeObject<ActiveResponse<StudentDetails>>(content);
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
        [HttpPost]
        [Route("GetStudents")]
        [ProducesResponseType(typeof(ActiveResponse<List<StudentDetails>>), 200)]
        public async Task<IActionResult> GettAllStudents([FromBody] GetStudents DTO)
        {
            var id = "";
            DateTime startTime = DateTime.Now;
            try
            {
                if (!TryValidateModel(DTO))
                {
                    if (!ModelState.IsValid)
                    {
                        return await _helper.Response("err-Model", Level.Success, _helper.GetErrors(ModelState), ActiveErrorCode.Failed, startTime, HttpContext, _config, DTO.BaseClass, DTO, "", ReturnResponse.BadRequest, null, false);
                    }

                }
                // id = DTO.StudentID;
                var student = _studentRep.GetStudent();
                return await _helper.Response("succ-001", Level.Success, student, ActiveErrorCode.Success, startTime,  HttpContext, _config, DTO.BaseClass, DTO, "", ReturnResponse.Success, null, false);

            }
            catch (Exception ex)
            {
                return await _helper.Response("ex-0003", Level.Error, null, ActiveErrorCode.Failed, startTime,  HttpContext, _config, DTO?.BaseClass, DTO, "", ReturnResponse.BadRequest, ex, false);


            }


        }
        [HttpPost]
        [Route("CreateStudentAccount")]
        [ProducesResponseType(typeof(ActiveResponse<AddStudent>), 200)]
        public async Task<IActionResult> ApproveStudentAccount([FromBody] AddStudentDTO DTO)
        {

            DateTime _startTime = DateTime.Now;
            var name = "";
            bool exist = false;
            var id = "";
            try
            {
                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<AddStudentDTO>(jso);
                if (!TryValidateModel(DTO))
                {
                    //if (!ModelState.IsValid)
                    //{
                    return await _helper.Response("err-Model", Level.Success, _helper.GetErrors(ModelState), ActiveErrorCode.Failed, _startTime, HttpContext, _config, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, null, false);    //}

                }
                var addStudentt = new AddStudent
                {
                    Id = DTO.Id,
                    stId = DTO.stId,
                    cstID = DTO.cstID,
                    CreatedOn = DTO.CreatedOn,
                    IsActive = DTO.IsActive,
                    Name = DTO.Name,
                    LastName = DTO.LastName,
                    Email = DTO.Email,
                    Password = DTO.Password,
                    MobileNo = DTO.MobileNo,
                    IsGraduated = DTO.IsGraduated
                };
                 _addStudentRep.AddStudentDets(addStudentt);
                _addStudentRep.Save();
                return await _helper.Response("suc-001", Level.Success, addStudentt, ActiveErrorCode.Success, _startTime, HttpContext, _config, DTO.BaseClass, forLog, id, ReturnResponse.Success, null, true);

            }
            catch (Exception ex)
            {

                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<StudentDTO>(jso);

                return await _helper.Response("ex-0001", Level.Error, null, ActiveErrorCode.Failed, _startTime, HttpContext, null, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, ex, false);

            }



        }
        [HttpPost]
        [Route("GetCourseDues")]
        [ProducesResponseType(typeof(ActiveResponse<CourseDues>), 200)]
        public async Task<IActionResult> GetCourseFees([FromBody] AddCourseDue DTO)
        {

            DateTime _startTime = DateTime.Now;
            var name = "";
            bool exist = false;
            var _ref= "";
            var id = "";
            var cstid = "";
            try
            {
                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<AddCourseDue>(jso);
                if (!TryValidateModel(DTO))
                {
                    //if (!ModelState.IsValid)
                    //{
                    return await _helper.Response("err-Model", Level.Success, _helper.GetErrors(ModelState), ActiveErrorCode.Failed, _startTime, HttpContext, _config, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, null, false);    //}

                }
                cstid = DTO.cstid;
                //var Reff = Guid.NewGuid().ToString();
                //_ref = "#" + Reff;
              
                    var addCourse = new CourseDues
                    {
                        id = DTO.id,
                        cstid = cstid,
                        CourseDue = DTO.CourseDue,
                        Reference = DTO.Reference,
                        IsPaid = true
                    };


                    _courserep.AddCourseDue(addCourse);
                    _courserep.Save();
             
              
                  
                    


                
                return await _helper.Response("suc-001", Level.Success, _ref, ActiveErrorCode.Success, _startTime, HttpContext, _config, DTO.BaseClass, forLog,cstid , ReturnResponse.Success, null, true);

            }
            catch (Exception ex)
            {

                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<StudentDTO>(jso);

                return await _helper.Response("ex-0001", Level.Error, null, ActiveErrorCode.Failed, _startTime, HttpContext, null, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, ex, false);

            }



        }
        [HttpPost]
        [Route("PayCourseDues")]
        [ProducesResponseType(typeof(ActiveResponse<CourseDues>), 200)]
        public async Task<IActionResult> PayCourseFees([FromBody] AddCourseDue DTO)
        {

            DateTime _startTime = DateTime.Now;
            var name = "";
            bool exist = false;
            var _ref = "";
            var id = "";
            var cstid = "";
            try
            {
                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<AddCourseDue>(jso);
                if (!TryValidateModel(DTO))
                {
                    //if (!ModelState.IsValid)
                    //{
                    return await _helper.Response("err-Model", Level.Success, _helper.GetErrors(ModelState), ActiveErrorCode.Failed, _startTime, HttpContext, _config, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, null, false);    //}

                }
                cstid = DTO.cstid;
                //var Reff = Guid.NewGuid().ToString();
                //_ref = "#" + Reff;
                var course= _courserep.GetCourseDueBycstid(cstid);
                var addCourse = new CourseDues
                {
                    id = DTO.id,
                    cstid = cstid,
                    CourseDue = DTO.CourseDue,
                    Reference = DTO.Reference,
                    IsPaid = DTO.IsPaid
                };


                _courserep.AddCourseDue(addCourse);
                _courserep.Save();
                return await _helper.Response("suc-001", Level.Success, _ref, ActiveErrorCode.Success, _startTime, HttpContext, _config, DTO.BaseClass, forLog, cstid, ReturnResponse.Success, null, true);

            }
            catch (Exception ex)
            {

                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<StudentDTO>(jso);

                return await _helper.Response("ex-0001", Level.Error, null, ActiveErrorCode.Failed, _startTime, HttpContext, null, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, ex, false);

            }



        }
        [HttpPost]
        [Route("GetLibraryDues")]
        [ProducesResponseType(typeof(ActiveResponse<RegistraiontObject>), 200)]

        public async Task<IActionResult> GetLibraryDues([FromBody] LibraryDuesDTO DTO)
        {
            DateTime _startTime = DateTime.Now;
            var name = "";
            bool exist = false;
            var id = "";
            try
            {
                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<LibraryDuesDTO>(jso);
                if (!TryValidateModel(DTO))
                {
                    //if (!ModelState.IsValid)
                    //{
                    return await _helper.Response("err-Model", Level.Success, _helper.GetErrors(ModelState), ActiveErrorCode.Failed, _startTime, HttpContext, _config, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, null, false);    //}

                }
                var addLibraryDues = new LibraryDues
                {
                   id=DTO.id,
                   cstid=DTO.cstid,
                   Reference=DTO.Reference,
                   LibraryDue=DTO.LibraryDue,
                   IsCleared=DTO.IsCleared
                };

                _libg.AddLibraryDue(addLibraryDues);
                _libg.Save();
                return await _helper.Response("suc-001", Level.Success, addLibraryDues, ActiveErrorCode.Success, _startTime, HttpContext, _config, DTO.BaseClass, forLog, id, ReturnResponse.Success, null, true);

            }
            catch (Exception ex)
            {

                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<LibraryDuesDTO>(jso);

                return await _helper.Response("ex-0001", Level.Error, null, ActiveErrorCode.Failed, _startTime, HttpContext, null, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, ex, false);

            }



        }
        [HttpPost]
        [Route("ClearLibraryDues")]
        [ProducesResponseType(typeof(ActiveResponse<RegistraiontObject>), 200)]

        public async Task<IActionResult> ClearLibraryDues([FromBody] LibraryDuesDTO DTO,string Cid,string _ref)
        {
            DateTime _startTime = DateTime.Now;
            var name = "";
            bool exist = false;
            var id = "";
            try
            {
                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<LibraryDuesDTO>(jso);
                if (!TryValidateModel(DTO))
                {
                    //if (!ModelState.IsValid)
                    //{
                    return await _helper.Response("err-Model", Level.Success, _helper.GetErrors(ModelState), ActiveErrorCode.Failed, _startTime, HttpContext, _config, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, null, false);    //}

                }
                var _getLibraryDues = _libg.GetByStudentIdAndRef(Cid,_ref);
                var addLibraryDues = new LibraryDues
                {
                    
                    IsCleared = DTO.IsCleared
                };

                _libg.UpdateLibraryDues(addLibraryDues);
                _libg.Save();
                return await _helper.Response("suc-001", Level.Success, addLibraryDues, ActiveErrorCode.Success, _startTime, HttpContext, _config, DTO.BaseClass, forLog, id, ReturnResponse.Success, null, true);

            }
            catch (Exception ex)
            {

                var jso = JsonConvert.SerializeObject(DTO);

                var forLog = JsonConvert.DeserializeObject<LibraryDuesDTO>(jso);

                return await _helper.Response("ex-0001", Level.Error, null, ActiveErrorCode.Failed, _startTime, HttpContext, null, DTO.BaseClass, forLog, "", ReturnResponse.BadRequest, ex, false);

            }



        }
        [HttpPost]
        [Route("GraduationEligibility")]
        [ProducesResponseType(typeof(ActiveResponse<RegistraiontObject>), 200)]

        public async Task<IActionResult> GraduationEligibility(string cid)
        {
            DateTime _startTime = DateTime.Now;
            var name = "";
            bool exist = false;
            var id = "";
            try
            {

                var _eligible = _studentRep.GetByStudentCId(cid);
                var _course = _courserep.GetCourseDueBycstid(cid);
                var _lib = _libg.GetByStudentcid(cid);
                if (_course.IsPaid == true && _lib.IsCleared == true)
                {
                    var Grad = new StudentDetails
                    {

                        IsGraduated="Yes"
                    };
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://localhost:7120/");
                    var requestUri = "api/Auth/GetStudentDetails";
                    var requestBody = new StringContent(JsonConvert.SerializeObject(Grad), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(requestUri, requestBody);

                    // Check response status
                    var content = await response.Content.ReadAsStringAsync();
                    var financeResponse = JsonConvert.DeserializeObject<ActiveResponse<StudentDetails>>(content);
                    _studentRep.UpdateStudentDet(Grad);
                    _studentRep.Save();

                }
               
                return await _helper.Response("Graduated", Level.Success, _eligible, ActiveErrorCode.Success, _startTime, HttpContext, _config, null, null, id, ReturnResponse.Success, null, true);

            }
            catch (Exception ex)
            {

               

                return await _helper.Response("ex-0001", Level.Error, null, ActiveErrorCode.Failed, _startTime, HttpContext, null, null, null, "", ReturnResponse.BadRequest, ex, false);

            }



        }
    }
}
