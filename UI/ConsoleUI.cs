using System.Text.Json;
using IceCity.Services;

namespace IceCity.UI
{
    public static class ConsoleUI
    {
        public static string ReadNonEmptyLine(string prompt, string emptyMessage)
        {
            Console.Write(prompt);
            var line = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine(emptyMessage);
                line = Console.ReadLine();
            }
            return line;
        }

        public static int ReadInt32(string prompt)
        {
            Console.Write(prompt);
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Enter a valid whole number. Try again:");
                Console.Write(prompt);
            }
            return value;
        }

        public static double ReadPositivePowerKw(string prompt)
        {
            Console.Write(prompt);
            double value;
            while (!double.TryParse(Console.ReadLine(), out value) || value <= 0)
            {
                Console.WriteLine("Enter a positive number for power. Try again:");
                Console.Write(prompt);
            }
            return value;
        }

        public static EnumHeaterType ReadHeaterType(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "1" || line.Equals("Gas", StringComparison.OrdinalIgnoreCase)) return EnumHeaterType.Gas;
                if (line == "2" || line.Equals("Electric", StringComparison.OrdinalIgnoreCase)) return EnumHeaterType.Electric;
                if (line == "3" || line.Equals("Solar", StringComparison.OrdinalIgnoreCase)) return EnumHeaterType.Solar;
                
                Console.WriteLine("Enter '1' (Gas), '2' (Electric), or '3' (Solar). Try again:");
                Console.Write(prompt);
            }
        }

        public static double ReadWorkingHours()
        {
            Console.Write("Working Hours = ");
            double hours;
            while (!double.TryParse(Console.ReadLine(), out hours) || hours < 0 || hours > 24)
            {
                Console.WriteLine("Enter a valid number of hours (0 - 24). Try again:");
                Console.Write("Working Hours = ");
            }
            return hours;
        }

        public static string ReadInputState()
        {
            while (true)
            {
                Console.Write("Open The Heater y|n ? ");
                var userInput = Console.ReadLine();
                if (userInput == "y" || userInput == "n") return userInput;
                Console.WriteLine("Enter 'y' or 'n' else !");
            }
        }

        public static void DisplayDashboardMenu()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("             ICE CITY DASHBOARD         ");
            Console.WriteLine("========================================");
            Console.WriteLine("  1. Monthly Reports");
            Console.WriteLine("  2. Request Replacement");
            Console.WriteLine("  3. Weather status last month");
            Console.WriteLine("  4. Async/Threads/Tasks Printing Demo");
            Console.WriteLine("  5. Simulate Heater Failure");
            Console.WriteLine("  6. Exit");
            Console.WriteLine("========================================");
            Console.Write("Select an option (1-6): ");
        }

        public static void ConfigureHeater(Heater heater, IEnumerable<int> existingIds)
        {
            bool uniqueIdFound = false;
            while (!uniqueIdFound)
            {
                int possibleId = ReadInt32("Heater ID : ");
                if (existingIds.Contains(possibleId)) Console.WriteLine("This Heater ID already exists! Please enter a unique ID.");
                else { heater.HeaterId = possibleId; uniqueIdFound = true; }
            }
            heater.powerValue = ReadPositivePowerKw("Heater Power (Kilowatt) : ");
            heater.heaterType = ReadHeaterType("Heater Type (1: Gas, 2: Electric, 3: Solar) : ");
        }

        public static async Task DisplayWeatherReportAsync(HttpClient httpClient)
        {
            Console.WriteLine("\n--- Fetching Weather Data ---");
            try
            {
                DateTime now = DateTime.UtcNow;
                DateTime start = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                DateTime end = new DateTime(now.Year, now.Month, 1).AddDays(-1);
                string url = $"https://archive-api.open-meteo.com/v1/archive?latitude=31.0409&longitude=31.3785&start_date={start:yyyy-MM-dd}&end_date={end:yyyy-MM-dd}&daily=temperature_2m_max,temperature_2m_min,precipitation_sum";
                var response = await httpClient.GetStringAsync(url);
                using var json = JsonDocument.Parse(response);
                var daily = json.RootElement.GetProperty("daily");
                var dates = daily.GetProperty("time").EnumerateArray();
                var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray();
                var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray();
                var rain = daily.GetProperty("precipitation_sum").EnumerateArray();

                Console.WriteLine("Date       | Max Temp | Min Temp | Rain");
                Console.WriteLine("-----------|----------|----------|-----");
                while (dates.MoveNext() && maxTemps.MoveNext() && minTemps.MoveNext() && rain.MoveNext())
                {
                    Console.WriteLine($"{dates.Current.GetString()} | {maxTemps.Current.GetDouble(),5:N1}°C | {minTemps.Current.GetDouble(),5:N1}°C | {rain.Current.GetDouble(),5:N1}mm");
                }
            }
            catch (Exception ex) { Console.WriteLine($"[Error] Failed to fetch weather data: {ex.Message}"); }
        }

        public static void RunDailyUsageLoop(int year, int month, Heater heater, DailyUsage dailyUsage, CalculationService costService)
        {
            bool continueMonths = true;
            while (continueMonths)
            {
                int daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var currentDay = new DateOnly(year, month, day);
                    Console.Clear();
                    Console.WriteLine($"========[{currentDay:dd/MM/yyyy}]========");

                    string res = ReadInputState();
                    if (res == "y")
                    {
                        double workingHoursInput = ReadWorkingHours();
                        costService.workingHours!.Add(workingHoursInput);
                        double consumption = workingHoursInput * heater.powerValue;
                        costService.heaterValues!.Add(consumption);
                        var dateTime = currentDay.ToDateTime(TimeOnly.MinValue);
                        dailyUsage.RecordDailyUsage(dateTime, workingHoursInput, heater.powerValue);
                        heater.Open(dateTime);
                    }
                    else continueMonths = false;
                }
            }
        }
    }
}
