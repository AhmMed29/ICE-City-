using IceCity.Services;

namespace IceCity
{
    public class Report
    {
        private readonly Calculations _calculations;

        public Report(Calculations calculations)
        {
            _calculations = calculations;
        }

        public void PrintOwnerDetails(Owner owner)
        {
            Console.WriteLine("-----------Owner Report-------------");
            Console.WriteLine($"Owner Name: {owner.Name}");
            Console.WriteLine($"Total Working Hours For This Month: {_calculations.TotalWorkingTime()}");
            Console.WriteLine($"Average Heater Value: {_calculations.AverageHeaterValue()}");
            Console.WriteLine($"Average Cost For This Month: {_calculations.MonthlyAverageCost()}");
            Console.WriteLine("-----------------------------------------");
        }

        public void PrintHeaterDetails(Heater heater, Owner owner)
        {
            Console.WriteLine("-----------Heater Report------------");
            Console.WriteLine($"Owner Name: {owner.Name}");
            Console.WriteLine($"Heater Type: {heater.HeaterType}");
            Console.WriteLine($"Heater Power: {heater.PowerValue} KW");
            Console.WriteLine("-----------------------------------------");
        }
    }
}
