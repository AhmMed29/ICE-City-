namespace IceCity.Services
{
    public class CalculationService
    {
        public List<double>? workingHours = new();
        public List<double>? heaterValues = new();

        private readonly ICostCalculationStrategy _costStrategy;
        public CalculationService(ICostCalculationStrategy costStrategy)
        {
            _costStrategy = costStrategy;
        }

        public double MonthlyCost(List<double> WorkingHours, List<double> Heaters)
        {
            return _costStrategy.CalculateCost(WorkingHours, Heaters);
        }
    }
}
