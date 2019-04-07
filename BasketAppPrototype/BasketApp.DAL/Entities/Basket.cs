using System.Collections.Generic;

namespace BasketApp.DAL.Entities
{
    public class Basket
    {
        public string Id { get; set; } // UserId

        public virtual ICollection<BasketProducts> BasketItems { get; set; } = new List<BasketProducts>();
    }
}
