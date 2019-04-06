﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace BasketApp.Common.Contracts
{
    public class BasketModel
    {
        [JsonProperty(Order = 0)]
        public double Total { get; set; }

        [JsonProperty(Order = 1)]
        public List<ProductAmountModel> Items = new List<ProductAmountModel>();
    }
}
