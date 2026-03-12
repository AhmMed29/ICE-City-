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

        #region Heater Inputs (power and type)
        Console.Write("Heater Power (Kilowatt) : ");
        heater.powerValue = Convert.ToDouble(Console.ReadLine());

        Console.Write("Heater Type (Gas OR Electric) : ");
        heater.heaterType = Enum.Parse<HeaterType>(Console.ReadLine());
        #endregion

        #region user input the daily usage
        int year = 2026;
        int month = 2;
        bool isOPen = true;

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
                    Console.Write("Heater Values = ");
                    double htrValuesInput = Convert.ToDouble(Console.ReadLine());
                    serviceOne.heaterValues.Add(htrValuesInput);
                    dailyUsage.dailyUsages.Add(DateTime.Parse(currentDay.ToString()), (wrkingHoursInput, htrValuesInput));
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

        report.SubscribeToEvents(dailyUsage);
    }
}

