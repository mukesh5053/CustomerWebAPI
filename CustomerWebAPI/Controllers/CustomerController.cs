using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerWebAPI.Repositories;
using CustomerDataAccess;
using Microsoft.AspNetCore.Authorization;

namespace CustomerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerRespository _customers = null;

        public CustomerController(ICustomerRespository customerRespository)
        {
            _customers = customerRespository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Customer>> GetCustomers([FromQuery] QueryParameters queryParameters)
        {
            try
            { 
                //Paging is not supported hence we have commented the inner code of get customer and returning all the records...
                return Ok(await _customers.GetCustomers(queryParameters.Page, queryParameters.Size));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retreiving data from Database...");
            }

        }


        //[Authorize]
        //[HttpGet]
        //public async Task<ActionResult<Customer>> GetCustomers()
        //{
        //    try
        //    {               
        //        return Ok(await _customers.GetCustomers());
        //    }
        //    catch (Exception)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error in retreiving data from Database...");
        //    }

        //}


        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<string> GetUser()
        {
            try
            {
                return HttpContext.User.Identity.Name.ToString();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retreiving data from Database...");
            }

        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer([FromRoute]int id)
        {
            try
            {
                var result = await _customers.GetCustomer(id);

              if(result == null)
                {
                  return  NotFound($"Customer not found with id : {id}");
                }

                return result;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retreiving data from Database...");
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    return BadRequest();
                }

                var result = await _customers.SaveCustomer(customer);
                return CreatedAtAction(nameof(GetCustomer), new { id = result.ID }, result);

               
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving customer data...");
            }
           
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> UpdateCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            try
            {
                if (id != customer.ID)
                {
                    return BadRequest("Id missmatch");
                }

                if (await _customers.GetCustomer(id) == null)
                {
                    return NotFound($"Customer {id} not exists in database");
                }
                return Ok(await _customers.UpdateCustomer(id, customer));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating customer data...");
            }
           
        
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Customer>> DeleteCustomer([FromRoute]int id)
        {
            try
            {
                if (await _customers.GetCustomer(id) == null)
                {
                    return NotFound($"Customer {id} not exists in database...");
                }

                return Ok( await _customers.DeleteCustomer(id));
            }
            catch (Exception)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting customer data...");
            }
        }


    }
}
