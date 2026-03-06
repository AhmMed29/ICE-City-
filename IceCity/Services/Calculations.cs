namespace IceCity.Services
{
    /// <summary>
    /// Performs statistical and energy usage calculations on monthly heater data.
    /// </summary>
    public class Calculations
    {
        // WHY CHANGED: Data was hardcoded as public mutable fields inside the business logic class.
        // That made it impossible to use real user input, and any code could accidentally modify
        // the lists. Now data comes in through the constructor (private, read-only).
        private readonly List<double> _workingHours;
        private readonly List<double> _heaterValues;

        // Default electricity rate per kWh — easily changed without touching every call site.
        // SUGGESTED FEATURE: Make this configurable per owner/region for multi-region support.
        public const double DefaultElectricityRatePerKwh = 0.12;

        /// <param name="workingHours">Daily heater working hours for each day of the month.</param>
        /// <param name="heaterValues">Daily heater load/efficiency values for each day.</param>
        public Calculations(List<double> workingHours, List<double> heaterValues)
        {
            _workingHours = workingHours ?? throw new ArgumentNullException(nameof(workingHours));
            _heaterValues = heaterValues ?? throw new ArgumentNullException(nameof(heaterValues));
        }

        // Total heater usage hours for the whole month.
        public double TotalWorkingTime() => _workingHours.Sum();

        // WHY RENAMED: The old name "MedianHeaterValue" was wrong — the code computed an
        // arithmetic mean (sum / count), not a statistical median. Misleading names cause
        // real bugs when other developers rely on incorrect assumptions.
        public double AverageHeaterValue() =>
            _heaterValues.Count > 0 ? _heaterValues.Sum() / _heaterValues.Count : 0;

        // What fraction of the maximum possible running time was the heater actually used.
        // WHY CHANGED: The old MonthlyAverageCost() had two critical bugs:
        //   1. workingHours.Sort() mutated the shared list — permanently changing order
        //      for every future caller, causing silent data corruption.
        //   2. The variable was named "medianValue" but computed a mean; and the formula
        //      produced a dimensionless number with no clear meaning or unit.
        // This replaces it with a clean utilization rate: totalHours / (24h * days).
        public double MonthlyUtilizationRate() =>
            _workingHours.Count > 0 ? TotalWorkingTime() / (24.0 * _workingHours.Count) : 0;

        // Calculates estimated monthly electricity cost: hours × kilowatts × rate.
        // SUGGESTED FEATURE: Add a gas cost overload using cubic-meters and a gas tariff rate.
        public double CalculateMonthlyCost(double powerKw, double ratePerKwh = DefaultElectricityRatePerKwh) =>
            TotalWorkingTime() * powerKw * ratePerKwh;

        // Returns the day index (1-based) with the highest usage — useful for peak analysis.
        // SUGGESTED FEATURE: Surface this in the report as a "peak day" insight.
        public int PeakUsageDay() =>
            _workingHours.Count > 0 ? _workingHours.IndexOf(_workingHours.Max()) + 1 : 0;
    }
}
