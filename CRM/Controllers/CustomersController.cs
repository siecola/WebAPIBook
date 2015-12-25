using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CRM.Models;

namespace CRM.Controllers
{
    [Authorize]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private CRMContext db = new CRMContext();
        private CRMContext dbValidation = new CRMContext();

        // GET: api/Customers
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpGet]
        [Route("bycpf")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomerByCpf(string cpf)
        {
            Customer customer = (Customer)db.Customers.FirstOrDefault(p => p.cpf.Equals(cpf));

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customer);
            }
        }

        [HttpGet]
        [Route("byemail")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomerByEmail(string email)
        {
            Customer customer = (Customer)db.Customers.FirstOrDefault(p => p.email.Equals(email));

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customer);
            }
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest("O ID da requisição deve ser o mesmo do customer");
            }

            Customer customerAux = findCustomerByCPF(customer.cpf);
            if ((customerAux != null) && (customerAux.Id != customer.Id)) 
            {
                return BadRequest("Já existe um customer com o mesmo CPF.");
            }

            customerAux = findCustomerByEmail(customer.email);
            if ((customerAux != null) && (customerAux.Id != customer.Id))
            {
                return BadRequest("Já existe um customer com o mesmo e-mail.");
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (findCustomerByCPF(customer.cpf) != null)
            {
                return BadRequest("Já existe um customer com o mesmo CPF.");
            }

            if (findCustomerByEmail(customer.email) != null)
            {
                return BadRequest("Já existe um customer com o mesmo e-mail.");
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }

        private Customer findCustomerByCPF (string cpf)
        {
            return dbValidation.Customers.FirstOrDefault(c => c.cpf == cpf);            
        }

        private Customer findCustomerByEmail(string email)
        {
            return dbValidation.Customers.FirstOrDefault(c => c.email == email);
        }

    }
}