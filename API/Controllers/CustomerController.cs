using API.Helpers;
using Application.Dto;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace API.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(
            ICustomerService customerService
        )
        {
            _customerService = customerService;
        }

        #region Customer Operations

        /// <summary>
        /// Retrieves a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>An ActionResult with information about the result of the operation.</returns>
        [ApiExplorerSettings(GroupName = "Customer")]
        [HttpGet("GetCustomer/{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            return CreateHttpResponse(() =>
            {
                var response = _customerService.GetCustomer(id).Result;
                var isSuccess = response != null;
                return new OkObjectResult(new SuccessResponseVM
                {
                    Message = isSuccess ? "Customer retrieved successfully" : "No customer found with the given ID",
                    Result = isSuccess ? response : null,
                    StatusCode = HttpStatusCode.OK,
                    Success = isSuccess
                });
            });
        }

        /// <summary>
        /// Inserts or updates a customer.
        /// </summary>
        /// <param name="customer">The customer information to insert or update.</param>
        /// <returns>An ActionResult with information about the result of the operation.</returns>
        [ApiExplorerSettings(GroupName = "Customer")]
        [HttpPost("UpsertCustomer")]
        public async Task<IActionResult> UpsertCustomer(CustomerDto customer)
        {
            return CreateHttpResponse(() =>
            {
                var response = _customerService.UpsertCustomer(customer).Result;
                return new OkObjectResult(new SuccessResponseVM
                {
                    Message = response == null ? "Customer not found" : "Customer upserted successfully",
                    Result = response,
                    StatusCode = HttpStatusCode.OK,
                    Success = response != null
                });
            });
        }

        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>An ActionResult with information about the result of the operation.</returns>
        [ApiExplorerSettings(GroupName = "Customer")]
        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            return CreateHttpResponse(() =>
            {
                var response = _customerService.DeleteCustomer(id).Result;
                return new OkObjectResult(new SuccessResponseVM
                {
                    Message = response == null ? "Customer not found" : "Customer removed successfully",
                    Result = response,
                    StatusCode = HttpStatusCode.OK,
                    Success = response != null
                });
            });
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>An ActionResult with information about the result of the operation.</returns>
        [ApiExplorerSettings(GroupName = "Customer")]
        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            return CreateHttpResponse(() =>
            {
                var response = _customerService.GetAllCustomers().Result;
                return new OkObjectResult(new SuccessResponseVM
                {
                    Message = response == null ? "No customers found" : $"{response.Count()} customer(s) found.",
                    Result = response,
                    StatusCode = HttpStatusCode.OK,
                    Success = response != null
                });
            });
        }

        #endregion
    }
}
