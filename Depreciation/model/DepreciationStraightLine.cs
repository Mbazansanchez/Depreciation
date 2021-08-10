using System;
using System.Collections.Generic;
using System.Text;

namespace Depreciation.model
{
    public class DepreciationStraightLine
    {
        public string Title { get; set; }

        public decimal StartValue { get; set; }
        public decimal EndValue { get; set; }
        public decimal SalvageValue { get { return Calc(); } }
        public int Lifetime { get; set; }

        public DateTime DateAddedToInventory { get; set; }
        public DateTime DateRemovedFromInventory { get; set; }


        protected virtual decimal Calc()
        {
            decimal depreciationPerYear = (StartValue - EndValue) / Lifetime;

            TimeSpan usedTime = DateRemovedFromInventory - DateAddedToInventory;
            decimal yearsUsed = (decimal)(usedTime.TotalDays / 365.24);

            //return yearsUsed * depreciationPerYear;
            return StartValue - yearsUsed * depreciationPerYear - EndValue; // salvage value
        }

        public override string ToString()
        {
            return $"Title: {Title} Start value: {StartValue} End value: {EndValue} Lifetime: {Lifetime}";
        }
    }
}
