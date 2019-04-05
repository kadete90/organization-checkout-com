using System.Collections.Generic;
using Newtonsoft.Json;

namespace BasketApi.Common.Contracts
{
    public class BasketItemsModel
    {
        [JsonProperty(Order = 0)]
        public double Total { get; set; }

        [JsonProperty(Order = 1)]
        public List<ProductAmountModel> Items = new List<ProductAmountModel>();
    }
}
