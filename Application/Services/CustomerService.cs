using Application.Dto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Provides services related to managing customers.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Customer> _customerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService"/> class.
        /// </summary>
        /// <param name="customerRepository">The repository for accessing customer data.</param>
        /// <param name="mapper">The mapper for mapping between entities and DTOs.</param>
        public CustomerService(
            IGenericRepository<Customer> customerRepository,
            IMapper mapper
        )
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        #region Customer Operations

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>A list of customer DTOs.</returns>
        public async Task<List<CustomerDto>> GetAllCustomers()
        {
            var items = _customerRepository.GetAll().Result.ToList();
            return _mapper.Map<List<Customer>, List<CustomerDto>>(items);
        }

        /// <summary>
        /// Retrieves a customer by ID.
        /// </summary>
        /// <param name="ID">The ID of the customer to retrieve.</param>
        /// <returns>The customer DTO.</returns>
        public async Task<CustomerDto> GetCustomer(int ID)
        {
            var item = _customerRepository.GetFirst(x => x.ID == ID);
            return _mapper.Map<Customer, CustomerDto>(item);
        }

        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="ID">The ID of the customer to delete.</param>
        /// <returns>The deleted customer DTO.</returns>
        public async Task<CustomerDto> DeleteCustomer(int ID)
        {
            var item = _customerRepository.GetFirst(x => x.ID == ID);
            if (item != null)
            {
                _customerRepository.Delete(item);
                _customerRepository.Complete();
            }
            return _mapper.Map<Customer, CustomerDto>(item);
        }

        /// <summary>
        /// Inserts or updates a customer.
        /// </summary>
        /// <param name="item">The customer DTO to insert or update.</param>
        /// <returns>The inserted or updated customer DTO.</returns>
        public async Task<CustomerDto> UpsertCustomer(CustomerDto item)
        {
            Customer DBitem;
            if (item.ID == 0)
            {
                DBitem = _mapper.Map<CustomerDto, Customer>(item);
                await _customerRepository.Add(DBitem);
            }
            else
            {
                DBitem = _customerRepository.GetFirst(x => x.ID == item.ID);
                if (DBitem != null)
                {
                    item.CreatedDate = DBitem.CreatedDate;
                    item.CreatedBy = DBitem.CreatedBy;
                    DBitem = _mapper.Map<CustomerDto, Customer>(item, DBitem);
                    _customerRepository.Update(DBitem);
                }
            }
            if (DBitem != null)
                _customerRepository.Complete();
            return _mapper.Map<Customer, CustomerDto>(DBitem);
        }

        #endregion
    }
}
