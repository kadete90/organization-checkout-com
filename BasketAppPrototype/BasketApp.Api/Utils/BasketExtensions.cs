using System.Linq;
using BasketApp.Common.Contracts;
using BasketApp.Api.DAL.Entities;

namespace BasketApp.Api.Utils
{
    public static class BasketExtensions
    {
        public static BasketModel ToModel(this Basket basket) =>
            new BasketModel
            {
                Total = basket.BasketItems.Sum(b => b.Amount*b.Product.Price),
                Items = basket.BasketItems.Select(b => new ProductAmountModel
                {
                    Id = b.ProductId,
                    Name = b.Product.Name,
                    Price = b.Product.Price,
                    Amount = b.Amount
                }).ToList()
            };
    }
}
