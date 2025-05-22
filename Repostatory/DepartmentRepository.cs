using CareNet_System.Models;

namespace CareNet_System.Repostatory
{
    public class DepartmentRepository:IDepartmentRepository

    {
        HosPitalContext context;
        
        public DepartmentRepository( HosPitalContext ctx) {
        context = ctx ;
                
        }

        public List<Department> GetAll() { 
        
            return context.Departments.ToList();
        }

        public void Add (Department obj)
        {
            context.Departments.Add(obj);
        }

        public void Update(Department obj) {

            List<Department> deptsList = context.Departments.ToList();

            Department dept = context.Departments.FirstOrDefault(d => d.Id == obj.Id);

            dept.name = obj.name;
            dept.manager = obj.manager;
            dept.employees_num = obj.employees_num;
            dept.Patients_num = obj.Patients_num;
           // context.Departments.Update(obj);
            
        }
        public void Delete(int id) { 
        context.Departments.Remove(context.Departments.FirstOrDefault(d=>d.Id==id));
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public Department GetById(int id)
        {
           return context.Departments.FirstOrDefault(d => d.Id == id);

            

        }
    }
}
