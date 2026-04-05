using IceCity.Services;
using System.Diagnostics.Metrics;
using static Heater;
using static IceCity.DailyUsage;
using static Owner;

namespace IceCity
{
    public class Report
    {
        List<Owner> Owners = new();


        // must be in heater not here !
        public static void getHeaterLastOpened(Heater heater)
        {
            if (heater.LastOpenedDate.HasValue)
                Console.WriteLine($"Last Opened Date : {heater.LastOpenedDate.Value:dd/MM/yyyy}");
            else
                Console.WriteLine("Last Opened Date : Not opened yet");
        }


        private void PrintReports(DailyUsage dailyUsage)
        {


            //foreach (var kvp in dailyUsage.dailyUsages)
            //{
            //    var day = kvp.Key;
            //    var (workingHours, consumption) = kvp.Value;
            //    Console.WriteLine($"Opened on [{day:dd/MM/yyyy}] WorkingHours: {workingHours}, Consumption: {consumption}");
            //}

            ////foreach (var heater in Heaters)
            ////{
            ////    heaterInfDelegate(heater);
            ////}

            //Owner.printOwnerData printOwnerDataDelegate = (m) => Console.WriteLine("-----------Owner Report-------------");
            //printOwnerDataDelegate += printOwnerName;

            //foreach (var owner in Owners)
            //{
            //    printOwnerDataDelegate(owner);
            //}
            //Console.WriteLine("==========================");
        }


        // must be changed
        public void printOwnerName(Owner owner)
        {
            Console.WriteLine($"Owner Name : [{owner.Name}]");
        }


        //public void SubscribeToHeaterEvents(Heater heater)
        //{
        //    Heaters.Add(heater);
        //}
        //public void SubscribeToEvents(DailyUsage dailyUsage)
        //{
        //    dailyUsage.MonthConsumptionEvent += PrintReports;
        //}
    } 
}
