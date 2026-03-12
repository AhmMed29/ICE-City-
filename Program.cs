using IceCity;
using IceCity.Services;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static IceCity.Report;
using static Owner;
using Spectre.Console;
using System.Threading;

partial class Program
{
    public static void Main()
    {
        ServiceOne serviceOne = new();
        DailyUsage dailyUsage = new();
        Heater heater = new();
        Report report = new();
        House house = new();

        #region owner input (Name)
        Console.Write("Owner Name : ");
        var ownerName = Console.ReadLine();
        while (string.IsNullOrEmpty(ownerName))
        {
            Console.WriteLine("Cant be Empty ! Enter a valid name:");
            ownerName = Console.ReadLine();
        }
        Owner owner = new(ownerName);
        owner.Name = ownerName;
        #endregion

        #region Heater Inputs (id, power and type)
        Console.Write("Heater ID : ");
        heater.HeaterId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Heater Power (Kilowatt) : ");
        heater.powerValue = Convert.ToDouble(Console.ReadLine());

        Console.Write("Heater Type (Gas OR Electric) : ");
        heater.heaterType = Enum.Parse<HeaterType>(Console.ReadLine());
        #endregion

        #region House Input (id) and assign heater
        Console.Write("House ID : ");
        house.HouseID = Convert.ToInt32(Console.ReadLine());
        heater.houseID = house.HouseID;
        house.AddHeater(heater);
        #endregion

        #region user input the daily usage
        int year = 2026;
        int month = 2;
        bool isOPen = true;

        // Fake list for open/close state (for testing purposes):
        // List<string> fakeStates = new()
        // {
        //     "y","n","y","y","n","y","y","n","y","y",
        //     "n","y","y","y","n","y","y","n","y","y",
        //     "n","y","y","y","n","y","y","n"
        // };

        // Fake list for working hours (for testing purposes):
        // List<double> fakeWorkingHours = new()
        // {
        //     1.5, 2, 1.25, 2.5, 2.25, 4.1, 1.75, 2.8, 3.5, 2.9,
        //     1.2, 2.3, 3.8, 1.9, 2.6, 2.4, 4.0, 1.6, 2.7, 3.2,
        //     2.1, 3.6, 1.8, 2.9, 2.0, 4.2, 1.7, 2.4
        // };

        while (isOPen)
        {
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                DateOnly currentDay = new DateOnly(year, month, day);
                Console.Clear();
                Console.WriteLine($"========[{currentDay.ToString("dd/MM/yyyy")}]========");
                string res = DailyUsage.ReadInputState();
                if (res == "y")
                {
                    Console.Write("Working Hours = ");
                    double wrkingHoursInput = Convert.ToDouble(Console.ReadLine());
                    serviceOne.workingHours.Add(wrkingHoursInput);
                    double consumption = wrkingHoursInput * heater.powerValue;
                    serviceOne.heaterValues.Add(consumption);
                    dailyUsage.RecordDailyUsage(currentDay.ToDateTime(TimeOnly.MinValue), wrkingHoursInput, heater.powerValue);
                    heater.Open(currentDay.ToDateTime(TimeOnly.MinValue));
                }
                else
                {
                    isOPen = false;
                }
                // Raising the Event
                if (currentDay.Day == DateTime.DaysInMonth(year, month))
                {
                    dailyUsage.OnMonthExpired();
                }
            }
        }
        #endregion

        report.SubscribeToHeaterEvents(heater);
        report.SubscribeToEvents(dailyUsage);
    }
}

