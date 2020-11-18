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

                var billingAddresse = await _dbContext.BillingAddress.Where(c => c.UserId == payment.UserId && !c.IsDeleted).FirstOrDefaultAsync();

                var cart = await _dbContext.UserCart.Where(c => c.UserId == payment.UserId && !c.IsDeleted).ToListAsync();
                if (cart.Count > 0)
                {
                    foreach (var item in cart)
                    {
                        var bookedProduct = new BookedProduct();
                        bookedProduct.BillingAddressId = billingAddresse.Id;
                        bookedProduct.CreatedBy = loggedInUser;
                        bookedProduct.CreatedDt = DateTime.Now;
                        bookedProduct.DeliveryStatus = "None";
                        bookedProduct.IsApprovedByAdmin = false;
                        bookedProduct.IsApprovedByDealer = false;
                        bookedProduct.IsCancelledByUser = false;
                        bookedProduct.IsDeleted = false;
                        bookedProduct.IsRejectedByAdmin = false;
                        bookedProduct.IsRejectedByDealer = false;
                        bookedProduct.PaymentMode = payment.PaymentType;
                        bookedProduct.ProductId = item.ProductId;
                        bookedProduct.Quantity = item.Quantity;
                        bookedProduct.Type = item.Type;
                        bookedProduct.UserId = item.UserId;
                        bookedProduct.PaymentId = payment.Id;
                        ///////////
                        bookedProducts.Add(bookedProduct);
                        /////////////
                        item.IsDeleted = true;
                        item.ModifiedBy = loggedInUser;
                        item.ModifiedDt = DateTime.Now;
                    }

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
    }
}
