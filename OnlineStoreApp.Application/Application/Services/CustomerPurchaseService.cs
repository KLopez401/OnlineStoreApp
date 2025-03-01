﻿using OnlineStoreApp.Application.DTO.CustomerPurchases;
using OnlineStoreApp.Application.DTO.Customers;
using OnlineStoreApp.Application.DTO.Products;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Application.Interface.Services;
using OnlineStoreApp.Domain.Entities;
using System.Text.Json;

namespace OnlineStoreApp.Application.Services;

public class CustomerPurchaseService : ICustomerPurchaseService
{
    private ICustomerPurchaseRepository _customerPurchaseRepository;
    private ICustomerRepository _customerRepository;
    public CustomerPurchaseService(ICustomerPurchaseRepository customerPurchaseRepository,
                                   ICustomerRepository customerRepository)
    {
        _customerPurchaseRepository = customerPurchaseRepository;
        _customerRepository = customerRepository;
    }
    public async Task<Guid> CreateCustomerPurchase(CustomerPurchaseDto dto)
    {
        try
        {
            var id = Guid.NewGuid();

            var jsonString = string.Empty;

            if (dto.Products != null)
            {
                jsonString = JsonSerializer.Serialize(dto.Products, new JsonSerializerOptions { WriteIndented = true });
            }

            var purchase = new CustomerPurchases
            {
                Id = id,
                CustomerId = dto.CustomerId,
                Products = jsonString,
                Total = dto.Products.Sum(x => x.Price),
                PurchaseDate = DateTime.Now,
                ReceiptReference = id.ToString().Substring(id.ToString().Length - 7),
                IsDeleted = false
            };

            await _customerPurchaseRepository.AddAsync(purchase);

            return purchase.Id;
        }
        catch
        {
           throw;
        }
    }

    public async Task<GetCustomerPurchaseDto> GetCustomerPurchaseById(Guid id)
    {
        try
        {
            var purchase = await _customerPurchaseRepository.FindActiveRecordByIdAsync(id);

            var productInfo = new List<GetProductDto>();
            if (!string.IsNullOrEmpty(purchase.Products))
            {
                productInfo = JsonSerializer.Deserialize<List<GetProductDto>>(purchase.Products);
            }

            var getCustomerPurchaseDto = new GetCustomerPurchaseDto
            {
                Id = purchase.Id,
                Products = productInfo,
                Total = purchase.Total,
                PurchaseDate = purchase.PurchaseDate,
                ReceiptReference = purchase.ReceiptReference
            };

            var customer = await _customerRepository.FindActiveRecordByIdAsync(purchase.CustomerId);

            if (customer != null)
            {
                getCustomerPurchaseDto.Customer = new GetCustomerDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    DateAdded = customer.DateAdded
                };
            }

            return getCustomerPurchaseDto;
        }
        catch
        {
           throw;
        }
    }

    public async Task<IEnumerable<GetCustomerPurchaseDto>> GetCustomerPurchases()
    {
        try
        {
            var purchases = await _customerPurchaseRepository.GetAllAsync();

            var getCustomerPurchaseDtos = purchases.Select(purchase => new GetCustomerPurchaseDto
            {
                Id = purchase.Id,
                CustomerId = purchase.CustomerId,
                Products = !string.IsNullOrEmpty(purchase.Products) ? JsonSerializer.Deserialize<List<GetProductDto>>(purchase.Products) : new List<GetProductDto>(),
                Total = purchase.Total,
                PurchaseDate = purchase.PurchaseDate,
                ReceiptReference = purchase.ReceiptReference
            }).ToList()
              .OrderByDescending(x => x.PurchaseDate);

            return getCustomerPurchaseDtos;
        }
        catch
        {
            throw;
        }
    }

    public async Task DeleteCustomerPurchase(Guid id)
    {
        try
        {
            var purchase = await _customerPurchaseRepository.FindActiveRecordByIdAsync(id);

            if (purchase == null)
            {
                throw new Exception("Purchase not found");
            }

            purchase.IsDeleted = true;

            await _customerPurchaseRepository.UpdateAsync(purchase);
        }
        catch
        {
            throw;
        }
    }
    #region HelperMethod
    private IEnumerable<CustomerPurchases> GetPaginatedPurchases(IEnumerable<CustomerPurchases> purchases, int page, int pageSize)
    {
        return purchases = purchases.Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();
    }
    #endregion
}
