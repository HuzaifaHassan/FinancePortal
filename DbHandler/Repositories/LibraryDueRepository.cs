using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbHandler.Model;
using DbHandler.Data;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace DbHandler.Repositories
{
    public class LibraryDueRepository:ILibraryDueRepository
    {
        private readonly ApplicationDbContext _ctx;
        public LibraryDueRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public void AddLibraryDue(LibraryDues model)
        { 
         _ctx.TLibraryDue.Add(model);
        }
        public LibraryDues GetByStudentid(string id)
        {
            var resp = _ctx.TLibraryDue.Where(x => x.id == id).FirstOrDefault();
            return resp;
        }
        public LibraryDues GetByStudentcid(string cid)
        {
            var resp = _ctx.TLibraryDue.Where(x => x.cstid == cid).FirstOrDefault();
            return resp;
        }
        public LibraryDues GetByStudentIdAndRef(string cid, string _ref)
        {
            return _ctx.TLibraryDue.Where(x => x.cstid == cid && x.Reference == _ref).FirstOrDefault();
        }
        public void UpdateLibraryDues(LibraryDues model)
        {
            _ctx.TLibraryDue.Update(model);
        }
        public bool Save()
        {
            return _ctx.SaveChanges() >= 0;
        }
         
    }

}
