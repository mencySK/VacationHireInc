using System;
using System.Collections.Generic;

#nullable disable

namespace VacationHireInc.data.Entities
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            HireOrders = new HashSet<HireOrder>();
        }

        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductionYear { get; set; }
        public string Transmission { get; set; }
        public decimal Price { get; set; }
        public decimal Deposit { get; set; }

        public virtual ICollection<HireOrder> HireOrders { get; set; }
    }
}
