﻿using System.Linq;
using BasketApi.Common.Contracts;
using BasketApp.Api.Data.Entities;

namespace BasketApp.Api.Utils
{
    public static class BasketExtensions
    {
        public static BasketItemsModel ToModel(this Basket basket) =>
            new BasketItemsModel
            {
                Total = basket.BasketItems.Sum(b => b.Amount*b.Product.Price),
                Items = basket.BasketItems.Select(b => new ProductAmountModel
                {
                    ProductId = b.ProductId,
                    Name = b.Product.Name,
                    Price = b.Product.Price,
                    Amount = b.Amount
                }).ToList()
            };
    }
}
