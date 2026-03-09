using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace IceCity
{
    public class DailyUsage
    {
        public Dictionary<DateTime, (double WorkingHours, double Consumption)> dailyUsages = new();
         
        private double _workingHours;
        public double workingHours
        { get => _workingHours;
          set
            {
                if (value >= 0)
                {
                    _workingHours = value;
                }
                else
                {
                    Console.WriteLine("Wrong Hour Value !! (must be >= 0)");
                }
            }
        }

        private double _heaterValue;
        public double heaterValue
        {
            get => _heaterValue;
            set
            {
                if (value >= 0)
                {
                    _heaterValue = value;
                }
                else
                {
                    Console.WriteLine("Wrong Heater Value !! (must be >= 0)");
                }
            }
        }

        public static string ReadInputState()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Open The Heater?[/]")
                    .AddChoices("y", "n"));
            return choice;
        }

        public delegate void DailyUsageDelegate(DailyUsage dailyUsage);
        public event DailyUsageDelegate? MonthConsumptionEvent;
        public void OnMonthExpired()
        {
            MonthConsumptionEvent?.Invoke(this);
        }

    }
}
