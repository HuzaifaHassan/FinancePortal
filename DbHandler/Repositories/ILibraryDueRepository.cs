using System;
using System.Collections.Generic;
using System.Linq;
using DbHandler.Data;
using DbHandler.Model;
using System.Text;
using System.Threading.Tasks;

namespace DbHandler.Repositories
{
    public  interface ILibraryDueRepository
    {
        public void AddLibraryDue(LibraryDues model);
        public LibraryDues GetByStudentid(string id);
        public LibraryDues GetByStudentIdAndRef(string cid, string _ref);
        public LibraryDues GetByStudentcid(string cid);
        public void UpdateLibraryDues(LibraryDues model);
        public bool Save();
    }
}
