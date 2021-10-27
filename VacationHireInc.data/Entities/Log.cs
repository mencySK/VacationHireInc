using System;
using System.Collections.Generic;

#nullable disable

namespace VacationHireInc.data.Entities
{
    public partial class Log
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
