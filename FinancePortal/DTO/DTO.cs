using DbHandler.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancePortal.DTO
{
    public class DTO
    {
      
    }
    public class BaseClass
    {
        public string IP { get; set; }

        [Required(ErrorMessage = "dto-0001")]
        public string DeviceID { get; set; }

        [Required(ErrorMessage = "err-0002")]
        public string OnBoardingChannel { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }




    }
    public class LoginDTO
    {
        [Required]
      public string name { get; set; }
        [Required]
        public string password { get; set; }

        public BaseClass BaseClass { get; set; }
    
    }
    public class StudentDTO
    {
        public string Id { get; set; }
        public string stId { get; set; }
        public string cstID { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }

        //  public string CourseDues { get; set; }

        //  public string LibraryDues { get; set; }

       // public string IsGraduated { get; set; }

        public BaseClass BaseClass { get; set; }
    }
    public class GetStudents
    {
       public string StudentID { get; set; }
        public BaseClass BaseClass { get; set; }
    }
    public class AddStudentDTO
    {
        public string Id { get; set; }
        public string stId { get; set; }
        public string cstID { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }

        //  public string CourseDues { get; set; }

        //  public string LibraryDues { get; set; }

         public string IsGraduated { get; set; }

        public BaseClass BaseClass { get; set; }
    }
    public class AddCourseDue
    {
        public string id { get; set; }
        public string cstid { get; set; }
        public string CourseDue { get; set; }
        public string Reference { get; set; }
        public bool IsPaid { get; set; }
        public BaseClass BaseClass { get; set; }
    }
}
