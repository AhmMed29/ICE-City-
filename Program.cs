using IceCity;
using IceCity.Services;
using IceCity.UI;

partial class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task Main()
    {
        var house = new House();
        var ownerName = ConsoleUI.ReadNonEmptyLine("Owner Name : ", "Cant be Empty ! Enter a valid name:");
        var owner = new Owner(ownerName);
        // NOTE: This manual instantiation of strategies is functional, but as the project grows,
        // consider using a Factory pattern or a Dependency Injection container to manage the creation of strategies.
        ICostCalculationStrategy standardCost = new StandardCostStrategy();
        ICostCalculationStrategy ecoCost = new EcoCostStrategy();


        house.HouseID = ConsoleUI.ReadInt32("House ID : ");

        var heatersData = new List<(Heater Heater, DailyUsage Usage, CalculationService costService)>();

        bool addMoreHeaters = true;
        while (addMoreHeaters)
        {
            try
            {
                var dailyUsage = new DailyUsage();
                CalculationService serviceOne = new(standardCost);
                var heater = new Heater(dailyUsage);
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
                        
                        ICostCalculationStrategy selectedStrategy = (totalHours < 120) ? ecoCost : standardCost;
                        CalculationService costCalc = new CalculationService(selectedStrategy);
                        
                        var consumptionValues = currentHeater._dailyUsage.dailyUsages.Values.Select(v => v.Consumption).ToList();
                        double totalCost = costCalc.MonthlyCost(workingHoursList, consumptionValues);
                        reportLines.Add($"-------------------------------------------");
                        reportLines.Add($"Strategy Used: {(totalHours < 120 ? "Eco" : "Standard")}");
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
