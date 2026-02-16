using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCity.Services
{
    public class Calculations
    {
        public List<double> workingHours = new()
        {
            1.5, 2, 1.25, 2.5, 2.25, 4.1, 1.75, 2.8, 3.5, 2.9,
            1.2, 2.3, 3.8, 1.9, 2.6, 2.4, 4.0, 1.6, 2.7, 3.2,
            2.1, 3.6, 1.8, 2.9, 2.0, 4.2, 1.7, 2.4, 3.3, 1
        };
        public List<double> heaterValues = new()
        {
            12, 14, 11, 15, 13, 16, 12.5, 14.5, 13.2, 15.8,
            11.5, 14.8, 13.7, 15.3, 12.8, 16.2, 13.1, 14.9, 12.3,
            15, 13.4, 14.6, 12.9, 16.5, 13.6, 15.1, 14.2, 13.8, 15.4
        };

        public double TotalWorkingTime()
        {
            return workingHours.Sum();
        }
        public double MedianHeaterValue()
        {
            return heaterValues.Sum() / heaterValues.Count();
        }
        public double MonthlyAverageCost()
        {
            workingHours.Sort();
            double medianValue = workingHours.Sum() / workingHours.Count();
            return medianValue * (workingHours.Sum() / (24 * 30));
        }
    }
}
