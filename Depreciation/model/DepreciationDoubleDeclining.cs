using System;
using System.Collections.Generic;
using System.Text;

namespace Depreciation.model
{
    class DepreciationDoubleDeclining : DepreciationStraightLine
    {
        protected override decimal Calc()
        {
            decimal depreciationRate = 1m / Lifetime;

            TimeSpan usedTime = DateRemovedFromInventory - DateAddedToInventory;
            decimal yearsUsed = Math.Round((decimal)(usedTime.TotalDays / 365.24), 2, MidpointRounding.ToEven);

            return StartValue * (decimal)(Math.Pow((double)(1 - 2 * depreciationRate), (double)yearsUsed)) - EndValue;
        }
    }
}
