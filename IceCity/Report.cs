using IceCity.Services;

namespace IceCity
{
    public class Report
    {
        // WHY CHANGED: The old Report created its own Calculations internally.
        // That is "tight coupling" — Report always used the hardcoded test data
        // regardless of what the user entered, making user input completely ignored.
        // Injecting Calculations from outside (dependency injection) means the
        // Report always reflects real data passed in from Program.cs.
        private readonly Calculations _calculations;

        public Report(Calculations calculations)
        {
            _calculations = calculations;
        }

        public void PrintOwnerDetails(Owner owner)
        {
            Console.WriteLine("-------- Owner Report ----------");
            Console.WriteLine($"Owner Name          : {owner.Name}");
            Console.WriteLine($"Total Working Hours : {_calculations.TotalWorkingTime():F2} hrs");
            Console.WriteLine($"Avg Heater Value    : {_calculations.AverageHeaterValue():F2}");
            Console.WriteLine($"Utilization Rate    : {_calculations.MonthlyUtilizationRate():P1}");
            Console.WriteLine($"Peak Usage Day      : Day {_calculations.PeakUsageDay()}");
            Console.WriteLine("--------------------------------");
        }

        public void PrintHeaterDetails(Heater heater, Owner owner)
        {
            Console.WriteLine("-------- Heater Report ---------");
            Console.WriteLine($"Owner Name   : {owner.Name}");
            Console.WriteLine($"Heater Type  : {heater.HeaterType}");
            Console.WriteLine($"Heater Power : {heater.PowerValue} kW");
            // Estimated cost uses total hours × power × default electricity rate.
            // SUGGESTED FEATURE: Accept different rates for gas vs electric heaters.
            Console.WriteLine($"Monthly Cost : ${_calculations.CalculateMonthlyCost(heater.PowerValue):F2}");
            Console.WriteLine("--------------------------------");
        }
    }
}
