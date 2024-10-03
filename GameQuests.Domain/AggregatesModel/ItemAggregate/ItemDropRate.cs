using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameQuests.Domain.AggregatesModel.ItemAggregate
{
    public class ItemDropRate
    {
        public Item Item { get; set; }

        public double DropRate { get; set; }
    }
}
