using IceCity;
using IceCity.Services;
using IceCity.UI;

using Microsoft.Extensions.DependencyInjection;

partial class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<List<DailyUsage>> FetchLastMonthWeatherAsync()
    {
        var usageList = new List<DailyUsage>();
        try
        {
            DateTime now = DateTime.UtcNow;
            DateTime start = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            DateTime end = new DateTime(now.Year, now.Month, 1).AddDays(-1);

            string url = $"https://archive-api.open-meteo.com/v1/archive?latitude=31.0409&longitude=31.3785&start_date={start:yyyy-MM-dd}&end_date={end:yyyy-MM-dd}&daily=temperature_2m_max,temperature_2m_min,precipitation_sum";

            var response = await _httpClient.GetStringAsync(url);
            using var json = System.Text.Json.JsonDocument.Parse(response);
            var daily = json.RootElement.GetProperty("daily");
            var dates = daily.GetProperty("time").EnumerateArray();
            var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray();
            var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray();
            var rain = daily.GetProperty("precipitation_sum").EnumerateArray();

            while (dates.MoveNext() && maxTemps.MoveNext() && minTemps.MoveNext() && rain.MoveNext())
            {
                if (DateTime.TryParse(dates.Current.GetString(), out DateTime date))
                {
                    // Simulated storage for weather data mapped to DailyUsage properties
                    usageList.Add(new DailyUsage
                    {
                        Date = date,
                        HeaterValue = maxTemps.Current.GetDouble(), // Storing max temp as heater value for demonstration
                        HoursWorked = rain.Current.GetDouble()      // Storing rain as hours for demonstration
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Failed to fetch weather async: {ex.Message}");
        }
        return usageList;
    }

    public static void PrintLastMonthDailyUsageWithThreads(List<DailyUsage> usageList)
    {
        var t1 = new Thread(() => PrintUsageWithThreadId(usageList));
        var t2 = new Thread(() => PrintUsageWithThreadId(usageList));
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();
    }

    public static async Task PrintLastMonthDailyUsageWithTasks(List<DailyUsage> usageList)
    {
        var tasks = new[] {
            Task.Run(() => PrintUsageWithTaskId(usageList)),
            Task.Run(() => PrintUsageWithTaskId(usageList))
        };
        await Task.WhenAll(tasks);
    }

    private static void PrintUsageWithThreadId(IEnumerable<DailyUsage> usages)
    {
        foreach (var u in usages)
        {
            Console.WriteLine($"{u.Date:yyyy-MM-dd} | Hours={u.HoursWorked:N1} | HeaterVal={u.HeaterValue:N1} | Thread={Thread.CurrentThread.ManagedThreadId}");
        }
    }

    private static void PrintUsageWithTaskId(IEnumerable<DailyUsage> usages)
    {
        foreach (var u in usages)
        {
            Console.WriteLine($"{u.Date:yyyy-MM-dd} | Hours={u.HoursWorked:N1} | HeaterVal={u.HeaterValue:N1} | Task={Task.CurrentId} | Thread={Thread.CurrentThread.ManagedThreadId}");
        }
    }

    public static async Task Main()
    {
        // -------------------------------------------------------------
        // PART 6: Demonstrate Dependency Inversion (DI)
        // Set up the DI container
        // -------------------------------------------------------------
        var serviceProvider = new ServiceCollection()
            .AddTransient<ICostCalculationStrategy, StandardCostStrategy>() // Default
            .AddSingleton<ICostStrategyFactory, CostStrategyFactory>()
            .AddTransient<CalculationService>() // By default it will get ICostCalculationStrategy injected
            .BuildServiceProvider();

        // Resolve factory from DI
        var strategyFactory = serviceProvider.GetRequiredService<ICostStrategyFactory>();

        var house = new House();
        var ownerName = ConsoleUI.ReadNonEmptyLine("Owner Name : ", "Cant be Empty ! Enter a valid name:");
        var owner = new Owner(ownerName);
        // NOTE: Strategies are now resolved dynamically via the Factory injected through DI.
        // ICostCalculationStrategy standardCost = new StandardCostStrategy();
        // ICostCalculationStrategy ecoCost = new EcoCostStrategy();


        house.HouseID = ConsoleUI.ReadInt32("House ID : ");

        var heatersData = new List<(Heater Heater, DailyUsage Usage, CalculationService costService)>();

        bool addMoreHeaters = true;
        while (addMoreHeaters)
        {
            try
            {
                var dailyUsage = new DailyUsage();
                // Factory creates the strategy. For now we default to Standard for new heaters,
                // but the report uses the factory to pick the right one.
                ICostCalculationStrategy currentStrategy = strategyFactory.GetStrategy("standard");
                CalculationService serviceOne = new CalculationService(currentStrategy);
                
                Heater heater;
                Console.Write("Will you use a Solar heater for this configuration? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    heater = new SolarHeater(dailyUsage);
                }
                else
                {
                    heater = new Heater(dailyUsage);
                }

                heater.OpenHeater += (sender, e) => { Console.WriteLine($"Heater opened at {e.Date:yyyy-MM-dd HH:mm}"); };
                heater.CloseHeater += (sender, e) => { 
                    SaveDailyUsageDelegate saveDelegate = usage => house.DailyUsages.Add(usage);
                    var usage = new DailyUsage { Date = e.StartTime.Date, HoursWorked = e.HoursWorked, HeaterValue = heater.powerValue };
                    saveDelegate(usage);
                };

                var existingIds = heatersData.Where(h => h.Heater.HeaterId.HasValue).Select(h => h.Heater.HeaterId!.Value);
                
                ConsoleUI.ConfigureHeater(heater, existingIds);

                heater.houseID = house.HouseID;
                house.AddHeater(heater);

                heatersData.Add((heater, dailyUsage, serviceOne));

                ConsoleUI.RunDailyUsageLoop(2026, 2, heater, dailyUsage, serviceOne);

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
            ConsoleUI.DisplayDashboardMenu();

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
                    int totalPages = heatersData.Count;
                    bool viewingReports = true;

                    while (viewingReports)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("========================================");
                        Console.WriteLine($"      ❄️ HEATER REPORT (Page {currentIndex + 1}/{totalPages}) ❄️      ");
                        Console.WriteLine("========================================");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.White;
                        var currentHeater = heatersData[currentIndex].Heater;
                        
                        var reportLines = Report.GetDailyUsageReport(currentHeater._dailyUsage).ToList();
                        
                        var workingHoursList = currentHeater._dailyUsage.dailyUsages.Values.Select(v => v.WorkingHours).ToList();
                        double totalHours = workingHoursList.Sum();
                        
                        // Use Factory to determine Strategy
                        string strategyType = (totalHours < 120) ? "eco" : "standard";
                        ICostCalculationStrategy selectedStrategy = strategyFactory.GetStrategy(strategyType);
                        CalculationService costCalc = new CalculationService(selectedStrategy);
                        
                        var consumptionValues = currentHeater._dailyUsage.dailyUsages.Values.Select(v => v.Consumption).ToList();
                        double totalCost = costCalc.MonthlyCost(workingHoursList, consumptionValues);
                        reportLines.Add($"-------------------------------------------");
                        reportLines.Add($"Strategy Used: {strategyType.ToUpper()}");
                        reportLines.Add($"Total Cost: {totalCost:N2}");

                        foreach(var line in reportLines) Console.WriteLine(line);


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
                        foreach (var h in house.Heaters) Console.WriteLine($"  --> Heater ID: {h.HeaterId}");

                        bool replaced = false;
                        while (!replaced)
                        {
                            int repHeaterId = ConsoleUI.ReadInt32("Enter Heater ID to replace (or 0 to cancel): ");
                            if (repHeaterId == 0) break;

                            var cityCenter = new CityCenterService();
                            // Using the updated logic that returns bool
                            if (cityCenter.RequestReplacement(house, repHeaterId))
                            {
                                heatersData.RemoveAll(x => x.Heater.HeaterId == repHeaterId);
                                Console.WriteLine($"Heater [{repHeaterId}] has been successfully replaced.");
                                replaced = true;
                            }
                            else Console.WriteLine($"Heater [{repHeaterId}] not found or cannot be replaced. Please try again.");
                        }
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "3":
                    await ConsoleUI.DisplayWeatherReportAsync(_httpClient);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "4":
                    Console.WriteLine("\n--- Fetching Weather Data Async & Printing with Threads/Tasks ---");
                    var weatherUsage = await FetchLastMonthWeatherAsync();
                    
                    Console.WriteLine("\n[Threads Printing]");
                    PrintLastMonthDailyUsageWithThreads(weatherUsage);

                    Console.WriteLine("\n[Tasks Printing]");
                    await PrintLastMonthDailyUsageWithTasks(weatherUsage);
                    
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "5":
                    Console.WriteLine("\n--- Simulating Heater Failure ---");
                    if (house.Heaters != null && house.Heaters.Count > 0)
                    {
                        var heaterToFail = house.Heaters[0];
                        if (heaterToFail != null)
                        {
                            try
                            {
                                // Simulate failure
                                throw new HeaterFailedException($"Heater {heaterToFail.HeaterId} has catastrophically failed!");
                            }
                            catch (HeaterFailedException ex)
                            {
                                Console.WriteLine($"[Alert] Caught exception: {ex.Message}");
                                Console.WriteLine("Contacting City Center Service for replacement...");
                                
                                var cityCenter = new CityCenterService();
                                var newHeater = await cityCenter.RequestReplacementAsync(house, heaterToFail.HeaterId);
                                
                                if (newHeater != null)
                                {
                                    Console.WriteLine($"[Success] Replaced with new heater ID: {newHeater.HeaterId}");
                                    // Update our tracking list if needed
                                    int idx = heatersData.FindIndex(x => x.Heater.HeaterId == heaterToFail.HeaterId);
                                    if (idx >= 0)
                                    {
                                        heatersData[idx] = (newHeater, newHeater._dailyUsage, heatersData[idx].costService);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("[Error] Replacement failed or heater was null.");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No heaters to fail.");
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "6":
                    runningDashboard = false;
                    break;

                default:
                    Console.WriteLine("\n[Warning] Invalid selection. Try again.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }
}
