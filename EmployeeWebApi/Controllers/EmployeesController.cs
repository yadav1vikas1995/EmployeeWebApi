using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeWebApi.Controllers
{
    public class EmployeesController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
            {
                return employeeDBEntities.Employees.ToList();
            }
        }
        //[HttpGet]
        //public IEnumerable<Employee> LoadAllEmployee()
        //{
        //    try {

        //        using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
        //        {
        //            return employeeDBEntities.Employees.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public HttpResponseMessage Get(int Id)
        {
            using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
            {
                var entities= employeeDBEntities.Employees.FirstOrDefault(e=>e.id==Id);
                if (entities != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entities);
                }
                else 
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Employee with Id " +Id.ToString()+ " Not Found");
                }
            }
        }

        //public void Post([FromBody]Employee employee)
        //{
        //    using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
        //    {
        //        employeeDBEntities.Employees.Add(employee);
        //        employeeDBEntities.SaveChanges();
        //    }
        //}
        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
               using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    employeeDBEntities.Employees.Add(employee);
                    employeeDBEntities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.id.ToString());
                    return message;
                }
            } catch(Exception ex)
            {
               return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex);
            }
           
        }

        public HttpResponseMessage Delete(int Id)
        {
            try {

                using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    var entites = employeeDBEntities.Employees.FirstOrDefault(e => e.id == Id);
                    if (entites == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id" + Id.ToString() + "Not Found");
                    }
                    else
                    {
                        employeeDBEntities.Employees.Remove(entites);
                        employeeDBEntities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            

        }

        //public HttpResponseMessage Put(int Id,[FromBody]Employee employee)
        //public HttpResponseMessage Put(int Id, [FromUri]Employee employee)
        public HttpResponseMessage Put([FromBody]int Id, [FromUri]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    var entites = employeeDBEntities.Employees.FirstOrDefault(e=>e.id==Id);
                    if (entites != null)
                    {
                       // entites.id=employee.id;
                        entites.Name = employee.Name;
                        entites.Salary = employee.Salary;
                        employeeDBEntities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK,entites);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id " + Id.ToString() + " Not Found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
