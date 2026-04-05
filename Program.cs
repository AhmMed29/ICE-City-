using IceCity;
using IceCity.Services;
using System.Text.Json;

partial class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task Main()
    {
        var house = new House();
        var ownerName = ReadNonEmptyLine("Owner Name : ", "Cant be Empty ! Enter a valid name:");
        var owner = new Owner(ownerName);

        // Ask for House ID once
        house.HouseID = ReadInt32("House ID : ");

        // BEST PRACTICE: Store the Heater, its Usage, and Service together in a single list
        var heatersData = new List<(Heater Heater, DailyUsage Usage, ServiceOne Service)>();
        bool addMoreHeaters = true;
        
        while (addMoreHeaters)
        {
            try
            {
                var dailyUsage = new DailyUsage();
                var serviceOne = new ServiceOne();
                var heater = new Heater(dailyUsage);

                var existingIds = heatersData.Where(h => h.Heater.HeaterId.HasValue).Select(h => h.Heater.HeaterId!.Value);
                ConfigureHeaterFromConsole(heater, existingIds);

                heater.houseID = house.HouseID;
                house.AddHeater(heater);

                // Add them together as a tuple
                heatersData.Add((Heater: heater, Usage: dailyUsage, Service: serviceOne));

                // Run the daily usage data collection for the newly added heater
                RunDailyUsageLoop(2026, 2, heater, dailyUsage, serviceOne);

                bool validInput = false;
                while (!validInput)
                {
                    Console.Write("\nDo you want to add another heater? (y/n): ");
                    string? choice = Console.ReadLine()?.Trim().ToLower();
                    switch (choice)
                    {
                        case "y": validInput = true; Console.WriteLine(); break;
                        case "n": validInput = true; addMoreHeaters = false; break;
                        default: Console.WriteLine("[Error] Invalid input. Please enter 'y' or 'n'."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] An unexpected exception occurred: {ex.Message}");
            }
        }

        bool runningDashboard = true;
        while (runningDashboard)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("             ICE CITY DASHBOARD         ");
            Console.WriteLine("========================================");
            Console.WriteLine("  1. Monthly Reports");
            Console.WriteLine("  2. Request Replacement");
            Console.WriteLine("  3. Weather status last month");
            Console.WriteLine("  4. Exit");
            Console.WriteLine("========================================");
            Console.Write("Select an option (1-4): ");

            string? menuSelection = Console.ReadLine()?.Trim();

            switch (menuSelection)
            {
                case "1":
                    if (heatersData.Count == 0)
                    {
                        Console.WriteLine("\n[Info] No heaters available to report.");
                        Thread.Sleep(1500);
                        break;
                    }
                    
                    int currentIndex = 0;
                    int totalPages = heatersData.Count + 1; // +1 for the initial threads/tasks report page
                    bool viewingReports = true;

                    while (viewingReports)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("========================================");
                        if (currentIndex == 0)
                            Console.WriteLine($"  ❄️ DAILY USAGE REPORT (Page 1/{totalPages}) ❄️   ");
                        else
                            Console.WriteLine($"      ❄️ HEATER REPORT (Page {currentIndex + 1}/{totalPages}) ❄️      ");
                        Console.WriteLine("========================================");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.White;
                        if (currentIndex == 0)
                        {
                            var firstDailyUsage = heatersData[0].Usage;
                            Console.WriteLine("\n===== Monthly Report Using Threads =====");
                            var t1 = new Thread(() => PrintUsageWithThreadId(firstDailyUsage));
                            var t2 = new Thread(() => PrintUsageWithThreadId(firstDailyUsage));
                            t1.Start(); t2.Start();
                            t1.Join(); t2.Join();

                            Console.WriteLine("\n===== Monthly Report Using Task =====");
                            var tasks = new[]
                            {
                                Task.Run(() => PrintUsageWithThreadId(firstDailyUsage)),
                                Task.Run(() => PrintUsageWithThreadId(firstDailyUsage))
                            };
                            await Task.WhenAll(tasks);
                        }
                        else
                        {
                            var currentHeater = heatersData[currentIndex - 1].Heater;
                            currentHeater.PrintMonthlyReport();
                        }

                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n========================================");
                        Console.WriteLine("[ <- Left Arrow: Prev | Right Arrow: Next -> | Esc/X: Exit ]");
                        Console.ResetColor();

                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.RightArrow) currentIndex = (currentIndex + 1) % totalPages;
                        else if (key == ConsoleKey.LeftArrow) currentIndex = (currentIndex - 1 + totalPages) % totalPages;
                        else if (key == ConsoleKey.Escape || key == ConsoleKey.X) viewingReports = false;
                    }
                    break;

                case "2":
                    Console.WriteLine("\n--- Request Replacement ---");
                    Console.WriteLine($"House ID: {house.HouseID}");
                    if (house.Heaters == null || house.Heaters.Count == 0)
                    {
                        Console.WriteLine("No heaters found in this house.");
                    }
                    else
                    {
                        foreach (var h in house.Heaters) Console.WriteLine($"  - Heater ID: {h.HeaterId}");

                        bool replaced = false;
                        while (!replaced)
                        {
                            int repHeaterId = ReadInt32("Enter Heater ID to replace (or -1 to cancel): ");
                            if (repHeaterId == -1) break;

                            var cityCenter = new CityCenterService();
                            if (house.Heaters.Any(h => h.HeaterId == repHeaterId))
                            {
                                cityCenter.RequestReplacement(house, repHeaterId);
                                heatersData.RemoveAll(x => x.Heater.HeaterId == repHeaterId);
                                replaced = true;
                            }
                            else Console.WriteLine($"Heater [{repHeaterId}] not found. Please try again.");
                        }
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "3":
                    await DisplayWeatherReportAsync();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "4":
                    runningDashboard = false;
                    break;

                default:
                    Console.WriteLine("\n[Warning] Invalid selection. Try again.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    private static async Task DisplayWeatherReportAsync()
    {
        Console.WriteLine("\n--- Fetching Weather Data ---");
        try
        {
            DateTime now = DateTime.UtcNow;
            DateTime start = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            DateTime end = new DateTime(now.Year, now.Month, 1).AddDays(-1);
            string url = $"https://archive-api.open-meteo.com/v1/archive?latitude=31.0409&longitude=31.3785&start_date={start:yyyy-MM-dd}&end_date={end:yyyy-MM-dd}&daily=temperature_2m_max,temperature_2m_min,precipitation_sum";
            var response = await _httpClient.GetStringAsync(url);
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

    private static void PrintUsageWithThreadId(DailyUsage usages)
    {
        foreach (var kvp in usages.dailyUsages)
            Console.WriteLine($"{kvp.Key:yyyy-MM-dd} | Hours={kvp.Value.WorkingHours} | Thread={Thread.CurrentThread.ManagedThreadId}");
    }

    private static string ReadNonEmptyLine(string prompt, string emptyMessage)
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

    private static int ReadInt32(string prompt)
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

    private static double ReadPositivePowerKw(string prompt)
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

    private static HeaterType ReadHeaterType(string prompt)
    {
        Console.Write(prompt);
        while (true)
        {
            var line = Console.ReadLine();
            if (Enum.TryParse(line, ignoreCase: true, out HeaterType type) && (type == HeaterType.Gas || type == HeaterType.Electric))
                return type;
            Console.WriteLine("Enter Gas or Electric. Try again:");
            Console.Write(prompt);
        }
    }

    private static void ConfigureHeaterFromConsole(Heater heater, IEnumerable<int> existingIds)
    {
        bool uniqueIdFound = false;
        while (!uniqueIdFound)
        {
            int possibleId = ReadInt32("Heater ID : ");
            if (existingIds.Contains(possibleId)) Console.WriteLine("This Heater ID already exists! Please enter a unique ID.");
            else { heater.HeaterId = possibleId; uniqueIdFound = true; }
        }
        heater.powerValue = ReadPositivePowerKw("Heater Power (Kilowatt) : ");
        heater.heaterType = ReadHeaterType("Heater Type (Gas OR Electric) : ");
    }

    private static double ReadWorkingHours()
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

    private static void RunDailyUsageLoop(int year, int month, Heater heater, DailyUsage dailyUsage, ServiceOne serviceOne)
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

                string res = DailyUsage.ReadInputState();
                if (res == "y")
                {
                    double workingHoursInput = ReadWorkingHours();
                    serviceOne.workingHours!.Add(workingHoursInput);
                    double consumption = workingHoursInput * heater.powerValue;
                    serviceOne.heaterValues!.Add(consumption);
                    var dateTime = currentDay.ToDateTime(TimeOnly.MinValue);
                    dailyUsage.RecordDailyUsage(dateTime, workingHoursInput, heater.powerValue);
                    heater.Open(dateTime);
                }
                else continueMonths = false;

                if (currentDay.Day == daysInMonth) dailyUsage.OnMonthExpired();
            }
        }
    }
}
