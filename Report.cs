namespace IceCity
{
    public class Report
    {
        public static IEnumerable<string> GetDailyUsageReport(DailyUsage dailyUsage)
        {
            var report = new List<string>();
            foreach (var kvp in dailyUsage.dailyUsages)
            {
                var day = kvp.Key;
                var (workingHours, consumption) = kvp.Value;
                report.Add($"Opened on [{day:dd/MM/yyyy}] WorkingHours: {workingHours}, Consumption: {consumption}");
            }
            return report;
        }

        public static IEnumerable<string> GetOwnerReport(Owner owner)
        {
            return new List<string>
            {
                "-----------Owner Report-------------",
                $"Owner Name : [{owner.Name}]",
                "=========================="
            };
        }
    } 
}
