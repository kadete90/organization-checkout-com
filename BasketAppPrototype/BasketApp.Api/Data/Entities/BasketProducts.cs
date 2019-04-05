using System;
using System.ComponentModel.DataAnnotations;

namespace BasketApp.Api.Data.Entities
{
    public class BasketProducts
    {
        public string BasketId { get; set; }

        public Guid ProductId { get; set; }

        public virtual Basket Basket { get; set; }
        public virtual Product Product { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Amount { get; set; }
    }
}
