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
using Exemplo3.Models;
using System.Security.Principal;
using Exemplo3.br.com.correios.ws;

namespace Exemplo3.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private Exemplo3Context db = new Exemplo3Context();

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("frete")]
        public IHttpActionResult CalculaFrete()
        {
            string frete;

            CalcPrecoPrazoWS correios = new CalcPrecoPrazoWS();

            cResultado resultado = correios.CalcPrecoPrazo("", "", "40010", "37540000", "37002970", "1", 1, 30, 30, 30, 30, "N", 100, "S");

            if (resultado.Servicos[0].Erro.Equals("0"))
            {
                frete = "Valor do frete: " + resultado.Servicos[0].Valor + " - Prazo de entrega: " + resultado.Servicos[0].PrazoEntrega + " dia(s)";
                return Ok(frete);
            }
            else
            {
                return BadRequest("Código do erro: " + resultado.Servicos[0].Erro + "-" + resultado.Servicos[0].MsgErro);
            }
            
        }

        // GET: api/Orders
        [Authorize(Roles = "ADMIN")]
        public List<Order> GetOrders()
        {
            return db.Orders.Include(order => order.OrderItems).ToList();
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (checkUserFromOrder(User, order))
            {

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            if (checkUserFromOrder(User, order))
            {
                db.Entry(order).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(id))
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
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (checkUserFromOrder(User, order))
            {
                db.Orders.Add(order);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);

            if (checkUserFromOrder(User, order))
            {
                if (order == null)
                {
                    return NotFound();
                }

                db.Orders.Remove(order);
                db.SaveChanges();

                return Ok(order);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool checkUserFromOrder (IPrincipal user, Order order)
        {
            return ((user.Identity.Name.Equals(order.userName)) || (user.IsInRole("ADMIN")));
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}