using System;
using Newtonsoft.Json;

namespace BasketApp.Common.Contracts
{
    public class ProductModel
    {
        [JsonProperty(Order = 0, PropertyName = "productId")]
        public Guid Id { get; set; }

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

    public class ProductUpdateModel
    {
        [JsonProperty(Order = 0, PropertyName = "productId")]
        public Guid Id { get; set; }

        [JsonProperty(Order = 2)]
        public int Amount { get; set; }
    }
}
