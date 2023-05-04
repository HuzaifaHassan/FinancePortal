﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbHandler.Data;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DbHandler.Model
{
    [Table("t_AddStudent")]
    public class AddStudent
    {
        [Key]
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
    }
}
