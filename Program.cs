using IceCity;
using IceCity.Services;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static IceCity.Report;
using static Owner;
using Spectre.Console;

partial class Program
{
    public static void Main()
    {
        #region Input Values

        ServiceOne serviceOne = new();
        DailyUsage dailyUsage = new();
        Heater heater = new();
        Report report = new();

        #region Name
        Console.Write("Owner Name : ");
        var ownerName = "Ahmed"; 
            //Console.ReadLine();
        while (string.IsNullOrEmpty(ownerName))
        {
            Console.WriteLine("Cant be Empty ! Enter a valid name:");
            ownerName = Console.ReadLine();
        }
        Owner owner = new(ownerName);
        owner.Name = ownerName;
        #endregion

        #region Heater Power
        Console.Write("Heater Power (Kilowatt) : ");
        heater.powerValue = 45.34;
            //Convert.ToDouble(Console.ReadLine());
        #endregion

        #region Heater Type
        Console.Write("Heater Type (Gas OR Electric) : ");
        heater.heaterType = HeaterType.Gas;
        //Enum.Parse<HeaterType>(Console.ReadLine());
        #endregion

        #endregion

        // this prints all the report
        report.SubscribeToEvents(dailyUsage);

        #region Daily Usage Tracking From user inputs
        int year = 2026;
        int month = 2;
        bool isOPen = true;

        while (isOPen)
        {
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                DateOnly currentDay = new DateOnly(year, month, day);
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule($"[bold cyan]{currentDay.ToString("dd/MM/yyyy")}[/]").RuleStyle("cyan").Centered());
                string res = DailyUsage.ReadInputState();
                if (res == "y")
                {
                    double wrkingHoursInput = AnsiConsole.Ask<double>("[bold yellow]Working Hours =[/]");
                    serviceOne.workingHours.Add(wrkingHoursInput);
                    double htrValuesInput = AnsiConsole.Ask<double>("[bold orange1]Heater Values =[/]");
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

        

    }
}
