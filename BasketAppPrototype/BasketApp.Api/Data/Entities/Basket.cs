using System.Collections.Generic;

namespace BasketApp.Api.Data.Entities
{
    public class Basket
    {
        public string Id { get; set; } // UserId

        public virtual ICollection<BasketProducts> BasketItems { get; set; } = new List<BasketProducts>();
    }
}
