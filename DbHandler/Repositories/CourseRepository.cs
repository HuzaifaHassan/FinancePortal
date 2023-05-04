using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbHandler.Data;
using DbHandler.Model;
using System.Threading.Tasks;

namespace DbHandler.Repositories
{
    public class CourseRepository: ICourseDueRepository
    {
        private readonly ApplicationDbContext _ctx;

        public CourseRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public void AddCourseDue(CourseDues model)
        {
            _ctx.TCourseDue.Add(model);
        }
        public CourseDues GetCourseDueById(string id)
        {
            return _ctx.TCourseDue.Where(x => x.id == id).FirstOrDefault();
        }
        public CourseDues GetCourseDueByRef(string Ref)
        {
            return _ctx.TCourseDue.Where(x => x.Reference == Ref).FirstOrDefault();
        
        }
        public CourseDues GetCourseDueBycstid(string cstid)
        {
            return _ctx.TCourseDue.Where(x => x.cstid == cstid).FirstOrDefault();
        }
        public void UpdateCourseDue(CourseDues model)
        {
            _ctx.TCourseDue.Update(model);
        }
        public bool Save()
        {
            return _ctx.SaveChanges() >= 0;
        
        }
         

    }
}
