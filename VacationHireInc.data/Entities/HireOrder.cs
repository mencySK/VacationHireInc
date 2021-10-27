using System;
using System.Collections.Generic;

#nullable disable

namespace VacationHireInc.data.Entities
{
    public partial class HireOrder
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string Damage { get; set; }
        public bool GasolineFilled { get; set; }
        public Guid? VehicleId { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
