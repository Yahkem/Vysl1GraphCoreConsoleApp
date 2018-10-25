using System;
using System.Collections.Generic;
using System.Text;

namespace Vysl1GraphCoreConsoleApp
{
    public class ResourceCostPair
    {
        public ResourceCostPair(int resourceValue, int cost=0)
        {
            this.CostToNode = cost;
            this.ResourceValue = resourceValue;
        }

        public int ResourceValue { get; set; }
        public int CostToNode { get; set; }
    }
}
