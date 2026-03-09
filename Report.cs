using IceCity.Services;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IceCity
{
    public class Report
    {
        Calculations calculations = new();
        public void PrintOwnerDetails(Owner owner)
        {
            Console.WriteLine("-----------Owner Report-------------");
            Console.WriteLine($"Owner Name: {owner.Name}");
            Console.WriteLine($"Total Working Hours For This Month : {calculations.TotalWorkingTime()}");
            Console.WriteLine($"Median Heater Value : {calculations.MedianHeaterValue()}");
            Console.WriteLine($"Average Cost For This Month : {calculations.MonthlyAverageCost()}");
            Console.WriteLine("-----------------------------------------");
        }
        public void PrintHeaterDetails(Heater heater, Owner owner)
        {
            Console.WriteLine("-----------Heater Report------------");
            Console.WriteLine($"Owner Name {owner.Name}");
            Console.WriteLine($"Heater Type {heater.heaterType}");
            Console.WriteLine($"Heater Power {heater.powerValue} KW");
            Console.WriteLine("-----------------------------------------");
        }
    }
}
