using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbHandler.Data;
using DbHandler.Model;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbHandler.Repositories
{
    
    public class StudentRepository:IStudentRepository
    {
        private readonly ApplicationDbContext _ctx;
        public StudentRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public void AddStudentDets(StudentDetails Model)
        {
            _ctx.TStudent.Add(Model);
        }
        public StudentDetails GetByStudentId(string StudentId)
        {
            return _ctx.TStudent.Where(x => x.stId == StudentId).FirstOrDefault();


        }
        public StudentDetails GetByStudentCId(string CId)
        {
            return _ctx.TStudent.Where(x => x.cstID == CId).FirstOrDefault();


        }
        public List<StudentDetails> GetStudent()
        {
            return _ctx.TStudent.Where(x => x.IsActive == true).ToList();
        }
        public StudentDetails GetByStudentandCstudentId(string studentid, string cstudentid)
        {
            return _ctx.TStudent.Where(x => x.Id == studentid && x.cstID == cstudentid).FirstOrDefault();
        
        }
        public void UpdateStudentDet(StudentDetails model)
        {
            _ctx.TStudent.Update(model);

        }
        public bool Save()
        {
            return _ctx.SaveChanges() >= 0;
        }
    }

}
