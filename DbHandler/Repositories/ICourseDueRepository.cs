using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbHandler.Data;
using DbHandler.Model;

namespace DbHandler.Repositories
{
    public  interface ICourseDueRepository
    {
        public void AddCourseDue(CourseDues model);
        public CourseDues GetCourseDueById(string id);
        public CourseDues GetCourseDueByRef(string Ref);

        public CourseDues GetCourseDueBycstid(string cstid);
        public void UpdateCourseDue(CourseDues model);
        public bool Save();
      //  void AddCourseDue(global::FinancePortal.DTO.AddCourseDue addCourse);
    }
}
