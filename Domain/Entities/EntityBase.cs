using System;
using System.Collections.Generic;
using System.Text;

namespace Armin.Suitsupply.Domain.Entities
{
    public class EntityBase
    {
        public long Id { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
