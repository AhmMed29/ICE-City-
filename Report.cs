using IceCity.Services;
using System.Diagnostics.Metrics;
using static Heater;
using static IceCity.DailyUsage;
using static Owner;

namespace IceCity
{
    public class Report
    {
        List<Heater> Heaters = new();
        List<Owner> Owners = new();

        public static void getHeaterType(Heater heater)
        {
            if (heater.heaterType == HeaterType.Gas)
            {
                Console.WriteLine("The Heater Type Is: Gas");
            }
            else if (heater.heaterType == HeaterType.Electric)
            {
                Console.WriteLine("The Heater Type Is : Electric");
            }
        }
        public static void getHeaterPower(Heater heater)
        {
            Console.WriteLine($"Heater Power Is: {heater.powerValue} KW");
        }

        private void PrintReports(DailyUsage dailyUsage)
        {
            Heater.heaterInfoDelegate heaterInfDelegate = (e) => Console.WriteLine("-----------Heater Report-------------");
            heaterInfDelegate += getHeaterType;
            heaterInfDelegate += getHeaterPower;

            foreach (var kvp in dailyUsage.dailyUsages)
            {
                var day = kvp.Key;
                var (workingHours, consumption) = kvp.Value;
                Console.WriteLine($"Opened on [{day:dd/MM/yyyy}] WorkingHours: {workingHours}, Consumption: {consumption}");
            }

            foreach (var heater in Heaters)
            {
                heaterInfDelegate(heater);
            }

            Owner.printOwnerData printOwnerDataDelegate = (m) => Console.WriteLine("-----------Owner Report-------------");
            printOwnerDataDelegate += printOwnerName;

            foreach (var owner in Owners)
            {
                printOwnerDataDelegate(owner);
            }
            Console.WriteLine("==========================");
        }
        public void printOwnerName(Owner owner)
        {
            Console.WriteLine($"Owner Name : [{owner.Name}]");
        }
        public void SubscribeToEvents(DailyUsage dailyUsage)
        {
            dailyUsage.MonthConsumptionEvent += PrintReports;
        }
    } 
}
