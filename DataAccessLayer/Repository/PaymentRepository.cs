using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Database.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<PaymentRepository> _logger;
        public PaymentRepository(BlinkandBuysContext dbContext, ILogger<PaymentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        public async Task<int> PostAsync(Payment payment, int loggedInUser)
        {
            try
            {
                payment.CreatedBy = loggedInUser;
                payment.CreatedDt = DateTime.Now;
                payment.ModifiedDt = DateTime.Now;
                await _dbContext.Payment.AddAsync(payment);
                await _dbContext.SaveChangesAsync();

                List<BookedProduct> bookedProducts = new List<BookedProduct>();
                List<BookedService> bookedServices = new List<BookedService>();

                var billingAddress = await _dbContext.BillingAddress.Where(c => c.UserId == payment.UserId && !c.IsDeleted).FirstOrDefaultAsync();
                var cart = await _dbContext.UserCart.Where(c => c.UserId == payment.UserId && !c.IsDeleted).ToListAsync();
                if (cart.Count > 0)
                {
                    foreach (var item in cart)
                    {
                        if (item.Type == "service")
                        {
                            var bookedService = new BookedService();
                            bookedService.BillingAddressId = billingAddress.Id;
                            bookedService.CreatedBy = loggedInUser;
                            bookedService.CreatedDt = DateTime.Now;
                            bookedService.DeliveryStatus = "Not Delivered";
                            bookedService.IsApprovedByAdmin = false;
                            bookedService.IsApprovedByServiceProvider = false;
                            bookedService.IsCancelledByUser = false;
                            bookedService.IsDeleted = false;
                            bookedService.IsActive = true;

                            bookedService.IsRejectedByAdmin = false;
                            bookedService.IsRejectedByServiceProvider = false;
                            bookedService.PaymentMode = payment.PaymentType;
                            bookedService.ServiceId = item.BookedItemId;
                            bookedService.Quantity = item.Quantity;
                            bookedService.Type = "service";
                            bookedService.UserId = item.UserId;
                            bookedService.PaymentId = payment.Id;
                            ////
                            bookedServices.Add(bookedService);
                        }
                        else
                        {
                            var bookedProduct = new BookedProduct();
                            bookedProduct.BillingAddressId = billingAddress.Id;
                            bookedProduct.CreatedBy = loggedInUser;
                            bookedProduct.CreatedDt = DateTime.Now;
                            bookedProduct.DeliveryStatus = "Not Delivered";
                            bookedProduct.IsApprovedByAdmin = false;
                            bookedProduct.IsApprovedByDealer = false;
                            bookedProduct.IsCancelledByUser = false;
                            bookedProduct.IsDeleted = false;
                            bookedProduct.IsRejectedByAdmin = false;
                            bookedProduct.IsRejectedByDealer = false;
                            bookedProduct.PaymentMode = payment.PaymentType;
                            bookedProduct.ProductId = item.BookedItemId;
                            bookedProduct.Quantity = item.Quantity;
                            bookedProduct.Type = "product";
                            bookedProduct.UserId = item.UserId;
                            bookedProduct.PaymentId = payment.Id;
                            ///////////
                            bookedProducts.Add(bookedProduct);
                        }
                        /////////////
                        item.IsDeleted = true;
                        item.ModifiedBy = loggedInUser;
                        item.ModifiedDt = DateTime.Now;
                    }
                    await _dbContext.BookedService.AddRangeAsync(bookedServices);
                    await _dbContext.BookedProduct.AddRangeAsync(bookedProducts);

                    _dbContext.UserCart.UpdateRange(cart);
                    await _dbContext.SaveChangesAsync();
                }
                return payment.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<Payment> Get(int paymentId)
        {
            try
            {
                var payment = await _dbContext.Payment.Where(x => x.Id == paymentId).FirstOrDefaultAsync();
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
    }
}
