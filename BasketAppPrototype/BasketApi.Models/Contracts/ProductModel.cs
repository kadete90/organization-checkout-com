using System;
using Newtonsoft.Json;

namespace BasketApi.Common.Contracts
{
    public class ProductModel
    {
        [JsonProperty(Order = 0)]
        public Guid ProductId { get; set; }

        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 3)]
        public double Price { get; set; }
    }

    public class ProductAmountModel : ProductModel
    {
        [JsonProperty(Order = 2)]
        public int Amount { get; set; }
    }

    public class ProductUpdateAmountModel
    {
        public int Amount { get; set; }
    }
}
